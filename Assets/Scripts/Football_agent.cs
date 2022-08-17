using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Football_agent : Agent {

    private Main_brain brain;
    public Bots rivals;
    private float speed = 15.0f;
    private float rotationSpeed = 400.0f;
    private float hit_power = 700f;
    public GameObject ball;
    private Ball ball_script;
    private Transform ball_transform;
    private Rigidbody ball_rb;
    public GameObject[] my_players = new GameObject[5];
    public Transform spartak_gates;
    public Transform dinamo_gates;
    public Material spartak;
    public Material dinamo;
    private Transform[] my_transform = new Transform[5];
    private Renderer[] my_renderer = new Renderer[5];
    private Rigidbody[] my_rb = new Rigidbody[5];
    [System.NonSerialized]public bool red_team;
    private float ball_to_my_players_distance, ball_to_my_players_distance_past_step;
    private float ball_gates_distance, ball_gates_distance_past_step;

    private void Awake() {
        brain = GetComponent<Main_brain>();
        ball_script = ball.GetComponent<Ball>();
        ball_rb = ball.GetComponent<Rigidbody>();
        ball_transform = ball.GetComponent<Transform>();
        for (int i = 0; i < 5; i++){
            my_transform[i] = my_players[i].GetComponent<Transform>();
            my_rb[i] = my_players[i].GetComponent<Rigidbody>();
            my_renderer[i] = my_players[i].GetComponent<Renderer>();
        }
    }

    public void Start_episode_part1(bool red_team){
        for (int i = 0; i < my_players.Length; i++){
            if (red_team){
                my_renderer[i].material = spartak;
                my_transform[i].position.Set(-5f, 2.106f, -4+i*2);
            }else{
                 my_renderer[i].material = dinamo;
                 my_transform[i].position.Set(5f, 2.106f, -4+i*2);
            }

        }

    }

    public override void OnEpisodeBegin(){
        red_team = Random.Range(0, 1) == 1;
        Start_episode_part1(red_team);
        rivals.FixPosition(!red_team);
        ball_script.RandomPosition();
        for (int i = 0; i < 5; i++)
            ball_to_my_players_distance += Vector3.Distance(my_transform[i].position, ball_transform.position);
        ball_to_my_players_distance /= 5;
        ball_gates_distance = Vector3.Distance(ball_transform.position, red_team ? spartak_gates.position : dinamo_gates.position);
    }
    private void FixedUpdate() {
        ball_to_my_players_distance_past_step = ball_to_my_players_distance;
        ball_gates_distance_past_step = ball_gates_distance;
        for (int i = 0; i < 5; i++)
            ball_to_my_players_distance += Vector3.Distance(my_transform[i].position, ball_transform.position);
        ball_to_my_players_distance /= 5;
        ball_gates_distance = Vector3.Distance(ball_transform.position, red_team ? spartak_gates.position : dinamo_gates.position);

        float reward = ball_to_my_players_distance - ball_to_my_players_distance_past_step;
        reward *= reward > 0? 1:0;
        AddReward(reward);
        reward = ball_gates_distance - ball_gates_distance_past_step;
        reward *= reward > 0? 1:0;
        AddReward(reward);
        Debug.Log("T$GEF");
    }

    public override void CollectObservations(VectorSensor sensor){
        Debug.Log("YOU");
        sensor.AddObservation(ball_transform.position);
        sensor.AddObservation(ball_rb.velocity);
        for (int i = 0; i < 5; i++){
            sensor.AddObservation(my_transform[i].position);
            sensor.AddObservation(my_transform[i].rotation);
        }
        foreach (Transform rival in rivals.GetRivals()){
            sensor.AddObservation(rival.position);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> actions = actionsOut.ContinuousActions;
        ActionSegment<int> disc_actions = actionsOut.DiscreteActions;
        int ind = 0;
        for (int i = 0; i < 5; i++){
            actions[ind++] = Input.GetAxisRaw("Horizontal");
            actions[ind++] = Input.GetAxisRaw("Vertical");
            actions[ind++] = Input.GetAxis("Mouse X");
            actions[ind++] = 0;
            actions[ind++] = Input.GetAxis("Mouse Y");
            disc_actions[i] = Input.GetKeyDown(KeyCode.Space)? 1:0;
        }
    }

    public override void OnActionReceived(ActionBuffers actions){
        int cont_ind = 0;
        int disc_ind = 0;
        for (int i = 0; i < 5; i++){
            my_transform[i].position += new Vector3(actions.ContinuousActions[cont_ind++], 0, 0)*Time.deltaTime*speed;
            my_transform[i].Rotate(new Vector3(0, actions.ContinuousActions[cont_ind++], 0)*Time.deltaTime*rotationSpeed);
            if (actions.DiscreteActions[disc_ind++] == 1){
                if (Vector3.Distance(ball_transform.position, my_transform[i].position) <= 3)
                    ball_rb.AddForce(my_transform[i].rotation * new Vector3(Mathf.Clamp(actions.ContinuousActions[cont_ind++], 0, 1)
                        , Mathf.Clamp(actions.ContinuousActions[cont_ind++], 0, 1)
                        , Mathf.Clamp(actions.ContinuousActions[cont_ind++], -1, 1)));
                else
                    cont_ind += 3;
            }
            else
                cont_ind += 3;
        }
    }

}

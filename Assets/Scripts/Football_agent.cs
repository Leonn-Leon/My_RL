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
    private bool key_repeat;
    private int main_player;


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
                my_transform[i].localPosition = new Vector3(-5f, 3.02f, -4+i*2);
                my_transform[i].localRotation = new Quaternion(0, 0, 0, 0);
            }else{
                 my_renderer[i].material = dinamo;
                 my_transform[i].localPosition = new Vector3(5f, 3.02f, -4+i*2);
                 my_transform[i].localRotation = new Quaternion(0, -90, 0, 0);
            }
        }

    }

    public override void OnEpisodeBegin(){
        red_team = Random.Range(0, 2) == 1;
        main_player = 0;
        Start_episode_part1(red_team);
        rivals.FixPosition(!red_team);
        ball_script.RandomPosition();
        
        for (int i = 0; i < 5; i++)
            ball_to_my_players_distance += Vector3.Distance(my_transform[i].localPosition, ball_transform.localPosition);
        ball_to_my_players_distance /= 5;
        
        ball_to_my_players_distance = Vector3.Distance(my_transform[main_player].localPosition, ball_transform.localPosition);
        ball_gates_distance = Vector3.Distance(ball_transform.localPosition, red_team ? dinamo_gates.localPosition : spartak_gates.localPosition);
    }
    private void FixedUpdate() {
        ball_to_my_players_distance_past_step = ball_to_my_players_distance;
        ball_gates_distance_past_step = ball_gates_distance;
        
        for (int i = 0; i < 5; i++)
            ball_to_my_players_distance += Vector3.Distance(my_transform[i].localPosition, ball_transform.localPosition);
        ball_to_my_players_distance /= 5;
        
        ball_to_my_players_distance = Vector3.Distance(my_transform[main_player].localPosition, ball_transform.localPosition);
        ball_gates_distance = Vector3.Distance(ball_transform.localPosition, red_team ? dinamo_gates.localPosition : spartak_gates.localPosition);

        float reward = ball_to_my_players_distance_past_step - ball_to_my_players_distance;
        if (ball_rb.velocity.magnitude == 0)
            AddReward(reward);
        /*
        if (ball_to_my_players_distance < 3){
            AddReward(10);
            EndEpisode();
        }
        */
        reward = ball_gates_distance_past_step - ball_gates_distance;
        AddReward(reward);
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(ball_transform.localPosition);
        sensor.AddObservation(ball_rb.velocity);
        for (int i = 0; i < 5; i++){
            sensor.AddObservation(my_transform[i].localPosition.x);
            sensor.AddObservation(my_transform[i].localPosition.z);
            sensor.AddObservation(my_transform[i].rotation.y);
        }
        foreach (Transform rival in rivals.GetRivals()){
            sensor.AddObservation(rival.localPosition.x);
            sensor.AddObservation(rival.localPosition.z);
        }
        sensor.AddObservation(red_team);
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> actions = actionsOut.ContinuousActions;
        ActionSegment<int> disc_actions = actionsOut.DiscreteActions;
        int ind = 0;
        //for (int i = 0; i < 5; i++){
        actions[ind++] = Input.GetAxisRaw("Vertical");
        actions[ind++] = Input.GetAxisRaw("Horizontal");
        actions[ind++] = 0.7f;
        actions[ind++] = 0.7f;
        actions[ind++] = 0;
        int space_key = Input.GetKey(KeyCode.Space)? 1:0;
        
        
        if (space_key == 1){
            if (!key_repeat){
                disc_actions[0] = space_key;
                float corner = Vector3.Angle(my_transform[0].rotation * new Vector3(1, 0, 0), ball_transform.localPosition - my_transform[0].localPosition);
                Debug.Log(corner);
            }
            key_repeat = true;
        }else
            key_repeat = false;
    }

    public override void OnActionReceived(ActionBuffers actions){
        int cont_ind = 0;
        int disc_ind = 0;
        float corner;
        for (int i = 0; i < 5; i++){
            
            if (my_transform[i].localPosition.y < 0){
                AddReward(-0.001f);
                EndEpisode();
                break;
            }
    
            my_transform[i].localPosition += my_transform[i].rotation * new Vector3(actions.ContinuousActions[cont_ind++], 0, 0)*Time.deltaTime*speed;
            my_transform[i].Rotate(new Vector3(0, actions.ContinuousActions[cont_ind++], 0)*Time.deltaTime*rotationSpeed);

            corner = Vector3.Angle(my_transform[0].rotation * new Vector3(1, 0, 0), ball_transform.localPosition - my_transform[0].localPosition);
            
            if (corner < 10)
                AddReward(0.0001f);
            else if (corner < 50)
                AddReward(0.00005f);
            else if (corner < 90)
                AddReward(0.000005f);
            else
                AddReward(-1e-6f);
            
            if (actions.DiscreteActions[disc_ind++] == 1 && corner < 50){
                if (Vector3.Distance(ball_transform.localPosition, my_transform[i].localPosition) <= 3f){
                    ball_rb.AddForce(my_transform[i].rotation * new Vector3(Mathf.Clamp(actions.ContinuousActions[cont_ind++], 0, 1)
                        , Mathf.Clamp(actions.ContinuousActions[cont_ind++], 0, 1)
                        , Mathf.Clamp(actions.ContinuousActions[cont_ind++], -1, 1))*hit_power);
                }
                else
                    cont_ind += 3;
            }
            else
                cont_ind += 3;
        }

    }

}

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
    public GameObject ball;
    private Ball ball_script;
    public GameObject[] my_players = new GameObject[5];
    private Transform[] my_transform = new Transform[5];
    private Rigidbody[] my_rb = new Rigidbody[5];
    [System.NonSerialized]public bool red_team;

    private void Awake() {
        brain = GetComponent<Main_brain>();
        ball_script = ball.GetComponent<Ball>();
        for (int i = 0; i < 5; i++){
            my_transform[i] = my_players[i].GetComponent<Transform>();
            my_rb[i] = my_players[i].GetComponent<Rigidbody>();
        }
    }

    public override void OnEpisodeBegin(){
            red_team = Random.Range(0, 1) == 1;
            brain.Start_episode_part1(red_team);
            rivals.Start_episode_part1(!red_team);
            ball_script.Start_position();
    }

    public override void CollectObservations(VectorSensor sensor){
        //brain.GetComponent(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions){
        int cont_ind = 0;
        int disc_ind = 0;
        for (int i = 0; i < 5; i++){
            my_transform[i].position += new Vector3(actions.ContinuousActions[cont_ind++], 0, actions.ContinuousActions[cont_ind++])*Time.deltaTime*speed;
            my_transform[i].Rotate(new Vector3(actions.ContinuousActions[cont_ind++], 0, actions.ContinuousActions[cont_ind++])*Time.deltaTime*rotationSpeed);
            if (actions.DiscreteActions[disc_ind++] == 1){

            }
        }
    }

}

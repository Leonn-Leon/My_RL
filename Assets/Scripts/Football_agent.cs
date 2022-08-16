using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Football_agent : Agent {

    private Main_brain brain;
    public Bots rivals;
    public Ball space;
    [System.NonSerialized]public bool red_team;

    private void Awake() {
        brain = GetComponent<Main_brain>();
    }

    public override void OnEpisodeBegin(){
            red_team = Random.Range(0, 1) == 1;
            brain.Start_episode_part1(red_team);
            rivals.Start_episode_part1(!red_team);
            space.Start_position()
            // second_team.start_position(!red_team);
    }

    public override void CollectObservations(VectorSensor sensor){

    }

    public override void OnActionReceived(ActionBuffers actions){

    }

}

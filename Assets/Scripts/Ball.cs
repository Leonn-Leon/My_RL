using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private Transform ball_transform;
    public GameObject main_team;
    public Material spartak;
    public Material dinamo;
    private Football_agent main_team_agent;

    private void Awake() {
        main_team_agent = main_team.GetComponent<Football_agent>();
        ball_transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Wall"){
            main_team_agent.AddReward(-1e-3f);
            main_team_agent.EndEpisode();
        }
    }
    private void OnTriggerEnter(Collider other) {
        Material trigger_mat = other.GetComponent<Renderer>().material;
        if (trigger_mat == spartak){
            if (main_team_agent.red_team)
                main_team_agent.AddReward(100f);
            else
                main_team_agent.AddReward(-100f);
            main_team_agent.EndEpisode();
        }
        else if (trigger_mat == dinamo){
            if (!main_team_agent.red_team)
                main_team_agent.AddReward(100f);
            else
                main_team_agent.AddReward(-100f);
            main_team_agent.EndEpisode();
        }
    }
    public void RandomPosition(){
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        ball_transform.localPosition  = new Vector3(Random.Range(-3f, 3f), 1.61f, Random.Range(-33f, 33f));
    }
}

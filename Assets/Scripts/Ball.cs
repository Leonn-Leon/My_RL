using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public GameObject main_team;
    public Material spartak;
    public Material dinamo;
    private Football_agent main_team_agent;
    public GameObject help_team;    private void Awake() {
        main_team_agent = main_team.GetComponent<Football_agent>();
    }
    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate(){
    }

    private void OnTriggerExit(Collider other) {
        main_team_agent.EndEpisode();
    }
    private void OnTriggerEnter(Collider other) {
        Material trigger_mat = other.GetComponent<Renderer>().material;
        if (trigger_mat == spartak){
            if (main_team_agent.red_team)
                main_team_agent.SetReward(10f);
            else
                main_team_agent.SetReward(-10f);
            main_team_agent.EndEpisode();
        }
        else if (trigger_mat == dinamo){
            if (!main_team_agent.red_team)
                main_team_agent.SetReward(10f);
            else
                main_team_agent.SetReward(-10f);
            main_team_agent.EndEpisode();
        }
    }
}

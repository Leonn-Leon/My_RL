using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_brain : MonoBehaviour{

    // Actions
    

    private float[] translation = new float[5]; // [-1 ... 1]
    private float[] rotation = new float[5]; // [-1 ... 1]
    private bool[] hit = new bool[5]; // [0, 1]
    private Vector3[] hit_angle = new Vector3[5]; // Vector3([0 ... 1], [0 ... 1], [-1 ... 1])

    
    // States
    /*
    private Vector3 ball_position; // ball_transform.position;
    private Vector3 ball_velocity; // ball_rb.velocity;
    private Vector3[] my_positions = new Vector3[5];
    private Vector3[] my_rotations = new Vector3[5];
    private Vector3[] rivals_positions = new Vector3[5];
    */

    // Rewards
    /*
    
    //////// PART 1 //////// 
    
    Freezy rivals in (0, 0, -30)
    ball in random position
    random team

    if face_to_ball < 10 => 0.001
    elif face_to_ball < 50 => 0.0005
    +
    mean(relu(ball_to_my_players_distance / ball_to_my_players_distance_past_step))
    +
    relu(ball_gates_distance / ball_gates_distance_past_step)
    +
    if red_team and ball in blue gates => +10
    if not red_team and ball in red gates => +10
    +
    on collision except for the floor => -0.01

    //////// PART 2 ////////

    Freezy rivals in random positions
    ball in random position
    random team

    if red_team and ball in blue gates => +10
    if not red_team and ball in red gates => +10
    +
    relu(ball_gates_distance / ball_gates_distance_past_step)
    +
    on collision except for the floor => -0.01

    //////// PART 3 ////////

    non learning rivals
    ball in centre
    random team

    if red_team and ball in blue gates => +10
    if not red_team and ball in red gates => +10
    +
    relu(ball_gates_distance / ball_gates_distance_past_step)
    +
    on collision except for the floor => -0.01

    */

    public float speed = 15.0f;
    public float rotationSpeed = 400.0f;
    public Transform face;
    public GameObject ball;
    private Transform ball_transform;
    private Rigidbody ball_rb;
    private float hit_power = 700f;
    private float leg_len = 2f;
    public GameObject[] my_players = new GameObject[5];
    private Renderer[] my_players_renderer = new Renderer[5];
    public GameObject my_gates;
    public Material spartak;
    public Material dinamo;
    /*
    private void Awake() {
        ball_transform = ball.GetComponent<Transform>();
        ball_rb = ball.GetComponent<Rigidbody>();
        for (int i = 0; i < my_players.Length; i++){
            my_positions[i] = my_players[i].GetComponent<Transform>().position;
            my_players_renderer[i] = my_players[i].GetComponent<Renderer>();
        }
    }
    

    public void Start_episode_part1(bool red_team){
        for (int i = 0; i < my_players.Length; i++){
            if (red_team){
                my_players_renderer[i].material = spartak;
                my_positions[i].Set(-5f, 2.106f, -4+i*2);
            }else{
                 my_players_renderer[i].material = dinamo;
                 my_positions[i].Set(5f, 2.106f, -4+i*2);
            }

        }

    }
    */

}

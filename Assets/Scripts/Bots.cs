using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bots : MonoBehaviour
{
    // Start is called before the first frame update
    public Material spartak, dinamo;
    public GameObject[] rivals = new GameObject[5];
    private Transform[] rivals_transform = new Transform[5];
    private Renderer[] rivals_renderer = new Renderer[5];

    private void Awake() {
        for (int i = 0; i < 5; i++){
            rivals_transform[i] = rivals[i].GetComponent<Transform>();
            rivals_renderer[i] = rivals[i].GetComponent<Renderer>();
        }
    }

    public void Start_episode_part1(bool red_team){
        for (int i = 0; i < 5; i++){
            if (red_team)
                rivals_renderer[i].material = spartak;
            else
                rivals_renderer[i].material = dinamo;
            rivals_transform[i].position.Set(0, 0, -30);

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bots : MonoBehaviour
{
    // Start is called before the first frame update
    public Material spartak, dinamo;
    public GameObject[] rivals = new GameObject[5];
    private Rigidbody[] rivals_rb = new Rigidbody[5];
    private Transform[] rivals_transform = new Transform[5];
    private Renderer[] rivals_renderer = new Renderer[5];

    private void Awake() {
        for (int i = 0; i < 5; i++){
            rivals_transform[i] = rivals[i].GetComponent<Transform>();
            rivals_renderer[i] = rivals[i].GetComponent<Renderer>();
            rivals[i].GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public Transform[] GetRivals(){
        return rivals_transform;
    }

    public void FixPosition(bool red_team){
        for (int i = 0; i < 5; i++){
            if (red_team)
                rivals_renderer[i].material = spartak;
            else
                rivals_renderer[i].material = dinamo;
            rivals_transform[i].localPosition = new Vector3(-4f, 0, -40f);

        }

    }
}

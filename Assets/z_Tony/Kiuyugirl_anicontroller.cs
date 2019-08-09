using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiuyugirl_anicontroller : MonoBehaviour {

    public Animator ani;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ani.Play("idle");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ani.Play("action");
        }

    }


    
}

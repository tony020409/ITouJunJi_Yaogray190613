using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_anicontroller : MonoBehaviour {

    public Animator ani;

    public string ani1;
    public string ani2;
    public string ani3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ani.Play(ani1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ani.Play(ani2);
            if (gameObject.name == "FuFace_9F")
            {
                GetComponent<AimIK>().enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ani.Play(ani3);

        }


    }


    
}

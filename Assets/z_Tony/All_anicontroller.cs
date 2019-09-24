using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_anicontroller : MonoBehaviour {

    public Animator ani;
    public AimIK aimik;
    public string ani1;
    public string ani2;
    public string ani3;

	// Use this for initialization
	void Start () {
        if (gameObject.name == "FuFace_9F")
        {
            aimik.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ani.Play(ani1);
            if(gameObject.name == "FuFace_9F")
            {
                aimik.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ani.Play(ani2);
            if (gameObject.name == "FuFace_9F")
            {
                aimik.enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ani.Play(ani3);

        }


    }


    
}

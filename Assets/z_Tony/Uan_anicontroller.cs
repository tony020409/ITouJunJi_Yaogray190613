using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uan_anicontroller : MonoBehaviour {

    public Animator ani;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ani.Play("idle01");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ani.Play("attack");
        }

    }


    
}

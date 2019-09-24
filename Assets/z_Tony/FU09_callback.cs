using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class FU09_callback : MonoBehaviour {

    public AimIK aimik;

	// Use this for initialization
	void Start () {
        aimik.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void open_aimik()
    {
        aimik.enabled = true;
    }

}

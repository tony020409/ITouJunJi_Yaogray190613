using RootMotion.Demos;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {

    
    public GameObject Target_Monster;
    private AimIK aimIK;
    private InteractionSystemTestGUI InteractionSystemTestGUI;

    // Use this for initialization
    void Start () {

        aimIK = Target_Monster.GetComponent<AimIK>();
        InteractionSystemTestGUI = Target_Monster.GetComponent<InteractionSystemTestGUI>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void Start_interaction()
    {
        InteractionSystemTestGUI.Start_Interaction();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayTarget")
        {
            //aimIK.enabled = false;
            Start_interaction();
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}

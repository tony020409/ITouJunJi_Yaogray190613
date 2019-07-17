using RootMotion.Demos;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_In : MonoBehaviour {

    
    public GameObject Target_Monster;
    private InteractionSystemTestGUI InteractionSystemTestGUI;
    private Collider In_Collider;
    public Collider Out_Collider;
   
    // Use this for initialization
    void Start () {
        InteractionSystemTestGUI = Target_Monster.GetComponent<InteractionSystemTestGUI>();
        In_Collider = GetComponent<Collider>();
    }
	

    public void Start_interaction()
    {
        InteractionSystemTestGUI.Start_Interaction();
    }
    public void Stop_interaction()
    {
        InteractionSystemTestGUI.Stop_Interaction();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "MyselfPlayer")
        {
            Start_interaction();
            In_Collider.enabled = false;
            Out_Collider.enabled = true;
        }
    }


}

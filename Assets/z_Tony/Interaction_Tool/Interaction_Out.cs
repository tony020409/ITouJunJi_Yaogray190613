using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Demos;
using RootMotion.FinalIK;

public class Interaction_Out : MonoBehaviour {

    public GameObject Target_Monster;
    private InteractionSystemTestGUI InteractionSystemTestGUI;
    private Collider Out_Collider;
    public Collider In_Collider; 
    void Start () {
        InteractionSystemTestGUI = Target_Monster.GetComponent<InteractionSystemTestGUI>();
        Out_Collider = GetComponent<Collider>();
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
            Stop_interaction();
            Out_Collider.enabled = false;
            In_Collider.enabled = true;
        }
    }
}

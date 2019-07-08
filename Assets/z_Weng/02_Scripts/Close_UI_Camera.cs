using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close_UI_Camera : MonoBehaviour {

    public GameObject uiCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
        {
            uiCamera.SetActive(false);
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headshot_look : MonoBehaviour {

    public Transform camera_eye;
   

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(camera_eye);
    }


    
}

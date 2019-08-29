using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water_open : MonoBehaviour {

    public Transform camera_eye;
    public Transform head_shoot;

    public GameObject water1;
    public GameObject water2;
    public GameObject water3;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        head_shoot.LookAt(camera_eye);
    }

    void water1_open()
    {
        water1.SetActive(true);
    }
    void water2_open()
    {
        water2.SetActive(true);
    }
    void water3_open()
    {
        water3.SetActive(true);
    }

}

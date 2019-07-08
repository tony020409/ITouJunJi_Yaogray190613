using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {
    public GameObject G1;
    public int kkk;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K))
        {
            kkk++;
            Instantiate(G1, transform.position, Quaternion.identity);
        }
	}
}

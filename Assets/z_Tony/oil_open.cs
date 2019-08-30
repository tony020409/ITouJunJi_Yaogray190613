using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oil_open : MonoBehaviour {

    public GameObject[] oillmesh;
    int i = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void open_oil()
    {
        for (i = 0; i < oillmesh.Length; i++)
        {
            oillmesh[i].SetActive(true);
        }
    }
    
}

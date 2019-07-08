using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathpoint : MonoBehaviour {
    public GameObject g1;
	// Use this for initialization
	void Start ()
    {
        Correctposition();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Correctposition()
    {
        g1.transform.parent = transform.root.transform;
        g1.name = transform.name;
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathmanagement : MonoBehaviour {
    public static Pathmanagement INI;

    public List<PathScript> attackPathScript;
    public List<PathScript> CirclingPathScript;

    // Use this for initialization
    void Start () {
        INI = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

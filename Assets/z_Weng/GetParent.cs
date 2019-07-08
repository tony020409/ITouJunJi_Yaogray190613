using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetParent : MonoBehaviour {

    public GameObject childObj;

	// Use this for initialization
	void Start () {
        Debug.LogWarning(childObj.transform.parent.name);
        Debug.LogWarning(childObj.transform.parent.root.name);
        //Debug.LogWarning();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

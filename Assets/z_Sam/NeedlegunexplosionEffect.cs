using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedlegunexplosionEffect : MonoBehaviour {
    public float disappearTime;
	// Use this for initialization
	void Start () {
       StartCoroutine(disappear());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
     IEnumerator disappear()
    {
        yield return new WaitForSeconds(disappearTime);
        Destroy(gameObject);
       
    }
}

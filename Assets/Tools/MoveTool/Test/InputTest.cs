using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour {

	public Transform t1;
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.S)) {
			MoveTools.f_StopMoving(t1, true);
		}
	}
}

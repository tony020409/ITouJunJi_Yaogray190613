using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RzFX_VRtest : MonoBehaviour {

    [Header("按F1~F12產生對應特效")]
    public Transform[] fxObj;
    public Transform effectPos;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1)) { Transform effect = Instantiate(fxObj[0], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F2)) { Transform effect = Instantiate(fxObj[1], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F3)) { Transform effect = Instantiate(fxObj[2], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F4)) { Transform effect = Instantiate(fxObj[3], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F5)) { Transform effect = Instantiate(fxObj[4], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F6)) { Transform effect = Instantiate(fxObj[5], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F7)) { Transform effect = Instantiate(fxObj[6], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F8)) { Transform effect = Instantiate(fxObj[7], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F9)) { Transform effect = Instantiate(fxObj[8], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F10)) { Transform effect = Instantiate(fxObj[9], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F11)) { Transform effect = Instantiate(fxObj[10], effectPos.position, Quaternion.identity) as Transform; }
        else if (Input.GetKeyDown(KeyCode.F12)) { Transform effect = Instantiate(fxObj[11], effectPos.position, Quaternion.identity) as Transform; }
    }
}

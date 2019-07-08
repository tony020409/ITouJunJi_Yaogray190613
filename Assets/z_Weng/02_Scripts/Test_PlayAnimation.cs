using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayAnimation : MonoBehaviour {

    [Header("動畫物件")]
    public Animator AnimObj;

    [Header("動畫名稱(最多六個，對應鍵盤0~5)")]
    public string[] tAnimName;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha0)){
            AnimObj.Play(tAnimName[0],-1,0);
            //AnimObj.Play(tAnimName, 0, 1);
            //AnimObj.CrossFade(tAnimName, 0.25f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            AnimObj.Play(tAnimName[1], -1, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){
            AnimObj.Play(tAnimName[2], -1, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)){
            AnimObj.Play(tAnimName[3], -1, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)){
            AnimObj.Play(tAnimName[4], -1, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)){
            AnimObj.Play(tAnimName[5], -1, 0);
        }

    }
}

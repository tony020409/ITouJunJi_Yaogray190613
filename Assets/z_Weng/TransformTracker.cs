using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 為了傳送門問題寫的，
/// 傳送物件如果有光<Light>，容易造成遊戲當機，
/// 所以另外弄個程式，讓光從外面去追位置
/// </summary>
public class TransformTracker : MonoBehaviour {

    [Rename("目標物件")]
    public Transform targetObj;


	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = targetObj.position;
        transform.rotation = targetObj.rotation;
    }



}

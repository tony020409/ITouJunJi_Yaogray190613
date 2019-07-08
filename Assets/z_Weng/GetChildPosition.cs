using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChildPosition : MonoBehaviour {

    [Header("目標子物件")]
    public GameObject targetChild;

    [Header("顯示結果 (世界座標、在父物件下的座標)")]
    public Vector3 ChildWorldPos;
    public Vector3 ChildLocalPos;

    // Use this for initialization
    void Start () {
        ChildWorldPos = targetChild.transform.position;      //子物件的世界座標
        ChildLocalPos = targetChild.transform.localPosition; //子物件在父物件下的座標
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

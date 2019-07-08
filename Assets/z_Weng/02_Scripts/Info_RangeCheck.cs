using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info_RangeCheck : MonoBehaviour {

    [Header("放入要測距離的兩個物件")]
    [Rename("物件1")] public Transform tmpA;
    [Rename("物件2")] public Transform tmpB;

    [Space(5)]
    [Rename("兩者距離")]
    public float distance;
    [Rename("兩者距離(忽略Y)")]
    public float distanceIgnoreY;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (tmpA != null && tmpB != null) {

            distance = Vector3.Distance(tmpA.position, tmpB.position);

            float xDiff = tmpA.position.x - tmpB.position.x;
            float zDiff = tmpA.position.z - tmpB.position.z;
            distanceIgnoreY = Mathf.Sqrt((xDiff * xDiff) + (zDiff * zDiff));
        }
	}
}

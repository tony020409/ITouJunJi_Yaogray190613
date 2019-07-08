using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class RangeInfo_Sphere : MonoBehaviour {

    [Rename("躲藏點偵測範圍")] public float HideDistance = 10f;
    [Rename("顯示顏色")]       public Color tmpColor = new Color(0.07f, 1.0f, 1.0f, 0.2f);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}


    /// <summary>
    /// 顯示躲避點偵測範圍
    /// </summary>
    private void OnDrawGizmos() {
        if (!enabled){
            return;
        }
        #if UNITY_EDITOR
        Handles.color = tmpColor;
        Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, HideDistance);
        //Gizmos.color = tmpColor;
        //Gizmos.DrawSphere(transform.position, HideDistance);
        #endif
    }
}

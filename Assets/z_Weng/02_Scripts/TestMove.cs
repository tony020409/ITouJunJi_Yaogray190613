using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestMove : MonoBehaviour {

    public float moveSpeed = 10f;
    public float turnSpeed = 50f;

    [Header("前後左右 移動")]
    public KeyCode Key_Move = KeyCode.W;
    public KeyCode Key_Back = KeyCode.S;
    public KeyCode Key_L = KeyCode.A;
    public KeyCode Key_R = KeyCode.D;

    [Header("上下 移動")]
    public KeyCode Key_Up   = KeyCode.R;
    public KeyCode Key_Down = KeyCode.F;

    [Header("左右 平移")]
    public KeyCode Key_QR   = KeyCode.Q;
    public KeyCode Key_QL   = KeyCode.E;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



        // 前進
        if (Input.GetKey(Key_Move))
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

        // 後退
        if (Input.GetKey(Key_Back))
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // 左轉
        if (Input.GetKey(Key_L))
            transform.Rotate(-Vector3.up, turnSpeed * Time.deltaTime);

        // 右轉
        if (Input.GetKey(Key_R))
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        // 左平移
        if (Input.GetKey(Key_QL))
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 右平移
        if (Input.GetKey(Key_QR))
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // 上升
        if (Input.GetKey(Key_Up))
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 下降
        if (Input.GetKey(Key_Down))
            transform.Translate(-Vector3.up * moveSpeed * Time.deltaTime);

        // 上轉
        //if (Input.GetKey(KeyCode.Keypad8))
        //    cameraEye.transform.Rotate(Vector3.left * turnSpeed * Time.deltaTime);
        //
        //// 下轉
        //if (Input.GetKey(KeyCode.Keypad2))
        //    cameraEye.transform.Rotate(-Vector3.left * turnSpeed * Time.deltaTime);

    }
}

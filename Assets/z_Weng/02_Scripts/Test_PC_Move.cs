using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PC_Move : MonoBehaviour {


    public Transform HeadCam;
    public float moveSpeed;
    public float turnSpeed;

    // Update is called once per frame
    void Update () {

        Vector3 dirVector = HeadCam.transform.forward;
        dirVector.y = 0;
        dirVector = dirVector.normalized * moveSpeed;

        if (Input.GetKey(KeyCode.W)){
            transform.position += dirVector * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
           transform.position += (Quaternion.AngleAxis(-90, Vector3.up) * dirVector) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position += (Quaternion.AngleAxis(180, Vector3.up) * dirVector) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))  {
            transform.position += (Quaternion.AngleAxis(90, Vector3.up) * dirVector) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E)){
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * 20); //旋轉位置可能會因為 VR位置定位的關係而偏差
        }
        if (Input.GetKey(KeyCode.Q)){
            transform.Rotate(-Vector3.up, turnSpeed * Time.deltaTime * 20); //旋轉位置可能會因為 VR位置定位的關係而偏差
        }

    }
}

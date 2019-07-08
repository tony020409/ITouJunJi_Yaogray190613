//-------------------------------------------------------------------------
// https://www.jianshu.com/p/abb9d04f3710
//-------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoAI_Test : MonoBehaviour {

    /// <summary>
    /// 角色前方偵測點
    /// </summary>
    RaycastHit hitFoward, hitRight, hitLeft;

    /// <summary>
    /// 射線長度，控制距離障礙物多遠的時候開始觸發躲避
    /// </summary>
    public float rayLength = 2;
    //碰到障碍物时的反向作用力
    Vector3 reverseForce;
    //物体自身的速度
    public Vector3 velocitySelf;
    //判断是否在进行转弯
    bool IsTurn;
    void Start()
    {
        rayLength = 2;
    }


    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.cyan);
        Debug.DrawLine(transform.position, transform.position + (transform.forward + transform.right).normalized, Color.cyan);
        Debug.DrawLine(transform.position, transform.position + (transform.forward - transform.right).normalized, Color.cyan);
        if (Physics.Raycast(transform.position, transform.forward, out hitFoward, rayLength))
        {
            //Raycast.normal表示光线射到此表面时，在此处的法线单位向量
            reverseForce = hitFoward.normal * (rayLength - (hitFoward.point - transform.position).magnitude);
            IsTurn = true;
        }
        if (Physics.Raycast(transform.position, transform.forward + transform.right, out hitFoward, rayLength))
        {
            reverseForce = hitFoward.normal * (rayLength - (hitFoward.point - transform.position).magnitude);
            IsTurn = true;
        }
        if (Physics.Raycast(transform.position, transform.forward - transform.right, out hitFoward, rayLength))
        {
            reverseForce = hitFoward.normal * (rayLength - (hitFoward.point - transform.position).magnitude);
            IsTurn = true;
        }
        if (!IsTurn)
        {
            reverseForce = Vector3.zero;
            //通过这个控制当物体躲避完障碍物以后速度变为原来的速度，为防止物体的速度越来越大
            velocitySelf = velocitySelf.normalized * (new Vector3(3, 0, 3).magnitude);
        }
        velocitySelf += reverseForce;
        transform.position += velocitySelf * Time.deltaTime;
        if (velocitySelf.magnitude > 0.01)
        {
            //控制物体转弯，让物体的正前方和速度的方向保持一致
            transform.forward = Vector3.Slerp(transform.forward, velocitySelf, Time.deltaTime);
        }
        IsTurn = false;
    }
}
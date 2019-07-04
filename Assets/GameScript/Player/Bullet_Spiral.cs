using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Spiral : BaseBullet {

    private float t;
    private float k;
    private Vector3 v;

    private float StartFix = 0f;
    private float CircleSpeed = 0f;
    private float CircleSize = 10f;

    public override void f_Fired (int iBulletId, int iBulletDT, GameEM.TeamType tTeamType, int iPlayerId) {
        base.f_Fired(iBulletId, iBulletDT, tTeamType, iPlayerId);

        t = 0;
        k = -1 * Vector3.SignedAngle(new Vector3(0, 0, 1), transform.forward, Vector3.up) * Mathf.Deg2Rad;

        CircleSpeed = _BulletDT.fSpeed;

        //Debug.LogWarning("初始化");
    }

    protected override void Update () {
        if (!_bDoing) {
            return;
        }
        if (IsDie()) {
            return;
        }
        _fLiveTime -= Time.deltaTime;          //生命倒數

        //子彈壽命歸零時，銷毀子彈
        if (_fLiveTime < 0) {
            //rayLength = 0;
            Die();
        } else {
            if (f_CheckIsCollide()) {
                return;
            }
            if (f_CheckIsCollideRole()) {
                return;
            }
        }
    }

    void FixedUpdate () {

        t += 0.02f;

        v = new Vector3(Mathf.Sin(CircleSpeed * (t + StartFix)) / CircleSize, Mathf.Cos(CircleSpeed * (t + StartFix)) / CircleSize, 0.15f);
        v = new Vector3(Mathf.Cos(k) * v.x - Mathf.Sin(k) * v.z, v.y, Mathf.Sin(k) * v.x + Mathf.Cos(k) * v.z);

        //transform.localPosition += transform.forward * _BulletDT.fSpeed * Time.deltaTime; //動畫子彈向前飛

        transform.position += v;
        transform.rotation = Quaternion.LookRotation(v, transform.up);
    }

    private bool IsDie()
    {
        if (_iHp <= 0)
        {
            return true;
        }
        return false;
    }

}

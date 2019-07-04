using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sam;
using ccVerifySDK;

public class BombState : GunBaseState
{
    private bool _bIsReload;

    private MySelfPlayerControll2 _MySelfPlayerControll2;
    public float TrackTime = 0.5f;
    public float NowTrackTime;
    private int targetMask;


    public BombState (MySelfPlayerControll2 tMySelfPlayerControll2) 
        : base((int)GunEM.Bomb) {
        _MySelfPlayerControll2 = tMySelfPlayerControll2;
        f_Init(tMySelfPlayerControll2, (int)GunEM.Bomb);
    }


    public override void f_Enter(object Obj) {
        _MySelfPlayerControll2.BombLaser.enabled = true;
        _MySelfPlayerControll2.BombLaser.startColor = _MySelfPlayerControll2.BombLaser_NullColor;
        _MySelfPlayerControll2.BombLaser.endColor = _MySelfPlayerControll2.BombLaser_NullColor;
        _MySelfPlayerControll2.Ani_Attack1.Stop();
        _bIsReload = false;
        _MySelfPlayerControll2.Bomb_TrackImage.transform.parent.gameObject.SetActive(true);
    }


    public override void f_Execute() {
        if (_bShooting) {
            RaycastHit hit;
            Ray landRay = new Ray(_MySelfPlayerControll2.Bomb_Trigger.position, _MySelfPlayerControll2.Bomb_Trigger.forward);
            //Ray 的長度乘以100 大概等於 LineRender的長度
            if (Physics.Raycast(landRay, out hit, 0.1f)) {
                TracksStart();
                _MySelfPlayerControll2.BombLaser.startColor = _MySelfPlayerControll2.BombLaser_CatchColor;
                _MySelfPlayerControll2.BombLaser.endColor = _MySelfPlayerControll2.BombLaser_CatchColor;
            }
            else {
                TrackStop();
                _MySelfPlayerControll2.BombLaser.startColor = _MySelfPlayerControll2.BombLaser_NullColor;
                _MySelfPlayerControll2.BombLaser.endColor = _MySelfPlayerControll2.BombLaser_NullColor;
            }
        }
    }

    public override void f_Exit() {
        _MySelfPlayerControll2.Bomb_TrackImage.transform.parent.gameObject.SetActive(false);
    }


    /// <summary>
    /// 追踪开始
    /// </summary>
    public void TracksStart()  {
        if (NowTrackTime <= _GunDT.fSpeed)  {
            NowTrackTime += Time.deltaTime;
            _MySelfPlayerControll2.Bomb_TrackImage.fillAmount = NowTrackTime / _GunDT.fSpeed;
        } else {
            // print("設置完成");
        }
    }


    /// <summary>
    /// 追踪停止
    /// </summary>
    public void TrackStop() {
        NowTrackTime = 0;
        _MySelfPlayerControll2.Bomb_TrackImage.fillAmount = 0.0f;
    }

}
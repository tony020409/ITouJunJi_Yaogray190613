using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sam;

public class NeedlegunState : GunBaseState
{
    private bool _bIsReload;
    private MySelfPlayerControll2 _MySelfPlayerControll2;

    public NeedlegunTrack needlegunTrack;
    public Needlegun needlegun;
    public float TrackTime = 1;
    public float NowTrackTime;

    public TrackEM trackEM = TrackEM.NotTrack;


    public NeedlegunState(MySelfPlayerControll2 tMySelfPlayerControll2)
        : base((int)GunEM.Needlegun)
    {
        _MySelfPlayerControll2 = tMySelfPlayerControll2;

        if (_GunDT == null)
        {
            _GunDT = (GunDT)glo_Main.GetInstance().m_SC_Pool.m_GunSC.f_GetSC((int)GunEM.Needlegun);

        }
    }


    public override void f_Enter(object Obj)
    {
        _MySelfPlayerControll2.NeedlegunGameObject.gameObject.SetActive(true);
        _MySelfPlayerControll2.LasergunGameObject.gameObject.SetActive(false);
        _MySelfPlayerControll2.Ani_Attack1.Stop();
        PushBullet();
        _bIsReload = false;
    }

    public override void f_Execute()
    {
        if (_bShooting)
        {
            Ray landRay = new Ray(_MySelfPlayerControll2.m_BulletStart.transform.position, _MySelfPlayerControll2.m_BulletStart.transform.forward * 1000);
            RaycastHit hit;
            if (Physics.Raycast(landRay, out hit, 1000))
            {

                _MySelfPlayerControll2.Needlegunlaser.SetPosition(1, new Vector3(0, 0, hit.point.z + 50));
                if (hit.collider.GetComponent<NeedlegunTrack>() != null)
                {

                    _MySelfPlayerControll2.Needlegunlaser.enabled = true;
                    needlegunTrack = hit.collider.GetComponent<NeedlegunTrack>();
                    TracksStart();
                    //if (trackEM == TrackEM.Trackcomplete)
                    //{
                    //    if (needlegunTrack.aimscategory == Aimscategory.SmallPter_Big)
                    //    {
                    //        var N = glo_Main.GetInstance().m_ResourceManager.f_NeedlegunBullet();
                    //        N.transform.position = _MySelfPlayerControll2.m_BulletStart.transform.position;
                    //        N.transform.rotation = _MySelfPlayerControll2.m_BulletStart.transform.rotation;
                    //        N.GetComponent<Needlegun>().f_Fired(GameEM.TeamType.A, 20, 2, 50);
                    //        TrackStop();//這個不用管  整個程序不用管
                    //        trackEM = TrackEM.NotTrack;
                    //    }
                    //}
                }
                else
                {
                    if (needlegunTrack != null)
                    {
                        TrackStop();
                        needlegunTrack = null;
                    }
                }
            }
            else
            {
                if (needlegunTrack != null)
                {
                    TrackStop();
                    needlegunTrack = null;
                }
                _MySelfPlayerControll2.Needlegunlaser.enabled = false;
            }
        }
        else
        {
            _MySelfPlayerControll2.Needlegunlaser.enabled = false;
        }
    }

    /// <summary>
    /// 追踪开始
    /// </summary>
    public void TracksStart()
    {
        trackEM = TrackEM.IsTrack;
        _MySelfPlayerControll2.trackImageGameObject.gameObject.SetActive(true);
        _MySelfPlayerControll2.NeedlegunAudioSource.enabled = true;
        if (NowTrackTime <= TrackTime)
        {
            NowTrackTime += Time.deltaTime;
            display();
        }
        else
        {
            trackEM = TrackEM.Trackcomplete;
            // print("追瞄成功");
        }
    }
    /// <summary>
    /// 追踪停止
    /// </summary>
    public void TrackStop()
    {

        trackEM = TrackEM.NotTrack;
        _MySelfPlayerControll2.trackImageGameObject.gameObject.SetActive(false);
        _MySelfPlayerControll2.NeedlegunAudioSource.enabled = false;
        _MySelfPlayerControll2.trackImage.color = _MySelfPlayerControll2.TrackColor;
        NowTrackTime = 0;
        display();
    }
    /// <summary>
    /// 追踪UI显示
    /// </summary>
    public void display()
    {
        _MySelfPlayerControll2.trackImage.fillAmount = NowTrackTime / TrackTime;
    }

}

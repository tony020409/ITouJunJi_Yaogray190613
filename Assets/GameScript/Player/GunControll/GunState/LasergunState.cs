using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sam;
public class LasergunState : GunBaseState
{
    private bool _bIsReload;

    private MySelfPlayerControll2 _MySelfPlayerControll2;
    public NeedlegunTrack needlegunTrack;
    public Needlegun needlegun;
    public float TrackTime = 0.5f;
    public float NowTrackTime;
    public TrackEM trackEM = TrackEM.NotTrack;

    public float MaxCDTime=5;
    int targetMask;
    public LasergunState(MySelfPlayerControll2 tMySelfPlayerControll2)
        : base((int)GunEM.Lasergun)
    {
        _MySelfPlayerControll2 = tMySelfPlayerControll2;
        if (_GunDT == null)
        {
            _GunDT = (GunDT)glo_Main.GetInstance().m_SC_Pool.m_GunSC.f_GetSC((int)GunEM.Lasergun);
        }
    }

    public override void f_Enter(object Obj)
    {
        
        //_MySelfPlayerControll2.LasergunGameObject.gameObject.SetActive(true);
        //_MySelfPlayerControll2.NeedlegunGameObject.gameObject.SetActive(false);
        //_MySelfPlayerControll2.RifleGameObject.gameObject.SetActive(false);
        _MySelfPlayerControll2.Lasergunlaser.enabled = true;
        _MySelfPlayerControll2.Lasergunlaser.SetColors(_MySelfPlayerControll2.LasergunlaserNotaimingColor, _MySelfPlayerControll2.LasergunlaserNotaimingColor);
        _MySelfPlayerControll2.Ani_Attack1.Stop();
     //   PushBullet();
        _bIsReload = false;
        _MySelfPlayerControll2.LasergunlaserBulletMaxCDTime = MaxCDTime;
        _MySelfPlayerControll2.LasergunlaserBullettext.text = _iNowBullet.ToString();
    }

    public override void f_Execute()
    {
        if (_bShooting)
        {
            Ray landRay = new Ray(_MySelfPlayerControll2.m_BulletStart.transform.position, _MySelfPlayerControll2.m_BulletStart.transform.forward * 1000);
            RaycastHit hit;
            if (Physics.Raycast(landRay, out hit, 1000, _MySelfPlayerControll2.LM.value))
            {
                _MySelfPlayerControll2.Lasergunlaser.SetPosition(1, new Vector3(0, 0, hit.point.z + 50));
                if (hit.collider.GetComponent<NeedlegunTrack>() != null)
                {
                    if (_MySelfPlayerControll2.LasergunlaserBulletbool == false) return;
                    _MySelfPlayerControll2.Lasergunlaser.enabled = true;
                    _MySelfPlayerControll2.Lasergunlaser.SetColors(_MySelfPlayerControll2.LasergunlaserTargetedColor, _MySelfPlayerControll2.LasergunlaserTargetedColor);
                    needlegunTrack = hit.collider.GetComponent<NeedlegunTrack>();//
                    TracksStart();
                    if (trackEM == TrackEM.Trackcomplete)
                    {
                        if (needlegunTrack.aimscategory == Aimscategory.Trex)
                        {
                            //var N = glo_Main.GetInstance().m_ResourceManager.f_NeedlegunBullet();
                            //N.transform.position = _MySelfPlayerControll2.m_BulletStart.transform.position;
                            //N.transform.rotation = _MySelfPlayerControll2.m_BulletStart.transform.rotation;
                            //N.GetComponent<Needlegun>().Target = needlegunTrack.AttackSite.gameObject;
                            //N.GetComponent<Needlegun>().f_Fired(GameEM.TeamType.A, 20, 2, 50);

                            //needlegunTrack.AttackSite.gameObject.transform.position

                            //RoleArrowAttackAction tRoleArrowAttackAction = new RoleArrowAttackAction();
                            //tRoleArrowAttackAction.f_Attack2(_MySelfPlayerControll2.m_iId, GameEM.GunEM.Lasergun, _MySelfPlayerControll2.f_GetTeamType(), 0, _MySelfPlayerControll2.m_BulletStart.transform.position,, 0, 2);
                            //_MySelfPlayerControll2.f_AddMyAction(tRoleArrowAttackAction);


                            //RoleArrowAttackAction tRoleArrowAttackAction = new RoleArrowAttackAction();
                            //tRoleArrowAttackAction.f_Attack(ccMath.f_CreateKeyId(), _MySelfPlayerControll2.m_iId, _MySelfPlayerControll2.f_GetCurBulletDT().iId, _MySelfPlayerControll2.f_GetTeamType(), 2000, 
                            //    _MySelfPlayerControll2.m_BulletStart.transform.position, _MySelfPlayerControll2.m_BulletStart.transform.rotation, 
                            //    _MySelfPlayerControll2.detectionAims.target._iTargetIndex);
                            //_MySelfPlayerControll2.f_AddMyAction(tRoleArrowAttackAction);

                            TrackStop();
                            trackEM = TrackEM.NotTrack;
                            _iNowBullet -= 1;
                            _MySelfPlayerControll2.LasergunlaserBulletImage.fillAmount = (float)_iNowBullet / _iMaxNowBullet;
                            _MySelfPlayerControll2.LasergunlaserBullettext.text = _iNowBullet.ToString();
                            if (_iNowBullet<=0)
                            {
                                _MySelfPlayerControll2.LasergunlaserBulletNowCDTime = 0;
                               // PushBullet();
                                _MySelfPlayerControll2.LasergunlaserBulletbool = false;
                            }
                            if (_iNowBullet > 0)
                            {
                                _MySelfPlayerControll2.LasergunlaserBulletbool = true;
                            }
                        }

                    }
                }
                else
                {
                    if (needlegunTrack != null)
                    {
                        //   _MySelfPlayerControll2.ComboQuantity = 0;
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
                _MySelfPlayerControll2.Lasergunlaser.SetColors(_MySelfPlayerControll2.LasergunlaserNotaimingColor, _MySelfPlayerControll2.LasergunlaserNotaimingColor);
            }
        }
        else
        {
            _MySelfPlayerControll2.Lasergunlaser.SetColors(_MySelfPlayerControll2.LasergunlaserNotaimingColor, _MySelfPlayerControll2.LasergunlaserNotaimingColor);
            // _MySelfPlayerControll2.Lasergunlaser.enabled = false;
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

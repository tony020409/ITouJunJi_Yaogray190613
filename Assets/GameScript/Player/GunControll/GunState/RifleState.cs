   
using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sam;
using ccVerifySDK;

/// <summary>
/// 步槍模式
/// </summary>
public class RifleState : GunBaseState
{

    private bool _bIsReload;
    private float PushBulletSoundEffectTimee;
    private MySelfPlayerControll2 _MySelfPlayerControll2;
    private RoleArrowAttackAction tRoleArrowAttackAction = new RoleArrowAttackAction();


    public RifleState (MySelfPlayerControll2 tMySelfPlayerControll2) 
        : base((int)GunEM.Rifle) {
        _MySelfPlayerControll2 = tMySelfPlayerControll2;
        if (_GunDT == null){
            _GunDT = (GunDT)glo_Main.GetInstance().m_SC_Pool.m_GunSC.f_GetSC((int)GunEM.Rifle);
        }
        f_Init(tMySelfPlayerControll2, (int)GunEM.Rifle);
    }


    public override void f_Enter(object Obj){
        _MySelfPlayerControll2.GunEM = GunEM.Rifle;                //換成步槍
        _MySelfPlayerControll2.REM = RifleEM.single;               //換成單發             
        _MySelfPlayerControll2.Bullettext.text = _iNowBullet + ""; //UI更新為對應槍枝的資訊 (子彈數)
        _MySelfPlayerControll2.ClipText.text = _iClipNum + "";     //UI更新為對應槍枝的資訊 (彈夾數)
        _MySelfPlayerControll2.RifledGunObj.SetActive(true);       //開啟步槍模型
    }



    public override void f_Execute()  {
        PushBulletSoundEffectTimee += Time.deltaTime;
        if (_bShooting)  {
            if (_MySelfPlayerControll2.REM == RifleEM.single) {
                if (_MySelfPlayerControll2.RifleEMbool == true) {
                    Shoot();
                }
            }
        }

        f_CheckReLoad();
        f_CheckVerifyGameTime();
    }


    /// <summary>
    /// 射擊
    /// </summary>
    private void Shoot() {
        if (_iNowBullet > 0) {
            tRoleArrowAttackAction.f_Attack(ccMath.f_CreateKeyId(), 
                _MySelfPlayerControll2.m_iId, 
                1, 
                _MySelfPlayerControll2.f_GetTeamType(), 
                _MySelfPlayerControll2.m_BulletStart.transform.position, 
                _MySelfPlayerControll2.m_BulletStart.transform.rotation,
                GunEM.Rifle, 
                1);
            _MySelfPlayerControll2.f_AddMyAction(tRoleArrowAttackAction);

            //遊戲開始後才會扣子彈
            if (glo_Main.GetInstance().m_EM_GameStatic != EM_GameStatic.Waiting) {
                _iNowBullet -= 1;
                _MySelfPlayerControll2.Bullettext.text = _iNowBullet + "";
                _MySelfPlayerControll2.ClipText.text = _iClipNum + "";
                _MySelfPlayerControll2.BulletImage.fillAmount = (float)_iNowBullet / _iMaxNowBullet;
            }

            PlayerEffect.SoundEffect PlayerSoundEffect = new PlayerEffect.SoundEffect(SoundEM.Attack, ref _MySelfPlayerControll2.m_AudioSource);
            _MySelfPlayerControll2.Riflebool = true;
            _MySelfPlayerControll2._vp_MuzzleFlash.Shoot();
            _MySelfPlayerControll2.Rifleeffect.gameObject.SetActive(true);
            _MySelfPlayerControll2.Ani_Attack1.Play(1);
            _MySelfPlayerControll2.RifleEMbool = false;
            _MySelfPlayerControll2.Rifle_animator.Play("Shoot", -1, 0f);
            CreateBullets();

        }


        if (_iNowBullet <= 0) {
            f_StopShoot();
            _bIsReload = true;
            _MySelfPlayerControll2.Riflebool = false;
            _MySelfPlayerControll2.m_AudioSource.PlayOneShot(_MySelfPlayerControll2.HG_EmptySound, 1);
        }
    }



    /// <summary>
    /// 停火
    /// </summary>
    public override void f_StopShoot() {
        base.f_StopShoot();
        _MySelfPlayerControll2.Ani_Attack1.Stop();
        _MySelfPlayerControll2.Rifleeffect.gameObject.SetActive(false);
        tRoleArrowAttackAction.f_Attack(ccMath.f_CreateKeyId(),
            _MySelfPlayerControll2.m_iId,
            1,
            _MySelfPlayerControll2.f_GetTeamType(),
            _MySelfPlayerControll2.m_BulletStart.transform.position,
            _MySelfPlayerControll2.m_BulletStart.transform.rotation,
            GunEM.Rifle,
            0);
        _MySelfPlayerControll2.f_AddMyAction(tRoleArrowAttackAction);
    }


    /// <summary>
    /// 噴彈殼
    /// </summary>
    public void CreateBullets(){
        GameObject _pistolShell = MonoBehaviour.Instantiate(_MySelfPlayerControll2.pistolShell, _MySelfPlayerControll2.pistolShellPoint.transform.position, _MySelfPlayerControll2.pistolShell.transform.rotation) as GameObject;
        MonoBehaviour.Destroy(_pistolShell, 2.0f);
    }



    /// <summary>
    /// 檢查是否進行換子彈的動作
    /// </summary>
    public void f_CheckReLoad() {
        if (_MySelfPlayerControll2.m_oRightHand.transform.eulerAngles.x > 330 &&
            _MySelfPlayerControll2.m_oRightHand.transform.eulerAngles.x < 350 &&
            _iNowBullet != _iMaxNowBullet &&
            _iNowBullet == 0 &&
            _iClipNum > 0)
        {
            PushBullet();
            f_LostClip(1);
            _MySelfPlayerControll2.BulletImage.fillAmount = (float)_iNowBullet / _iMaxNowBullet;
            _MySelfPlayerControll2.Bullettext.text = _iNowBullet + "";
            _MySelfPlayerControll2.ClipText.text = _iClipNum + "";
            _bIsReload = false;
            _MySelfPlayerControll2.Riflebool = true;
            if (PushBulletSoundEffectTimee >= 1)  {
                PlayerEffect.SoundEffect PlayerSoundEffect = new PlayerEffect.SoundEffect(SoundEM.Changeclip, ref _MySelfPlayerControll2.m_AudioSource);
                PushBulletSoundEffectTimee = 0;
            }
        }
    }

   

    /// <summary>
    /// 驗證：檢查是否達到計次的條件 (打10顆子彈或打完一顆彈夾)
    /// </summary>
    public void f_CheckVerifyGameTime() {
        if (_iNowBullet == 0 || _iNowBullet <= _iMaxNowBullet - 10) {
            if (_MySelfPlayerControll2.Verify == false && glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
                //ccVerifyMain2.GetInstance().f_Submit_GameTime();                                                       //舊驗證2
                //ccVerityService.GetInstance().f_SubmitData(eVerifyDataType.eGameTime, System.DateTime.Now.ToString()); //舊驗證1
                if (BattleMain.GetInstance()._bGameSocketEor == true) {
                    MessageBox.ASSERT("Game Socket Ero, SubmitData Fail");
                }
                else {
                    VerityTools.f_SubmitData();
                    MessageBox.DEBUG("遊戲計次");
                }

                _MySelfPlayerControll2.Verify = true;

            }
        }
    }




    public override void f_Exit() {
        f_StopShoot();
        _MySelfPlayerControll2.RifledGunObj.SetActive(false);
    }



}

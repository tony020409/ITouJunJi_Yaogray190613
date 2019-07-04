using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sam;
using ccVerifySDK;
public class SubmachineState : GunBaseState
{
    private bool _bIsReload;
    private float PushBulletSoundEffectTimee;
    public CreateGunBullets createGunBullets;
    private MySelfPlayerControll2 _MySelfPlayerControll2;
    private RoleArrowAttackAction tRoleArrowAttackAction = new RoleArrowAttackAction();

    public SubmachineState(MySelfPlayerControll2 tMySelfPlayerControll2)
        : base((int)GunEM.Submachine) {
        _MySelfPlayerControll2 = tMySelfPlayerControll2;
        if (_GunDT == null) {
            _GunDT = (GunDT)glo_Main.GetInstance().m_SC_Pool.m_GunSC.f_GetSC((int)GunEM.Submachine);
        }
        f_Init(tMySelfPlayerControll2, (int)GunEM.Submachine);
    }


    public override void f_Enter(object Obj){
        _MySelfPlayerControll2.GunEM = GunEM.Submachine;           //換成衝鋒槍
        _MySelfPlayerControll2.REM = RifleEM.continuous;           //換成連發
        _MySelfPlayerControll2.Bullettext.text = _iNowBullet + ""; // UI更新為對應槍枝的資訊 (子彈數)
        _MySelfPlayerControll2.ClipText.text = _iClipNum + "";     // UI更新為對應槍枝的資訊 (彈夾數)
        _MySelfPlayerControll2.SubmachineObj.SetActive(true);      //開啟衝鋒槍模型
    }
    

    public override void f_Execute() {
        PushBulletSoundEffectTimee += Time.deltaTime;
        if (_bShooting) {
            if (_MySelfPlayerControll2.REM == RifleEM.continuous) {
                if (_fShootTime < 0) {
                    Shoot();
                    _fShootTime = _GunDT.fSpeed;
                }
            }
            _fShootTime = _fShootTime - Time.deltaTime;
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
                8, 
                _MySelfPlayerControll2.f_GetTeamType(), 
                _MySelfPlayerControll2.SubmachineGun_fire_ins.transform.position,
                _MySelfPlayerControll2.SubmachineGun_fire_ins.transform.rotation,
                GunEM.Submachine, 
                1);
            _MySelfPlayerControll2.f_AddMyAction(tRoleArrowAttackAction);
            _iNowBullet -= 1;                                                                    //子彈-1
            _MySelfPlayerControll2.ClipText.text = _iClipNum + "";                               //刷新彈夾UI
            _MySelfPlayerControll2.Bullettext.text = _iNowBullet + "";                           //刷新子彈UI (文字)
            _MySelfPlayerControll2.BulletImage.fillAmount = (float)_iNowBullet / _iMaxNowBullet; //刷新子彈UI (跑條)
            PlayerEffect.SoundEffect PlayerSoundEffect = new PlayerEffect.SoundEffect(SoundEM.Attack, ref _MySelfPlayerControll2.m_AudioSource);
            _MySelfPlayerControll2.Riflebool = true;
            _MySelfPlayerControll2._vp_MuzzleFlash.Shoot();
            _MySelfPlayerControll2.SubmachineGun_effect.gameObject.SetActive(true);
            _MySelfPlayerControll2.Ani_Attack1.Play();
            _MySelfPlayerControll2.RifleEMbool = false;
            CreateBullets();
        }

        if (_iNowBullet <= 0) {
            f_StopShoot();
            _bIsReload = true;
            _MySelfPlayerControll2.Riflebool = false;
            _MySelfPlayerControll2.m_AudioSource.PlayOneShot(_MySelfPlayerControll2.SMG_EmptySound, 1);
        }
    }


    /// <summary>
    /// 停火
    /// </summary>
    public override void f_StopShoot(){
        base.f_StopShoot();
        _MySelfPlayerControll2.Ani_Attack1.Stop();
        _MySelfPlayerControll2.SubmachineGun_effect.gameObject.SetActive(false);
        tRoleArrowAttackAction.f_Attack(ccMath.f_CreateKeyId(),
            _MySelfPlayerControll2.m_iId,
            8,
            _MySelfPlayerControll2.f_GetTeamType(),
            _MySelfPlayerControll2.SubmachineGun_fire_ins.transform.position,
            _MySelfPlayerControll2.SubmachineGun_fire_ins.transform.rotation,
            GunEM.Submachine,
            0);
        _MySelfPlayerControll2.f_AddMyAction(tRoleArrowAttackAction);
    }

    /// <summary>
    /// 噴彈殼
    /// </summary>
    public void CreateBullets(){
        GameObject _pistolShell = MonoBehaviour.Instantiate(_MySelfPlayerControll2.pistolShell, _MySelfPlayerControll2.SubmachineShellPoint.transform.position, _MySelfPlayerControll2.SubmachineShellPoint.transform.rotation) as GameObject;
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
    public void f_CheckVerifyGameTime(){
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
        _MySelfPlayerControll2.SubmachineObj.SetActive(false); //關閉衝鋒槍模型
    }


}

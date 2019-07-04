using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sam;
using ccVerifySDK;
public class SniperState : GunBaseState
{
    private bool _bIsReload;
    private MySelfPlayerControll2 _MySelfPlayerControll2;
    private float PushBulletSoundEffectTimee;
    private RoleArrowAttackAction tRoleArrowAttackAction = new RoleArrowAttackAction();


    public SniperState(MySelfPlayerControll2 tMySelfPlayerControll2)
        : base((int)GunEM.Sniper)
    {
        _MySelfPlayerControll2 = tMySelfPlayerControll2;

        if (_GunDT == null)
        {
            _GunDT = (GunDT)glo_Main.GetInstance().m_SC_Pool.m_GunSC.f_GetSC((int)GunEM.Sniper);
        }
        f_Init(tMySelfPlayerControll2, (int)GunEM.Sniper);

    }
    public override void f_Enter(object Obj){
        //_MySelfPlayerControll2.trackImageGameObject.gameObject.SetActive(false);
        //_MySelfPlayerControll2.Lasergunlaser.enabled = false;
        //_MySelfPlayerControll2.NeedlegunAudioSource.enabled = false;
        _MySelfPlayerControll2.GunEM = GunEM.Sniper;
        _MySelfPlayerControll2.REM = RifleEM.single;
        _MySelfPlayerControll2.Bullettext.text = _iNowBullet + "";
        _MySelfPlayerControll2.ClipText.text = _iClipNum + "";
        _MySelfPlayerControll2.SniperObj.SetActive(true);
    }

    public override void f_Execute() {
        PushBulletSoundEffectTimee += Time.deltaTime;
        if (_bShooting) {
            if (_MySelfPlayerControll2.REM == RifleEM.single) {
                if (_MySelfPlayerControll2.RifleEMbool == true) {
                    Shoot();
                }
                else {
                    _MySelfPlayerControll2.Snipereffect.gameObject.SetActive(false);
                    _MySelfPlayerControll2.Ani_Attack1.Stop();
                }
            }

            if (_MySelfPlayerControll2.REM == RifleEM.continuous) {
                if (_fShootTime < 0) {
                    Shoot();
                    _fShootTime = _GunDT.fSpeed;
                }
            }
            _fShootTime = _fShootTime - Time.deltaTime;
        }
        else{
            _MySelfPlayerControll2.Snipereffect.gameObject.SetActive(false);
            _MySelfPlayerControll2.Ani_Attack1.Stop();
        }


        f_CheckReLoad();
        f_CheckVerifyGameTime();
    }


    /// <summary>
    /// 射擊 
    /// </summary>
    private void Shoot(){
        if (_iNowBullet > 0){
            tRoleArrowAttackAction.f_Attack(ccMath.f_CreateKeyId(), 
                _MySelfPlayerControll2.m_iId, 
                6, 
                _MySelfPlayerControll2.f_GetTeamType(), 
                _MySelfPlayerControll2.SniperGun_fire_ins.transform.position,
                _MySelfPlayerControll2.SniperGun_fire_ins.transform.rotation,
                GunEM.Sniper,
                1);
            _MySelfPlayerControll2.f_AddMyAction(tRoleArrowAttackAction);
            _iNowBullet -= 1;
            _MySelfPlayerControll2.Bullettext.text = _iNowBullet + "";
            _MySelfPlayerControll2.ClipText.text = _iClipNum + "";
            _MySelfPlayerControll2.BulletImage.fillAmount = (float)_iNowBullet / _iMaxNowBullet;
            PlayerEffect.SoundEffect PlayerSoundEffect = new PlayerEffect.SoundEffect(SoundEM.Attack, ref _MySelfPlayerControll2.m_AudioSource);
            _MySelfPlayerControll2.Riflebool = true;
            _MySelfPlayerControll2.Sniper_vp_MuzzleFlash.Shoot();
            _MySelfPlayerControll2.Snipereffect.gameObject.SetActive(true);
            _MySelfPlayerControll2.Ani_Attack1.Play();
            _MySelfPlayerControll2.SniperGun_animator.Play("Shoot");
        }

        if (_iNowBullet <= 0) {
            tRoleArrowAttackAction.f_Attack(ccMath.f_CreateKeyId(), 
                _MySelfPlayerControll2.m_iId, 
                6, 
                _MySelfPlayerControll2.f_GetTeamType(), 
                _MySelfPlayerControll2.SniperGun_fire_ins.transform.position,
                _MySelfPlayerControll2.SniperGun_fire_ins.transform.rotation,
                GunEM.Sniper,
                0);
            _MySelfPlayerControll2.f_AddMyAction(tRoleArrowAttackAction);
            _bIsReload = true;
            _MySelfPlayerControll2.Riflebool = false;
            _MySelfPlayerControll2.Snipereffect.gameObject.SetActive(false);
            _MySelfPlayerControll2.Ani_Attack1.Stop();
        }
    }





    /// <summary>
    /// 檢查是否進行換子彈的動作
    /// </summary>
    public void f_CheckReLoad(){
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
    /// 驗證：檢查是否達到計次的條件 (打5顆子彈或打完一顆彈夾)
    /// </summary>
    public void f_CheckVerifyGameTime() {
        if (_iNowBullet == 0 || _iNowBullet <= _iMaxNowBullet - 5) {
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







    public override void f_Exit(){
        _MySelfPlayerControll2.Ani_Attack1.Stop();
        _MySelfPlayerControll2.SniperObj.SetActive(false);
        _MySelfPlayerControll2.Snipereffect.gameObject.SetActive(false);
    }



}

using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //使用Enum.Parse用

public class GunControll
{
    private ccMachineManager _GunControllMachineManager = null;
    public GunEM Gem;
    public GunBaseState _GunBaseState;


    public GunControll (MySelfPlayerControll2 tMySelfPlayerControll2)  {
        _GunControllMachineManager = new ccMachineManager(new RifleState(tMySelfPlayerControll2), true);
        _GunControllMachineManager.f_RegState(new BombState(tMySelfPlayerControll2));
        //_GunControllMachineManager.f_RegState(new NeedlegunState(tMySelfPlayerControll2));
        //_GunControllMachineManager.f_RegState(new LasergunState(tMySelfPlayerControll2));
        _GunControllMachineManager.f_RegState(new SniperState(tMySelfPlayerControll2));
        _GunControllMachineManager.f_RegState(new SubmachineState(tMySelfPlayerControll2));
        //_GunControllMachineManager.f_RegState(new WandState(tMySelfPlayerControll2));
        //_GunControllMachineManager.f_RegState(new ShieldState(tMySelfPlayerControll2));
        _GunControllMachineManager.f_ChangeState((int)GunEM.Rifle);
    }


    public void f_Update()  {
        _GunControllMachineManager.f_Update();
    }


    #region  外部接口

    /// <summary>
    /// 开始射击
    /// </summary>
    public void f_StartShoot() {
        GunBaseState tGunBaseState = (GunBaseState)_GunControllMachineManager.f_GetCurMachineState();
        _GunBaseState = tGunBaseState;
        tGunBaseState.f_StartShoot();
    }

    /// <summary>
    /// 停止射击
    /// </summary>
    public void f_StopShoot() {
        GunBaseState tGunBaseState = (GunBaseState)_GunControllMachineManager.f_GetCurMachineState();
        tGunBaseState.f_StopShoot();
    }

    /// <summary>
    /// 装弹
    /// </summary>
    public void f_PushBullet() {
        //MessageBox.DEBUG("======補子彈===========");
        GunBaseState tGunBaseState = (GunBaseState)_GunControllMachineManager.f_GetCurMachineState();
        tGunBaseState.PushBullet();
    }

    /// <summary>
    /// 取得子弹数量
    /// </summary>
    public string f_GetNowBullet()  {
        GunBaseState tGunBaseState = (GunBaseState)_GunControllMachineManager.f_GetCurMachineState();
       return  tGunBaseState.GetNowBullet();
    }


    public BulletDT f_GetCurBulletDT() {
        GunBaseState tGunBaseState = (GunBaseState)_GunControllMachineManager.f_GetCurMachineState();
        return tGunBaseState.f_GetCurBulletDT();
    }


    /// <summary>
    /// 取得彈夾數量
    /// </summary>
    public int f_GetClip() {
        GunBaseState tGunBaseState = (GunBaseState)_GunControllMachineManager.f_GetCurMachineState();
        return tGunBaseState.f_GetClip();
    }


    /// <summary>
    /// 增加彈夾數量
    /// </summary>
    public void f_AddClip(int value){
        GunBaseState tGunBaseState = (GunBaseState)_GunControllMachineManager.f_GetCurMachineState();
        tGunBaseState.f_AddClip(value);
    }


    /// <summary>
    /// 换枪接口1
    /// 要切換的槍枝記得到上方去註冊 XXXState
    /// </summary>
    /// <param name="tGunEM"></param>
    public bool f_ChangeGun(GunEM tGunEM) {
        ccMachineStateBase tGunBaseState = _GunControllMachineManager.f_GetCurMachineState();
        if (tGunBaseState.iId == ((int)tGunEM)) {
            return false;
        }
        _GunControllMachineManager.f_ChangeState((int)tGunEM);
        Gem = tGunEM;
        return true;
    }

    /// <summary>
    /// 换枪接口2 (後方參數使用 string接收)
    /// </summary>
    /// <param name="tGunEM"> </param>
    public void f_ChangeGun2(string tmp)  {
        GunEM tGunEM = (GunEM)Enum.Parse(typeof(GunEM), tmp, false); //第三個參數是是否在意大小寫
        _GunControllMachineManager.f_ChangeState((int)tGunEM);
    }
    


    #endregion


}
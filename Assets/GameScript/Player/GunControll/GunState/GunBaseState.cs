using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBaseState : ccMachineStateBase
{
    protected GunDT _GunDT;
    protected BulletDT _BulletDT;

    /// <summary>
    /// 当前弹夹里的子弹数量
    /// </summary>
    protected int _iNowBullet;


    protected int _iMaxNowBullet;
    protected float _fShootTime;

    /// <summary>
    /// 保存当前枪拥有的弹夹数量
    /// </summary>
    protected int _iClipNum;

    /// <summary>
    /// 开始射击 true射击中
    /// </summary>
    protected bool _bShooting = false;

    /// <summary>
    /// 开始射击 true射击中
    /// </summary>
    protected bool _bPushBullet = false;

    protected BaseRoleControllV2 m_BaseRoleControllV2;

    public GunBaseState(int tiId)
        : base(tiId)
    {
    }

    public void f_Init(BaseRoleControllV2 tBaseRoleControllV2, int iGunId)
    {
        m_BaseRoleControllV2 = tBaseRoleControllV2;

        _GunDT = (GunDT)glo_Main.GetInstance().m_SC_Pool.m_GunSC.f_GetSC(iGunId);
        if (_GunDT == null)
        {
            MessageBox.ASSERT("GunDT脚本未找到" + iGunId);
        }
        _BulletDT = (BulletDT)glo_Main.GetInstance().m_SC_Pool.m_BulletSC.f_GetSC(_GunDT.iBulletId);
        if (_BulletDT == null)
        {
            MessageBox.ASSERT("BulletDT脚本未找到" + _GunDT.iBulletId);
        }
        _iNowBullet = _GunDT.iMaxBullet;
        _iMaxNowBullet = _GunDT.iMaxBullet;
        _iClipNum = _GunDT.iClipNum;
    }

    /// <summary>
    /// 初始装弹
    /// </summary>
    public void PushBullet()
    {
        if (_iClipNum <= 0)
        {
            //MessageBox.ASSERT("弹夹用光不能再进行换单 " + _iId + ",弹夹数量:" + _iClipNum);
            return;
        }
        _iNowBullet = _GunDT.iMaxBullet;
        _iMaxNowBullet= _GunDT.iMaxBullet;
        _fShootTime = _GunDT.fSpeed;
        _bPushBullet = true;

        //_iClipNum--;
        MessageBox.DEBUG("PushBullet" + _iId);
    }

    protected void Empty()
    {
        _bPushBullet = false;
        _iNowBullet = 0;
    }

    protected bool IsHaveBullet()
    {
        if (_iNowBullet > 0)
        {
            return true;
        }
        return false;
    }

    public string GetNowBullet()
    {
        return _iNowBullet.ToString();
    }

    public BulletDT f_GetCurBulletDT()
    {
        return _BulletDT;
    }

    /// <summary>
    /// 开始射击
    /// </summary>
    public virtual void f_StartShoot()
    {
        _bShooting = true;
        //MessageBox.DEBUG("f_StartShoot" + this.ToString());
    }

    /// <summary>
    /// 停止射击
    /// </summary>
    public virtual void f_StopShoot()
    {
        _bShooting = false;
        //MessageBox.DEBUG("f_StopShoot" + this.ToString());
    }

    /// <summary>
    /// 射击动作
    /// </summary>
    /// <returns></returns>
    public BulletDT f_Shoot()
    {
        if (!_bShooting)
        {
            return null;
        }
        return _BulletDT;
    }

    #region 弹夹相关

    public void f_LostClip(int iNum)
    {
        if (_iClipNum > 0)
        {
            _iClipNum = _iClipNum - iNum;
        }
    }

    public int f_GetClip()
    {
        return _iClipNum;
    }

    public void f_AddClip(int iNum)
    {
        _iClipNum = _iClipNum + iNum;
    }

    
    #endregion

}

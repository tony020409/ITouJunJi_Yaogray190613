using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseBullet : MonoBehaviour
{
    private int _iId = 0;
    protected BulletDT _BulletDT;
    protected int _iHp = 0;
    //判斷是否完成子彈數值的設定(?)
    protected bool _bDoing = false;

    //子彈數值項目
    protected GameEM.TeamType _emTeamType;
    protected float _fLiveTime = 0;
    protected int _iPlayerId = 0;

    public string resPath = "Model/Bullet/";


    /// <summary>
    /// 設定子彈數值
    /// </summary>
    /// <param name="tTeamType"   > 攻擊發動者的隊伍 </param>
    /// <param name="fSpeed"      > 子彈速度 </param>
    /// <param name="fLive"       > 子彈存活時間 </param>
    /// <param name="iPlayerId"   > 攻擊發動者id </param>
    /// <param name="iAttackPower"> 攻擊傷害數值 </param>
    public virtual void f_Fired(int iBulletId, int iBulletDT, GameEM.TeamType tTeamType, int iPlayerId)
    {
        _iId = iBulletId;
        _BulletDT = (BulletDT)glo_Main.GetInstance().m_SC_Pool.m_BulletSC.f_GetSC(iBulletDT);
        if (_BulletDT == null)
        {
            MessageBox.ASSERT("BulletDT脚本未找到" + iBulletId);
        }

        _emTeamType = tTeamType;
        _fLiveTime = _BulletDT.fBulletLife;
        _iPlayerId = iPlayerId;
        _iHp = _BulletDT.iHp;
        gameObject.name = _BulletDT.szBulletResName + _iId;
        f_TypeFired();
        _bDoing = true;
        BattleMain.GetInstance().m_BulletPool.f_Save(this);
    }


    /// <summary>
    /// (覆寫用) 給其他類型的子彈設定其他數值
    /// </summary>
    protected virtual void f_TypeFired(){

    }

    /// <summary>
    /// 子弹被攻击
    /// </summary>
    /// <param name="iHp"></param>
    public void f_BeAttack(int iHp)
    {
        _iHp -= iHp;
        if (_iHp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 子弹死亡
    /// </summary>
    protected virtual void Die()
    {
        _iHp = 0;
        DoDestory();
    }

    private bool IsDie()
    {
        if (_iHp <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 子弹与其它子弹发生碰撞检测
    /// </summary>
    /// <returns></returns>
    protected virtual bool f_CheckIsCollide()
    {
        BaseBullet tBaseBullet = BattleMain.GetInstance().m_BulletPool.f_CheckIsCollide(this);
        if (tBaseBullet != null)
        {//子弹被其它子弹击中
            tBaseBullet.f_BeAttack(f_GetAttackPower());
            f_BeAttack(tBaseBullet.f_GetAttackPower());
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测角色被子弹攻击，可以根据子弹的不行类型重载此方法
    /// </summary>
    /// <returns></returns>
    protected virtual bool f_CheckIsCollideRole()
    {
        if (StaticValue.m_bIsMaster)
        {
            BaseRoleControllV2 BeAttackRoleControl = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_emTeamType, transform.position, f_GetAttackRang());
            if (BeAttackRoleControl != null)
            {
                if (!GloData.glo_bCanShootMySelf)
                {
                    if (BeAttackRoleControl.f_GetTeamType() == _emTeamType)
                    {//射中自己人
                        return false;
                    }
                }
                //射中别人
                RoleHpAction tRoleHpAction = new RoleHpAction();
                tRoleHpAction.f_Hp(_iPlayerId, BeAttackRoleControl.m_iId, GloData.glo_iArrowAttackHp, transform.position);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleHpAction);

                //播放命中特效

                Die();
                return true;
            }
        }
        return false;
    }

    protected virtual void Update()
    {
        if (!_bDoing)
        {
            return;
        }
        if (IsDie())
        {
            return;
        }
        _fLiveTime -= Time.deltaTime;          //生命倒數
        //rayLength += Time.deltaTime * _BulletDT.fSpeed; //射線向前延伸
        //if (oBullet != null)
        //{
        //    oBullet.transform.localPosition += oBullet.transform.forward * _BulletDT.fSpeed * Time.deltaTime; //動畫子彈向前飛
        //}
        transform.localPosition += transform.forward * _BulletDT.fSpeed * Time.deltaTime; //動畫子彈向前飛

        //子彈壽命歸零時，銷毀子彈
        if (_fLiveTime < 0)
        {
            //rayLength = 0;
            Die();
        }
        else
        {
            if (f_CheckIsCollide())
            {
                return;
            }
            if (f_CheckIsCollideRole())
            {
                return;
            }
        }
    }


    public virtual void OnTriggerEnter(Collider tmp) {
        Die();
    }



    /// <summary>
    /// 射中敵人 或子彈壽命歸零時的銷毀 (有射中敵人的話產生特效) 
    /// 子弹销毁
    /// </summary>
    /// <param name="BeAttackRoleControl"> </param>
    public virtual void DoDestory()
    {
        //// 射中敵人，產生特效
        //if (BeAttackRoleControl != null)
        //{
        //    //目前由 Test_VR_Shoot.cs 產生
        //}
        //if (oBullet != null)
        //{
        //    glo_Main.GetInstance().m_ResourceManager.f_DestorySD(oBullet);
        //}
        _bDoing = false;
        glo_Main.GetInstance().m_ResourceManager.f_DestorySD(gameObject);
        BattleMain.GetInstance().m_BulletPool.f_Del(this);
    }

    /// <summary>
    /// 获取子弹的有效中心坐标
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 f_GetPosition()
    {
        return transform.position;
    }

    /// <summary>
    /// 获取子弹的杀伤范围，或者是子弹的大小
    /// </summary>
    /// <returns></returns>
    public float f_GetAttackRang()
    {
        return _BulletDT.fAttackRange;
    }

    /// <summary>
    /// 获取子弹的杀伤力
    /// </summary>
    /// <returns></returns>
    public int f_GetAttackPower()
    {
        return _BulletDT.iPower;
    }

    public int f_GetId()
    {
        return _iId;
    }





}
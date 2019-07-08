using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 枪支状态通知
/// </summary>
[ProtoContract]
public class RoleWeaponStateAction : GameSysc.Action
{  
    [ProtoMember(27010)]
    public int m_iBulletId;

    /// <summary>
    /// 预留参数2
    /// </summary>
    [ProtoMember(27011)]
    public int m_iGun;

    /// <summary>
    /// 预留参数1
    /// </summary>
    [ProtoMember(27012)]
    public int m_iData1;

    /// <summary>
    /// 预留参数2
    /// </summary>
    [ProtoMember(27013)]
    public int m_iData2;
    

    public RoleWeaponStateAction() : base() {
        m_iType = (int)GameEM.EM_RoleAction.ArrowAttack;
    }
    
    public void f_Save(int iRoleId, GunEM tGunEM, int iBulletId, int iData1, int iData2)
    {
        m_iRoleId = iRoleId;
        m_iBulletId = iBulletId;
        m_iGun = (int)tGunEM;
        iData1 = m_iData1;
        iData2 = m_iData2;
    }

    public override void ProcessAction()
    {
        BaseRoleControllV2 tBaseRoleControllV2 = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);

        if (tBaseRoleControllV2 != null)
        {

        }
        else
        {
            MessageBox.ASSERT("RoleWeaponStateAction 未找到目标 " + m_iRoleId);
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;


//記得到Action.cs加
[ProtoContract]
public class RoleSpringAction : GameSysc.Action
{

    [ProtoMember(20101)]
    public float m_fPower;

    [ProtoMember(20102)]
    public float m_fHeight;

    [ProtoMember(20103)]
    public int m_iRoleId;

    public RoleSpringAction(): base() {
        m_iType = (int)GameEM.EM_RoleAction.Spring;
    }


    public void f_Send2OtherPlayer(int iRoleId, float fPower, float fHeight)
    {
        m_iRoleId = iRoleId;
        m_fPower = fPower;
        m_fHeight = fHeight;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl != null)
        {
            tRoleControl.f_Spring(m_fPower, m_fHeight);
        }
        else
        {
            MessageBox.ASSERT("Hp 未找到目标 " + m_iRoleId);
        }
    }


}

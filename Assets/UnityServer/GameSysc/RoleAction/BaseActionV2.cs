using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using System;

[ProtoContract]
public abstract class BaseActionV2 : GameSysc.Action
{    

    [ProtoMember(11)]
    public int m_iStateId;

    [ProtoMember(12)]
    public int m_iRoleId2;

    [ProtoMember(13)]
    public int m_iContidionAIId;

    [ProtoMember(14)]
    public int m_iRunAIId;


    public BaseActionV2(GameEM.EM_RoleAction tRoleAction) : base()
    {
        m_iType = (short)tRoleAction;
    }

    public void f_SaveStateId(int iId)
    {
        m_iStateId = iId;
    }

    public override void ProcessAction()
    {
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_iRoleId);
        if (tRoleControl != null)
        {
            tRoleControl.f_ProcessAction(this);
        }
        else
        {
            MessageBox.ASSERT("未找到Action对应的角色 " + m_iRoleId + " " + m_iType);
        }

    }


}

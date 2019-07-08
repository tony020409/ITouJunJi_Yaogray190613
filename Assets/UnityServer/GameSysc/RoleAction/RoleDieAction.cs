using ProtoBuf;
using System.Collections;
using System.Collections.Generic;

[ProtoContract]
public class RoleDieAction : GameSysc.Action
{
    [ProtoMember(10301)]
    public int m_iRoleId;

    [ProtoMember(10302)]
    public int m_iTeamType;

    public RoleDieAction()
            : base()
    {
        m_iType = (int)GameEM.EM_RoleAction.Die;
    }

    public void f_Die(int iRoleId, GameEM.TeamType tTeamType)
    {
        m_iRoleId = iRoleId;
        m_iTeamType = (int)tTeamType;
        MessageBox.DEBUG("f_Die " + iRoleId + " " + tTeamType.ToString());
    }

    public override void ProcessAction()
    {
        MessageBox.DEBUG("Die " + m_iRoleId);
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl != null)
        {
            MessageBox.DEBUG("Die " + m_iRoleId);
            tRoleControl.f_RunAIState(AI_EM.EM_AIState.Die, null);
        }
        else
        {
            MessageBox.ASSERT("Die 未找到目标 " + m_iRoleId);
        }
    }


}

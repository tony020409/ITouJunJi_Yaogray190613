using ProtoBuf;
using System.Collections;
using System.Collections.Generic;

[ProtoContract]
public class RoleWalkAction :BaseActionV2
{
    [ProtoMember(10502)]
    public int m_iTeamType;

    [ProtoMember(10503)]
    public int[] m_aPath;
    
    public RoleWalkAction(): base(GameEM.EM_RoleAction.Walk)
    {
    }
    
    public void f_Walk(int iId, GameEM.TeamType tTeamType, int[] aPath)
    {
        m_iRoleId = iId;
        m_iTeamType = (int)tTeamType;
        m_aPath = aPath;
        //MessageBox.DEBUG("f_Walk " + iId + " " + iUserId);
    }

    public override void ProcessAction()
    {
        //MessageBox.DEBUG("Walk " + m_iRoleId);
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl != null)
        {
            MessageBox.DEBUG("Walk " + m_iRoleId + ">>" + m_aPath.Length);
            tRoleControl.f_RunAIState(AI_EM.EM_AIState.Walk, this);
        }
        else
        {
            MessageBox.ASSERT("Walk 未找到目标 " + m_iRoleId);
        }
    }


}


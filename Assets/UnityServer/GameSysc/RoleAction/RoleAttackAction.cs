using ProtoBuf;
using System.Collections;
using System.Collections.Generic;

[ProtoContract]
public class RoleAttackAction : BaseActionV2
{

    [ProtoMember(10202)]
    public int m_iTeamType;

    [ProtoMember(10203)]
    public int m_iBeAttackRoleId;


    public RoleAttackAction()
            : base(GameEM.EM_RoleAction.Attack)
        {
    }


    public void f_Attack(int iRoleId, GameEM.TeamType tTeamType, int iBeAttackRoleId)
    {
        m_iRoleId = iRoleId;
        m_iTeamType = (int)tTeamType;
        m_iBeAttackRoleId = iBeAttackRoleId;
        //MessageBox.DEBUG("f_Attack " + iRoleId + " " + iUserId + " " + iBeAttackRoleId);
    }


    public override void ProcessAction()
    {
        //MessageBox.DEBUG("ProcessAction Attack " + m_iRoleId);
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl != null)
        {
            //MessageBox.DEBUG("ProcessAction Attack " + m_iRoleId + ">>" + m_iBeAttackRoleId);
            tRoleControl.f_RunAIState(AI_EM.EM_AIState.Attack, this);
        }
        else
        {
            MessageBox.ASSERT("ProcessAction Attack 未找到目标 " + m_iRoleId);
        }
    }


}
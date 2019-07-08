using ProtoBuf;
using System.Collections;
using System.Collections.Generic;

[ProtoContract]
public class RoleSpiralAction : GameSysc.Action
{
    [ProtoMember(14001)]
    public int m_iRoleId;

    [ProtoMember(14002)]
    public int m_iTeamType;

    [ProtoMember(14003)]
    public int m_iPathId;

    // =========================================================================
    public RoleSpiralAction() : base()
    { m_iType = (int)GameEM.EM_RoleAction.SpiralAction; }

    public void f_Spiral(int iId, GameEM.TeamType tTeamType, int iPathId)
    {
        m_iRoleId = iId;
        m_iTeamType = (int)tTeamType;
        m_iPathId = iPathId;
    }

    //切換到這個 Action 時，這個 ProcessAction 就會被執行一次() =================
    public override void ProcessAction()
    {
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl != null)
        {
            //tRoleControl.f_ChangeAI2Spiral(m_iPathId);
        }
        else
        {
            MessageBox.ASSERT("代號" + m_iRoleId + "未找到目标! (RoleSpiralAction.cs)");
        }
    }

}
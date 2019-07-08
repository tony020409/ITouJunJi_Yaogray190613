using ProtoBuf;
using System.Collections;
using System.Collections.Generic;

[ProtoContract]
public class RoleWalk2TargetAction : GameSysc.Action
{
    [ProtoMember(15001)]
    public int m_iRoleId;

    [ProtoMember(15002)]
    public int m_iTeamType;
    
    [ProtoMember(15003)]
    public float[] m_aPath;

    // =========================================================================
    public RoleWalk2TargetAction(): base()
    { m_iType = (int)GameEM.EM_RoleAction.Walk2Tartget; }

    public void f_Walk(int iId, GameEM.TeamType tTeamType, float[] aPath){
        m_iRoleId = iId;
        m_iTeamType = (int)tTeamType;
        m_aPath = aPath;
    }

    //切換到這個 Action 時，這個 ProcessAction 就會被執行一次() =================
    public override void ProcessAction(){
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);

        if (tRoleControl != null){
            //MessageBox.DEBUG("代號" + m_iRoleId + "Walk >>" + m_aPath.Length + " (RoleFlyAction.cs)");
            //tRoleControl.f_ChangeAI2Walk(m_aPath); //?? (會執行 AIState 是 Walk 的 AI_XX.cs 的樣子)
            //tRoleControl.f_ChangeAI2Target(m_aPath);    //?? (會執行 AIState 是 Fly 的 AI_XX.cs 的樣子)
        }
        else
        {
            MessageBox.ASSERT("代號" + m_iRoleId + "Walk 未找到目标! (RoleFlyAction.cs)");
        }
    }


}

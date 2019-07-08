using ProtoBuf;
using System.Collections;
using System.Collections.Generic;


[ProtoContract]
public class RolePathAction : GameSysc.Action
{

    [ProtoMember(17021)]
    public int m_iRoleId;

    [ProtoMember(17022)]
    public int m_iPathId;

    [ProtoMember(17023)]
    public int m_iEndAct;

    // =========================================================================
    public RolePathAction() : base(){
        m_iType = (int)GameEM.EM_RoleAction.Path;
    }

    /// <summary>
    /// 設定路徑
    /// </summary>
    /// <param name="iId"    > 執行對象 </param>
    /// <param name="iPathId"> 路徑編號 </param>
    /// <param name="iEndAct"> 結尾行動 </param>
    public void f_SetPath(int iId, int iPathId, int iEndAct){
        m_iRoleId = iId;
        m_iPathId = iPathId;
        m_iEndAct = iEndAct;
    }


    //切換到這個 Action 時，這個 ProcessAction 就會被執行一次() =================
    public override void ProcessAction(){
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl != null){
            //tRoleControl.f_ChangeAI2Spiral(m_iPathId);
        }
        else{
            MessageBox.ASSERT("代號" + m_iRoleId + "未找到目标! (RolePathAction.cs)");
        }
    }

}

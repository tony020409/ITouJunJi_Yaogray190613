using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//記得到 Action.cs 註冊

[ProtoContract]
public class Force_ServerActionState : GameSysc.Action
{
    [ProtoMember(13101)]
    public int m_iGameControllDTId;

    [ProtoMember(13102)]
    public int m_iConditionId;



    public Force_ServerActionState() : base() {
        m_iType = (int)GameEM.EM_RoleAction.ServerActionState;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="iConditionId"     > </param>
    /// <param name="iGameControllDTId"> </param>
    public void f_Save(int iConditionId, int iGameControllDTId) {
        m_iConditionId = iConditionId;
        m_iGameControllDTId = iGameControllDTId;
    }


    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
        Debug.LogWarning("<color=blue>【網路檢查】強制執行任務動作[" + m_iGameControllDTId + "] 成功 ============================= </color>");
        BattleMain.GetInstance().f_Force_RunServerActionState(m_iConditionId, m_iGameControllDTId);
    }




}

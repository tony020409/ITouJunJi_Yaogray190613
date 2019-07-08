using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ProtoContract]
public class ServerActionState : GameSysc.Action
{
    [ProtoMember(13001)]
    public int m_iGameControllDTId;

    [ProtoMember(13002)]
    public int m_iConditionId;



    public ServerActionState()
            : base()
    {
        m_iType = (int)GameEM.EM_RoleAction.ServerActionState;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="iConditionId"     ></param>
    /// <param name="iGameControllDTId"></param>
    public void f_Save(int iConditionId, int iGameControllDTId)
    {
        m_iConditionId = iConditionId;
        m_iGameControllDTId = iGameControllDTId;
    }


    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {
        if (BattleMain.GetInstance().Debug_GameControlRead) {
            Debug.LogWarning("<color=green>【網路檢查】任務動作[" + m_iGameControllDTId + "] 理應執行成功 ============================= </color>");
        }
        BattleMain.GetInstance().f_RunServerActionState(m_iConditionId, m_iGameControllDTId);
    }

    


}

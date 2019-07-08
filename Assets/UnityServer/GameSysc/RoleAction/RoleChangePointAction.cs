using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;

//記得到Action.cs加
[ProtoContract]
public class RoleChangePointAction : BaseActionV2 //GameSysc.Action
{
    [ProtoMember(28001)]
    public int m_ild;        //要變更AI的對象的ID

    [ProtoMember(28002)]
    public string m_AIState; //要變更的AI名稱

    [ProtoMember(28003)]
    public int m_Index;      // 變更陣列序號

    public RoleChangePointAction () : base(GameEM.EM_RoleAction.ChangePoint) {
    }

    public void f_ChangePoint (int newID, string newAI, int newIndex) {
        m_ild = newID;
        m_AIState = newAI;
        m_Index = newIndex;
    }

    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction () {
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().f_GetRoleControl2(m_ild);
        AI_EM.EM_AIState tGunEM = (AI_EM.EM_AIState)Enum.Parse(typeof(AI_EM.EM_AIState), m_AIState, true); //將string轉成Eunm (第三個參數表示是否在意大小寫)

        if (tmpRole == null) {
            MessageBox.ASSERT("找不到編號 " + m_ild + "的角色，它可能已經死了，所以無法讓它切換到 " + m_AIState + " 的AI，");
            return;
        }
        tmpRole.f_RunAIState(tGunEM, this);           //切換AI
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;

//記得到Action.cs加
[ProtoContract]
public class ChangeAIAction : BaseActionV2 //GameSysc.Action
{
    [ProtoMember(10858)]
    public int m_ild;        //要變更AI的對象的ID

    [ProtoMember(10859)]
    public string m_AIState; //要變更的AI名稱



    public ChangeAIAction(): base(GameEM.EM_RoleAction.ChangeAI)
    {
    }

    /// <summary>
    /// 設定AI變更的相關項目
    /// </summary>
    /// <param name="newID"> 要變更AI的對象ID </param>
    /// <param name="newAI"> 要變更的AI的類型名稱 </param>
    public void f_SetAI(int newID, string newAI){
        m_ild = newID;
        m_AIState = newAI;
    }


    /// <summary>
    /// 設定AI變更的相關項目
    /// </summary>
    /// <param name="newID"> 要變更AI的對象ID </param>
    /// <param name="newAI"> 要變更的AI在AIManager.cs裡註冊的編號 </param>
    public void f_GetAI_fromManager(int newID, string newAI){
        m_ild = newID;
        m_AIState = AIManager.GetInstance().f_GetRunAI(newAI).f_GetId().ToString();
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction(){
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().f_GetRoleControl2(m_ild);
        AI_EM.EM_AIState tGunEM = (AI_EM.EM_AIState) Enum.Parse(typeof(AI_EM.EM_AIState), m_AIState, true); //將string轉成Eunm (第三個參數表示是否在意大小寫)
        if (tmpRole == null) {
            MessageBox.ASSERT("找不到編號 " +m_ild + "的角色，它可能已經死了，所以無法讓它切換到 " + m_AIState +" 的AI，");
            return;
        }
        tmpRole.f_RunAIState(tGunEM, this);           //切換AI
        //if (tmpRole.f_GetRoleCurState() != tGunEM){ //如果要切換的AI，與現在的AI狀態不一樣
        //    tmpRole.f_ChangeAIState(tGunEM);        //切換AI
        //}
    }


}

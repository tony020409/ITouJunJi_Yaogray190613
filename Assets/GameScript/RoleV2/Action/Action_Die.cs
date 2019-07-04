using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;


//記得到Action.cs加
[ProtoContract]
public class Action_Die : BaseActionV2
{
    public Action_Die() 
        : base(GameEM.EM_RoleAction.Die)
    {}

    [ProtoMember(26001)]
    public int m_RoleId;

    [ProtoMember(26002)]
    public int m_TeamType;

    [ProtoMember(26003)]
    public float m_DieTime;


    /// <summary>
    /// 設定死亡
    /// </summary>
    /// <param name="newRoleId"></param>
    /// <param name="newTeamType"></param>
    public void f_Die(int newRoleId, GameEM.TeamType newTeamType, float newDieTime = 3f){
        m_RoleId = newRoleId;
        m_TeamType = (int)newTeamType;
        m_DieTime = newDieTime;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().f_GetRoleControl2(m_RoleId);
        if (tmpRole != null) {
            tmpRole.f_RunAIState(AI_EM.EM_AIState.Die, this);
        }
        else{
            //MessageBox.ASSERT("Die 未找到目标 " + m_RoleId);
        }
    }


}

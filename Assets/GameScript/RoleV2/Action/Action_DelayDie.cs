using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using ccU3DEngine;

//記得到Action.cs加
/// <summary>
/// 執行f_Die()
/// </summary>
[ProtoContract]
public class Action_DelayDie : BaseActionV2
{
    public Action_DelayDie()
        : base(GameEM.EM_RoleAction.Die)
    { }

    [ProtoMember(37001)]
    public int m_RoleId;

    [ProtoMember(37002)]
    public int m_TeamType;

    [ProtoMember(37003)]
    public float m_DelayTime;


    /// <summary>
    /// 延遲死亡設定
    /// </summary>
    /// <param name="newRoleId"   > 要死掉的角色 ID  </param>
    /// <param name="newTeamType" > 要死掉的角色隊伍 </param>
    /// <param name="newDelayTime"> 要多久才掉 </param>
    public void f_DelayDie(int newRoleId, GameEM.TeamType newTeamType, float newDelayTime = 0.0f){
        m_RoleId = newRoleId;
        m_TeamType = (int)newTeamType;
        m_DelayTime = newDelayTime;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()  {
        //BaseRoleControllV2 tmpRole = BattleMain.GetInstance().f_GetRoleControl2(m_RoleId);
        //if (tmpRole != null){
            //tmpRole.f_Die();
            ccTimeEvent.GetInstance().f_RegEvent(m_DelayTime, false, null, CallBack_DelayDie); //執行延後爆炸
        //}
        //else{
            //MessageBox.ASSERT("Die 未找到目标 " + m_RoleId);
        //}
    }


    private void CallBack_DelayDie(object Obj){
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().f_GetRoleControl2(m_RoleId);
        if (tmpRole != null){
            tmpRole.f_Die();
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;



//記得到Action.cs加
[ProtoContract]
public class Action_SetTarget : BaseActionV2 //GameSysc.Action
{

    public Action_SetTarget()
        : base(GameEM.EM_RoleAction.GetRandomRole)
    { }

    [ProtoMember(36001)] public int m_RoleId;   //攻擊發動者的id
    [ProtoMember(36002)] public int m_TargetId; //新攻擊目標的id

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="newiId"> 搜尋發動者 </param>
    /// <param name="size"  > 搜尋範圍 </param>
    public void f_Set(int newiId, int newTargetId){
        m_RoleId = newiId;
        m_TargetId = newTargetId;
    }


    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction(){

        //如果攻擊發動者和攻擊目標都存在
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_RoleId);
        if (tmpRole != null){
            BaseRoleControllV2 newTarget = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_TargetId);
            if (newTarget != null) {

                //遠攻類型的怪換目標
                if (tmpRole.GetComponent<Module_Shoot_TwoHand>() != null) {
                    tmpRole.GetComponent<Module_Shoot_TwoHand>().IKSee_SetTarget(newTarget.transform);
                }

                //其他類型
                //........

            }
        }
    }



}


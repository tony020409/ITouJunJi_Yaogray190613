using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 搜尋視野範圍內有無敵人
/// </summary>
public class ConditionAI_ViewSize_Out : AI_ConditionBaseStateV2 {

    private BaseRoleControllV2 _ReadyAttackTarget;
    public ConditionAI_ViewSize_Out() 
        : base(AI_EM.EM_AIState.ViewSize_Out)
    { }


    public override bool f_ConditionTest() {
        BaseRoleControllV2 tmpEnemy = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetViewSize());
        if (tmpEnemy == null) {
            ChangeAIAction tmpAction = new ChangeAIAction();
            tmpAction.f_SetAI(_BaseRoleControl.m_iId, "ZombieAI2_Idle");
            f_DoRunAIState(tmpAction);
            return true;
        }
        return false;
    }


}






using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 攻擊範圍內有敵人
/// </summary>
public class ConditionAI_AttackSize_In : AI_ConditionBaseStateV2
{

    private BaseRoleControllV2 _ReadyAttackTarget;
    public ConditionAI_AttackSize_In()
        : base(AI_EM.EM_AIState.AttackSize_In)
    { }


    public override bool f_ConditionTest() {
        BaseRoleControllV2 tmpEnemy = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetAttackSize());
        if (tmpEnemy != null) {
            ChangeAIAction tmpAction = new ChangeAIAction();
            tmpAction.f_GetAI_fromManager(_BaseRoleControl.m_iId, _CharacterAIDT.szRunAI);
            f_DoRunAIState(tmpAction);
            return true;
        }
        return false;
    }


}



//ChangeAIAction tmpAction = new ChangeAIAction();
//tmpAction.f_SetAI(_BaseRoleControl.m_iId, "ZombieAI2_FarAttack");
//f_DoRunAIState(tmpAction);
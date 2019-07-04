using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 攻擊範圍內沒敵人
/// </summary>
public class ConditionAI_AttackSize_Out : AI_ConditionBaseStateV2
{

    private BaseRoleControllV2 _ReadyAttackTarget;
    public ConditionAI_AttackSize_Out()
        : base(AI_EM.EM_AIState.AttackSize_Out)
    { }


    public override bool f_ConditionTest()
    {
        BaseRoleControllV2 tmpEnemy = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetAttackSize());
        if (tmpEnemy == null)  {
            ChangeAIAction tmpAction = new ChangeAIAction();
            tmpAction.f_GetAI_fromManager(_BaseRoleControl.m_iId, _CharacterAIDT.szRunAI);
            f_DoRunAIState(tmpAction);
            return true;
        }
        return false;
    }


}

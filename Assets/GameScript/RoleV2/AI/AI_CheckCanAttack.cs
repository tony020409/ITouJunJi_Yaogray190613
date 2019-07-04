using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_CheckCanAttack : AI_ConditionBaseStateV2
{

    private BaseRoleControllV2 _ReadyAttackTarget;
    private List<TileNode> _aWalkPath = null;

    private float fSleepTime = 0;
    List<int> _aPathIgnoreData = new List<int>();
    public AI_CheckCanAttack()
        : base(AI_EM.EM_AIState.CheckCanAttack)
    {
    }

    public override bool f_ConditionTest()
    {
        //移动同时实时检测目标是否进入攻击范围,寻找攻擊範圍內有無目标
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetAttackSize());
        if (tRoleControl != null)
        {
            RoleAttackAction tRoleAttackAction = new RoleAttackAction();
            tRoleAttackAction.f_Attack(_BaseRoleControl.m_iId, _BaseRoleControl.f_GetTeamType(), tRoleControl.m_iId);
            f_DoRunAIState(tRoleAttackAction);
            return true;
        }

        return false;
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionAI_ArcherInit : AI_ConditionBaseStateV2
{

    public ConditionAI_ArcherInit()
        : base(AI_EM.EM_AIState.ArcherInit)
    {
    }

    public override bool f_ConditionTest()
    {
        if (!_BaseRoleControl.GetComponent<ArcherRoleControl>().ArcherInit)
        {
            RoleArcherInitAction tmpAction = new RoleArcherInitAction();
            tmpAction.f_ArcherInit(_BaseRoleControl.m_iId, "MoveToHidePos");
            f_DoRunAIState(tmpAction);

            return true;
        }
        return false;
    }
}

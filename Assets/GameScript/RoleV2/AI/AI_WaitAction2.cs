using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_WaitAction2 : AI_ConditionBaseStateV2
{
    
    public AI_WaitAction2(BaseRoleControllV2 tRoleControl)
        : base(AI_EM.EM_AIState.WaitAction)
    {
        _BaseRoleControl = tRoleControl;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);

        RoleWaitAction tRoleWaitAction = new RoleWaitAction();
        tRoleWaitAction.f_Wait(_BaseRoleControl.m_iId, 1);
        f_DoRunAIState(tRoleWaitAction);
    }

    
}
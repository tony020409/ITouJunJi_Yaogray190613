using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_WaitActionRun : AI_RunBaseStateV2
{

    public AI_WaitActionRun()
        : base(AI_EM.EM_AIState.AI_WaitAction2)
    { }

    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);

        f_RunStateComplete();
    }

}

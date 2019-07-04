using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;


public class PlayerAIControll : BaseAIControllV2
{
    public PlayerAIControll(BaseRoleControllV2 tRoleControl) : base(tRoleControl)
    {
        //f_RegNewAIState(new AI_PlayerDie(tRoleControl));
        //f_RegNewAIState(new AI_PlayerIdle(tRoleControl));
        //f_RegNewAIState(new AI_WaitAction2(tRoleControl));
        //f_ChangeAIState(AI_EM.EM_AIState.Idle);
    }
}

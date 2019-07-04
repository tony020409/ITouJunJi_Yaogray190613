using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : BaseActionController
{

    public PlayerActionController(BaseRoleControllV2 tBaseRoleControl) 
        : base(tBaseRoleControl, GameEM.emRoleType.Player){}


    public override void f_PlayAction(AI_EM.EM_AIState tAIState)
    {
        if (tAIState == AI_EM.EM_AIState.Idle){
            MessageBox.DEBUG("===========玩家待機");
        }

        else if (tAIState == AI_EM.EM_AIState.Die){
            MessageBox.DEBUG("===========玩家死亡");
        }

        else if (tAIState == AI_EM.EM_AIState.WaitAction){
            MessageBox.DEBUG("===========玩家等待");
        }
    }


    
}

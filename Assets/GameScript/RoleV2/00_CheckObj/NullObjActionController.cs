using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullObjActionController : BaseActionController
{

    public NullObjActionController(BaseRoleControllV2 tBaseRoleControl) 
        :base(tBaseRoleControl, GameEM.emRoleType.CheckObj) { }


    public override void f_PlayAction(AI_EM.EM_AIState tAIState)  {

        if (animator == null){
            return;
        }

        if (tAIState == AI_EM.EM_AIState.WaitAction){
            return;
        }


        if (m_CurAIStatic != tAIState) {
            m_CurAIStatic = tAIState;
        }

    }




}

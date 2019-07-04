using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableActionController : BaseActionController
{

    public PickableActionController(BaseRoleControllV2 tBaseRoleControl) 
        :base(tBaseRoleControl, GameEM.emRoleType.Pickable) { }



    public override void f_PlayAction(AI_EM.EM_AIState tAIState)
    {

        if (animator == null) {
            return;
        }

        if (tAIState == AI_EM.EM_AIState.WaitAction) {
            return;
        }


        if (m_CurAIStatic != tAIState)
        {

            // 播放指定動畫 ========================================
            if (AI_EM.EM_AIState.PlayAnim == tAIState) {
                //照 AI_Animator.cs 收到的動畫名稱去播放
            }


            // 待機 ================================================        
            else if (AI_EM.EM_AIState.Idle == tAIState) {
                //animator.SetInteger("control", 71);
                //animator.CrossFade("Idle", 0.25f);
            }


            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.Die== tAIState) {
                //animator.SetInteger("control", 71);
                //if (animator != null) {
                //    animator.Play("Die");
                //}
            }

            m_CurAIStatic = tAIState;
        }

    }




}

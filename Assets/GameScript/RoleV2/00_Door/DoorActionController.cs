using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActionController : BaseActionController
{

    public DoorActionController (BaseRoleControllV2 tBaseRoleControl) 
        :base(tBaseRoleControl, GameEM.emRoleType.Door) { }


    public override void f_PlayAction(AI_EM.EM_AIState tAIState)
    {

        if (animator == null){
            return;
        }

        if (tAIState == AI_EM.EM_AIState.WaitAction) {
            return;
        }


        if (m_CurAIStatic != tAIState)  {

            //// 待機 ================================================
            //if (AI_EM.EM_AIState.Dazer == tAIState) {
            //    animator.SetInteger("control", 0);
            //}

            // 播放指定動畫 ========================================
            if (AI_EM.EM_AIState.PlayAnim == tAIState){
                //照 AI_Animator.cs 收到的動畫名稱去播放
            }

            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.Die == tAIState)  {
                animator.SetInteger("control", 71);
            }

            m_CurAIStatic = tAIState;
        }

    }




}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinActionController : BaseActionController
{

    public GoblinActionController(BaseRoleControllV2 tBaseRoleControl) 
        :base(tBaseRoleControl, GameEM.emRoleType.Goblin) { }


    public override void f_PlayAction(AI_EM.EM_AIState tAIState)
    {

        if (animator == null)  {
            return;
        }

        if (tAIState == AI_EM.EM_AIState.WaitAction) {
            return;
        }


        if (m_CurAIStatic != tAIState) {

            // 播放指定動畫 ========================================
            if (AI_EM.EM_AIState.PlayAnim == tAIState) {
                //照 AI_Animator.cs 收到的動畫名稱去播放
            }

            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.ZombieAI2_Idle == tAIState) {
                //animator.SetInteger("control", 71);
                animator.CrossFade("Idle", 0.25f);
            }

            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.ZombieAI2_Chase == tAIState) {
                //animator.SetInteger("control", 71);
                animator.CrossFade("Run", 0.25f);
            }

            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.ZombieAI2_NearAttack == tAIState) {
                //animator.SetInteger("control", 71);
                animator.CrossFade("Attack", 0.25f);
            }

            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.ZombieAI2_FarAttack == tAIState) {
                //animator.SetInteger("control", 71);
                animator.CrossFade("Attack", 0.25f);
            }

            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.Die == tAIState) {
                //animator.SetInteger("control", 71);
                animator.CrossFade("Die", 0.25f);
            }

            m_CurAIStatic = tAIState;
        }

    }




}

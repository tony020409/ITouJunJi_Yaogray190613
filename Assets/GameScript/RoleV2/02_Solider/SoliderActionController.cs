using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderActionController : BaseActionController
{

    BaseRoleControllV2 _BaseRoleControl = null;
    public SoliderActionController (BaseRoleControllV2 tBaseRoleControl) 
        :base(tBaseRoleControl, GameEM.emRoleType.Zombie) {
        _BaseRoleControl = tBaseRoleControl;
    }


    public override void f_PlayAction(AI_EM.EM_AIState tAIState)
    {

        if (animator == null){
            return;
        }

        if (tAIState == AI_EM.EM_AIState.WaitAction){
            return;
        }


        if (m_CurAIStatic != tAIState) {

            // 播放指定動畫 ========================================
            if (AI_EM.EM_AIState.PlayAnim == tAIState)  {
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
                //由主玩家亂數決定延遲播放攻擊動作
                if (StaticValue.m_bIsMaster){
                    float i = UnityEngine.Random.Range(0.5f, 5.0f);
                    ccTimeEvent.GetInstance().f_RegEvent(i, false, null, CallBack_DelayAttack);
                }
                
            }

            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.ZombieAI2_FarAttack == tAIState) {
                //animator.SetInteger("control", 71);
                animator.CrossFade("Attack", 0.25f);
            }

            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.Die == tAIState) {
                //animator.SetInteger("control", 71);
                animator.Play("Die");
            }

            //
            else if (AI_EM.EM_AIState.AI_ChangePoint == tAIState) {
                //animator.SetInteger("control", 71);
                animator.CrossFade("Run", 0.25f);
            }


            //換彈動作
            else if (AI_EM.EM_AIState.AI_Reload == tAIState){
            }


            m_CurAIStatic = tAIState;
        }

    }


    private void CallBack_DelayAttack(object Obj) {
        Action_Animator tmpAction = new Action_Animator();
        tmpAction.f_SetAnimation(_BaseRoleControl.m_iId, GameEM.EM_Animator.CrossFade, "Attack");
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);

        //animator.CrossFade("Attack", 0.25f);
    }

}

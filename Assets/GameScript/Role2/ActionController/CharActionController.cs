using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 动作控制器
/// </summary>
public class CharActionController
{
    private Animator animator;
    private AI_EM.EM_AIState m_CurAIStatic = AI_EM.EM_AIState.Idle;


    public CharActionController(BaseRoleControllV2 tBaseRoleControl)
    {
        animator = tBaseRoleControl.gameObject.GetComponent<Animator>();
    }



    public void f_PlayAction(AI_EM.EM_AIState tAIState)
    {
        MessageBox.DEBUG(" f_PlayAction " + m_CurAIStatic.ToString() + ">>" + tAIState.ToString());
        if (animator == null)
        {
            return;
        }
        if (tAIState == AI_EM.EM_AIState.WaitAction)
        {
            return;
        }
        if (m_CurAIStatic != tAIState)
        {
            if (AI_EM.EM_AIState.Attack == tAIState)
            {
                if (m_CurAIStatic == AI_EM.EM_AIState.Idle)
                {
                    animator.SetTrigger("Attack");
                }
                else if (m_CurAIStatic == AI_EM.EM_AIState.Walk || AI_EM.EM_AIState.Move == m_CurAIStatic)
                {
                    animator.SetTrigger("Walk2Attack");
                }
            }
            else if (AI_EM.EM_AIState.Die == tAIState)
            {
                if (m_CurAIStatic == AI_EM.EM_AIState.Idle)
                {
                    animator.SetTrigger("Death");
                }
                else if (m_CurAIStatic == AI_EM.EM_AIState.Walk)
                {
                    animator.SetTrigger("Walk2Die");
                }
                else if (m_CurAIStatic == AI_EM.EM_AIState.Attack)
                {
                    animator.SetTrigger("Attack2Die");
                }
            }
            else if (AI_EM.EM_AIState.Walk == tAIState || AI_EM.EM_AIState.Move == tAIState)
            {
                animator.SetTrigger("Walk");
            }
            else if (AI_EM.EM_AIState.Idle == tAIState)
            {
                animator.SetTrigger("Wait");
            }
            m_CurAIStatic = tAIState;
        }
    }

}

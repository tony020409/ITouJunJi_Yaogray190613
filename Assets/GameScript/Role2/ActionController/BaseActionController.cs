using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 动作控制器
/// </summary>
public class BaseActionController
{
    /// <summary>
    /// 动作控制事件回调
    /// </summary>
    protected RoleModelCallbackEvent m_RoleModelCallbackEvent;
    protected Animator animator;
    protected AI_EM.EM_AIState m_CurAIStatic = AI_EM.EM_AIState.None;


    public BaseActionController(BaseRoleControllV2 tBaseRoleControl, GameEM.emRoleType tRoleType)
    {
        if (tRoleType == GameEM.emRoleType.Player)
        {
            InitPlayerRole(tBaseRoleControl);
        }
        else
        {
            InitOtherRole(tBaseRoleControl);
        }
    }

    private void InitPlayerRole(BaseRoleControllV2 tBaseRoleControl)
    {

    }

    private void InitOtherRole(BaseRoleControllV2 tBaseRoleControl)
    { 
        animator = tBaseRoleControl.m_RoleModel.GetComponent<Animator>();
        m_RoleModelCallbackEvent = tBaseRoleControl.m_RoleModel.GetComponent<RoleModelCallbackEvent>();
        if (animator == null)
        {
            MessageBox.ASSERT("Animator控制器未找到");
        } else
        {
            animator.enabled = true;
        }
        if (m_RoleModelCallbackEvent == null)
        {
            MessageBox.ASSERT(tBaseRoleControl.m_iId + "的RoleModelCallbackEvent动作控制事件回调未找到");
        }
        m_RoleModelCallbackEvent.f_RegCallbackEvent(
            tBaseRoleControl.f_CallBack_Attacking,
            tBaseRoleControl.f_CallBack_AttacComplete,
            tBaseRoleControl.f_CallBack_ThrowStart,
            tBaseRoleControl.f_CallBack_ThrowEnd
            );

    }

    //--------------------------------------------------------------
    public virtual void f_PlayAction(AI_EM.EM_AIState tAIState)
    {}

    public virtual void f_PlayAnimator(string tAnimation_Name)
    {}


    //--------------------------------------------------------------
    protected void PlayAction(EM_RoleAnimator tEM_RoleAnimator)
    {
        animator.SetInteger("control", (int)tEM_RoleAnimator);
    }


}

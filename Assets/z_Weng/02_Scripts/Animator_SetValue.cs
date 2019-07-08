using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_SetValue : StateMachineBehaviour
{

    [Header("開頭")]
    [Rename("動畫位移狀態")] public bool motionEnter;


    [Header("結尾")]
    [Rename("動畫位移狀態")] public bool motionEnd;
    [Rename("要切換的數值")] public int newValueInEnd;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.applyRootMotion = motionEnter;
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.applyRootMotion = motionEnd;
        //ChangeAnimatorAction tmpAction = new ChangeAnimatorAction();
        //tmpAction.f_SetAnimator(tmpRole.m_iId, "control", newValueInEnd);
        //tmpRole.f_AddMyAction(tmpAction);
    }

}

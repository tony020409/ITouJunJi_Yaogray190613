using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AnimatorState觸發時機
/// </summary>
public enum AnimatorStateTime {
    Enter,
    Exit,
}


/// <summary>
/// [AnimatorState] 變更Int參數
/// </summary>
public class AnimatorState_SetInt : StateMachineBehaviour
{
    [HelpBox("變更Int參數", HelpBoxType.Info, height = 4)]
    [Rename("Int參數名稱")] public string m_HashName;
    [Rename("觸發時機")]    public AnimatorStateTime ValueEvent;
    [Rename("變更為")]      public int tmp = 3;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (ValueEvent != AnimatorStateTime.Enter) {
            return;
        }
        animator.SetInteger(Animator.StringToHash(m_HashName), tmp);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (ValueEvent != AnimatorStateTime.Exit) {
            return;
        }
        animator.SetInteger(Animator.StringToHash(m_HashName), tmp);
    }


}


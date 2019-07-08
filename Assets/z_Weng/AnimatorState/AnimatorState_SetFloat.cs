using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// [AnimatorState] 變更Float參數
/// </summary>
public class AnimatorState_SetFloat : StateMachineBehaviour
{
    [HelpBox("變更Float參數", HelpBoxType.Info, height = 4)]
    [Rename("Float參數名稱")] public string m_HashName;
    [Rename("觸發時機")]      public AnimatorStateTime ValueEvent;
    [Rename("變更為")]        public int tmp = 3;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if (ValueEvent != AnimatorStateTime.Enter){
            return;
        }
        animator.SetFloat(Animator.StringToHash(m_HashName), tmp);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if (ValueEvent != AnimatorStateTime.Exit){
            return;
        }
        animator.SetFloat(Animator.StringToHash(m_HashName), tmp);
    }


}

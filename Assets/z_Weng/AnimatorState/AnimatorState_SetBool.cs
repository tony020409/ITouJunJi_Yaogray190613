using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/// <summary>
/// 布爾值
/// </summary>
public enum Bool {
    True,
    False,
}


/// <summary>
/// [AnimatorState] 變更布爾值
/// </summary>
public class AnimatorState_SetBool : StateMachineBehaviour
{

    [HelpBox("變更布爾值", HelpBoxType.Info, height = 4)]
    [Rename("Bool參數名稱")] public string m_HashName;
    [Rename("觸發時機")]    public AnimatorStateTime ValueEvent;
    [Rename("變更為")]      public Bool tmp;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (ValueEvent != AnimatorStateTime.Enter){
            return;
        }
        animator.SetBool(m_HashName, tmp == Bool.True ? true : false);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ValueEvent != AnimatorStateTime.Exit)
        {
            return;
        }
        animator.SetBool(m_HashName, tmp == Bool.True ? true : false);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// [AnimatorState] 重置 AnimatorTrigger
/// </summary>
public class AnimatorState_ResetTrigger : StateMachineBehaviour
{

    [HelpBox("確保重置 AnimatorTrigger", HelpBoxType.Info, height = 4)]
    [Rename("Trigger參數名稱")] public string m_HashName;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger(m_HashName);
    }



}

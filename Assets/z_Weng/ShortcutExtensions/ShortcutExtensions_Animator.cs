using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class ShortcutExtensions_Animator
{

    //複製網路上的，看起來是沒用
    /// <summary>
    /// 通過Animator動畫狀態機獲取任意animation clip的播放時長
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="clip"></param>
    public static float GetClipLength(this Animator animator, string clip) {
        if (animator == null) {
            return 0;
        }
        if (string.IsNullOrEmpty(clip)) {
            return 0;
        }
        if (animator.runtimeAnimatorController == null) {
            return 0;
        }

        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        AnimationClip[] tmpClipAll = ac.animationClips;
        if (null == tmpClipAll || tmpClipAll.Length <= 0) {
            return 0;
        }
       
        AnimationClip tmpClip;
        for (int i = 0; i < tmpClipAll.Length; i++) {
            tmpClip = ac.animationClips[i];
            if (tmpClip != null && tmpClip.name == clip) {
                Debug.LogAssertion("GET");
                return tmpClip.length;
            }
        }
        Debug.LogAssertion("5");
        return 0.0f;
    }


}

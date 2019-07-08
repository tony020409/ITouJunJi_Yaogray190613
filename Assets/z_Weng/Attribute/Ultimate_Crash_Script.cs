using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 假裝當機程式 (設定秒數後讓遊戲自己關閉)
/// </summary>
public class Ultimate_Crash_Script : MonoBehaviour {


    /// <summary>
    /// 關閉遊戲的延遲秒數
    /// </summary>
    [Rename("多久後遊戲自動關閉")]
    public float delayTime;


	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        StartCoroutine(Crash_Simulate(delayTime));
    }


    /// <summary>
    /// 模擬當機 (指定秒數後關閉遊戲)
    /// </summary>
    /// <param name="tTime"> 指定的秒數 </param>
    IEnumerator Crash_Simulate(float tTime) {
        yield return new WaitForSeconds(tTime);
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }


}

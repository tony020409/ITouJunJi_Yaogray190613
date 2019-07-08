using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[System.Serializable]
public class  RotateObjInfo{
    /// <summary>
    /// 要轉動的物件
    /// </summary>
    [Rename("Transform")]
    public Transform transformValue;

    /// <summary>
    /// 轉動的方向
    /// </summary>
    [Rename("轉動的方向")]
    public Vector3 m_endValue;

}

/// <summary>
/// 展示用旋轉
/// </summary>
public class Test_Rotate : MonoBehaviour {

 /// <summary>
    /// 旋轉狀態
    /// </summary>
    public enum RotateState {

        /// <summary>
        /// 靜止
        /// </summary>
        Stop = 0,

        /// <summary>
        /// 轉動中
        /// </summary>
        Rotaing = 1,
    }




    /// <summary>
    /// 要轉動的物件
    /// </summary>
    [Rename("物件")]
    public RotateObjInfo[] m_Target;


    /// <summary>
    /// 轉一次需要的時間
    /// </summary>
    [Rename("轉一次需要的時間")]
    public float m_OneRoundTime;

    /// <summary>
    /// 當前狀態
    /// </summary>
    [Rename("當前狀態")]
    public RotateState m_State;


    /// <summary>
    /// 鍵盤偵測 CD 時間
    /// </summary>
    private float CD_KeyDown = 0.1f;

    /// <summary>
    /// 當下按鍵盤能否有反應
    /// </summary>
    private bool can_KeyDown = true;


    // Use this for initialization
    void Start () {
        f_Start();
        CD_KeyDown = 0.1f;
    }



    private void Update() {

        // Alt + 鍵盤「6」可以開關旋轉
        if (/*Input.GetKey(KeyCode.RightAlt) &&*/ Input.GetKeyDown(KeyCode.Alpha6)) {

            if (m_State == RotateState.Rotaing) {
                if (!can_KeyDown) {
                    return;
                }
                f_Stop();
                can_KeyDown = false;
                StartCoroutine(CD_KeyCanDown());
            }
            else if (m_State == RotateState.Stop) {
                if (!can_KeyDown) {
                    return;
                }
                f_Start();
                can_KeyDown = false;
                StartCoroutine(CD_KeyCanDown());
            }
        }

    }



    IEnumerator CD_KeyCanDown() {
        yield return new WaitForSeconds(CD_KeyDown);
        can_KeyDown = true;
    }



    /// <summary>
    /// 開始旋轉
    /// </summary>
    public void f_Start() {
         for (int i=0; i<m_Target.Length; i++) {
            if (m_Target[i] == null) {
                continue;
            }
            if (m_Target[i].transformValue == null) {
                continue;
            }
            f_Rotate(m_Target[i].transformValue, m_Target[i].transformValue.localEulerAngles + m_Target[i].m_endValue);
        }
        m_State = RotateState.Rotaing;

    }


    /// <summary>
    /// 停止旋轉
    /// </summary>
    public void f_Stop() {
        for (int i = 0; i < m_Target.Length; i++) {
            if (m_Target[i] == null) {
                continue;
            }
            if (m_Target[i].transformValue == null) {
                continue;
            }
            f_StopRotate(m_Target[i].transformValue);
        }
        m_State = RotateState.Stop;
    }


    /// <summary>
    /// 開始轉動
    /// </summary>
    private void f_Rotate(Transform tmp, Vector3 tValue) {
        if (tmp == null) {
                return;
        }
        if (m_State == RotateState.Rotaing) {
                return;
        }
        tmp.DOLocalRotate(tValue, m_OneRoundTime)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }


    /// <summary>
    /// 停止轉動
    /// </summary>
    private void f_StopRotate(Transform tmp) {
        if (tmp == null) {
            return;
        }
        //if (m_State == RotateState.Stop) {
        //        return;
        //}
        tmp.DOKill();
        //m_State = RotateState.Stop;
    }





}

using RootMotion.FinalIK;
using System.Collections;                                                                                                                                             
using System.Collections.Generic;
using UnityEngine;

public class RagdollControl : MonoBehaviour {


    [Header("是否使用布娃娃")]
    [Rename("打勾為使用，反之不使用")] public bool Enable;

    [Header("布娃娃前的動畫元件 (擇一)")]
    [Rename("Amiator")] public Animator _animator;
    [Rename("IK")]      public VRIK _ik;

    [Header("彈飛力道 (方向用正負數去控制)")]
    [Rename("前後飛的力道")] public float Power;
    [Rename("向上飛的力道")] public float PowerUp;

    [Header("彈飛時受力的部位 (沒設定的話就原地布娃娃)")]
    public Rigidbody[] HitObjects; //彈飛時受力的部位

    [HideInInspector] public CharacterJoint[] Joint; //獲取布娃娃所有關節用
    [HideInInspector] public Vector3[] AnchorPos;    //獲取布娃娃錨點位置用


    void Start () {
        Lucy();             //紀錄Prefabs最初的紀錄 (供之後回收池重生初始化用)
        setKinematic(true); //設置全身Kinematic
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad8)) {
            SetRadoll(true);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9)) {
            SetRadoll(false);
        }
    }


    //物件池生成第一隻時，紀錄各布娃娃身上的資訊 ========================================
    public void Lucy(){
        Joint = GetComponentsInChildren<CharacterJoint>(); //獲取所有關節
        AnchorPos = new Vector3[Joint.Length];             //設置關節紀錄點的位置
        for (int i = 0; i < Joint.Length; i++) {           //歷遍所有關節
            Joint[i].enableProjection = true;              //??? 不知道是什麼，總之設成true，布娃娃就不會拉皮
            Joint[i].autoConfigureConnectedAnchor = false; //不自動連結錨點，確保錨點位置不會因自動連結而跑掉
            AnchorPos[i] = Joint[i].connectedAnchor;       //紀錄錨點位置
        }
    }


    #region 布娃娃功能接口 ====================================================================================
    //設定布娃娃狀態(接口) =====================================================================================================================
    public void SetRadoll(bool state){
        if (state == true) {
            OpenRagdoll();
        } else {
            ResetRagdoll();
        }
    }

    //開啟布娃娃 ===============================================================================================================================
    public void OpenRagdoll() {
        if (_animator != null) {
            _animator.enabled = false; //不播動畫
        }
        if (_ik != null){
            _ik.enabled = false; //關閉ik
        }
        setKinematic(false); //剛體 = false 
        GiveForce();         //彈飛
    }

    //解除布娃娃 ===============================================================================================================================
    public void ResetRagdoll(){
        if (_animator != null) {
            _animator.enabled = true; //播動畫
        }
        if (_ik != null) {
            _ik.enabled = true; //開啟ik
        }
        setKinematic(true);     //剛體 = true
        ResetAnchor();          //回復錨點位置
    }                                                 
    #endregion


    #region 布娃娃功能元件 ====================================================================================
    //設置全身Kinematic ========================================================================================================================
    public void setKinematic(bool newValue){
        Component[] components = GetComponentsInChildren(typeof(Rigidbody));
        foreach (Component c in components){
            (c as Rigidbody).isKinematic = newValue;
        }
    }

    //給予彈飛力道 =============================================================================================================================
    public void GiveForce(){
        if (HitObjects.Length > 0){                                                               //如果有設定彈飛時受力的部位
            for (int i = 0; i < HitObjects.Length; i++){                                          //對受力的部位
                HitObjects[i].AddForce(transform.forward * Power * -1, ForceMode.VelocityChange); //給予向後彈飛的力
                HitObjects[i].AddForce(transform.up * PowerUp, ForceMode.VelocityChange);         //給予向上彈飛的力
            }
        }
        else {
            //如果沒設定受力部位就全部受力
            //GetComponentInChildren<Rigidbody>().AddForce((_BaseRoleControl.transform.position - bulletPos) * 50, ForceMode.VelocityChange);
            //GetComponentInChildren<Rigidbody>().AddForce((-_BaseRoleControl.transform.forward) * 150, ForceMode.VelocityChange);
        }
    }
                                                                                                                                                
    //回復錨點 =================================================================================================================================
    public void ResetAnchor(){                                                           
        for (int i = 0; i < Joint.Length; i++){      
            Joint[i].connectedAnchor = AnchorPos[i]; 
        } 
    }
    #endregion


}

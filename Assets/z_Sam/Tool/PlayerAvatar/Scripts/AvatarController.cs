using UnityEngine;
using System.Collections;
//using VRTK;

public class AvatarController : MonoBehaviour
{
    [Header("PlayerIK 下對應的物件")]
    public GameObject Avatar_Head;
    public GameObject Avatar_LeftHand;
    public GameObject Avatar_RightHand;
    [Space(4)]
    public GameObject Avatar_LeftFoot;
    public GameObject Avatar_RightFoot;
    [Space(4)]
    public GameObject Avatar_Tracker_1;
    public GameObject Avatar_Tracker_2;

    [Header("Player[CameraRig] 下對應的物件")]
    public GameObject Tartget_Head;
    public GameObject Tartget_LeftHand;
    public GameObject Tartget_RightHand;
    [Space(4)]
    public GameObject Tartget_LeftFoot;
    public GameObject Tartget_RightFoot;
    [Space(4)]
    public GameObject Tartget_Tracker_1;
    public GameObject Tartget_Tracker_2;


    void Start() {
        //ViveManager.inst.LVRTK_ControllerEvents.TriggerPressed += OnLeftTriggerPressed;
        //ViveManager.inst.RVRTK_ControllerEvents.TriggerPressed += OnRightTriggerPressed;
        //ViveManager.inst.LVRTK_ControllerEvents.TriggerReleased += OnLeftTriggerReleased;
        //ViveManager.inst.RVRTK_ControllerEvents.TriggerReleased += OnRightTriggerReleased;
    }

    // Update is called once per frame
    void Update() {


        #region 讓「玩家部位」跟「Player[CameraRig]」對應的部位位置與朝向一樣
        //頭部
        Avatar_Head.transform.position = Tartget_Head.transform.position;
        Avatar_Head.transform.rotation = Tartget_Head.transform.rotation;

        //右手
        Avatar_RightHand.transform.position = Tartget_RightHand.transform.position;
        Avatar_RightHand.transform.rotation = Tartget_RightHand.transform.rotation;

        //左手
        Avatar_LeftHand.transform.position = Tartget_LeftHand.transform.position;
        Avatar_LeftHand.transform.rotation = Tartget_LeftHand.transform.rotation;

        
        f_Avatar(Avatar_RightFoot, Tartget_RightFoot); //右腳
        f_Avatar(Avatar_LeftFoot , Tartget_LeftFoot);  //左腳
        f_Avatar(Avatar_Tracker_1, Tartget_Tracker_1); //Tracker 1
        f_Avatar(Avatar_Tracker_2, Tartget_Tracker_2); //Tracker 2
        #endregion

        //#region  按下滑鼠左鍵發射子彈 放開滑鼠左鍵取消發射子彈  滑鼠右鍵補充子彈 
        //if (Input.GetMouseButtonDown(0)) {
        //    Avatar_RightHand.GetComponent<FPSFireManager_network>().onTriggerPressed();
        //}
        //else if (Input.GetMouseButtonUp(0)) {
        //    Avatar_RightHand.GetComponent<FPSFireManager_network>().onTriggerReleased();
        //}
        //if (Input.GetMouseButtonDown(1)) {
        //    Avatar_RightHand.GetComponent<FPSFireManager_network>().onReloadPressed();
        //}
        //#endregion
    }



    /// <summary>
    /// 讓 左參數.位置朝向 跟右參數一樣
    /// </summary>
    /// <param name="_avator"> PlayerIK 下的物件 </param>
    /// <param name="_target"> Player[CameraRig] 下對應的物件 </param>
    private void f_Avatar(GameObject _avator, GameObject _target) {
        if (_avator == null) {
            return;
        }
        if (_target == null) {
            return;
        }
        _avator.transform.position = _target.transform.position;
        _avator.transform.rotation = _target.transform.rotation;
    }


  

    //void OnLeftTriggerPressed(object sender, ControllerInteractionEventArgs e) {
    //    //Avatar_LeftHand.GetComponent<HandModelController>().isTriggerPressed = true;
    //}
    //
    //
    //void OnRightTriggerPressed(object sender, ControllerInteractionEventArgs e) {
    //    Avatar_RightHand.GetComponent<FPSFireManager_network>().onTriggerPressed();
    //}
    //
    //
    //void OnLeftTriggerReleased(object sender, ControllerInteractionEventArgs e) {
    //    //Avatar_LeftHand.GetComponent<HandModelController>().isTriggerPressed = false;
    //}
    //
    //void OnRightTriggerReleased(object sender, ControllerInteractionEventArgs e)  {
    //    Avatar_RightHand.GetComponent<FPSFireManager_network>().onTriggerReleased();
    //}




}

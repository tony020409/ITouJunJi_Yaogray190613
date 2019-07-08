using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //使用DOTween

public class ChangRoom : MonoBehaviour {

    [Header("上一個場景、下一個場景")]
    public GameObject prevRoom;
    public GameObject nextRoom;

    [Header("放入門")]
    public Transform Door;
    private BaseRoleControllV2 Player;


    //List<BaseRoleControl> tRoleA;
    //List<BaseRoleControl> tRoleB;

    // Use this for initialization
    void Start () {
        //tRoleA = BattleMain.GetInstance().f_FindTeamTarget2("A");
        //tRoleB = BattleMain.GetInstance().f_FindTeamTarget2("B");
    }
	
	// Update is called once per frame
	//void Update () {
    //
    //    if (StaticValue.m_bIsMaster){
    //
    //        for (int i = 0; i < tRoleA.Count; i++){
    //            float distance = Vector3.Distance(this.transform.position, tRoleA[i].transform.position);
    //            if (distance <= 100){
    //                //判斷玩家到達
    //            }
    //        }
    //
    //    }
    //
    //}

    
    //抵達門前 ============================================================
    void OnTriggerEnter(Collider other ){
        if (other.GetComponent<MySelfPlayerControll2>() != null){
            Player = other.GetComponent<BaseRoleControllV2>();  //取得玩家
            RoomAction tAction = new RoomAction();           //新事件
            tAction.f_SetRoom(nextRoom.name, true);          //設定新事件內容(開啟 nextRoom)
            Player.f_AddMyAction(tAction);                   //玩家執行新事件
            OpenDoor();                                      //播放開門動畫
            //Debug.LogAssertion("接觸");                    //Log
        }
    }

    //離開門後 ============================================================
    void OnTriggerExit(Collider other){
        if (other.GetComponent<MySelfPlayerControll2>() != null){
            Player = other.GetComponent<BaseRoleControllV2>();  //取得玩家
            RoomAction tAction = new RoomAction();           //新事件
            tAction.f_SetRoom(prevRoom.name, false);         //設定新事件內容(關閉 prevRoom)
            Player.f_AddMyAction(tAction);                   //玩家執行新事件
            Invoke("CloseDoor",2.0f);                        //播放關門動畫
            //Debug.LogAssertion("離開");                    //Log
        }
    }


    //播放開門動畫
    void OpenDoor(){
        Door.DOLocalMoveY(0.1f, 5);
    }
    //播放關門動畫
    void CloseDoor(){
        Door.DOLocalMoveY(0.0f, 5);
    }

}

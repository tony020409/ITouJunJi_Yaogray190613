using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;


//記得到Action.cs加
[ProtoContract]
public class RoomAction : GameSysc.Action
{

    [ProtoMember(10840)]
    public string RoomName; //要執行動作房間

    [ProtoMember(10841)]
    public bool DoOpen;     //要執行的動作(開or關)


    public RoomAction() : base(){
        m_iType = (int)GameEM.EM_RoleAction.RoomAction;
    }


    //設定房間
    public void f_SetRoom(string tmpRoomName, bool tmpDo){
        RoomName = tmpRoomName;
        DoOpen = tmpDo;
    }
    public void f_SetTeam(string tmpTeam){
        RoomName = tmpTeam;
    }


    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction(){

       

        //List<BaseRoleControllV2> tRoleControl = BattleMain.GetInstance().f_FindTeamTarget2("A");
        //if (tRoleControl != null){
        //    if (DoOpen){
        //        BattleMain.GetInstance().OpenRoom(RoomName);  //如果 DoOpen = true就開房間
        //    } else {
        //        BattleMain.GetInstance().CloseRoom(RoomName); //如果 DoOpen = false就關閉房間
        //    }
        //}
        //else{
        //    MessageBox.ASSERT("未找到目标");
        //}




    }


}

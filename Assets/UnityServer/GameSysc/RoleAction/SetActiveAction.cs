using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;


//記得到Action.cs加
[ProtoContract]
public class SetActiveAction : GameSysc.Action
{
   
    [ProtoMember(10852)]
    public string n_PrevRoomName; //上一個房間

    [ProtoMember(10853)]
    public string n_NextRoomName; //下一個房間


    public SetActiveAction(): base() {
        m_iType = (int)GameEM.EM_RoleAction.SetActive;
    }


    public void f_SetActive(string PrevRoomName, string NextRoomName){
        n_PrevRoomName = PrevRoomName;
        n_NextRoomName = NextRoomName;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
        //List<BaseRoleControl> tRole = BattleMain.GetInstance().f_FindTeamTarget2("A");
        //if (tRole.Count > 0)
        //{
        //   //RoomMain.inst.OpenRoom((GameEM.TeamType)m_TeamType , RoomName);
        //}
        //else
        //{
        //   //MessageBox.ASSERT("Hp 未找到目标");
        //}
        //BattleMain.GetInstance().OpenRoom(n_NextRoomName);
        //BattleMain.GetInstance().CloseRoom(n_PrevRoomName);
    }


}

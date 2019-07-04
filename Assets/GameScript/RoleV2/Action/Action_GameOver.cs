using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

//記得到Action.cs加
[ProtoContract]
public class Action_GameOver : BaseActionV2
{

    public Action_GameOver()
        : base(GameEM.EM_RoleAction.GameOver)
    { }

    [ProtoMember(39001)]
    public int glo_LostTeam;  //任務失敗的隊伍


    /// <summary>
    /// 設定任務失敗的隊伍
    /// </summary>
    /// <param name="tmpLostTeam"> 任務失敗的隊伍 </param>
    public void f_SetLostTeam(int tmpLostTeam) {
        glo_LostTeam = tmpLostTeam;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {
        if (StaticValue.m_UserDataUnit.m_PlayerDT.f_GetTeamType() == (GameEM.TeamType) glo_LostTeam)
        {
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Lost); //遊戲以失敗結束
        }
        else {
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Win); //遊戲以勝利結束
        }
    }





}


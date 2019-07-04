using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMission
{

    Dictionary<BaseRoleControllV2, EM_GameResult> _dicData = new Dictionary<BaseRoleControllV2, EM_GameResult>();

    
    public void f_RegRole(BaseRoleControllV2 tBaseRoleControl, EM_GameResult tEM_GameResult)
    {
        _dicData.Add(tBaseRoleControl, tEM_GameResult);
    }

    public void f_RoleDie(BaseRoleControllV2 tBaseRoleControl)
    {
        EM_GameResult tEM_GameResult;

        if (_dicData.TryGetValue(tBaseRoleControl, out tEM_GameResult))
        {
            if (tEM_GameResult == EM_GameResult.Win)
            {
                MessageBox.DEBUG("Win！");
                glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Win);
            }
            else if (tEM_GameResult == EM_GameResult.Lost)
            {
                MessageBox.DEBUG("Lost！");
                glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Lost);
            }
        }
        //CheckAllPlayerIsDie(); //移到逐家死時檢查 f_CheckPVP_TeamLost()
    }


    /// <summary>
    /// 檢查全部玩家是否都死光了
    /// </summary>
    public void f_CheckAllPlayerIsDie()  {        
        if (BattleMain.GetInstance().m_BattleRolePool.f_CheckAllPlayerIsDie()) {
            MessageBox.DEBUG("玩家死光了！");
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Lost); //遊戲以失敗結束
        }
    }


    /// <summary>
    /// 檢查某隊是否玩家是否死光
    /// </summary>
    public void f_CheckPVP_TeamLost() {
        if (BattleMain.GetInstance().m_BattleRolePool.f_CheckPVP_TeamLost(StaticValue.m_UserDataUnit.m_PlayerDT.f_GetTeamType())) {
            MessageBox.DEBUG(StaticValue.m_UserDataUnit.m_PlayerDT.f_GetTeamType() +"隊的玩家死光了！");
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Lost); //遊戲以失敗結束
        }
    }











    public void f_Reset()
    {
        _dicData.Clear();
    }

}

    
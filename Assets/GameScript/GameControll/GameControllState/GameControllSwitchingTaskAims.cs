using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using System; //使用Enum.Parse用

public class GameControllSwitchingTaskAims : GameControllBaseState
{

    List<BaseRoleControllV2> tRole;//接收指定隊伍的所有角色
    private string tTeam;       //接收指定隊伍的類型名稱
    GameEM.TeamType tTeamEM;    //將參數轉成 Eunm 類型用

    BaseRoleControllV2 _BaseRoleControl;

    public GameControllSwitchingTaskAims() :
    base((int)EM_GameControllAction.SwitchingTaskAims)
    { }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj; //當前任務
        tTeam = _CurGameControllDT.szData1;       //接收指定隊伍名稱
        tTeamEM = (GameEM.TeamType)Enum.Parse(typeof(GameEM.TeamType), tTeam, true); //將string轉成Eunm (第三個參數表示是否在意大小寫)
        StartRun();
    }

    public override void f_Execute()
    {
        if (IsRuning())
        {
            tRole = BattleMain.GetInstance().m_BattleRolePool.f_FindTeamTarget(tTeamEM); //獲取所有指定隊伍的角色
            if (tRole.Count != 0) //如果指定的隊伍有玩家
            {
                for (int i = 0; i < tRole.Count; i++)
                {
                    //MessageBox.ASSERT(tRole[i].m_iId + " / " + tRole[i].name + " / " + tRole[i].f_GetTeamType());
                    if (tRole[i].GetComponent<MySelfPlayerControll2>() != null)
                    {
                        tRole[i].GetComponent<MySelfPlayerControll2>().f_GetAims(_CurGameControllDT.szData2, _CurGameControllDT.szData3); //
                    }
                    if (i == tRole.Count - 1)
                    {
                        EndRun();
                    }
                }
            }
            //_BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(0);
            //_BaseRoleControl.GetComponent<MySelfPlayerControll2>().f_ChangeWeapon(_CurGameControllDT.szData2); //角色換槍
            //
        }
    }

}

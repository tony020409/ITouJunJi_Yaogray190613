using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllRead : GameControllBaseState
{

    private int _iConditionId;

    public GameControllRead(int iConditionId)
        : base((int)EM_GameControllAction.Read) {
        _iConditionId = iConditionId;
    }


    public override void f_Enter(object Obj) {

        if (StaticValue.m_bIsMaster) {
            int _iActionId = -99;
            if (Obj != null) {
                _iActionId = (int)Obj;
                if (_iActionId > 0) {
                    _CurGameControllDT = (GameControllDT)glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetSC(_iActionId);
                    BattleMain.GetInstance().GCA_reserveList.Add(_iActionId);
                }
            }

            else {
                if (_CurGameControllDT == null) {
                    MessageBox.ASSERT("GameControllRead 读取的任务Id非法 ");
                }
                else {

                    if (_CurGameControllDT.iEndAction == 0 || _CurGameControllDT.iEndAction == (int)EM_GameControllAction.End) {
                        f_SetComplete((int)EM_GameControllAction.End);
                        return;
                    }
                    else {

                        GameControllDT tGameControllDT = (GameControllDT)glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetSC(_CurGameControllDT.iEndAction);
                        if (tGameControllDT == null) {
                            MessageBox.ASSERT("对应的任务的后续任务未找到 " + _CurGameControllDT.iId + ">>>" + _CurGameControllDT.iEndAction);
                        }
                        _CurGameControllDT = tGameControllDT;
                    }
                }
            }
            Disp();
        }
    }


    private void Disp()
    {
        if (_CurGameControllDT == null) {
            MessageBox.ASSERT("GameControllRead 读取的任务Id非法 ");
        }

        EM_GameControllAction tEM_GameControllAction = (EM_GameControllAction)_CurGameControllDT.iStartAction;
        if (GameControllTools.f_CheckStateTypeIsRight(tEM_GameControllAction)) {        
            
            //同步執行命令
            ServerActionState tServerActionState = new ServerActionState();
            tServerActionState.f_Save(_iConditionId, _CurGameControllDT.iId);
            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tServerActionState);

            // Log保存已經讀到指令的訊息，BattleMain() 也保留紀錄。
            if (BattleMain.GetInstance().Debug_GameControlRead) {
                Debug.LogWarning("<color=yellow>【主线】執行任務[" + _CurGameControllDT.iId + "]: " + tEM_GameControllAction.ToString() + " → " + _CurGameControllDT.szName + " - - - " + _CurGameControllDT.szData1 + " / " + _CurGameControllDT.szData2 + " / " + _CurGameControllDT.szData3 + " / " + _CurGameControllDT.szData4 + "</color>");
            }
            BattleMain.GetInstance().GCC_reserveList.Add(_iConditionId);

            f_SetComplete((int)EM_GameControllAction.Loop);
        }
        else if (_CurGameControllDT.iStartAction == 0) {
            //指令欄位空白(？)
        }
        else {
            MessageBox.ASSERT("【主线腳本】任務[" + _CurGameControllDT.iId.ToString("000") + "] 執行了未知的指令: " + _CurGameControllDT.iStartAction.ToString() 
                + " → " + _CurGameControllDT.szName + " - - - " + _CurGameControllDT.szData1 + " / " + _CurGameControllDT.szData2 + " / " + _CurGameControllDT.szData3);
        }
    }


}

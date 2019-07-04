
using UnityEngine;

public class GameControl_RolePath : GameControllBaseState
{

    public GameControl_RolePath() :
        base((int)EM_GameControllAction.RolePath)
    { }



    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj; //當前腳本
        StartRun();
    }
    

    protected override void Run(object Obj) {
        base.Run(Obj);

        //執行同步動作
        if (StaticValue.m_bIsMaster) {

            //參數 1~3分別是：要執行的角色或物件、要進入的路徑、移動速度(怪物強制用怪物自己的移動速度，物件預設速度1，但也可自訂速度)、到達路徑終點後的寫死事件
            Action_Path tAction = new Action_Path();
            tAction.f_Path( _CurGameControllDT.szData1, _CurGameControllDT.szData2, ccMath.atof(_CurGameControllDT.szData3), _CurGameControllDT.szData4);
            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tAction);
        }
        EndRun();


    }



}



            //if (BattleMain.GetInstance().Debug_GameControlRead) {
            //    Debug.LogError("- 【警告】任務[" + _CurGameControllDT.iId + "] 未找到指定要走路徑的的角色: " + _CurGameControllDT.szData1);
            //}
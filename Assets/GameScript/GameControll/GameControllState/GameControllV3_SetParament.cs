using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllV3_SetParament : GameControllBaseState
{

    int tmp = -99;

    public GameControllV3_SetParament() :
    base((int)EM_GameControllAction.V3_SetParament)
    { }


    public override void f_Enter(object Obj){
        _CurGameControllDT = (GameControllDT)Obj;
        //3001.设置变量的值（参数1为变量名, 参数2为变量值，参数3无效）    
        StartRun();
    }

    protected override void Run(object Obj) {
        base.Run(Obj);

        //參數3 = true 表示數值必須是依序的
        if (_CurGameControllDT.szData3 == "true") {
            //如果修改的參數能轉成int，且目前的數值大於新值不執行參數的變更
            if (int.TryParse(BattleMain.GetInstance().f_GetParamentData(_CurGameControllDT.szData1), out tmp)) {
                if (tmp > ccMath.atoi(_CurGameControllDT.szData2))  {
                    Debug.LogWarning("任務[" + _CurGameControllDT.iId + "] 修改參數指令啟用依序(參數3 = true)，因要修改的數值小於目前值，故不執行參數修改。");
                    EndRun();
                    return;
                }
            }
        }


        BattleMain.GetInstance().f_SetParamentData(_CurGameControllDT.szData1, _CurGameControllDT.szData2);
        EndRun();
    }


    
}



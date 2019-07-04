using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;


public class GameControllV3_Init : ConditionState_Base
{

    public string exParament_1_Name;
    public string exParament_1_Value;

    public GameControllV3_Init(int iId, GameControllPara tGameControllPara) : base(iId, tGameControllPara) {

    }
    
    public override void f_Init(string szParament, string szParamentData, string szData1, string szData2, string szData3, string szData4) {
        base.f_Init(szParament, szParamentData, szData1, szData2, szData3, szData4);
        exParament_1_Name  = szData1;
        exParament_1_Value = szData2;
    }


    //3000.系统初始化指令,只有szParament参数有效（参数1无效,参数2无效，参数3无效，参数4无效）
    public override bool f_Check() {
        base.f_Check();

        if (exParament_1_Name != "") {
            if (BattleMain.GetInstance().f_GetParamentData(exParament_1_Name) != exParament_1_Value) {
                return false;
            }
        }

        return base.f_Check(); ;
    }




}

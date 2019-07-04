using System;
using UnityEngine;
using ccU3DEngine;


public class GameControllRoleMoveAndAnim : GameControllBaseState
{

    //因為有「沒設定第二座標的狀況」需要判斷，所以需要額外弄
    Vector3 newPos2;

    public GameControllRoleMoveAndAnim(): 
        base((int)EM_GameControllAction.MoveAndAnim){ }


    public override void f_Enter(object Obj){
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }


    //2.角色移动（参数1为角色分配的指定KeyId, 参数2:移動點1+到達後的動作, 参数3:移動點2+到達後的動作）
    protected override void Run(object Obj){
        base.Run(Obj);

        //獲取要執行的角色
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1));
        if (tRoleControl == null){
            MessageBox.ASSERT("- 任務[" + _CurGameControllDT.iId + "] 未找到指定的角色: " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }

        //分析資訊----------------------------------------------------------------------------------------
        String[] aPos1   = ccMath.f_String2ArrayString(_CurGameControllDT.szData2, ";"); //分析資訊
        String[] aPos2   = ccMath.f_String2ArrayString(_CurGameControllDT.szData3, ";"); //分析資訊

        Vector3 newPos1 = new Vector3(float.Parse(aPos1[0]), float.Parse(aPos1[1]), float.Parse(aPos1[2])); //取得座標1
        if (_CurGameControllDT.szData3 != ""){                                                              //如果有填寫座標2
           newPos2 = new Vector3(float.Parse(aPos2[0]), float.Parse(aPos2[1]), float.Parse(aPos2[2]));      //取得座標2
        } else{                                                                                             //如果沒填寫座標2
            aPos2 = new string[] { "null","null","null"};                                                   //讓 aPos2.Length = 3方便下面去做判斷
            newPos2 = new Vector3(float.Parse(aPos1[0]), float.Parse(aPos1[1]), float.Parse(aPos1[2]));     //座標2 = 座標1 
        }

        //要等待的情況
        ccCallback tccCallback = CallBack_WalkComplete;

        ////座標1、座標2都要做動作--------------------------------------------------------------------------
        //if (aPos1.Length == 5 && aPos2.Length == 5){       //座標1、座標2都要做動作，且都等待
        //    tRoleControl.f_Walk4Target(newPos1, newPos2, aPos1[3], aPos2[3], tccCallback, true, true);
        //}
        //else if (aPos1.Length == 4 && aPos2.Length == 4) { //座標1、座標2都要做動作，且都不等待
        //    tRoleControl.f_Walk4Target(newPos1, newPos2, aPos1[3], aPos2[3], tccCallback, false, false);
        //}
        //else if (aPos1.Length == 5 && aPos2.Length == 4){ //座標1、座標2都要做動作，但座標1要等待
        //    tRoleControl.f_Walk4Target(newPos1, newPos2, aPos1[3], aPos2[3], tccCallback, true, false);
        //}
        //else if (aPos1.Length == 4 && aPos2.Length == 5){ //座標1、座標2都要做動作，但座標2要等待
        //    tRoleControl.f_Walk4Target(newPos1, newPos2, aPos1[3], aPos2[3], tccCallback, false, true);
        //}

        ////只有座標1要做動作-------------------------------------------------------------------------------
        //else if (aPos1.Length == 5 && aPos2.Length == 3){ //座標1做動作(等待)、座標2不動作
        //    tRoleControl.f_Walk4Target(newPos1, newPos2, aPos1[3], null, tccCallback, true, false);
        //}
        //else if (aPos1.Length == 4 && aPos2.Length == 3){ //座標1做動作(不等待)、座標2不動作
        //    tRoleControl.f_Walk4Target(newPos1, newPos2, aPos1[3], null, tccCallback, false, false);
        //}

        ////只有座標2要做動作-------------------------------------------------------------------------------
        //else if (aPos1.Length == 3 && aPos2.Length == 5){ //座標1不動作、座標2做動作(等待)
        //    tRoleControl.f_Walk4Target(newPos1, newPos2, null, aPos2[3], tccCallback, false, true);
        //}
        //else if (aPos1.Length == 3 && aPos2.Length == 4){ //座標1不動作、座標2做動作(不等待)
        //    tRoleControl.f_Walk4Target(newPos1, newPos2, null, aPos2[3], tccCallback, false, false);
        //}

        ////座標1、座標2都不做動作 -------------------------------------------------------------------------
        //else{
        //    tRoleControl.f_Walk4Target(newPos1, newPos2, null, null, tccCallback, false, false);
        //}


        //不等待的情況
        if (_CurGameControllDT.iNeedEnd == 0){
            tccCallback = null;
            EndRun();
        }
    }


    //要等待的情況
    private void CallBack_WalkComplete(object Obj){
        if (_CurGameControllDT.iNeedEnd == 1){
            EndRun();
        }
    }





}
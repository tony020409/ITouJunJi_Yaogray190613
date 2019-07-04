using System;
using UnityEngine;
using ccU3DEngine;


public class GameControl_RoleGrab : GameControllBaseState
{

    //因為有「沒設定第二座標的狀況」需要判斷，所以需要額外弄
    Vector3 newPos2;

    public GameControl_RoleGrab() :
        base((int)EM_GameControllAction.RoleGrab)
    { }


    public override void f_Enter(object Obj){
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }


    //41.抓玩家（参数1为角色分配的指定KeyId, 参数2:移動點1+到達後的動作, 参数3:移動點2+到達後的動作）
    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //獲取要執行的角色
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1));
        if (tRoleControl == null){
            MessageBox.ASSERT("- 任務[" + _CurGameControllDT.iId + "] 未找到指定的角色: " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }

        //分析資訊----------------------------------------------------------------------------------------
        String[] aPos1 = ccMath.f_String2ArrayString(_CurGameControllDT.szData2, ";"); //分析資訊
        String[] aPos2 = ccMath.f_String2ArrayString(_CurGameControllDT.szData3, ";"); //分析資訊

        Vector3 newPos1 = new Vector3(float.Parse(aPos1[0]), float.Parse(aPos1[1]), float.Parse(aPos1[2])); //取得座標1
        if (_CurGameControllDT.szData3 != ""){                                                              //如果有填寫座標2
            newPos2 = new Vector3(float.Parse(aPos2[0]), float.Parse(aPos2[1]), float.Parse(aPos2[2]));     //取得座標2
        } else {                                                                                            //如果沒填寫座標2
            aPos2 = new string[] { "null", "null", "null" };                                                //讓 aPos2.Length = 3方便下面去做判斷
            newPos2 = new Vector3(float.Parse(aPos1[0]), float.Parse(aPos1[1]), float.Parse(aPos1[2]));     //座標2 = 座標1 
        }

        ////要等待的情況
        //ccCallback tccCallback = CallBack_WalkComplete;
        //tRoleControl.f_GrabPlayer(newPos1, newPos2, 4, 4, 0, tccCallback);   //寫死的：彈飛力道=4 / 彈飛高度=4 / 誤差值=0


        ////不等待的情況
        //if (_CurGameControllDT.iNeedEnd == 0){
        //    tccCallback = null;
        //    EndRun();
        //}
    }


    //要等待的情況
    private void CallBack_WalkComplete(object Obj){
        if (_CurGameControllDT.iNeedEnd == 1){
            EndRun();
        }
    }


}
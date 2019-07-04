using System;
using UnityEngine;
using ccU3DEngine;
using DG.Tweening; //使用DOTween

public class GameControl_RoleJump : GameControllBaseState
{

    public GameControl_RoleJump() :
        base((int)EM_GameControllAction.RoleJump)
    { }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }


    //28.角色跳躍 (參數1是目標座標, 參數2是跳多高, 參數3是跳多久)
    protected override void Run(object Obj)
    {
        base.Run(Obj);


        //獲取要執行的角色--------------------------------------------------------------------------------------------------------------
        BaseRoleControllV2 tRoleControl;                                                       //要執行的角色
        String[] _tmp1 = ccMath.f_String2ArrayString(_CurGameControllDT.szData1, ";");      //分析參數1
        string anim1 = "Jump";                                                              //起跳動作
        if (_tmp1.Length > 1)
        {                                                              //如果有預設起跳動作
            anim1 = _tmp1[1];                                                               //起跳動作=自己設定的動作
            tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_tmp1[0])); //獲取要執行的角色
        }
        else {
            tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1)); //獲取要執行的角色
        }

        if (tRoleControl == null)
        {
            MessageBox.ASSERT("- 任務[" + _CurGameControllDT.iId + "] 未找到指定要跳躍的角色: " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }

        //分析其他資訊------------------------------------------------------------------------------------------------------------------
        ccCallback tccCallback = CallBack_WalkComplete;                                //傳遞事件用
        String[] _tmp2 = ccMath.f_String2ArrayString(_CurGameControllDT.szData2, ";"); //分析參數2
        String[] _tmp3 = ccMath.f_String2ArrayString(_CurGameControllDT.szData3, ";"); //分析參數3
        Vector3 endPos = new Vector3(float.Parse(_tmp2[0]), float.Parse(_tmp2[1]), float.Parse(_tmp2[2])); //取得要跳到的座標
        float JumpPower = float.Parse(_tmp3[0]);                                                 //跳多高
        float Duration = float.Parse(_tmp3[1]);                                                 //跳多久
        int Number = 1;                                                                          //跳幾次(預設1次)                                 
        //if (_tmp3.Length == 3)
        //{                                                                  //如果有設定跳幾次
        //    Number = int.Parse(_tmp3[2]);                                                        //就採用自己的設定值 
        //    tRoleControl.f_Jump2Target(endPos, anim1, JumpPower, Duration, Number, tccCallback); //跳到哪、跳的姿勢、跳多高、跳多久、跳幾次
        //}
        //else {                                                                                 //如果沒設定跳幾次，就預設跳一次
        //    tRoleControl.f_Jump2Target(endPos, anim1, JumpPower, Duration, Number, tccCallback); //跳到哪、跳的姿勢、跳多高、跳多久、跳幾次
        //}

        //不等待的情況
        if (_CurGameControllDT.iNeedEnd == 0)
        {
            tccCallback = null;
            EndRun();
        }

    }


    //要等待的情況
    private void CallBack_WalkComplete(object Obj)
    {
        if (_CurGameControllDT.iNeedEnd == 1)
        {
            EndRun();
        }
    }


}
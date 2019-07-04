using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameControll_Role_Position_Rotation : GameControllBaseState
{

    public GameControll_Role_Position_Rotation() :
      base((int)EM_GameControllAction.Role_Position_Rotation)
    { }


    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj; //當前腳本
        StartRun();
    }


    //29. 角色進入路徑 (參數1是目標座標, 參數2是跳多高, 參數3是跳多久)
    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //獲取要執行的角色--------------------------------------------------------------------------------------------------------------
        int m_iRoleId = ccMath.atoi(_CurGameControllDT.szData1);
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl == null) {
            MessageBox.DEBUG("- 【警告】任務[" + _CurGameControllDT.iId + "] 未找到角色: " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }

        //獲取位置和朝向資訊------------------------------------------------------------------------------------------------------------
        if (_CurGameControllDT.szData2 != "") {
            float[] aPos = ccMath.f_String2ArrayFloat(_CurGameControllDT.szData2, ";");
            tRoleControl.transform.position = new Vector3(aPos[0], aPos[1], aPos[2]); //修改位置
        }
            
        if (_CurGameControllDT.szData3 != "") {
            float[] aRot = ccMath.f_String2ArrayFloat(_CurGameControllDT.szData3, ";");
            tRoleControl.transform.eulerAngles = new Vector3(aRot[0], aRot[1], aRot[2]); //修改朝向
        }


        EndRun();

    }



}

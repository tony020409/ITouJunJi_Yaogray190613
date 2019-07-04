using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllRoleMove2 : GameControllBaseState {
    public GameControllRoleMove2 ()
        : base((int)EM_GameControllAction.RoleMove2) {

    }


    public override void f_Enter (object Obj) {
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }

    protected override void Run (object Obj) {
        base.Run(Obj);

        //2.角色移动（参数1为角色分配的指定KeyId,参数2为移动的目标坐标，参数3无效）
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1));
        if (tRoleControl == null) {
            MessageBox.ASSERT("未找到指定的角色信息 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }
        int[] aPos1 = ccMath.f_String2ArrayInt(_CurGameControllDT.szData2, ";");
        int[] aPos2 = ccMath.f_String2ArrayInt(_CurGameControllDT.szData3, ";");

       MessageBox.DEBUG("移動模式3");
        Vector3 newPos1 = new Vector3(aPos1[0], aPos1[1], aPos1[2]);
        Vector3 newPos2 = new Vector3(aPos2[0], aPos2[1], aPos2[2]);
        //tRoleControl.f_PterHover(newPos1, newPos2);

    }

    private void CallBack_WalkComplete (object Obj) {
        EndRun();
    }





}

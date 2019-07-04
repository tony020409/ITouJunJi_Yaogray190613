using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;


public class GameControlRoleChangeHP : GameControllBaseState
{
    BaseRoleControllV2 tRoleControl;
    private int hpValue;

    public GameControlRoleChangeHP() : 
    base((int)EM_GameControllAction.RoleChangeHP){ }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj; //當前任務
        StartRun();
    }

    //22.角色受到傷害（参数1为角色分配的指定KeyId,参数2为受到的傷害數值，参数3无效）
    protected override void Run(object Obj)
    {
        base.Run(Obj);
        hpValue = ccMath.atoi(_CurGameControllDT.szData2);                                                  //指定變化值
        tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1)); //指定角色

        //如果找不到角色 --------------------------------------------------------------------------------------------
        if (tRoleControl == null)  {
            if (BattleMain.GetInstance().Debug_Object_isNull) {
                Debug.Log("【腳本】步驟[" + _CurGameControllDT.iId + "]未找到指定的角色 id= " + _CurGameControllDT.szData1);
            }
            EndRun();
            return;
        }

        //如果給予的血量變化是正數 ----------------------------------------------------------------------------------
        else if (hpValue > 0) {
            tRoleControl.f_AddHp(hpValue);
            //MessageBox.DEBUG("【腳本】步驟" + _CurGameControllDT.iId + "讓角色:" + _CurGameControllDT.szData1 + " 增加 " + hpValue + "的血量！");
        }

        //如果給予的血量變化是0，表示直接殺死-------------------------------------------------------------------------
        else if (hpValue == 0) {
            //tRoleControl.f_BeAttack(tRoleControl.f_GetHp());
            tRoleControl.f_ForceBeAttack(tRoleControl.f_GetHp());
            //MessageBox.DEBUG("【腳本】步驟" + _CurGameControllDT.iId + "讓角色:" + _CurGameControllDT.szData1 + "失去所有血量(死亡)！");
        }

        //如果給予的血量變化是負數 ----------------------------------------------------------------------------------
        else {
            if (tRoleControl.f_IsInv()) {
                MessageBox.DEBUG("【腳本】步驟" + _CurGameControllDT.iId + "因角色:" + _CurGameControllDT.szData1 + "無敵而無法讓它受到傷害");
            }
            tRoleControl.f_BeAttack(hpValue * -1, -99, GameEM.EM_BodyPart.Body);
            //MessageBox.DEBUG("【腳本】步驟" + _CurGameControllDT.iId + "讓角色:" + _CurGameControllDT.szData1 + " 受到 " + hpValue * (-1) + "的傷害！");
        }
        //結束 --------------------------------------------------------------------------------------------------------
        EndRun();
    }

}

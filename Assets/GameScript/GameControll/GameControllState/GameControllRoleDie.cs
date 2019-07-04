using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllRoleDie : GameControllBaseState
{
    BaseRoleControllV2 _BaseRoleControl;

    public GameControllRoleDie()
        : base((int)EM_GameControllAction.RoleDie)
    {

    }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        //3.角色死亡事件（参数1为角色分配的指定KeyId,参数23无效）
        _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1));
        if (_BaseRoleControl == null)
        {
            MessageBox.DEBUG("未找到指定的角色信息，直接结束任务 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szData1);
            EndRun();
        }
        else
        {
            StartRun();
        }
    }

    public override void f_Execute()
    {
        if (IsRuning())
        {
            if (_BaseRoleControl != null)
            {
                if (_BaseRoleControl.f_IsDie())
                {
                    Debug.LogWarning("代號:" +_BaseRoleControl + " 死亡!");
                    EndRun();
                }
            }
        }
    }
}


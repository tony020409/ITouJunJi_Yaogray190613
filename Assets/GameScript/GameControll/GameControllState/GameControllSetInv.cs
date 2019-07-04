using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllSetInv : GameControllBaseState
{
    BaseRoleControllV2 _BaseRoleControl;
    private bool isChangeInv = false;

    public GameControllSetInv()
        : base((int)EM_GameControllAction.setInv)
    {
    }


    //20.角色Animator事件（参数1为角色分配的指定KeyId,参数2为狀態機，参数3无效）
    public override void f_Enter(object Obj)
    {
        isChangeInv = false;
        _CurGameControllDT = (GameControllDT)Obj;  //當前任務
        //StartRun();
    }

    public override void f_Execute()
    {
        if (isChangeInv == false)
        {
            _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1)); //指定角色
            if (_BaseRoleControl == null)                                                                           //錯誤訊息
            {
                MessageBox.ASSERT("【任務腳本】步驟" + _CurGameControllDT.iId + "未找到角色 :" + _CurGameControllDT.szData1);
                EndRun();
                return;
            }

            if (_BaseRoleControl != null)
            {
                StartRun();
                isChangeInv = true;
            }
        }
    }

    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //設定無敵狀態開關 (1=無敵, 0=解除無敵)
        if (ccMath.atoi(_CurGameControllDT.szData2) == 1)
        {
            _BaseRoleControl.f_SetInv(true);
            //_BaseRoleControl.GetComponent<TrexRoleControl>().isInvMode = true; //測試用(讓TrexRoleControl.cs上可以顯示暴龍是否為無敵)

        }
        else if (ccMath.atoi(_CurGameControllDT.szData2) == 0)
        {
            _BaseRoleControl.f_SetInv(false);
            //_BaseRoleControl.GetComponent<TrexRoleControl>().isInvMode = false; //測試用
        }

        //如果參數3有設定數值，則參數3表示無敵的時間
        if (_CurGameControllDT.szData3 != ""){
            ccTimeEvent.GetInstance().f_RegEvent(ccMath.atoi(_CurGameControllDT.szData3), false, Obj, EndInv);
        }

        EndRun();
    }

    /// <summary>
    /// 結束無敵
    /// </summary>
    private void EndInv(object Obj)
    {
        _BaseRoleControl.f_SetInv(false);
        //_BaseRoleControl.GetComponent<TrexRoleControl>().isInvMode = false; //測試用
    }

}

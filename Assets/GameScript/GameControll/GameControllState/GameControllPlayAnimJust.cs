using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllPlayAnimJust : GameControllBaseState
{

    private BaseRoleControllV2 _BaseRoleControl; //執行的角色
    private bool isChangeAnimator = false;    //為了讓 f_Execute() 只執行一次用
    private string tAnimName;                 //動畫的名稱


    public GameControllPlayAnimJust():
        base((int)EM_GameControllAction.RoleAnimatorJust)
    { }


    //25. 角色Animator事件（参数1为角色分配的指定KeyId,参数2为狀態機，参数3无效）
    public override void f_Enter(object Obj){
        isChangeAnimator = false;                 //為了讓 f_Execute() 只執行一次用
        _CurGameControllDT = (GameControllDT)Obj; //當前任務
    }


    public override void f_Execute()
    {
        if (isChangeAnimator == false) {

            //指定角色
            _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1));

            //錯誤訊息回報
            if (_BaseRoleControl == null) {                                                                          
                MessageBox.ASSERT("【任務腳本】步驟" + _CurGameControllDT.iId + "未找到角色 :" + _CurGameControllDT.szData1);
                EndRun();
                return;
            }

            //執行角色如果存在就叫他播動畫
            else {
                StartRun();
                isChangeAnimator = true;
            }
        }
    }


    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //如果角色死了就不執行動畫
        if (_BaseRoleControl.f_IsDie() == true){
            EndRun();
            return;
        }

        //角色沒死就正常播放動畫
        else{
            _BaseRoleControl.GetComponent<Animator>().CrossFade(_CurGameControllDT.szData2.ToString(), 0.25f); 
        }
    }
}

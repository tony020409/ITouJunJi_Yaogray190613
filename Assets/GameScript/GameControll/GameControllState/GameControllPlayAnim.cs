using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;


//者指令目前只適用在 BattleMain場景上的大兵、暴龍動作控制
public class GameControllPlayAnim : GameControllBaseState
{
    BaseRoleControllV2 _BaseRoleControl;
    private bool isChangeAnimator = false;
    private string tAnimName;


    public GameControllPlayAnim()
        : base((int)EM_GameControllAction.RoleAnimator)
    {
    }
    
    //20.角色Animator事件（参数1为角色分配的指定KeyId,参数2为狀態機，参数3无效）
    public override void f_Enter(object Obj)
    {
        isChangeAnimator = false;
        _CurGameControllDT = (GameControllDT)Obj;
    }

    public override void f_Execute(){

        //如果動畫切換了
        //if (isChangeAnimator == true)
        //{
        //    //當動畫結束時
        //    if (_BaseRoleControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        //    {
        //        Debug.LogWarning("結束.............................................");
        //        _BaseRoleControl.f_ChangeAIState(AI_EM.EM_AIState.Idle);
        //        EndRun();
        //    }
        //    //還沒結束，暴龍就被殺死的情況
        //    if (_BaseRoleControl == null)
        //    {
        //        EndRun();
        //    }
        //}

        if (isChangeAnimator == false)
        {
            _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1));       //指定角色
            if (_BaseRoleControl == null)                                                                                 //如果找不到角色
            {
                MessageBox.ASSERT("【腳本】步驟" + _CurGameControllDT.iId + "未找到角色 :" + _CurGameControllDT.szData1);
                EndRun();
                return;
            } //就回報錯誤訊息

            //如果找到角色了
            if (_BaseRoleControl != null)
            {
                StartRun();
                isChangeAnimator = true; //結束Execute()
            }
        }
    }

    protected override void Run(object Obj){
        base.Run(Obj);
   
    }


    //動畫結束調用用 ----------------------------------------
    private void Callback_MvPlayComplete(object Obj){
        MessageBox.DEBUG("結尾");
        EndRun();
    }
}




/*
GameControllPlayAnim
    ↓
BaseRoleControl
    ↓
BaseAIControl 
    ↓
TrexAIControll再覆寫一次
    ↓
AI_Animator#

*/


/*
//public void ForceCrossFade(this Animator animator, string name, float transitionDuration, int layer = 0, float normalizedTime = float.NegativeInfinity)
//{
//    animator.Update(0); if (animator.GetNextAnimatorStateInfo(layer).fullPathHash == 0)
//    {
//        animator.CrossFade(name, transitionDuration, layer, normalizedTime);
//    }
//    else
//    {
//        animator.Play(animator.GetNextAnimatorStateInfo(layer).fullPathHash, layer);
//        animator.Update(0);
//        animator.CrossFade(name, transitionDuration, layer, normalizedTime);
//    }
//}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllRoleAttack : GameControllBaseState
{
    public GameControllRoleAttack()
        : base((int)EM_GameControllAction.RoleAttack)
    {

    }

    // Enter ===============================================
    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }


    // Run =================================================
    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //10.角色攻击 (参数1为角色分配的指定KeyId, 参数2无效，参数3无效）
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1));
        if (tRoleControl == null)
        {
            MessageBox.ASSERT("未找到指定的角色信息 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }
        RoleAttackAction tRoleAttackAction = new RoleAttackAction();
        //纯播放攻击动画
        tRoleAttackAction.f_Attack(tRoleControl.m_iId, tRoleControl.f_GetTeamType(), -99);
        tRoleControl.f_RunAIState(AI_EM.EM_AIState.Attack, tRoleAttackAction);
        if (_CurGameControllDT.iNeedEnd == 1)
        {
            ccTimeEvent.GetInstance().f_RegEvent(_CurGameControllDT.fEndSleepTime, false, Obj, CallBack_IdelComplete);
        }
    }


    private void CallBack_IdelComplete(object Obj)
    {
        EndRun();
    }





}
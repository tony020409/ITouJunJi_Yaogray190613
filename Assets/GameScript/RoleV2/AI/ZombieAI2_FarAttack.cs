using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieAI2_FarAttack : AI_RunBaseStateV2
{

    private Vector3 tmpLookAtPos;                  //怪物朝向位置 (目標去掉Y軸)
    private Module_Shoot_TwoHand ModuleShoot2;     //開槍模組
    private BaseRoleControllV2 _ReadyAttackTarget; //攻擊目標

    public ZombieAI2_FarAttack()
        : base(AI_EM.EM_AIState.ZombieAI2_FarAttack) { }


    public override void f_Enter(object Obj){
        base.f_Enter(Obj);

        ModuleShoot2 = _BaseRoleControl.GetComponent<Module_Shoot_TwoHand>(); //取得射擊模組(雙手)
        if (ModuleShoot2 != null) {
            ModuleShoot2.BulletAmount = ccMath.atoi(_CharacterAIRunDT.szData3); //取得攜彈量
            ModuleShoot2.f_Init();                                              //射擊模組初始化
            _ReadyAttackTarget = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetViewSize());
            if (_ReadyAttackTarget != null) {
                ModuleShoot2.IKSee_SetTarget(_ReadyAttackTarget.transform); //設定瞄準對象
            }
        }


    }


    public override void f_Execute() {
        base.f_Execute();

        //检测到目标对象不存在，结束整个AI状态机
        if (_ReadyAttackTarget == null) {
            f_RunStateComplete();
            return;
        }

        //检测到目标对象已死亡，结束整个AI状态机
        if (_ReadyAttackTarget.f_IsDie()) {
            f_RunStateComplete();
            return;
        }


        if (_BaseRoleControl.f_GetHp() <= 0) {
            _BaseRoleControl.f_Die();
            return;
        }


        //取得敵人位置
        //tmpLookAtPos = _ReadyAttackTarget.transform.position;   //取得位置
        //tmpLookAtPos.y = _BaseRoleControl.transform.position.y; //忽略Y軸

        //敵人超出攻擊範圍，结束整个AI状态机
        //if (Vector3.Distance(_BaseRoleControl.transform.position, tmpLookAtPos) > _BaseRoleControl.f_GetAttackSize()) {
        //    f_RunStateComplete();
        //}
        //
        ////朝向玩家
        //else {
        //    //if (ModuleShoot._bSee == true) {                      //如果在瞄準狀態
        //    //    _BaseRoleControl.transform.LookAt(tmpLookAtPos);  //怪物面向玩家
        //    //}
        //}

    }


    public override void f_Exit() {
        base.f_Exit();
        if (ModuleShoot2 != null){
            ModuleShoot2.ResetShoot(); //重置射擊模組
        }
    }


}

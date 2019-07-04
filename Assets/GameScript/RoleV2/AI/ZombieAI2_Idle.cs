using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI2_Idle : AI_RunBaseStateV2
{

    public ZombieAI2_Idle()
        : base(AI_EM.EM_AIState.ZombieAI2_Idle) { }
    private NavMeshAgent Agent;


    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);
        Agent = _BaseRoleControl.GetComponent<NavMeshAgent>();
        if (Agent != null) {
            if (Agent.enabled == true){
                Agent.speed = _BaseRoleControl.f_GetWalkSpeed(); //取得移動速度
                Agent.Warp(_BaseRoleControl.transform.position); //設定起點
            }
        }
    }


    public override void f_Execute() {
        base.f_Execute();
        CheckEnemy();
    }

    public override void f_Exit() {
        base.f_Exit();
    }


    /// <summary>
    /// 檢查有無敵人
    /// </summary>
    public void CheckEnemy() {

        if (_BaseRoleControl.f_GetHp() <= 0){
            _BaseRoleControl.f_Die();
            return;
        }

        //視野有敵人的話，結束當前AI
        BaseRoleControllV2 tmpEnemy = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetViewSize());
        if (tmpEnemy != null) {
            f_RunStateComplete();
        }
    }




}

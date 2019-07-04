using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class ZombieAI2_Chase : AI_RunBaseStateV2
{

    public ZombieAI2_Chase()
        : base(AI_EM.EM_AIState.ZombieAI2_Chase) { }


    private NavMeshAgent Agent;
    private BaseRoleControllV2 tmpEnemy;

    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);
        Agent = _BaseRoleControl.GetComponent<NavMeshAgent>();
        if (Agent != null) {
            //Agent.isStopped = false;                       //開啟官方尋路
            Agent.enabled = true;
            Agent.speed = _BaseRoleControl.f_GetWalkSpeed(); //取得移動速度
            Agent.Warp(_BaseRoleControl.transform.position); //設定起點
        }
        tmpEnemy = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetViewSize());
    }


    public override void f_Execute(){
        base.f_Execute();
        CheckEnemyInAttack();  //檢查敵人位置與狀態
        if (tmpEnemy!=null) {
            Agent.SetDestination(tmpEnemy.transform.position);
        }
    }


    public override void f_Exit(){
        base.f_Exit();
        if (Agent != null) {
            Agent.enabled = false;
            //Agent.isStopped = true; //關閉官方尋路
        }
    }


    /// <summary>
    /// 檢查敵人位置與狀態
    /// </summary>
    private void CheckEnemyInAttack() {

        if (_BaseRoleControl.f_GetHp() <= 0) {
            _BaseRoleControl.f_Die();
            return;
        }

        //視野有敵人的話
        if (tmpEnemy != null)  {

            //找不到敵人，結束當前AI
            if (tmpEnemy == null) {
                f_RunStateComplete();
                return;
            }

            //敵人死了，結束當前AI
            if (tmpEnemy.f_IsDie()){
                f_RunStateComplete();
                return;
            }

            //當敵人進到攻擊範圍，結束當前AI
            Vector3 tmpPos = tmpEnemy.transform.position;
            tmpPos.y = _BaseRoleControl.transform.position.y;
            if (Vector3.Distance(_BaseRoleControl.transform.position, tmpPos) < _BaseRoleControl.f_GetAttackSize()) {
                f_RunStateComplete();
                return;
            }
        }

        //敵人超出視野，結束當前AI
        else {
            f_RunStateComplete();
        }
    }


}

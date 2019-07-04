
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI2_NearAttack : AI_RunBaseStateV2
{

    public ZombieAI2_NearAttack()
        : base(AI_EM.EM_AIState.ZombieAI2_NearAttack) { }
    
    
    private BaseRoleControllV2 tmpEnemy; //要攻擊的對像
    private Vector3 tmpLookAtPos;        //怪物朝向位置 (目標去掉Y軸)
    private Quaternion tmpRotation;      //怪物朝向(緩衝LookAt用)

    public override void f_Enter(object Obj)  {
        base.f_Enter(Obj);
        tmpEnemy = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetAttackSize());
    }


    public override void f_Execute() {
        base.f_Execute();

        if (_BaseRoleControl.f_GetHp() <= 0){
            _BaseRoleControl.f_Die();
            return;
        }

        //視野有敵人的話
        if (tmpEnemy != null) {
            //當敵人離開攻擊範圍，結束當前AI
            Vector3 tmpPos = tmpEnemy.transform.position;
            tmpPos.y = _BaseRoleControl.transform.position.y;
            if (Vector3.Distance(_BaseRoleControl.transform.position, tmpPos) > _BaseRoleControl.f_GetAttackSize()) {
                f_RunStateComplete();
                return;
            }

            //敵人死了，結束當前AI
            if (tmpEnemy.f_IsDie()) {
                f_RunStateComplete();
                return;
            }

            tmpLookAtPos = tmpEnemy.transform.position;
            tmpLookAtPos.y = transform.position.y;
            tmpRotation = Quaternion.LookRotation(tmpLookAtPos - transform.position);
            _BaseRoleControl.transform.rotation = Quaternion.Slerp(transform.rotation, tmpRotation, Time.deltaTime * 1.0f);

        }


        //敵人不存在，結束當前AI
        else {
            f_RunStateComplete();
        }
    }


    public override void f_Exit(){
        base.f_Exit();
    }
    

}

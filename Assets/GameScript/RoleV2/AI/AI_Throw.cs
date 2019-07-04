using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Throw : AI_RunBaseStateV2
{

    private Vector3 tmpLookAtPos;                  //怪物朝向位置 (=目標去掉Y軸)
    private BaseRoleControllV2 _ReadyAttackTarget; //攻擊目標

    public AI_Throw()
        : base(AI_EM.EM_AIState.Throw) { }


    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        _ReadyAttackTarget = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetViewSize());
        GameObject oBullet = null;
        oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateResource(_CharacterAIRunDT.szData1, _CharacterAIRunDT.szData2); //產生資源
        if (oBullet != null) {                                                                                                     //如果資源存在
            //oBullet.transform.position = new Vector3(m_CreatePosX, m_CreatePosY, m_CreatePosZ);                                    //設定資源位置
            //oBullet.transform.rotation = new Quaternion(m_CreateRotX, m_CreateRotY, m_CreateRotZ, m_CreateRotW);                   //設定資源朝向
            //BaseBullet tBaseBullet = oBullet.GetComponent<BaseBullet>();                                                           //取得子彈元件
            //tBaseBullet.f_Fired((GameEM.TeamType)m_TeamType, m_BulletSpeed, m_BulletLife, m_RoleId, m_AttackPower);                //子彈發動攻擊
        }
        



    }


    public override void f_Execute() {
        base.f_Execute();

        tmpLookAtPos = _ReadyAttackTarget.transform.position;   //取得敵人位置
        tmpLookAtPos.y = _BaseRoleControl.transform.position.y; //忽略敵人位置的Y軸

        //检测到目标对象不存在，结束整个AI状态机
        if (_ReadyAttackTarget == null) {
            f_RunStateComplete();
        }

        //检测到目标对象已死亡，结束整个AI状态机
        else if (_ReadyAttackTarget.f_IsDie()) {
            f_RunStateComplete();
        }

        //朝向玩家
        else {
            _BaseRoleControl.transform.LookAt(tmpLookAtPos);        //怪物面向玩家
        }

    }


    public override void f_Exit() {
        base.f_Exit();


    }


}

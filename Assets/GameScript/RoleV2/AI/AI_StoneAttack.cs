using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击动作
/// </summary>
public class AI_StoneAttack : AI_RunBaseStateV2
{

    // AI類型宣告
    public AI_StoneAttack() : base(AI_EM.EM_AIState.StoneAttack) { }

    //攻擊目標
    private BaseRoleControllV2 _ReadyAttackRoleControl = null;

    public override void f_Enter(object Obj)
    {
        //MessageBox.DEBUG(_BaseRoleControl.m_iId + " AI_Attack2");
        base.f_Enter(Obj);
        if (_CurAction != null)
        {
            RoleAttackAction tRoleAttackAction = (RoleAttackAction)_CurAction;
            if (tRoleAttackAction.m_iBeAttackRoleId == -99)
            {//纯播放攻击动画

            }
            else
            {
                _ReadyAttackRoleControl = BattleMain.GetInstance().m_BattleRolePool.f_Get(tRoleAttackAction.m_iBeAttackRoleId);
                MessageBox.DEBUG("代號" + _BaseRoleControl.m_iId + " AI_Attack >> " + "代號" + _ReadyAttackRoleControl.m_iId);
            }
        }
    }

    private Vector3 _LookPos;
    public override void f_Execute()
    {
        base.f_Execute();
        if (_ReadyAttackRoleControl != null)
        {
            if (_LookPos != _ReadyAttackRoleControl.transform.position)
            {
                //_RoleControl.transform.rotation = Quaternion.Slerp(_RoleControl.transform.rotation, Quaternion.LookRotation(_ReadyAttackRoleControl.transform.position - _RoleControl.transform.position), 2f * Time.deltaTime);
                Vector3 v1 = new Vector3(_ReadyAttackRoleControl.transform.position.x, _ReadyAttackRoleControl.transform.position.y, _ReadyAttackRoleControl.transform.position.z);
                //如果角色沒被彈起 就看向被攻擊的對象
                if (_ReadyAttackRoleControl.f_IsSpring() == false)
                {
                    ChangeFace(v1);
                }
                //ChangeFace(_ReadyAttackRoleControl.transform.position);
                _LookPos = v1;
            }
        }
    }

    /// <summary> 来此动作的触发伤害 </summary> ============================================================
    public void f_Callback_MV_Attack()
    {
        if (StaticValue.m_bIsMaster)
        {
            if (_ReadyAttackRoleControl != null && !_ReadyAttackRoleControl.f_IsDie())
            {
                RoleHpAction tRoleHpAction = new RoleHpAction();
                tRoleHpAction.f_Hp(_BaseRoleControl.m_iId, _ReadyAttackRoleControl.m_iId, _BaseRoleControl.f_GetAttackPower(), Vector3.zero);
                _BaseRoleControl.f_AddMyAction(tRoleHpAction);

                //if (_BaseRoleControl.f_GetRoleType() == GameEM.emRoleType.Archer)
                //{//远程攻击
                //    Vector3 v3TargetPos = _ReadyAttackRoleControl.f_GetBeAttackPos();
                //    Vector3 PlayerPos = _BaseRoleControl.transform.position;
                //    PlayerPos -= (Vector3.up * 0.25f);
                //    //Vector3 ShootPlacePos = ShootPlace.position;
                //    Vector3 v3StartPos = PlayerPos; // _BaseRoleControl.ShootPlace.position;
                //    Quaternion rotation = Quaternion.LookRotation(v3TargetPos - PlayerPos);

                //    RoleArrowAttackAction tRoleArrowAttackAction = new RoleArrowAttackAction();
                //    tRoleArrowAttackAction.f_Attack(_BaseRoleControl.m_iId, GameEM.GunEM.Rifle, _BaseRoleControl.f_GetTeamType(), _ReadyAttackRoleControl.m_iId, v3StartPos, rotation, 2);
                //    _BaseRoleControl.f_AddMyAction(tRoleArrowAttackAction);
                //}
                //else {
                //    //Debug.LogWarning("代號" + _BaseRoleControl.m_iId + " RoleHpAction ");
                //    RoleHpAction tRoleHpAction = new RoleHpAction();
                //    tRoleHpAction.f_Hp(_ReadyAttackRoleControl.m_iId, _BaseRoleControl.f_GetAttackPower(), Vector3.zero);
                //    _BaseRoleControl.f_AddMyAction(tRoleHpAction);
                //}
            }
            f_RunStateComplete();
        }
    }


}


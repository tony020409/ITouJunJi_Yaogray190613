using UnityEngine;
using ccU3DEngine;
using System.Collections.Generic;

public class AI_PlayerIdle : AI_RunBaseStateV2
{

    MySelfPlayerControll2 _MySelfPlayerControll2;
    public AI_PlayerIdle( )
        : base(AI_EM.EM_AIState.Idle)  {
    }
        

    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);
        //自己
        if (_BaseRoleControl.m_iId == StaticValue.m_UserDataUnit.m_PlayerDT.m_iId) {
            _MySelfPlayerControll2 = (MySelfPlayerControll2)_BaseRoleControl;
            _MySelfPlayerControll2.HPImage.fillAmount = (float)_BaseRoleControl.f_GetHp() / _MySelfPlayerControll2.f_GetMaxHp();
            _MySelfPlayerControll2._Lutifybool = true;                         //不倒數
            _MySelfPlayerControll2.DeathreciprocalGameObject.SetActive(false); //關閉死亡倒數
            if (_BaseRoleControl.f_GetHaveLife() > 0) {
                _MySelfPlayerControll2.DieTipObj.SetActive(false);
            }

            if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Lost) {
                _MySelfPlayerControll2._Lutify.Blend = 1; //畫面變黑
            }
            else {
                _MySelfPlayerControll2._Lutify.Blend = 0; //畫面變亮
            }
        }

        //其它玩家
        else {

        }
    }


    public override void f_Execute() {
        base.f_Execute();

        //自己
        if (_BaseRoleControl.m_iId == StaticValue.m_UserDataUnit.m_PlayerDT.m_iId)  {
            if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Lost) {
                _MySelfPlayerControll2._Lutify.Blend = 1;
            }
            else {
                _MySelfPlayerControll2._Lutify.Blend = 0;
            }
        }

        //其它玩家
        else {

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionState_RoleHp : ConditionState_Base
{
    BaseRoleControllV2 _BaseRoleControl;
    int _iHp;

    public ConditionState_RoleHp(int iId, GameControllPara tGameControllPara) :base(iId, tGameControllPara)
    {

    }

    //4.角色血量事件（参数1为角色分配的指定KeyId,参数2为触发血量，参数3无效）
    public override void f_Init(string szParament, string szParamentData, string szData1, string szData2, string szData3, string szData4)
    {
        base.f_Init(szParament, szParamentData, szData1, szData2, szData3, szData4);

        _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(szData1));
        _iHp = ccMath.atoi(szData2);
        
        if (_BaseRoleControl == null)
        {
            MessageBox.ASSERT("未找到指定的角色信息 ConditionState_RoleRangCheck" + " " + szData1);
            return;
        }

    }

    public override bool f_Check()
    {
        if (!base.f_Check())
        {
            return false;
        }
        if (_BaseRoleControl.f_GetHp() <= _iHp)
        {
            return true;
        }
        return false;
    }




}
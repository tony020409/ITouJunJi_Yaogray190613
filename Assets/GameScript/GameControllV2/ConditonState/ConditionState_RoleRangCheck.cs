using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionState_RoleRangCheck : ConditionState_Base
{
    BaseRoleControllV2 _BaseRoleControl;
    float _fRang;
    int _iPeopleCount;

    public ConditionState_RoleRangCheck(int iId, GameControllPara tGameControllPara) :base(iId, tGameControllPara)
    {

    }

    //2002.当有其它阵营角色进入有效检测范围时触发执行参数3里指令的任务（参数1为角色分配的指定KeyId,参数2为检测范围，参数3进入检测范围内角色的数量）
    public override void f_Init(string szParament, string szParamentData, string szData1, string szData2, string szData3, string szData4)
    {
        base.f_Init(szParament, szParamentData, szData1, szData2, szData3, szData4);

        _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(szData1));
        _fRang = ccMath.atof(szData2);
        _iPeopleCount = ccMath.atoi(szData3);
        
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

        List<BaseRoleControllV2> tBaseRoleControl = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemyAll2(_BaseRoleControl, _fRang);
        if (tBaseRoleControl.Count >= _iPeopleCount)
        {
            return true;
        }
        return false;
    }




}
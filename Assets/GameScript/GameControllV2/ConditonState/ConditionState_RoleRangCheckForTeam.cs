using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionState_RoleRangCheckForTeam : ConditionState_Base
{
    private GameEM.TeamType _TeamType;
    private float _fRang = 0;
    private int _iPeopleCount = 0;
    BaseRoleControllV2 _BaseRoleControl;
    private int _iRoleId;

    public ConditionState_RoleRangCheckForTeam(int iId, GameControllPara tGameControllPara) :base(iId, tGameControllPara)
    {

    }

    //2007.当有其它阵营角色是否进入有效的检测范围（参数1为角色分配的指定KeyId,参数2为有效检测队伍，参数3为检测范围，参数4进入检测范围内角色的数量）
    public override void f_Init(string szParament, string szParamentData, string szData1, string szData2, string szData3, string szData4) {
        base.f_Init(szParament, szParamentData, szData1, szData2, szData3, szData4);
        _BaseRoleControl = null;
        _iRoleId = ccMath.atoi(szData1);
        _TeamType = (GameEM.TeamType) Enum.Parse(typeof(GameEM.TeamType), szData2, true); //將string轉成Eunm (第三個參數表示是否在意大小寫)
        _fRang = ccMath.atof(szData3);
        if (szData4.ToLower() != "auto") {
            _iPeopleCount = ccMath.atoi(szData4);
        } else {
            _iPeopleCount = -99; //如果0以外的玩家未登入，這裡抓指定隊伍的玩家人數好像會不正確，所以在f_Check()時才抓人數
        }

    }


    public override bool f_Check() {
        if (!base.f_Check()) {
            return false;
        }

        //取得當偵測點的怪
        _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(_iRoleId);
        if (_BaseRoleControl == null) {
            return false;
        }

        //自動計算一次指定隊伍的人數
        if (_iPeopleCount == -99) {
            _iPeopleCount = Data_Pool.m_PlayerPool.GetTeamPlayerCount(_TeamType);
        }

        //計算範圍內指定隊伍的人數是否符合要求
        List<BaseRoleControllV2> aData = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetFriendAll2(_TeamType, _BaseRoleControl.transform.position, _fRang, true, false, true);
        if (aData.Count == _iPeopleCount){
            return true;
        }
        return false;
    }




}
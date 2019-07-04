using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionState_ObjectRangCheckForTeam : ConditionState_Base
{

    private GameObject tGameObj = null; //偵測點的參考物件 (取得位置用)
    private Vector3 _Pos;               //偵測點的位置
    private GameEM.TeamType _TeamType;  //要偵測的隊伍
    private float _fRang = 0;           //偵測距離
    private int _iPeopleCount = 0;      //偵測人數
    private int m_iId;


    public ConditionState_ObjectRangCheckForTeam (int iId, GameControllPara tGameControllPara) 
        : base(iId, tGameControllPara)
    {
        m_iId = iId;
    }


    //2010.指定阵营的角色，进入指定「物件」的指定範圍內，触发指令任务 (參數1=參考物件的名稱, 參數2=要偵測的隊伍, 參數三=偵測範圍, 參數4=偵測人數)
    public override void f_Init(string szParament, string szParamentData, string szData1, string szData2, string szData3, string szData4) {
        base.f_Init(szParament, szParamentData, szData1, szData2, szData3, szData4);

        //取得參考物件的位置
        tGameObj = BattleMain.GetInstance().f_GetGameObj(szData1);
        if (tGameObj != null) {
            _Pos = tGameObj.transform.position;
        } else {
            _Pos = new Vector3(0,0,0);
        }

        //取得指定隊伍資訊 (將string轉成Eunm (第三個參數表示是否在意大小寫))
        _TeamType = (GameEM.TeamType) Enum.Parse(typeof(GameEM.TeamType), szData2, true);


        //取得範圍資訊
        _fRang = ccMath.atof(szData3);

        //取得人數資訊
        if (szData4.ToLower() != "auto") {
            _iPeopleCount = ccMath.atoi(szData4);
        }  else {
            _iPeopleCount = -99; //如果0以外的玩家未登入，這裡抓指定隊伍的玩家人數好像會不正確，所以在f_Check()時才抓人數
        }

    }


    //-------------------------------------------
    public override bool f_Check() {
        if (!base.f_Check()) {
            return false;
        }

        //檢查當偵測點的參考物件是否存在
        if (tGameObj == null) {
            return false;
        }

        //如果是自動計算人數，就去抓一次玩家人數的資訊 (人數從-99變成玩家人數)
        if (_iPeopleCount == -99) {
            _iPeopleCount = Data_Pool.m_PlayerPool.GetTeamPlayerCount(_TeamType);
        }


        //計算範圍內指定隊伍的人數是否符合要求
        List<BaseRoleControllV2> aData = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetFriendAll2(_TeamType, _Pos, _fRang, true, false, true);
        if (aData.Count == _iPeopleCount) {
            //MessageBox.DEBUG("<color=green>條件表[" + m_iId + "] 物件範圍偵測達成，將執行指令[" + );
            return true;
        }

        return false;
    }




}
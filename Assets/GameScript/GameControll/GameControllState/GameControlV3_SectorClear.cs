using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 消除指定物件周圍指定範圍的怪物 (未測試)
/// </summary>
public class GameControlV3_SectorClear : GameControllBaseState
{
    GameObject oGameObj = null;                  // (參數1) 位置參考的物件
    private float _fRange = 0.5f;                // (參數2) 指定的範圍
    private GameEM.TeamType _TeamType;           // (參數3) 指定的隊伍
    private GameEM.emRoleType[] _IgnoreRoleType; // (參數4) 隊伍內忽略的類型


    public GameControlV3_SectorClear() :
    base((int)EM_GameControllAction.V3_SectorClear)
    { }


    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }

    protected override void Run(object Obj) {
        base.Run(Obj);

        //指定位置的參考物件
        oGameObj = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData2);
        if (oGameObj == null) {
            EndRun();
            return;
        }

        //取得要清除的範圍
        if (_CurGameControllDT.szData2 != "") {
            _fRange = ccMath.atof(_CurGameControllDT.szData3);
        } else {
            _fRange = 0.5f;
        }

        //取得要清除的隊伍
        //將string轉成Eunm (第三個參數表示是否在意大小寫)
        _TeamType = (GameEM.TeamType) Enum.Parse(typeof(GameEM.TeamType), _CurGameControllDT.szData3, true);


        //取得忽略的類型
        if (_CurGameControllDT.szData4 != "") {
            int[] aPos = ccMath.f_String2ArrayInt(_CurGameControllDT.szData4, ";");
            if (aPos.Length == 0) {
                _IgnoreRoleType = new GameEM.emRoleType[0];
                _IgnoreRoleType[0] = (GameEM.emRoleType) Enum.Parse(typeof(GameEM.emRoleType), _CurGameControllDT.szData4, true);
            }
            else {
                _IgnoreRoleType = new GameEM.emRoleType[aPos.Length];
                for (int i = 0; i < aPos.Length - 1; i++) {
                    _IgnoreRoleType[i] = (GameEM.emRoleType)aPos[i];
                }
            }
        }


        //清除怪物
        List<BaseRoleControllV2> aData = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetFriendAll2(_TeamType, oGameObj.transform.position, _fRange);
        for (int i=0; i<aData.Count; i++) {

            //判斷是否忽略
            if (_CurGameControllDT.szData4 != "") {
                for (int k = 0; i < _IgnoreRoleType.Length - 1; k++) {
                    if (aData[i].f_GetRoleType() == _IgnoreRoleType[k]) {
                        continue;
                    }
                }
            }

            //清怪
            aData[i].f_ForceBeAttack(aData[i].f_GetHp());
        }


        EndRun();
    }


}
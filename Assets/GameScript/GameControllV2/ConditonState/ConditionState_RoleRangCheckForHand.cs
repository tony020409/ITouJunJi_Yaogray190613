using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//記得到 GameControllCondition 註冊
public class ConditionState_RoleRangCheckForHand : ConditionState_Base
{

    private GameEM.TeamType _TeamType;
    BaseRoleControllV2 _BaseRoleControl;
    private int _iRoleId;
    private float _fRang = 0;



    public ConditionState_RoleRangCheckForHand(int iId, GameControllPara tGameControllPara) 
        :base(iId, tGameControllPara) { }



    //2009.当有玩家角色进入有效检测范围时触发任务（参数1为角色分配的指定KeyId，参数2为检测范围）
    public override void f_Init(string szParament, string szParamentData, string szData1, string szData2, string szData3, string szData4){
        base.f_Init(szParament, szParamentData, szData1, szData2, szData3, szData4);
        _BaseRoleControl = null;
        _iRoleId = ccMath.atoi(szData1);
        _fRang = ccMath.atof(szData2);
    }


    public override bool f_Check()
    {
        if (!base.f_Check()) {
            return false;
        }

        _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(_iRoleId);
        if (_BaseRoleControl == null){
            return false;
        }

        List<BaseRoleControllV2> aData = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemyAll2( _BaseRoleControl, _fRang);
        //List<BaseRoleControllV2> aData2 = BattleMain.GetInstance().f_FindEnemyAllIgnoreY(_BaseRoleControl, _fRang);
        for (int i = 0; i < aData.Count; i++) {
            if (aData[i].GetComponent<MySelfPlayerControll2>() != null) {
                if (Vector3.Distance(aData[i].GetComponent<MySelfPlayerControll2>().m_BulletStart.transform.position, _BaseRoleControl.transform.position) < _fRang) {
                    return true;
                }
            }
            else if (aData[i].GetComponent<OtherPlayerControll2>() != null) {
                if (Vector3.Distance(aData[i].GetComponent<OtherPlayerControll2>().m_BulletStart.position, _BaseRoleControl.transform.position) < _fRang)  {
                    return true;
                }
            }
        }
        return false;
    }




}

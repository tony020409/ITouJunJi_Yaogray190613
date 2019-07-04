using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConditionState_CheckRoleIsNull : ConditionState_Base
{
    private List<BaseRoleControllV2> tRoleData = null;
    private int[] _iRoleIds;

    public ConditionState_CheckRoleIsNull(int iId, GameControllPara tGameControllPara) 
        : base(iId, tGameControllPara)
    { }



    //5.当有其它阵营角色进入有效检测范围时触发执行参数3里指令的任务（参数1为角色分配的指定KeyId）
    public override void f_Init(string szParament, string szParamentData, string szData1, string szData2, string szData3, string szData4){
        base.f_Init(szParament, szParamentData, szData1, szData2, szData3, szData4);
        tRoleData.Clear();
        _iRoleIds = ccMath.f_String2ArrayInt(szData1,";");
    }

    public override bool f_Check()  {

        if (!base.f_Check()) {
            return false;
        }

        //_BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(_iRoleIds[0]);
        //if (_BaseRoleControl == null)  {
        //    return false;
        //}

        return false;
    }




}
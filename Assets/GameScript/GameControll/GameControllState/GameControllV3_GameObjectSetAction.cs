using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllV3_GameObjectSetAction : GameControllBaseState
{
    GameObject _oGameObj = null;
    BaseRoleControllV2 _BaseRoleControl;
    bool _bSetAction = false;
    public GameControllV3_GameObjectSetAction()
        : base((int)EM_GameControllAction.V3_GameObjectSetAction)
    {

    }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT) Obj;
        //2003.设置场景里的GameObject对象显示与不显示（参数1为场景里的GameObject的名字,参数2为true显示 false不显示，参数3无效）
        _oGameObj = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData1);
        if (_oGameObj == null) {
            if (BattleMain.GetInstance().Debug_Object_isNull) {
                Debug.LogWarning("腳本[" + _CurGameControllDT.iId + "] 未找到指定GameObject - " + _CurGameControllDT.szData1);
            }
            EndRun();
            return;
        }



        _CurGameControllDT.szData2 = _CurGameControllDT.szData2.ToLower();
        if (_CurGameControllDT.szData2 == "true") {
            _bSetAction = true;
        }
        else {
            _bSetAction = false;
        }

        StartRun();
    }



    protected override void Run(object Obj)
    {
        if (_oGameObj != null)
        {
            _oGameObj.SetActive(_bSetAction);
            EndRun();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllV3_GameObjDestory : GameControllBaseState
{
    BaseRoleControllV2 _BaseRoleControl;
    GameObject _oGameObj = null;
    public GameControllV3_GameObjDestory()
        : base((int)EM_GameControllAction.V3_GameObjectDestory)
    {

    }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        //2004.设置场景里的GameObject对象销毁（参数1为场景里的GameObject的名字,参数2无效，参数3无效）
        _oGameObj = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData1);
        StartRun();
    }

    protected override void Run(object Obj)
    {
        if (_oGameObj != null)  {
            GameObject.Destroy(_oGameObj);
            EndRun();
        } else {
            MessageBox.ASSERT("未找到指定GameObject未找到 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szData1);
            EndRun();
        }
    }

}

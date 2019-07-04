using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

/// <summary>
/// 2005.GameObject 创建,（参数1为等待创建的GameObject的名字，GameObject的保存位置为：Resources\BattleGameObj,参数2,X;Y;Z世界坐标，参数3X;Y;Z;W世界方向）
/// </summary>
public class GameControllV3_GameObjectCreate : GameControllBaseState
{

    GameObject _oGameObj = null;
    BaseRoleControllV2 _BaseRoleControl;

    public GameControllV3_GameObjectCreate()
        : base((int)EM_GameControllAction.V3_GameObjectCreate) {
    }


    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }


    protected override void Run(object Obj) {

        GameObject tGameObject = glo_Main.GetInstance().m_ResourceManager.f_CreateBattleGameObj(_CurGameControllDT.szData1);
        if (tGameObject == null) {
            MessageBox.ASSERT("未找到指定GameObject未找到 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }
        float[] aPos = ccMath.f_String2ArrayFloat(_CurGameControllDT.szData2, ";");
        float[] aRotation = ccMath.f_String2ArrayFloat(_CurGameControllDT.szData3, ";");

        BattleMain.GetInstance().f_SetChildForGameObj(tGameObject);
        if (aPos.Length == 3) {
            tGameObject.transform.position = new Vector3(aPos[0], aPos[1], aPos[2]);
        }
        if (aRotation.Length == 3) {
            tGameObject.transform.eulerAngles = new Vector3(aRotation[0], aRotation[1], aRotation[2]);
        }
        if (aRotation.Length == 4) {
            tGameObject.transform.rotation = new Quaternion(aRotation[0], aRotation[1], aRotation[2], aRotation[4]);
        }

        EndRun();
    }

}

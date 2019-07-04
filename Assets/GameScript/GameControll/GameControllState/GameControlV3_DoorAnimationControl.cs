using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControlV3_DoorAnimationControl : GameControllBaseState{


    private GameObject _TargetDoor;             //執行的角色
    private DoorAnimation _TargetDoorAnimation; //門動畫程式
    private string szData2ToLow;                //儲存開關門動作文字

    public GameControlV3_DoorAnimationControl():
        base((int)EM_GameControllAction.V3_DoorControl)
    { }



    //2025. 角色Animator事件（参数1為門的名字(門需要放在BattleMain的門清單), 参数2為指定要開還是關，参数3无效）
    public override void f_Enter(object Obj)  {

        //當前任務
        _CurGameControllDT = (GameControllDT)Obj;

        _TargetDoorAnimation = BattleMain.GetInstance().GetDoor2(_CurGameControllDT.szData1);
        if (_TargetDoorAnimation == null) {
            if (BattleMain.GetInstance().Debug_Object_isNull) {
                Debug.LogWarning("腳本 [" + _CurGameControllDT.iId + "] 的門: " + _CurGameControllDT.szData1 + "取得失敗！");
            }
            EndRun();
            return;
        }


        //開門類型
        szData2ToLow = _CurGameControllDT.szData2.ToLower();

        //執行角色如果存在就叫他播動畫
        StartRun();
    }



    protected override void Run(object Obj) {
        base.Run(Obj);
        if (szData2ToLow == "open") {
            _TargetDoorAnimation.OpenDoor();
        }
        else if (szData2ToLow == "close") {
            _TargetDoorAnimation.CloseDoor();
        }
        else if (szData2ToLow == "on") {
            _TargetDoorAnimation.OpenDoor();
        }
        else if (szData2ToLow == "off") {
            _TargetDoorAnimation.CloseDoor();
        }
        else {
            MessageBox.DEBUG("腳本 [" + _CurGameControllDT.iId + "] 參數輸入錯誤!");
        }


        if (_CurGameControllDT.iNeedEnd == 1) {
            ccTimeEvent.GetInstance().f_RegEvent(_TargetDoorAnimation.OpenTime, false, null, DelayEnd);
        }
        else {
            EndRun();
        }

    }


    private void DelayEnd(object Obj) {
        EndRun();
    }


}

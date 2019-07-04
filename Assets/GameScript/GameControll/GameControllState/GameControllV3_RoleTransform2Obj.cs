using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllV3_RoleTransform2Obj : GameControllBaseState
{
    private int _iKeyId;            // (參數1) 指定的怪物id
    GameObject oGameObj = null;     // (參數2) 位置參考的物件
    private float fadeTime = 0.5f;  // (參數3) 畫面變暗、變亮漸變的時間
    private float blackTime = 2.0f; // (參數4) 畫面暗多久才開始變明亮

    public GameControllV3_RoleTransform2Obj() :
    base((int)EM_GameControllAction.V3_RoleTransfor2Obj)
    { }


    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj;
        _CurGameControllDT.iNeedEnd = 1;
        _iKeyId = -99;
        //MessageBox.DEBUG("V3_RoleTransfor2Obj " + _CurGameControllDT.iId);
        StartRun();
    }

    protected override void Run(object Obj)
    {
        base.Run(Obj);
        
        //TileNode tTileNode = null;
        //tTileNode = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndexXY((int)oGameObj.transform.position.x, (int)oGameObj.transform.position.z);     
        //if (tTileNode == null){
        //    MessageBox.ASSERT("位置坐标TileNode未找到 " + _CurGameControllDT.szData2 + ",错误任务Id " + _CurGameControllDT.iId);
        //}

        //要移動的角色
        _iKeyId = ccMath.atoi(_CurGameControllDT.szData1);
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(_iKeyId);
        if (tRoleControl == null) {
            //MessageBox.ASSERT("【警告】腳本[" + _CurGameControllDT.iId + "] 未找到指定的角色: " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }

        //要移動的位置參考物件
        oGameObj = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData2);
        if (oGameObj == null) {
            EndRun();
            return;
        }


        //畫面變暗變亮的過程時間
        if (_CurGameControllDT.szData3 != "") {
            fadeTime = ccMath.atof(_CurGameControllDT.szData3);
        } else {
            fadeTime = 1.0f;
        }


        //黑畫面的的時間
        if (_CurGameControllDT.szData4 != "") {
            blackTime = ccMath.atof(_CurGameControllDT.szData4);
        }  else {
            blackTime = 2.0f;
        }

        ////玩家
        //if (tRoleControl.GetComponent<MySelfPlayerControll2>() != null) { 
        //    BattleMain.GetInstance().SceneFadeTo(1, fadeTime);                                         //玩家畫面慢慢黑掉
        //    ccTimeEvent.GetInstance().f_RegEvent(fadeTime + 0.3f, false, null, CallBack_FadeComplete); //0.8秒後，待畫面變暗完成再移位置，然後恢復畫面
        //}
        //
        ////同步時，不讓其它玩家先跳離開
        //else if (tRoleControl.GetComponent<OtherPlayerControll2>() != null) {
        //    ccTimeEvent.GetInstance().f_RegEvent(fadeTime + 0.3f, false, null, CallBack_FadeComplete); //0.8秒後，待畫面變暗完成再移位置，然後恢復畫面
        //}
        //
        ////其他怪物
        //else {
        //    //tRoleControl.f_SetPos(tTileNode, true);
        //    tRoleControl.transform.position = oGameObj.transform.position;
        //    tRoleControl.transform.localRotation = Quaternion.Euler(oGameObj.transform.localRotation.x, oGameObj.transform.localRotation.y, oGameObj.transform.localRotation.z);
        //    //EndRun();
        //}

        if (tRoleControl.GetComponent<MySelfPlayerControll2>() != null) {
            if (StaticValue.m_bIsMaster) {
                Action_ChangePlayerPos tAction = new Action_ChangePlayerPos();
                tAction.f_Change(_iKeyId, fadeTime + 0.1f, blackTime, oGameObj.transform.position, oGameObj.transform.localEulerAngles);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tAction);
            }
        }
        
        
        else if (tRoleControl.GetComponent<OtherPlayerControll2>() != null) {
            if (StaticValue.m_bIsMaster) {
                Action_ChangePlayerPos tAction = new Action_ChangePlayerPos();
                tAction.f_Change(_iKeyId, fadeTime + 0.1f, blackTime, oGameObj.transform.position, oGameObj.transform.localEulerAngles);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tAction);
            }
        }
        else {
            //tRoleControl.f_SetPos(tTileNode, true);
            tRoleControl.transform.position = oGameObj.transform.position;
            tRoleControl.transform.localRotation = Quaternion.Euler(oGameObj.transform.localRotation.x, oGameObj.transform.localRotation.y, oGameObj.transform.localRotation.z);
        }
        EndRun();
    }



    ///// <summary>
    ///// 玩家畫面變暗完畢後，移動玩家
    ///// </summary>
    //private void CallBack_FadeComplete(object obj) {
    //    BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(_iKeyId);
    //    if (tRoleControl != null) {
    //
    //        //MySelfPlayerControll2
    //        if (tRoleControl.GetComponent<MySelfPlayerControll2>() != null) {
    //            MySelfPlayerControll2 tmpPlayer = tRoleControl.GetComponent<MySelfPlayerControll2>();
    //            tmpPlayer.f_SetPos(oGameObj.transform.position);         //改玩家的位置     
    //            tmpPlayer.f_SetRot(oGameObj.transform.localEulerAngles); //改玩家的朝向
    //        } 
    //
    //    }
    //    ccTimeEvent.GetInstance().f_RegEvent(blackTime, false, null, CallBack_MoveComplete); //指定秒數後，恢復畫面明亮
    //}

    ///// <summary>
    ///// 移動玩家後，在指定的時間內保持黑畫面，時間過後畫面變亮
    ///// </summary>
    //private void CallBack_MoveComplete(object obj) {
    //    BattleMain.GetInstance().SceneFadeTo(0, fadeTime); //玩家畫面慢慢明亮
    //    EndRun();                                          //結束指令
    //}
    
}



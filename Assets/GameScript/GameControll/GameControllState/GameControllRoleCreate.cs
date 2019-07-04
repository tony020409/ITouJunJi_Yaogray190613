using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllRoleCreate : GameControllBaseState
{
    private int _iKeyId;          //指定的怪物id
    private float _x;
    private float _y;
    private float _z;
    private float _rx;
    private float _ry;
    private float _rz;
    private bool isMode1 = false; //是否使用物件座標
    private bool isMode2 = false; //是否使用 xyz座標

    public GameControllRoleCreate() : 
    base((int)EM_GameControllAction.RoleCreate){}


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        //MessageBox.DEBUG("RoleCreate " + _CurGameControllDT.iId);
        //所有的出生行为强制修改成必须等待
        _CurGameControllDT.iNeedEnd = 1;
        _iKeyId = -99;
        StartRun();
    }

    protected override void Run(object Obj)
    {
        base.Run(Obj);
        TileNode tTileNode = null;
        Vector3 tNewPos = new Vector3(0, 0, 0);

        if (_CurGameControllDT.szData4 != "")
        {
            GameObject tGameObj = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData4);
            //tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(tGameObj.transform.position);
            if (tGameObj == null) {
                Debug.LogWarning("腳本 [" + _CurGameControllDT.iId + "]生怪參考物件找不到，跳過執行!");
                EndRun();
            }
            isMode1 = true;
            _x = tGameObj.transform.position.x;     //實際出生的x座標
            _y = tGameObj.transform.position.y;     //實際出生的y座標
            _z = tGameObj.transform.position.z;     //實際出生的z座標
            _rx = tGameObj.transform.eulerAngles.x; //實際出生朝向x
            _ry = tGameObj.transform.eulerAngles.y; //實際出生朝向y
            _rz = tGameObj.transform.eulerAngles.z; //實際出生朝向z
            tNewPos = tGameObj.transform.position;  //
        }
        else
        {
            float[] aPos = ccMath.f_String2ArrayFloat(_CurGameControllDT.szData3, ";");
            //如果有添加出生的指定高度
            if (aPos.Length == 3)
            {
                tTileNode = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndexXY((int)aPos[0], (int)aPos[1]);
                _x = aPos[0];   //實際出生的x座標
                _y = aPos[1];   //實際出生的y座標
                _z = aPos[2];   //實際出生的z座標
                isMode2 = true; //判斷是使用xyz座標出生
            }
            //否則就單純使用節點座標
            else if (aPos.Length == 2)
            {
                tTileNode = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndexXY((int)aPos[0], (int)aPos[1]);
                tNewPos = tTileNode.transform.position;
                isMode2 = false;
            }
            else
            {
                MessageBox.ASSERT("创建角色时角色坐标错误 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szData3);
            }
            //如果找不到節點
            if (tTileNode == null)
            {
                MessageBox.ASSERT("位置坐标未找到 " + (int)aPos[0] + ":" + (int)aPos[1]);
            }
        }

        //設定怪
        _iKeyId = ccMath.atoi(_CurGameControllDT.szData1);
        GameEM.TeamType tTeamType = (GameEM.TeamType)_CurGameControllDT.iTeam;
        CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(ccMath.atoi(_CurGameControllDT.szData2));

        //生怪
        BaseRoleControllV2 tRoleControl = RoleTools.f_CreateRoleForNetWork(_iKeyId, tTeamType, tCharacterDT, tTileNode, 0, tNewPos);

        //設定位置
        SetPosition();

        if (tRoleControl == null) {
            MessageBox.ASSERT("角色创建失败 " + _CurGameControllDT.iId + " " + tCharacterDT.iId);
        }
        DispGameResult(tRoleControl);       
        //MessageBox.DEBUG("腳本 [" + _CurGameControllDT.iId + "] 創建角色 " + tCharacterDT.iId + " 完成！ (" +  ((GameEM.emRoleType)tCharacterDT.iType) + ")");
    }





    /// <summary>
    /// 处理角色对游戏结果的影响 
    /// </summary>
    private void DispGameResult(BaseRoleControllV2 tRoleControl)
    {
        EM_GameResult tEM_GameResult = (EM_GameResult)_CurGameControllDT.iGameResult;
      
        //角色出生时对游戏结果影响0未影响1死亡游戏失败2死亡游戏胜利
        if (tEM_GameResult != EM_GameResult.Default) {
            BattleMain.GetInstance().f_RegRoleDieMission(tRoleControl, tEM_GameResult);
        }       
    }



    public override void f_Execute()
    {
        base.f_Execute();
        //if (_iKeyId > 0 && _CurGameControllDT.iNeedEnd == 1)
        //{
        //    BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(_iKeyId);
        //    //如果怪物生出來了
        //    if (tRoleControl != null) {
        //        //如果用物件座標就跳位置和朝向
        //        if (isMode1) {
        //            SetPosition();
        //        } else {
        //            //結束
        //            EndRun();
        //        }
        //    }
        //}
    }

    private void SetPosition() {
        if (_iKeyId > 0 && _CurGameControllDT.iNeedEnd == 1)
        {
            BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(_iKeyId);
            if (tRoleControl != null){
                //如果用物件座標就跳位置和朝向
                if (isMode1) {
                    tRoleControl.transform.position = new Vector3(_x, _y, _z);
                    tRoleControl.transform.eulerAngles = new Vector3(_rx, _ry, _rz);
                    isMode1 = false;
                }

                //如果用xyz座標就跳位置
                else if (isMode2) {
                    tRoleControl.transform.position = new Vector3(_x, _y, _z);
                    isMode2 = false;
                }

                //抓到物件座標了
                //tRoleControl._bGetObjectPos = true;

                //結束
                EndRun();
            }
        }
    }

}


//tRoleControl.transform.position = new Vector3(tRoleControl.transform.position.x, _Height, tRoleControl.transform.position.z);
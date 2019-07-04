using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllV3_RoleCreateForGameObj : GameControllBaseState
{
    private int _iKeyId;          //指定的怪物id
    private float _Height;          //出生高度
    private bool isMode2 = false; //是否有使用出生高度

    public GameControllV3_RoleCreateForGameObj() :
    base((int)EM_GameControllAction.V3_RoleCreateForGameObj)
    { }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        MessageBox.DEBUG("V3_RoleCreateForGameObj " + _CurGameControllDT.iId);
        //所有的出生行为强制修改成必须等待
        _CurGameControllDT.iNeedEnd = 1;
        _iKeyId = -99;
        StartRun();
    }

    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //2001.根据场景里的GameObject位置创建角色（参数1为角色分配的指定KeyId,参数2为角色模板Id，参数3为角色出生时场景里的GameObject的名字）
        TileNode tTileNode = null;
        GameObject oGameObj = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData3);

        tTileNode = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndexXY((int)oGameObj.transform.position.x, (int)oGameObj.transform.position.z);
               
        //如果找不到節點
        if (tTileNode == null)
        {
            MessageBox.ASSERT("位置坐标未找到 " + _CurGameControllDT.szData3);
        }

        //生怪
        _iKeyId = ccMath.atoi(_CurGameControllDT.szData1);
        GameEM.TeamType tTeamType = (GameEM.TeamType)_CurGameControllDT.iTeam;
        CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(ccMath.atoi(_CurGameControllDT.szData2));
        BaseRoleControllV2 tRoleControl = RoleTools.f_CreateRoleForNetWork(_iKeyId, tTeamType, tCharacterDT, tTileNode, 0);
        if (tRoleControl == null)
        {
            MessageBox.ASSERT("角色创建失败 " + _CurGameControllDT.iId + " " + tCharacterDT.iId);
        }
        DispGameResult(tRoleControl);
        MessageBox.DEBUG("CreateEnd");
    }

    /// <summary>
    /// 处理角色对游戏结果的影响 
    /// </summary>
    private void DispGameResult(BaseRoleControllV2 tRoleControl)
    {
        EM_GameResult tEM_GameResult = (EM_GameResult)_CurGameControllDT.iGameResult;

        //角色出生时对游戏结果影响0未影响1死亡游戏失败2死亡游戏胜利
        if (tEM_GameResult != EM_GameResult.Default)
        {
            BattleMain.GetInstance().f_RegRoleDieMission(tRoleControl, tEM_GameResult);
        }
    }

    public override void f_Execute()
    {
        base.f_Execute();

        if (_iKeyId > 0 && _CurGameControllDT.iNeedEnd == 1)
        {
            BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(_iKeyId);
            //如果怪物生出來了
            if (tRoleControl != null)
            {
                ////如果有給指定高度就變更高度
                //if (isMode2)
                //{
                //    tRoleControl.transform.position = new Vector3(tRoleControl.transform.position.x, _Height, tRoleControl.transform.position.z);
                //    isMode2 = false;
                //}
                //然後結束
                EndRun();
            }
        }
    }

}



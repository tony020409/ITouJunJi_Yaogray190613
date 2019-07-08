using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ProtoContract]
public class RoleBirthAction : GameSysc.Action
{
    [ProtoMember(10601)]
    public int m_iRoleId;

    [ProtoMember(10602)]
    public int m_iCharacterDTId;

    [ProtoMember(10603)]
    public int m_iTileNodeId;

    [ProtoMember(10604)]
    public int m_iTeamType;

    public RoleBirthAction()
            : base()
    {
		m_iType = (int)GameEM.EM_RoleAction.Birth;
	}

    /// <summary>
    /// 创建角色动作
    /// </summary>
    /// <param name="iId">角色Id</param>
    /// <param name="tTeamType">角色所属队伍</param>
    /// <param name="iCharacterDTId">角色模板Id</param>
    /// <param name="iTileNodeId">角色所在位置</param>
    /// 
    /// <summary>
    /// BasePlayerControll tBasePlayerControll, CharacterDT tCharacterDT, TileNode tTileNode
    /// </summary>
    public void f_Birth(int iId, GameEM.TeamType tTeamType, int iCharacterDTId, int iTileNodeId)
    {
		m_iRoleId = iId;
        m_iTeamType = (int)tTeamType;
        m_iCharacterDTId = iCharacterDTId;
        m_iTileNodeId = iTileNodeId;
	}

    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
        CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(m_iCharacterDTId);
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndex(m_iTileNodeId);

        if (tTileNode == null)
        {
            MessageBox.ASSERT("位置坐标未找到 " + m_iTileNodeId);
        }
        //BasePlayerControll tBasePlayerControll = Data_Pool.m_TeamPool.f_GetBasePlayerControl(m_iMasterUserId);
        RoleTools.f_CreateRoleForNetWork(m_iRoleId, (GameEM.TeamType)m_iTeamType, tCharacterDT, tTileNode, 0);

        MessageBox.DEBUG("Birth " + m_iRoleId + " Tile:" + tTileNode.idx);

    }




}

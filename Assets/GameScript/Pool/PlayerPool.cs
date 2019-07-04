using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPool
{

    private List<PlayerDT> _aPlayerList = new List<PlayerDT>();

    public void f_InitPool()
    {
        InitMessage();

    }

    private void InitMessage()
    {

        CTS_PlayerJion tCTS_PlayerJion = new CTS_PlayerJion();
        GameSocket.GetInstance().f_AddListener((int)SocketCommand.CTS_PlayerList_Resp, tCTS_PlayerJion, On_CTS_PlayerJion);
               
        CTS_LeaveGame tCTS_LeaveGame = new CTS_LeaveGame();
        GameSocket.GetInstance().f_AddListener((int)SocketCommand.CTS_PlayerLeave_Resp, tCTS_LeaveGame, On_CTS_PlayerLeave_Resp);
    }

    #region 外部接口

    public void f_RequestPlayerList()
    {
        MessageBox.DEBUG("f_RequestPlayerList");
        //_aPlayerList.Clear();
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(0);
        GameSocket.GetInstance().f_SendBuf2Force((int)SocketCommand.CTS_RequestPlayerList, tCreateSocketBuf.f_GetBuf());        
    }

    public List<PlayerDT> f_GetAll()
    {
        return _aPlayerList;
    }
    
    #endregion

    
    private void On_CTS_PlayerLeave_Resp(object Obj)
    {
        CTS_LeaveGame tPackage = (CTS_LeaveGame)Obj;
        Leave(tPackage.m_iRoomId);
    }

    private void On_CTS_PlayerJion(object Obj)
    {
        CTS_PlayerJion tPackage = (CTS_PlayerJion)Obj;
        Jion(tPackage.m_stPlayerInfor);
    }


    public PlayerDT f_GetPlayer(int iUserId)
    {
        PlayerDT tPlayerDT = _aPlayerList.Find(delegate (PlayerDT tItem)
                                                {
                                                    if (tItem.m_iId == iUserId)
                                                    {
                                                        return true;
                                                    }
                                                    return false;
                                                }
                                            );
        return tPlayerDT;
    }

    void Jion(stPlayerInfor tstPlayerInfor)
    {       
        PlayerDT tPlayerDT = f_GetPlayer(tstPlayerInfor.m_iUserId);
        if (tPlayerDT == null)
        {
            tPlayerDT = new PlayerDT(tstPlayerInfor.m_iUserId, tstPlayerInfor.m_iUserId, tstPlayerInfor.m_iTeam, tstPlayerInfor.m_fHeight);
            _aPlayerList.Add(tPlayerDT);
        }
        MessageBox.DEBUG("PlayerPool 玩家加入游戏 " + tPlayerDT.m_iId);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.PlayerJionGame, tPlayerDT);
       
        if (tstPlayerInfor.m_iUserId == StaticValue.m_UserDataUnit.m_iUserId)
        {
            MessageBox.DEBUG("设置玩家自己信息 " + tPlayerDT.m_iId);
            StaticValue.m_UserDataUnit.m_PlayerDT = tPlayerDT;
            BattleMain.GetInstance().Canvas_Server_WARING.gameObject.SetActive(false);
        }
        else
        {
            if (BattleMain.GetInstance() != null)
            {
                BaseRoleControllV2 tBaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(tPlayerDT.m_iId);
                if (tBaseRoleControl == null)
                {
                    BattleMain.GetInstance().f_CreateOtherPlayer(tPlayerDT);
                }
            }
        }
        
    }

    void Leave(int iUserId)
    {
        PlayerDT tPlayerDT = f_GetPlayer(iUserId);
        if (tPlayerDT != null)
        {
            _aPlayerList.Remove(tPlayerDT);
            BattleMain.GetInstance().f_DestoryOtherPlayer(tPlayerDT);
        }
        MessageBox.DEBUG("玩家加入游戏 " + tPlayerDT.m_iId);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.PlayerLeaveGame, tPlayerDT);
    }



    public void f_InitPlayerPool( )
    {
        InitMyselfPlayer();
        InitOtherPlay();
    }

    void InitOtherPlay()
    {
        for(int i = 0; i < _aPlayerList.Count; i++)
        {
            if (_aPlayerList[i].m_iId == StaticValue.m_UserDataUnit.m_PlayerDT.m_iId)
            {
                return;
            }
            BattleMain.GetInstance().f_CreateOtherPlayer(_aPlayerList[i]);
        }

    }


    /// <summary>
    /// 取得指定隊伍的玩家人數
    /// </summary>
    public int GetTeamPlayerCount(GameEM.TeamType tType) {
        int tmp = 0;
        for (int i = 0; i < _aPlayerList.Count; i++) {
            if (_aPlayerList[i].f_GetTeamType() == tType) {
                tmp++;
            }
        }
        return tmp;
    }

    

    #region 玩家自身相关

    /// <summary>
    /// 创建玩家自身
    /// </summary>
    void InitMyselfPlayer()
    {
        BattleMain.GetInstance().f_CreateMyselfPlayer(StaticValue.m_UserDataUnit.m_PlayerDT);
    }


    #endregion

    #region 击杀得分相关

    /// <summary>
    /// 击杀得分
    /// </summary>
    public void  f_Shot(int iPlayerId)
    {
        if (StaticValue.m_bIsMaster)
        {
            PlayerDT tPlayerDT = f_GetPlayer(iPlayerId);
            if (tPlayerDT != null)
            {
                tPlayerDT.m_PlayerScorePool.f_Shot();
            }
        }
    }

    public void f_Hit(int iPlayerId, GameEM.EM_BodyPart tEM_BodyPart, bool bDie)
    {
        if (StaticValue.m_bIsMaster)
        {
            PlayerDT tPlayerDT = f_GetPlayer(iPlayerId);
            if (tPlayerDT != null)
            {
                tPlayerDT.m_PlayerScorePool.f_Hit(tEM_BodyPart, bDie);
            }
        }
    }

    public void f_Die(int iPlayerId)
    {
        if (StaticValue.m_bIsMaster)
        {
            PlayerDT tPlayerDT = f_GetPlayer(iPlayerId);
            if (tPlayerDT != null)
            {
                tPlayerDT.m_PlayerScorePool.f_Die();
            }
        }
    }

    public void f_GameOver()
    {
        stScore tstScore;
        //for (int i = 0; i < _aPlayerList.Count; i++)
        //{
        //    int iNum = _aPlayerList[i].f_GetKillRole();
        //    MessageBox.DEBUG(_aPlayerList[i].m_iId + " 击杀得分 " + iNum);
        //}
    }

    #endregion




}
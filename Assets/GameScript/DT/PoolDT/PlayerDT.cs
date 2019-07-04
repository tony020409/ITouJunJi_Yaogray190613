using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDT
{

    public int m_iId;

    private int _iPos = -99;
    private GameEM.TeamType _emTeamType;

    private bool _bIsReady = false;
    private TeamPoolDT _TeamPoolDT = null;


    public BaseRoleControllV2 m_PlayerControll;
    private GameObject _oPlayerHome;

    //public MySelfPlayerControll m_MySelfPlayerControll;

    /// <summary>
    /// 玩家选择的类型
    /// </summary>
    private GameEM.PlayerJob m_ePlayerJob = GameEM.PlayerJob.Ero;

    private float _fHeight = 0;

    public PlayerScorePool m_PlayerScorePool = new PlayerScorePool();

    public PlayerDT(int iId, int iPos, int iTeamId, float fHeight)
    {
        m_iId = iId;
        _iPos = iPos;
        _emTeamType = (GameEM.TeamType)iTeamId;
        _fHeight = fHeight;

        m_PlayerScorePool.f_Reset();
    }
    
    public float f_GetHeight()
    {
        return _fHeight;
    }

    public GameEM.TeamType f_GetTeamType()
    {
        return _emTeamType;
    }

    public void f_SaveTeam(TeamPoolDT tTeamPoolDT)
    {
        _TeamPoolDT = tTeamPoolDT;

    }
   
    //public void f_SetPos(int iPos)
    //{
    //    _iPos = iPos;
    //}

    public int f_GetPos()
    {
        return _iPos;
    }

    public void f_SetJob(GameEM.PlayerJob tePlayType)
    {
        m_ePlayerJob = tePlayType;
    }

    public GameEM.PlayerJob f_GetJob()
    {
        return m_ePlayerJob;
    }

    public void f_ReadyGame()
    {
        _bIsReady = true;
    }

    public bool f_IsReadyGame()
    {
        return _bIsReady;
    }

    public TeamPoolDT f_GetPlayerJionTeam()
    {
        return _TeamPoolDT;
    }

    public void f_SetPlayerInfor(BaseRoleControllV2 tBasePlayerControll, GameObject oHome)
    {
        m_PlayerControll = tBasePlayerControll;
        _oPlayerHome = oHome;
    }

    public GameObject f_GetPlayerHome()
    {
        return _oPlayerHome;
    }

    private int _iCreateRoleSleep = 0;
    public void f_Update()
    {
        //if (f_GetTeamType() == GameEM.TeamType.Computer)
        //{//执行电脑AI
        //    if (_iCreateRoleSleep != -99)
        //    {
        //        if (_iCreateRoleSleep <= 0)
        //        {
        //            _iCreateRoleSleep = 200;
        //            if (!BattleMain.GetInstance().f_CheckComputerIsFull())
        //            {                        
        //                //glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.CREATEROLE, 1);
        //                ((ComputerPlayerControl)m_PlayerControll).f_CreateRole();
        //                return;
        //            }
        //        }
        //        _iCreateRoleSleep--;
        //    }
        //}

    }























    }
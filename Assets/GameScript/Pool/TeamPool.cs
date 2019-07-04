using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPool
{
    class stTeamPos
    {
        public int m_iIndex;
        public PlayerDT m_PlayerDT;

    }

    Dictionary<int, TeamPoolDT> _aTeamPool = new Dictionary<int, TeamPoolDT>();
    

    private TeamPoolDT CreateTeam(GameEM.TeamType tTeamType, int iMaxPos)
    {
        TeamPoolDT tTeamPoolDT = new TeamPoolDT(tTeamType, iMaxPos);
        _aTeamPool.Add((int)tTeamType, tTeamPoolDT);    
        return tTeamPoolDT;
    }

    public void f_Destory()
    {
        _aTeamPool.Clear();
    }

    public bool f_Jion(GameEM.TeamType tTeamType, int iPos, PlayerDT tPlayerDT)
    {
        TeamPoolDT tTeamPoolDT = null;
        if (!_aTeamPool.TryGetValue((int)tTeamType, out tTeamPoolDT))
        {
            tTeamPoolDT = CreateTeam(tTeamType, 32);
        }
        if (tTeamPoolDT.f_CheckIsFull())
        {
            MessageBox.DEBUG(tTeamType.ToString() + " 位置已满");
            return false;
        }
        //tPlayerDT.f_SetPos(iPos);
        tTeamPoolDT.f_Jion(tPlayerDT);
        return true;
    }

    public bool f_Leave(GameEM.TeamType tTeamType, PlayerDT tPlayerDT)
    {
        TeamPoolDT tTeamPoolDT = null;
        if (_aTeamPool.TryGetValue((int)tTeamType, out tTeamPoolDT))
        {
            tTeamPoolDT.f_Leave(tPlayerDT);
            return true;
        }
        return false;
    }

    public bool f_CheckCanStartGame()
    {
        if (_aTeamPool.Count == 0)
        {
            return false;
        }
        foreach (KeyValuePair<int, TeamPoolDT> tItem in _aTeamPool)
        {
            if (tItem.Value.f_GetTeamType() == GameEM.TeamType.Computer)
            {
                continue;
            }
            if (!tItem.Value.f_IsReadyGame())
            {
                return false;
            }
        }
        return true;
    }


    public void f_CreatePlayerForBattleMain(BattleMain tBattleMain)
    {
        foreach (KeyValuePair<int, TeamPoolDT> tItem in _aTeamPool)
        {
            tItem.Value.f_CreatePlayerForBattleMain(tBattleMain);
        }
    }

    public void f_CreateDoorForBattleMain(BattleMain tBattleMain)
    {
        foreach (KeyValuePair<int, TeamPoolDT> tItem in _aTeamPool)
        {
            CreateDoorForBattleMain(tBattleMain, tItem.Value);
        }
    }

    private void CreateDoorForBattleMain(BattleMain tBattleMain, TeamPoolDT tTeamPoolDT)
    {
       
    }
    

    public void f_Update()
    {
        foreach (KeyValuePair<int, TeamPoolDT> tItem in _aTeamPool)
        {
            tItem.Value.f_Update();
        }
    }


    public BasePlayerControll f_GetBasePlayerControl(int iUserId)
    {
        foreach (KeyValuePair<int, TeamPoolDT> tItem in _aTeamPool)
        {
            BasePlayerControll tBasePlayerControll = tItem.Value.f_GetBasePlayerControl(iUserId);
            if (tBasePlayerControll != null)
            {
                return tBasePlayerControll;
            }
        }
        return null;
    }

    public PlayerDT f_GetPlayerDT(int iPlayerId)
    {
        foreach (KeyValuePair<int, TeamPoolDT> tItem in _aTeamPool)
        {
            PlayerDT tPlayerDT = tItem.Value.f_GetPlayerDT(iPlayerId);
            if (tPlayerDT != null)
            {
                return tPlayerDT;
            }
        }
        return null;
    }


}
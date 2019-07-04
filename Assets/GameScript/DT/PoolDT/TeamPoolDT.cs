using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPoolDT
{

    private GameEM.TeamType _emTeamType;
    private Dictionary<int, PlayerDT> _aTeamData = new Dictionary<int, PlayerDT>();
    PlayerDT[] _aPlayerInfor = null;

    public TeamPoolDT(GameEM.TeamType tTeamType, int iMaxPos)
    {
        _emTeamType = tTeamType;
        _aPlayerInfor = new PlayerDT[iMaxPos];
        //if (f_GetTeamType() == GameEM.TeamType.Computer)
        //{//如果是电脑就自动初始化
        //    if (StaticValue.m_bIsMaster)
        //    {
        //        for (int i = 0; i < iMaxPos; i++)
        //        {
        //            PlayerDT tPlayerDT = new PlayerDT(ccMath.f_CreateKeyId());
        //            f_Jion(tPlayerDT);
        //        }
        //    }
        //}
    }
    

    public GameEM.TeamType f_GetTeamType()
    {
        return _emTeamType;
    }

    public BasePlayerControll f_GetDefaultPlayerControll()
    {
        foreach (KeyValuePair<int, PlayerDT> tItem in _aTeamData)
        {
            //return tItem.Value.m_PlayerControll;
        }
        return null;
    }

    public BasePlayerControll f_GetBasePlayerControl(int iUserId)
    {
        foreach (KeyValuePair<int, PlayerDT> tItem in _aTeamData)
        {
            if (tItem.Value.m_iId == iUserId)
            {
                //return tItem.Value.m_PlayerControll;
            }
        }
        return null;
    }

    public bool f_Jion(PlayerDT tPlayerDT)
    {
        if (_aTeamData.ContainsKey(tPlayerDT.m_iId))
        {
            return false;
        }
        int iPos = f_GetFreePos();
        if (iPos == -99)
        {
            return false;
        }
        _aPlayerInfor[iPos] = tPlayerDT;
        _aTeamData.Add(tPlayerDT.m_iId, tPlayerDT);
        tPlayerDT.f_SaveTeam(this);
        return true;
    }

    public bool f_Leave(PlayerDT tPlayerDT)
    {
        if (!_aTeamData.ContainsKey(tPlayerDT.m_iId))
        {
            return false;
        }
        _aTeamData.Remove(tPlayerDT.m_iId);
        tPlayerDT.f_SaveTeam(null);
        for (int i = 0; i < _aPlayerInfor.Length; i++)
        {
            if (_aPlayerInfor[i] == null)
            {
                continue;
            }
            if (_aPlayerInfor[i].m_iId == tPlayerDT.m_iId)
            {
                _aPlayerInfor[i] = null;
            }
        }
        return true;
    }


    public bool f_CheckIsFull()
    {
        if (_aTeamData.Count >= _aPlayerInfor.Length)
        {
            return true;
        }
        return false;
    }

    public bool f_IsReadyGame()
    {
        for (int i = 0; i < _aPlayerInfor.Length; i++)
        {
            if (_aPlayerInfor[i] == null)
            {
                return false;
            }
            if (!_aPlayerInfor[i].f_IsReadyGame())
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 没有合适的位置 返回-99
    /// </summary>
    /// <returns></returns>
    private int f_GetFreePos()
    {
        for (int i = 0; i < _aPlayerInfor.Length; i++)
        {
            if (_aPlayerInfor[i] == null)
            {
                return i;
            }
        }
        return -99;
    }

    public void f_CreatePlayerForBattleMain(BattleMain tBattleMain)
    {
        //for (int i = 0; i < _aPlayerInfor.Length; i++)
        //{
        //    if (_aPlayerInfor[i] == null)
        //    {
        //        continue;
        //    }
        //    if (f_GetTeamType() == GameEM.TeamType.Computer)
        //    {
        //        tBattleMain.f_CreateComptuerPlayer(_aPlayerInfor[i]);
        //    }
        //    else
        //    {
        //        if (_aPlayerInfor[i].m_iId == StaticValue.m_UserDataUnit.m_PlayerDT.m_iId)
        //        {
        //            tBattleMain.f_CreateMySelfPlayer(_aPlayerInfor[i]);
        //        }
        //        else
        //        {
        //            tBattleMain.f_CreateOtherPlayer(_aPlayerInfor[i]);
        //        }
        //    }
        //}
    }


    public void f_Update()
    {
        for (int i = 0; i < _aPlayerInfor.Length; i++)
        {
            if (_aPlayerInfor[i] != null)
            {
                _aPlayerInfor[i].f_Update();
            }
        }
    }

    public PlayerDT f_GetPlayerDT(int iPlayerId)
    {
        foreach (KeyValuePair<int, PlayerDT> tItem in _aTeamData)
        {
            if (tItem.Value.m_iId == iPlayerId)
            {
                return tItem.Value;
            }
        }
        return null;
    }


}
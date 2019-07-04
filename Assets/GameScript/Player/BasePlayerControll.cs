using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerControll : MonoBehaviour
{
    protected PlayerDT _PlayerDT;
    public TileNode m_StayPos;

    public virtual void f_Init(PlayerDT tPlayerDT, GameObject oPosObj)
    {
        _PlayerDT = tPlayerDT;
        gameObject.transform.position = oPosObj.transform.position;

        m_StayPos = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(gameObject.transform.position);

    }

    public GameEM.TeamType f_GetTeamType()
    {
        return _PlayerDT.f_GetTeamType();
    }

    public int f_GetUserId()
    {
        return _PlayerDT.m_iId;
    }

}

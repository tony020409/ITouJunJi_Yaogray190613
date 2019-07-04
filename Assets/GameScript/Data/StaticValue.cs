using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

/// <summary>
/// 保存玩家的各数据
/// </summary>
public class StaticValue
{
    //////////////////////////////////////////////////////////////////////////
    //环境相关

    public static bool m_isPlayMusic = true;
    public static bool m_isPlaySound = true;

    //////////////////////////////////////////////////////////////////////////
    public static GameEM.TeamType m_PlayerSelTeam = GameEM.TeamType.Ero;
    public static GameEM.PlayerJob m_PlayerSelJob = GameEM.PlayerJob.Ero;

    public static UserDataUnit m_UserDataUnit = new UserDataUnit();

    /// <summary>
    /// 是否是主玩家 true 为主玩家
    /// </summary>
    public static bool m_bIsMaster = true;
    
    //public static ccSoket m_ChatSocket = new ccSoket();

    //public static PlayerDT m_PlayerDT = new PlayerDT();

    //public static MapMain m_CurMapMain;
    //public static Transform m_YPos;

    /// <summary>
    /// 已经进行的时间
    /// </summary>
    public static float m_fPlayingTime = 0;
    public static int m_iNewServerTime = 0;

    public static float m_fNetAverage = 0; 

    //-----------------游戏环境变量------------------------------


    //-----------------------------------------------------------





}

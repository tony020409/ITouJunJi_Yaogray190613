using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;



#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTS_GameCreateRoom : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_iUserId;

    /// <summary>
    /// 
    /// </summary>
    public int m_iMaxNum;

    /// <summary>
    /// 服务器名称
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]//SocketDT.MAX_USER_NAME)]
    public string m_strName;                      //// 机器码
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTS_GameJionRoom : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_iUserId;
    public int m_iRoomId;
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stPlayerInfor : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_iUserId;
    public int m_iJob;
    public int m_iTeam;
    public int m_iPlayerPos;
    public float m_fHeight;
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTS_StartGame : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_iPlayerNum;
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
    //public stPlayerInfor[] m_aPlayerInfor;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stScore : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    /// <summary>
    /// 玩家在游戏中的Id(0123456789..)
    /// </summary>
    public int m_iGamePlayerId;
    /// <summary>
    /// 发射子弹的量
    /// </summary>
    public int m_iShot;
    /// <summary>
    /// 发射子弹的命中量
    /// </summary>
    public int m_iShotHit;
    /// <summary>
    /// 命中头部
    /// </summary>
    public int m_iHeadShot;
    /// <summary>
    /// 命中头部击杀数
    /// </summary>
    public int m_iHeadShotDie;
    /// <summary>
    /// 击杀数
    /// </summary>
    public int m_iShotDie;
    /// <summary>
    /// 自己死亡次数
    /// </summary>
    public int m_iDie;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTS_GameOver : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_iUserId;
    /// <summary>
    /// 游戏结果
    /// </summary>
    public int m_iGameResult;

    /// <summary>
    /// 游戏时长(秒)
    /// </summary>
    public int m_iTime;

    /// <summary>
    /// 玩家数 (当前这次游戏里有几个玩家)
    /// </summary>
    public int m_iPlayerNum;

    /// <summary>
    /// 保存每个玩家的成绩 (stScore固定长度为16，也就是当前这盘游戏里最多保存16个游戏玩家的排行数据)
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
    public stScore[] m_aScore;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTS_PlayerJion : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public stPlayerInfor m_stPlayerInfor;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTS_PlayerReady : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_iRoomId;
    public int m_iTeamId;
    public int m_iPlayerJob;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTS_LeaveGame : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_iRoomId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTS_GameData : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iRoomId;
    public int iUserId;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
    public byte[] szBuf;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct STC_BroadCastAction : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
    public byte[] szBuf;

}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct STC_SyscUpdate : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iCurGameSyscFrame;
    /// <summary>
    /// 当前渲染次数
    /// </summary>
    public int fCurFrameTime;
    /// <summary>
    /// 计算当前的游戏渲染FPS 
    /// </summary>
    public int iGameFpsRunTime;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct STC_PlayerActionConfirm : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iActionId;
    public int iCurGameSyscFrame;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTS_ServerActionConfirm : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_iActionGameSyscFrame;
}




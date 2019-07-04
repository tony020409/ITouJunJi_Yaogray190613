using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;



public enum SocketCommand
{
    /// <summary>
    /// PING
    /// </summary>
    PING = 10000,
    PING_Reps = 30000,

    /// <summary>
    /// --客户端<-->游戏服务器
    /// </summary>
    MSG_CGameMsg = 6,



    //////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// 操作结果回应
    /// </summary>
    CONTROL_CTG_OperateResult = 3010,       //


    /// <summary>
    /// 游戏开始通知  
    /// </summary>
    GTC_GameStartNotfy = 3139,


    /// <summary>
    /// --客户端<-->游戏服务器
    /// </summary>
    MSG_GameServerLogin = 6,

    /// <summary>
    /// 操作结果回应
    /// </summary>
    STC_OperateResult = 1000,

    CTS_GameCreateRoom = 1010,
    CTS_GameJionRoom = 1020,
    CTS_GameData = 1030,
    CTS_ReadyStartGame = 1035,
    CTS_StartGame = 1040,    
    CTS_PlayerReady = 1050,
    CTS_LeaveGame = 1060,
    CTS_GameOver = 1070,
    CTS_RequestPlayerList = 1080,
    CTS_RestartGame = 1090,

    CTS_GameCreateRoom_Resp = 2010,
    CTS_GameJionRoom_Resp = 2020,
    CTS_GamePlayerJionRoomSuc_Resp = 2030,
    CTS_LeaveGame_Resp = 2060,
    CTS_ServerActionConfirm = 2070,

    CTS_PlayerList_Resp = 2080,
    CTS_PlayerLeave_Resp = 2090,

    /// <summary>
    /// 向同步服务器发送正式开始游戏指令
    /// </summary>
    CTS_StartPlayGame = 2100,

    STC_BroadCastAction = 20000,
    STC_SyscUpdate = 20010,
    STC_PlayerAction = 20020,
    STC_PlayerActionConfirm = 20030,

    

}

public enum ControllSocketCommand
{
    CTS_GameJion = 50020,
    CTS_ActionBuf = 50030,



}


public class stCommandData : BasePoolDT<string>
{
    //public int m_iCode
    //{
    //    get
    //    {
    //        return (int)iId;
    //    }
    //    set
    //    {
    //        iId = value;
    //    }
    //}

    public string m_strCommandName
    {
        get
        {
            return _iId;
        }
        set
        {
            _iId = value;
        }
    }
    
    public SocketCallbackDT m_SocketCallbackDT;
    public int m_iOperateTypeCode;


}

public class SocketCommandV2 : ccBasePool<string>
{

    public SocketCommandV2()
        : base("stCommandData", true)  
    {

    }


    public void f_AddCommand(string strCommandName, int iCode, string strOperateType = "", int iOperateTypeCode = 0)
    {
        stCommandData tstCommandData = new stCommandData();
        //tstCommandData.m_iCode = iCode;     
        tstCommandData.m_strCommandName = strCommandName;
        f_Save(tstCommandData);
    }

    public void f_SendBuf(string strCommandName, CreateSocketBuf tCreateSocketBuf, SocketCallbackDT m_SocketCallbackDT, int iOperateTypeCode = 0)
    {
        BasePoolDT<string> tData = f_GetForId(strCommandName);
        if (tData == null)
        {
            MessageBox.ASSERT("无此命令" + strCommandName);
        }
        stCommandData tstCommandData = (stCommandData)tData;

    }


    //public void f_RemoveCommand()
    //{

    //}

}
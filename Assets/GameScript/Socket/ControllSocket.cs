using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
using GameControllAction;

public class ControllSocket : BaseSocket
{   

    private static ControllSocket _Instance = null;
    public static ControllSocket GetInstance()
    {
        if (null == _Instance)
        {
            _Instance = new ControllSocket();
        }

        return _Instance;
    }

    public ControllSocket()
    {
        InitMessage();
        InitMachine();
    }

    protected override void InitMachine()
    {
        Socket_Wait tSocket_Wait = new Socket_Wait(this);
        _SocketMachineManger = new ccMachineManager(new Socket_Loop(this));
        //_SocketMachineManger.f_RegState(new Socket_Regedit(this));
        _SocketMachineManger.f_RegState(new Socket_Connect(this, GloData.glo_iSvrControllPort));
        _SocketMachineManger.f_RegState(new Socket_ControllLogin(this));
        _SocketMachineManger.f_RegState(new Socket_Wait(this));

        _SocketMachineManger.f_ChangeState(tSocket_Wait);

    }


#region 创建帐号

    public void f_CreateAccount(string strName, string strPwd, ccCallback handler, UnityEngine.Object pParent = null)
    {
        Socket_Regedit tSocket_Regedit = (Socket_Regedit)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Regedit);
        tSocket_Regedit.f_CreateAccount(strName, strPwd, handler);
        _SocketMachineManger.f_ChangeState(tSocket_Regedit);
    }

#endregion


#region 登陆相关

    public void f_Login(ccCallback func = null)
    {
        Socket_ControllLogin tSocket_ControllLogin = (Socket_ControllLogin)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.ControllLogin);
        tSocket_ControllLogin.f_Login(func);
        _SocketMachineManger.f_ChangeState(tSocket_ControllLogin, func);
    }

    #endregion



    #region 内部消息处理

    public override void Destroy()
    {
        base.Destroy();
        ControllSocket._Instance = null;
    }

    public override void f_Update()
    {
        base.f_Update();

    }
    
    #endregion



    #region 外部消息处理

    protected override void InitMessage()
    {
        base.InitMessage();

        //m_GMSocketMessagePool.f_AddListener(SocketCommand.GM_SCK_UNBUILD.ToString(), On_GM_SCK_UNBUILD, null);
        stGameCommandReturn tGameCommandRet = new stGameCommandReturn();
        f_AddListener((int)SocketCommand.CONTROL_CTG_OperateResult, tGameCommandRet, On_CMsg_GameCommandReturn);

        //CMsg_GTC_AccountLoginRelt tCMsg_GTC_AccountLoginRelt = new CMsg_GTC_AccountLoginRelt();
        //f_AddListener((int)SocketCommand.SC_UserAttrInit, tCMsg_GTC_AccountLoginRelt, On_RoleData, null);

        //SC_UserAttr tSC_UserAttr = new SC_UserAttr();
        //f_AddListener((int)SocketCommand.SC_UserAttr, tSC_UserAttr, On_Data_CTG_ChangeRoleData, null, 1);


        //////////////////////////////////////////////////////////////////////////
        //DATA
        STC_BroadCastAction tSTC_BroadCastAction = new STC_BroadCastAction();
        f_AddListener_Buf_V2((int)ControllSocketCommand.CTS_ActionBuf, tSTC_BroadCastAction, On_STC_PlayerAction, null);


        basicNode1 tPing = new basicNode1();
        f_AddListener((int)SocketCommand.PING_Reps, tPing, On_Ping);
               

    }

    #endregion

    private CreateSocketBuf _CreateSocketBuf = new CreateSocketBuf();
    public void f_SendAction(GameControllAction.BasePlayerAction action)
    {
        action.f_Save(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId, 0);
        byte[] aBuf = GameSysc.ControllActionTools.Serialize(action);
        //byte[] aZipBuf = ZipTools.aaaa3221(aBuf);
        short iPackLen = (short)aBuf.Length;
        _CreateSocketBuf.f_Reset();
        _CreateSocketBuf.f_Add(iPackLen);
        _CreateSocketBuf.f_Add(aBuf);
        f_SendBuf2Force((int)ControllSocketCommand.CTS_ActionBuf, _CreateSocketBuf.f_GetBuf());
    }

    ReadBuf _DispPlayerActionBuf = new ReadBuf();

    private void On_STC_PlayerAction(int iLen, byte[] aBuf)
    {//玩家提交的Action
        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming)
        {           
            GameControllAction.BasePlayerAction action = null;
            _DispPlayerActionBuf.f_Reset();
            _DispPlayerActionBuf.f_Save(aBuf, iLen);          
            short iUnZipPackLen = _DispPlayerActionBuf.f_ReadShort();
            byte[] aZipBuf = _DispPlayerActionBuf.f_ReadBufToEnd();
            //byte[] aUnZipBuf = ZipTools.aaa557788(aZipBuf, iUnZipPackLen);
            action = GameSysc.ControllActionTools.DeSerialize(aZipBuf);
            BattleMain.GetInstance().f_UpdatePlayerAction((PlayerTransformAction)action);
        }
    }


}//END Class
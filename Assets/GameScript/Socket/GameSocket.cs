using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;


public class GameSocket : BaseSocket
{      
    private static GameSocket _Instance = null;
    public static GameSocket GetInstance()
    {
        if (null == _Instance)
        {
            _Instance = new GameSocket();
        }

        return _Instance;
    }

    public GameSocket()
    {
        InitMessage();
        InitMachine();
    }

    protected override void InitMachine()
    {
        Socket_Wait tSocket_Wait = new Socket_Wait(this);
        _SocketMachineManger = new ccMachineManager(new Socket_Loop(this));
        _SocketMachineManger.f_RegState(new Socket_Regedit(this));
        _SocketMachineManger.f_RegState(new Socket_Connect(this, GloData.glo_iSvrPort));
        _SocketMachineManger.f_RegState(new Socket_Login(this));
        _SocketMachineManger.f_RegState(new Socket_Wait(this));

        _SocketMachineManger.f_ChangeState(tSocket_Wait);

    }

  
    ///// <summary>
    ///// 切换状态
    ///// </summary>
    ///// <param name="state">需切换至的状态</param>
    ///// <param name="nextState">下一个状态</param>
    //public void f_ChangeState(ccMachineStateBase state,ccMachineStateBase nextState = null)
    //{
    //    if (nextState == null)
    //    {
    //        nextState = new Socket_Wait(this);
    //    }
    //    _SocketMachineManger.f_ChangeState(state, nextState);
    //}
   

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
        Socket_Login tSocket_Login = (Socket_Login)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Login);
        tSocket_Login.f_Login(func);
        _SocketMachineManger.f_ChangeState(tSocket_Login, func);
    }

    #endregion



    #region 内部消息处理

    public override void Destroy()
    {
        base.Destroy();
        GameSocket._Instance = null;
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

        

        basicNode1 tPing = new basicNode1();
        f_AddListener((int)SocketCommand.PING_Reps, tPing, On_Ping);
               

    }

    #endregion



    #region 游戏结束

    public void f_GameOver() {
        MessageBox.DEBUG("f_GameOver");
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(0);
        tCreateSocketBuf.f_Add(0);
        f_SendBuf2Force((int)SocketCommand.CTS_GameOver, tCreateSocketBuf.f_GetBuf());
    }

    #endregion


    #region 开始游戏
    public void f_StartPlayGame() {
        if (StaticValue.m_bIsMaster) {
            if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Waiting) {
                CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
                tCreateSocketBuf.f_Add(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId);
                GameSocket.GetInstance().f_SendBuf2Force((int)SocketCommand.CTS_StartPlayGame, tCreateSocketBuf.f_GetBuf());
            }
        }
    }
    #endregion


}//END Class
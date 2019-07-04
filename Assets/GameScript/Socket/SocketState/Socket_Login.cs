using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class Socket_Login : Socket_StateBase
{
    private ccCallback _LoginCallbackFunc;
    private System.DateTime _dtLoginTimeOut;
    private int _iReLogin = 0;
    public Socket_Login(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Login, tBaseSocket)
    {

    }

    public void f_Login(ccCallback func = null)
    {
        _LoginCallbackFunc = func;

        CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = new CMsg_GTC_LoginRelt();
        _BaseSocket.f_AddListener((int)SocketCommand.CTS_GameJionRoom_Resp, tCMsg_GTC_LoginRelt, On_LoginSuc);
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        MessageBox.DEBUG("登陆...." + _BaseSocket.ToString());

        Login();
    }

    private void Login()
    {
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.Connecting)
        {
            MessageBox.DEBUG("Login 操作失败，当前状态为 Connect....");
        }
        else if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.ConnectSuc)
        {
            SendLogin(_iReLogin);
        }
        else
        {
            f_SetComplete((int)EM_Socket.Connect, this);
        }
    }

    public override void f_Execute()
    {
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.Logining)
        {
            if ((System.DateTime.Now - _dtLoginTimeOut).TotalSeconds > 30)
            {
                CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = new CMsg_GTC_LoginRelt();
                tCMsg_GTC_LoginRelt.m_result = (int)eMsgOperateResult.OR_Error_LoginTimeOut;
                On_LoginSuc(tCMsg_GTC_LoginRelt);
            }
        }
    }

    private void SendLogin(int iReLogin = 0)
    {
        _dtLoginTimeOut = System.DateTime.Now;

        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(GloData.glo_iGameModel);
        tCreateSocketBuf.f_Add(0);
        tCreateSocketBuf.f_Add(GloData.glo_iPos);
        tCreateSocketBuf.f_Add(GloData.glo_iVer);
        tCreateSocketBuf.f_Add(GloData.glo_iTeamId);
        tCreateSocketBuf.f_Add(GloData.glo_fMyselfHeight);
        _BaseSocket.f_SendBuf2Force((int)SocketCommand.CTS_GameJionRoom, tCreateSocketBuf.f_GetBuf());
        MessageBox.DEBUG("发送登陆游戏申请 " + _dtLoginTimeOut);

        _BaseSocket.f_SetSocketStatic(EM_SocketStatic.Logining);
    }

    private void On_LoginSuc(object Obj)
    {
		/*
        if (_GameSocket.f_GetSocketStatic() == EM_SocketStatic.LoginEro)
        {
            return;
        }
        */
        CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = (CMsg_GTC_LoginRelt)Obj;
        //_GameSocket.f_RemoveListener(SocketCommand.GTC_AccountEnterResult);
        MessageBox.DEBUG("登陆返回玩家Id：" + tCMsg_GTC_LoginRelt.m_PlayerId);
        if (_LoginCallbackFunc != null)
        {
            _LoginCallbackFunc(tCMsg_GTC_LoginRelt.m_result);
            _LoginCallbackFunc = null;
        }
        if (tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.OR_Succeed)
        {
            StaticValue.m_UserDataUnit.m_iUserId = tCMsg_GTC_LoginRelt.m_PlayerId;
            StaticValue.m_UserDataUnit.m_PlayerDT = Data_Pool.m_PlayerPool.f_GetPlayer(tCMsg_GTC_LoginRelt.m_PlayerId);       
            if (StaticValue.m_UserDataUnit.m_PlayerDT == null)
            {
                MessageBox.DEBUG("m_PlayerDT创建失败");
            }
            if (tCMsg_GTC_LoginRelt.m_PlayerId == 0)
            {
                StaticValue.m_bIsMaster = true;
            }
            else
            {
                StaticValue.m_bIsMaster = false;
            }

            _iReLogin = 1;
            _BaseSocket.f_SetSocketStatic(EM_SocketStatic.OnLine);            
            MessageBox.DEBUG("登陆成功." + _BaseSocket.ToString());
            _BaseSocket.f_Ping();
            f_SetComplete((int)EM_Socket.Loop);
        }
        else
        {
            _BaseSocket.f_Close();
            _BaseSocket.f_SetSocketStatic(EM_SocketStatic.LoginEro);
                        
            eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result;            
            MessageBox.ASSERT("登陆失败。 " + teMsgOperateResult.ToString());
           
        }

    }

    


}

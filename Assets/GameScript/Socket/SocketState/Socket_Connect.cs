using UnityEngine;
using System.Collections;
using ccU3DEngine;

/// <summary>
/// socket连接状态
/// </summary>
public class Socket_Connect : Socket_StateBase
{
    private Socket_StateBase _Socket_StateBaseForNext = null;
    private bool _bRetryConnect = false;
    private int _iPort;
    public Socket_Connect(BaseSocket tBaseSocket, int iPort)
        : base((int)EM_Socket.Connect, tBaseSocket)
    {
        _iPort = iPort;
    }
    /// <summary>
    /// 状态进入
    /// </summary>
    /// <param name="Obj"></param>
    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        MessageBox.DEBUG("初始连接...." + _BaseSocket.ToString());
        if (Obj == null)
        {
            MessageBox.ASSERT("连接后下一状态不明确");
            return;
        }
        _Socket_StateBaseForNext = (Socket_StateBase)Obj;
        Connect();
    }
    /// <summary>
    /// 连接服务器
    /// </summary>
    private void Connect()
    {
        _bRetryConnect = true;
        InitSocket();
    }
    /// <summary>
    /// 初始化socket
    /// </summary>
    /// <param name="iTime"></param>
    /// <returns></returns>
    private bool InitSocket(int iTime = 2000)
    {
        _BaseSocket.f_Close();
        _BaseSocket.f_SetSocketStatic(EM_SocketStatic.Connecting);
        //MessageBox.DEBUG("初始连接....");
        

        _dtLoginTimeOut = System.DateTime.Now;
        if (!_BaseSocket.Connect(GloData.glo_strSvrIP, _iPort, _BaseSocket.f_Router, false, ConnectCallBack, iTime, true))
        {
            _BaseSocket.f_Close();
            //MessageBox.ASSERT("连接游戏失败");
            ConnectCallBack(false);
            return false;
        }

        return true;
    }

    private void ConnectCallBack(object oData)
    {
        bool bRet = (bool)oData;
        if (bRet)
        {
            Debug.Log("连接成功");
            _bRetryConnect = false;
            _BaseSocket.f_SetSocketStatic(EM_SocketStatic.ConnectSuc);
            f_SetComplete((int)_Socket_StateBaseForNext.iId);
        }
    }

    private System.DateTime _dtLoginTimeOut;
    public override void f_Execute()
    {
        if (_bRetryConnect)
        {
            if ((System.DateTime.Now - _dtLoginTimeOut).TotalSeconds > 6)
            {
                MessageBox.DEBUG("重连....");
                InitSocket();                
                return;

            }
        }
    }
    /// <summary>
    /// 重连
    /// </summary>
    /// <param name="Obj"></param>
    private void Callback_RetryConnect(object Obj)
    {
        Connect();
    }


}

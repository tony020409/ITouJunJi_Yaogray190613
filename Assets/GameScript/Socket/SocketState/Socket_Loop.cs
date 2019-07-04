using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Diagnostics;

public class Socket_Loop : Socket_StateBase
{
    private int _iPingId = -99;
    private int m_iSleepTime = 0;
    private bool _bTestTime = false;
    private int _iTestTimeEventId = -99;

    public Socket_Loop(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Loop, tBaseSocket)
    {

    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        if (_iPingId == -99)
        {
            _iPingId = ccTimeEvent.GetInstance().f_RegEvent(GloData.glo_iPingTime, true, null, Callback_Ping);
        }        
    }
        
    public override void f_Execute()
    {
        base.f_Execute();
        
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OnLine)
        {
            if (TestSocket() && _BaseSocket.f_CheckHaveBuf())
            {
                //if (TestTime())
                //{
                    _BaseSocket.f_DispSendCatchBuf();
                //}
            }
        }
        else if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OffLine)
        {
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMESOCKETERO);
            f_SetComplete((int)EM_Socket.Login, -99);

        }
        else 
        {
            TestSocket();           
        }
        
        
    }

    private bool TestSocket()
    {
        if (_BaseSocket.f_TestSocket() || Application.internetReachability != NetworkReachability.NotReachable)       
        {
            //if (!glo_Main.GetInstance().m_StarSDK.f_CheckIsPaying())
            //{
                if (BufTimeOut())
                {
                    return false;
                }
            //}
        }
        else
        {
            _BaseSocket.f_Close();
            return false;
        }
        return true;
    }

    private bool TestTime()
    {
        if (_bTestTime)
        {
            return false;
        }
        if ((_BaseSocket.f_GetServerTime() - m_iSleepTime) > 15)
        {
            _bTestTime = true;
            _BaseSocket.f_Ping();
            _iTestTimeEventId = ccTimeEvent.GetInstance().f_RegEvent(3, false, null, Callback_TestTime);
            return false;
        }
        return true;
    }

    public void f_UpdateTestTime()
    {
        _iTestTimeEventId = -99;
        _bTestTime = false;
        m_iSleepTime = _BaseSocket.f_GetServerTime();
    }

    private void Callback_TestTime(object Obj)
    {
        if (_iTestTimeEventId != -99)
        {
            _bTestTime = false;
            TestTime();
        }
    }

    private void Callback_Ping(object Obj)
    {
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OnLine)
        {
            //int iTime = (int)(System.DateTime.Now - _BaseSocket.m_dtSocketTimeout).TotalSeconds;
            //if (iTime > GloData.glo_iPingTime)
            //{
            _BaseSocket.f_Ping();
            //}
        }
    }

    private bool BufTimeOut()
    {
        //int iTime = (int)(System.DateTime.Now - _BaseSocket.m_dtSocketTimeout).TotalSeconds;
        //if (iTime > GloData.glo_iRecvPingTime)
        //{
        //    MessageBox.DEBUG("BufTimeOut 网络超时。");
        //    _BaseSocket.f_Close();
        //    return true;
        //}
        return false;
    }

}

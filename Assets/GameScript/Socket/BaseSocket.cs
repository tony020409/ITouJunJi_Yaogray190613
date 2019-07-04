using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;


public enum EM_SocketStatic
{
    OffLine = 0,
    Connecting = 1,
    ConnectSuc = 2,
    Logining = 3,
    OnLine = 4,
    LoginEro = 5,
}


public class BaseSocket
{
    private bool _bIsRun = true;

    public delegate int Callback_DoRouter(aaa12334 stHead, byte[] bytes, int iPos);
    private Callback_DoRouter _Callback_DoRouter;

    class sCommandReturn
    {
        public SocketCallbackDT m_SocketCallbackDT = new SocketCallbackDT();
        public bool m_bAlive;
    }
    class CatchBuf
    {
        public CatchBuf()
        {
            m_fSleepTime = 0;
            bytes = null;
        }
        public float m_fSleepTime;
        public byte[] bytes;
    }
#if UNITY_WEBPLAYER
     private ccSoket m_Socket = new ccSoket(true);
#else
    private ccSoket m_Socket = new ccSoket();
#endif

    public System.DateTime m_dtSocketTimeout;
    private bool _bLoginSuc = false;
    private bool m_bSendConnectEro = false;
    private EM_SocketStatic _EM_SocketStatic = EM_SocketStatic.OffLine;
    protected ccMachineManager _SocketMachineManger = null;
    private Socket_Loop _Socket_Loop = null;
    
    private int m_iId = 0;

    private SocketState m_SocketState = new SocketState();    
    private ccSocketMessagePool m_GMSocketMessagePool = new ccSocketMessagePool();
    private ccMessagePoolV2 m_MessagePool = new ccMessagePoolV2();

    private ccCallback m_LoginCallbackFunc = null;
    
    private System.DateTime m_fLastTime;

    public BaseSocket()
    {
        InitMessage();
        InitMachine();
    }

    protected virtual void InitMachine()
    {
       
    }

    public ccSoket f_GetSocket()
    {
        return m_Socket;
    }

    public void f_RegOtherRouter(Callback_DoRouter tCallback_DoRouter)
    {
        _Callback_DoRouter = tCallback_DoRouter;
    }


    ///// <summary>
    ///// 连接服务器
    ///// </summary>
    //public void f_ConnectServer(ccCallback callback)
    //{
    //    Socket_Connect tSocket_Connect = (Socket_Connect)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Connect);
    //    tSocket_Connect.f_SetCallback(callback);
    //    Socket_Loop tSocket_Loop = (Socket_Loop)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Loop);
    //    _SocketMachineManger.f_ChangeState(tSocket_Connect, tSocket_Loop);
    //}
    public bool Connect(string strSvrIP, int iPort, aaaa222235 func, bool bCloseThread, ccCallback tccCallback = null, int iTime = 10000, bool bDelay = false)
    {
        return m_Socket.Connect(strSvrIP, iPort, func, bCloseThread, tccCallback, iTime, bDelay);
    }

    public virtual void f_StopAndClose()
    {
        _bIsRun = false;
        f_Close();
    }

    public virtual void f_Close()
    {
        _EM_SocketStatic = EM_SocketStatic.OffLine;
        MessageBox.DEBUG("关闭连接..." + this.ToString());
        m_Socket.Close();
    }

    public bool f_TestSocket()
    {
        if (m_Socket.f_TestSocket() || Application.internetReachability != NetworkReachability.NotReachable)
        {
            return true;
        }
        return false;
    }

    public void f_SetSocketStatic(EM_SocketStatic tEM_SocketStatic)
    {
        _EM_SocketStatic = tEM_SocketStatic;
    }

    public EM_SocketStatic f_GetSocketStatic()
    {
        return _EM_SocketStatic;
    }

    #region 内部消息处理

    public virtual void Destroy()
    {
        f_Close();
        m_aTTTTT.Clear();
        m_aRRRRR.Clear();
        m_aTTTTT = null;
        m_aRRRRR = null;
        m_aOutCatchBuf.Clear();
        m_aOutCatchBuf = null;
        
    }

    public void f_AddListener_EndString(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_EndString handler)
    {
        m_GMSocketMessagePool.f_AddListenerEndString(tSocketCommand.ToString(), tSockBaseDT, handler, null);
    }

    public void f_RegMessage(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback ccCallbackSuc, int teMsgOperateType = (int)eMsgOperateType.OT_NULL, ccCallback ccCallbackFail = null, bool bAlive = true)
    {
        f_AddListener(iSocketCommand, tSockBaseDT, ccCallbackSuc, 0, bAlive);
        if (teMsgOperateType != (int)eMsgOperateType.OT_NULL)
        {
            if (ccCallbackFail == null)
            {
                MessageBox.ASSERT("定义的命令返回回调为空 " + teMsgOperateType.ToString());
            }
            f_RegCommandReturn(teMsgOperateType, null, ccCallbackFail);
        }
    }

    public void f_RegMessage_Int0_EndString(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_EndString ccCallbackSuc)
    {
        f_AddListener_EndString(iSocketCommand, tSockBaseDT, ccCallbackSuc);
    }

    public void f_AddListener_Buf_Int2_V2(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Buf_Int2_V2 handler)
    {
        m_GMSocketMessagePool.f_AddListener_Buf_Int2_V2(iSocketCommand.ToString(), tSockBaseDT, handler, null);
    }


    public void f_RegMessage_Int0(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 ccCallbackSuc,
       int tEroeMsgOperateType = (int)eMsgOperateType.OT_NULL, ccCallback ccCallbackFail = null)
    {
        f_AddListener_Int0_V2(iSocketCommand, tSockBaseDT, ccCallbackSuc, null);
        if (tEroeMsgOperateType != (int)eMsgOperateType.OT_NULL)
        {
            if (ccCallbackFail == null)
            {
                MessageBox.ASSERT("定义的命令返回回调为空 " + tEroeMsgOperateType.ToString());
            }
            f_RegCommandReturn(tEroeMsgOperateType, null, ccCallbackFail);
        }
    }

    /// <summary>
    /// 前2位为独立的int,第3位开始为数量
    /// </summary>
    public void f_RegMessage_Int2(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 ccCallbackSuc,
       int tEroeMsgOperateType = (int)eMsgOperateType.OT_NULL, ccCallback ccCallbackFail = null)
    {
        f_AddListener_Int2_V2(iSocketCommand, tSockBaseDT, ccCallbackSuc, null);
        if (tEroeMsgOperateType != (int)eMsgOperateType.OT_NULL)
        {
            if (ccCallbackFail == null)
            {
                MessageBox.ASSERT("定义的命令返回回调为空 " + tEroeMsgOperateType.ToString());
            }
            f_RegCommandReturn(tEroeMsgOperateType, null, ccCallbackFail);
        }
    }

    /// <summary>
    /// 前1位为独立的int,第2位开始为数量
    /// </summary>
    public void f_RegMessage_Int1(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 ccCallbackSuc,
       int tEroeMsgOperateType = (int)eMsgOperateType.OT_NULL, ccCallback ccCallbackFail = null)
    {
        f_AddListener_Int1_V2(iSocketCommand, tSockBaseDT, ccCallbackSuc, null);
        if (tEroeMsgOperateType != (int)eMsgOperateType.OT_NULL)
        {
            if (ccCallbackFail == null)
            {
                MessageBox.ASSERT("定义的命令返回回调为空 " + tEroeMsgOperateType.ToString());
            }
            f_RegCommandReturn(tEroeMsgOperateType, null, ccCallbackFail);
        }
    }

    /// <summary>
    /// 定义游戏消息，并按定义的消息对相应的数据进行拆分和转发
    /// </summary>
    /// <param name="tSocketCommand"></param>
    /// <param name="tSockBaseDT"></param>
    /// <param name="handler"></param>
    /// <param name="pParent"></param>
    /// <param name="iHaveNum"></param>
    public void f_AddListener(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback handler, int iHaveNum = 0, bool bAlive = true)
    {
        m_GMSocketMessagePool.f_AddListener(tSocketCommand.ToString(), tSockBaseDT, handler, null, iHaveNum, bAlive);
    }

    
    public void f_RemoveListener(int tSocketCommand)
    {
        m_GMSocketMessagePool.f_RemoveListener(tSocketCommand.ToString());
    }
    
    /// <summary>
    /// 前0位为独立的int,第1位开始为数量
    /// </summary>
    private void f_AddListener_Int0_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Int0_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    /// <summary>
    /// 前1位为独立的int,第2位开始为数量
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="tSockBaseDT"></param>
    /// <param name="handler"></param>
    /// <param name="pParent"></param>
    public void f_AddListener_Int1_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Int1_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    /// <summary>
    /// 前2位为独立的int,第3位开始为数量
    /// </summary>
    private void f_AddListener_Int2_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Int2_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    /// <summary>
    /// 前三位为独立的int,第4位开始为数量
    /// </summary>
    private void f_AddListener_Int3_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int3_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Int3_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    public void f_AddListener_Buf_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Buf_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Buf_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    public bool f_CheckHaveBuf()
    {
        return m_aOutCatchBuf.Count > 0 ? true : false;
    }
    object _QueueLock = new object();
    Queue m_aOutCatchBuf = new Queue();
    Queue m_aTTTTT = new Queue();
    Queue m_aRRRRR = new Queue();
    public void f_Router(int iResult, aaa12334 stHead, byte[] bytes, aaaa343 NetMsgHead, ushort wTemp)
    {
        if (iResult < 0)
        {
            Debug.Log("游戏接收数据错误 " + iResult);
            f_Close();
            return;
        }
        lock (_QueueLock)
        {
            byte[] TmpBytes = new byte[stHead.iBodyLen];
            Array.Copy(bytes, TmpBytes, stHead.iBodyLen);
            //string ppSQL = wTemp + " 接收(" + iResult + ") - 类型:" + stHead.iPackType + " 长度:" + stHead.iBodyLen + "-" + stHead.iRecvUserID + "-" + stHead.iTemp + "-" + stHead.iTemp1 + "-" + stHead.iTemp2;
            //MessageBox.DEBUG(ppSQL);
            //int iDoLen = DoRouter(stHead, bytes, 0);
            //Debug.Log(ppSQL);
       
            m_aTTTTT.Enqueue(stHead);
            m_aRRRRR.Enqueue(TmpBytes);
        }
    }

    private void RouterUpdate()
    {
        lock (_QueueLock)
        {
            if (m_aTTTTT == null || m_aRRRRR == null)
            {
                Debug.LogError("m_aTTTTT == null || m_aRRRRR == null");
            }
            while (m_aTTTTT.Count > 0)
            {                
                aaa12334 stHead = (aaa12334)m_aTTTTT.Dequeue();
                byte[] bytes = (byte[])m_aRRRRR.Dequeue();

                iRecvNum += 18;
                iRecvNum += bytes.Length;

                string ppSQL = "发:" + m_fSendBufSize + " 收:" + iRecvNum + " 接收 - 类型:" + stHead.iPackType + " 长度:" + stHead.iBodyLen + "-" + stHead.iRecvUserID + "-" + stHead.iTemp + "-" + stHead.iTemp1 + "-" + stHead.iTemp2;
                //MessageBox.DEBUG(ppSQL);

                if (DoRouter(stHead, bytes, 0) == 0)
                {//未找到对应的处理转到外部方法处理
                    if (_Callback_DoRouter != null)
                    {
                        _Callback_DoRouter(stHead, bytes, 0);
                    }
                    else
                    {
                        Debug.LogError("未找到对应的处理方法 " + stHead.iPackType);
                    }
                }

                m_dtSocketTimeout = System.DateTime.Now;
                if (stHead.iPackType <= 0)
                {
                    f_Printf("错包", bytes);
                    //m_bRunThread = false;
                    MessageBox.ASSERT("EEEEE");
                }
                else
                {
                    //MessageBox.DEBUG("OK");
                }
                //MessageBox.DEBUG("发:" + m_fSendBufSize + " 收:" + iRecvNum);
            }
        }
    }

    private int iSendNum;
    private int iRecvNum;
    private float m_fSendBufSize = 0;
    private float m_fPingTime = 60;
    //private float m_fRecvPingTime = 0;
    private float m_fReLoginSleepTime = 3;
    private float m_fSendBufSleepTime = -99;
    public virtual void f_Update()
    {
        if (_bIsRun)
        {
            RouterUpdate();
            m_MessagePool.f_Update();
            _SocketMachineManger.f_Update();
        }
    }

    private int DoRouter(aaa12334 stHead, byte[] bytes, int iPos)
    {
        return m_GMSocketMessagePool.f_Router(stHead, bytes, iPos);
    }

    public void f_DispSendCatchBuf()
    {
        if (m_aOutCatchBuf.Count > 0)
        {
            CatchBuf tCatchBuf = (CatchBuf)m_aOutCatchBuf.Dequeue();

            iSendNum = m_Socket.f_Send(tCatchBuf.bytes);
            if (-1 == iSendNum)
            {
                MessageBox.ASSERT("发送失败");
                return;
            }
            //MessageBox.DEBUG("发送: " + tCatchBuf.bytes.Length);
        }
    }

    public int f_SendBuf(SocketCommand tSocketCommand, byte[] bBodyBuf)
    {
        return f_SendBuf((int)tSocketCommand, bBodyBuf);
    }
    
    public int f_SendBuf(int wAssistantCmd, byte[] bBodyBuf)
    {
        byte[] bytes = m_Socket.f_CreateSocketPack(wAssistantCmd, bBodyBuf);
        CatchBuf tCatchBuf = new CatchBuf();
        tCatchBuf.bytes = bytes;
        m_aOutCatchBuf.Enqueue(tCatchBuf);
        return bytes.Length;
    }

    public int f_SendBuf2Force(int wAssistantCmd, byte[] bBodyBuf)
    {
        return m_Socket.f_SendBuf(wAssistantCmd, bBodyBuf);
    }

    public void f_Printf(string strHead, byte[] bytes)
    {
        string ppSQL = strHead + " >>>>> ";
        int iLen = bytes.Length / 4;
        for (int i = 0; i < iLen; i++)
        {
            int DDD = BitConverter.ToInt32(bytes, i * 4);
            ppSQL = ppSQL + string.Format(" {0} ", DDD);
        }
        ppSQL = ppSQL + " >>> ";
        MessageBox.DEBUG(ppSQL);
    }

    #endregion

    #region 外部消息处理

    private bool _bInitMessage = false;
    protected virtual void InitMessage()
    {
        if (_bInitMessage)
        {
            return;
        }
        _bInitMessage = true;
        
    }

    protected virtual void UnInitMessage()
    {
        m_GMSocketMessagePool.f_Clear();
        m_GMSocketMessagePool = null;
    }
    #endregion


    #region 操作返回

    private Dictionary<int, List<sCommandReturn>> _aGameCommandReturn = new Dictionary<int, List<sCommandReturn>>();
    public void f_RegCommandReturn(int tEroeMsgOperateType, ccCallback tccCallbackSuc, ccCallback tccCallbackFail, bool bAlive = false)
    {
        List<sCommandReturn> ttList = null;
        sCommandReturn tsCommandReturn = new sCommandReturn();
        tsCommandReturn.m_SocketCallbackDT.m_ccCallbackSuc = tccCallbackSuc;
        tsCommandReturn.m_SocketCallbackDT.m_ccCallbackFail = tccCallbackFail;
        tsCommandReturn.m_bAlive = bAlive;
        if (_aGameCommandReturn.TryGetValue(tEroeMsgOperateType, out ttList))
        {
            ttList.Add(tsCommandReturn);
        }
        else
        {
            ttList = new List<sCommandReturn>();
            ttList.Add(tsCommandReturn);
            _aGameCommandReturn.Add((int)tEroeMsgOperateType, ttList);
        }
    }

    public void f_UnRegCommandReturnAll(int tEroeMsgOperateType)
    {
        List<sCommandReturn> ttList = null;
        if (_aGameCommandReturn.TryGetValue(tEroeMsgOperateType, out ttList))
        {
            _aGameCommandReturn.Remove((int)tEroeMsgOperateType);
        }
    }

    public void f_UnRegCommandReturn(int tEroeMsgOperateType, ccCallback tccCallbackSuc, ccCallback tccCallbackFail)
    {
        List<sCommandReturn> ttList = null;
        if (_aGameCommandReturn.TryGetValue(tEroeMsgOperateType, out ttList))
        {
            foreach (sCommandReturn tsCommandReturn in ttList)
            {
                if (tsCommandReturn.m_SocketCallbackDT.m_ccCallbackFail == tccCallbackFail && tsCommandReturn.m_SocketCallbackDT.m_ccCallbackSuc == tccCallbackSuc)
                {
                    ttList.Remove(tsCommandReturn);
                    return;
                }
            }
        }
    }

    protected void On_CMsg_GameCommandReturn(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        stGameCommandReturn tstGameCommandReturn = (stGameCommandReturn)Obj;

        //if (GloData.m_bDebug)
        //{
        //    eMsgOperateType teMsgOperateType = (eMsgOperateType)tstGameCommandReturn.iCommand;
        //    if (tstGameCommandReturn.iResult != 0)
        //    {
        //        MessageBox.DEBUG("操作失败：" + tstGameCommandReturn.iCommand + ">>" + teMsgOperateType.ToString() + " >> " + tstGameCommandReturn.iResult);
        //    }
        //}

        List<sCommandReturn> ttList = null;
        if (_aGameCommandReturn.TryGetValue(tstGameCommandReturn.iCommand, out ttList))
        {
            List<sCommandReturn> aWaitDelList = new List<sCommandReturn>();
            foreach (sCommandReturn tsCommandReturn in ttList)
            {
                if (tstGameCommandReturn.iResult == (int)eMsgOperateResult.OR_Succeed)
                {
                    if (tsCommandReturn.m_SocketCallbackDT.m_ccCallbackSuc != null)
                    {
                        tsCommandReturn.m_SocketCallbackDT.m_ccCallbackSuc(tstGameCommandReturn.iResult);
                    }
                }
                else
                {
                    if (tsCommandReturn.m_SocketCallbackDT.m_ccCallbackFail != null)
                    {
                        tsCommandReturn.m_SocketCallbackDT.m_ccCallbackFail(tstGameCommandReturn.iResult);
                    }
                }
                if (!tsCommandReturn.m_bAlive)
                {
                    aWaitDelList.Add(tsCommandReturn);
                }
            }
            foreach (sCommandReturn tsCommandReturn in aWaitDelList)
            {
                ttList.Remove(tsCommandReturn);
            }
            aWaitDelList.Clear();
        }

    }
    #endregion



    #region 时间

    public virtual void f_Ping()
    {
       
    }

    protected void On_Ping(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        basicNode1 tPing = (basicNode1)Obj;
        UpdateServerTime(tPing.value1);
    }

    public void UpdateServerTime(int iTime)
    {
        StaticValue.m_iNewServerTime = iTime;
        m_fLastTime = System.DateTime.Now;

        if (_Socket_Loop == null)
        {
            _Socket_Loop = (Socket_Loop)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Loop);
        }
        _Socket_Loop.f_UpdateTestTime();
    }


    public int f_GetServerTime()
    {
        System.TimeSpan ts = System.DateTime.Now - m_fLastTime;
        return StaticValue.m_iNewServerTime + (int)ts.TotalSeconds;        // Time.deltaTime;
    }

    #endregion



}//END Class
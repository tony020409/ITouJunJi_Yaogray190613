using ccU3DEngine;
using GameSysc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class GameSyscStatic
{
    /// <summary>
    /// 当前渲染次数
    /// </summary>
    public int m_iCurGameFpsNum = 0;
    /// <summary>
    /// 当前FPS时间
    /// </summary>
    public int m_iCurFpsTime = 0;
    /// <summary>
    /// 当前FPS已处理时间
    /// </summary>
    public int m_AccumilatedTime = 0;
    public int m_iAllGameFrame = 0;
    public bool m_bIsEnd = true;
}

public class GameSyscManager : MonoBehaviour
{
    private object _Locker = new object();
    private object _ActionLocker = new object();

    private GameSyscStatic _GameSyscStatic = new GameSyscStatic();
    private GameSyscObjectPool _GameSyscObjectPool = new GameSyscObjectPool();
    private GameStepSocket _GameStepSocket;
        
    /// <summary>
    /// 等待发送的动作队列
    /// </summary>
    private Queue<GameSysc.Action> actionsToSend = new Queue<GameSysc.Action>();
    /// <summary>
    /// 当前Turn执行的GameFrame
    /// </summary>
    private int _iGameFrame = 0;

    /// <summary>
    /// 当前数据FPS
    /// </summary>
    private FpsAverage _FpsAverage = new FpsAverage();

    private FpsAverage _NetAverage = new FpsAverage();

    private Stopwatch StopSocketWatchSW;
    private Stopwatch gameTurnSW;
    private int _iPlayerNum;
    public int m_iPlayerNum
    {
        get
        {
            return _iPlayerNum;
        }
    }

    /// <summary>
    /// 当前运行的总工作帧数
    /// </summary>
    private int _iCurGameSyscFrame;

    /// <summary>
    /// 当前运行的总工作帧数
    /// </summary>
    public int m_iCurGameSyscFrame
    {
        get
        {
            return _iCurGameSyscFrame;
        }
    }

    private int _iUpdateFps = 0;
    private int _iDoAction = 0;
    void Start()
    {
        enabled = false;
        _GameStepSocket = new GameStepSocket(this);
        gameTurnSW = new Stopwatch();
        StopSocketWatchSW = new Stopwatch();
        _iUpdateFps = ccTimeEvent.GetInstance().f_RegEvent(0.5f, true, null, Callback_UpdateFps);
        _iDoAction = ccTimeEvent.GetInstance().f_RegEvent(GloData.glo_fActionFPS, true, null, Callback_DoAction);
    }

    public void f_Stop()
    {
        enabled = false;
        ccTimeEvent.GetInstance().f_UnRegEvent(_iUpdateFps);
        ccTimeEvent.GetInstance().f_UnRegEvent(_iDoAction);
    }

    public void On_Start(CTS_StartGame tCTS_StartGame)
    {
        _iPlayerNum = tCTS_StartGame.m_iPlayerNum;
        enabled = true;

        _iCurGameSyscFrame = 0;       
        //StopSocketWatchSW.Start();
        //_GameSyscStatic.m_iNetWorkTime = 200;

        BattleMain.GetInstance().On_Start(tCTS_StartGame);
    }

    public void On_GameOver(CTS_GameOver tCTS_GameOver)
    {
        BattleMain.GetInstance().On_GameOver(tCTS_GameOver);
    }

    public void f_Reset()
    {
        _GameSyscObjectPool.f_Reset();
    }

    private void Callback_UpdateFps(object Obj)
    {
        if (BattleMain.GetInstance() != null)
        {
            _szLog.Remove(0, _szLog.Length);
            _szLog.AppendFormat("TT: {0} {1}", _NetAverage.f_GetAverage(), BattleMain.GetInstance().m_BattleRolePool.f_GetRoleCount());
            BattleMain.GetInstance().f_UpdateFps(_szLog.ToString());
        }
    }

    StringBuilder _szLog = new StringBuilder();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="iCurGameSyscFrame">当前运行的总工作帧数</param>
    /// <param name="iCurGameFpsNum">当前渲染次数</param>
    /// <param name="iCurFpsTime">当前FPS的时间</param>
    public void f_SyscUpdate(int iCurGameSyscFrame, int iCurGameFpsNum, int iCurFpsTime)
    {//来此服务器的更新事件

        //int iAllTime = 0;
        _iCurGameSyscFrame = iCurGameSyscFrame;
        //MessageBox.DEBUG("f_SyscUpdate:" + " " + iCurGameSyscFrame + " " + iCurGameFpsNum + " " + iCurFpsTime);
        //回复确认
        //_GameStepSocket.f_SendPlayerActionConfirm(m_iCurGameSyscFrame, StaticValue.m_UserDataUnit.m_PlayerDT.m_iId, _GameSyscStatic.m_iCurGameFpsNum, _FpsAverage.f_GetAverage());
        //_szLog.Remove(0, _szLog.Length);
        //_szLog.AppendFormat("{0} 回复确认: {1} {2} {3}", _iCurGameSyscFrame, _GameSyscStatic.m_iCurGameFpsNum, _GameSyscStatic.m_iCurFpsTime, _FpsAverage.f_GetAverage());
        ////MessageBox.DEBUG(_iCurGameSyscFrame + " 回复确认:" + " " + _GameSyscStatic.m_iCurGameFpsNum + " " + _GameSyscStatic.m_iCurFpsTime + " " + _FpsAverage.f_GetAverage());
        //MessageBox.DEBUG(_szLog.ToString());
        SendConfirmed();

        StaticValue.m_fNetAverage = _NetAverage.f_GetAverage();

        lock (_Locker)
        {
            _GameSyscStatic.m_iCurGameFpsNum = iCurGameFpsNum;
            _GameSyscStatic.m_iCurFpsTime = iCurFpsTime;
            _GameSyscStatic.m_AccumilatedTime = 0;
            _GameSyscStatic.m_bIsEnd = false;
            ccDeltaTime.deltaTime = (float)_GameSyscStatic.m_iCurFpsTime / 1000;
        }
    }

    void Update()
    {
        if (!enabled)
        {
            return;
        }
        int iDeltaTime = Convert.ToInt32((Time.deltaTime * 1000));
        int runtime = 0;

        lock (_Locker)
        {
            if (!_GameSyscStatic.m_bIsEnd)
            {
                gameTurnSW.Start();

                _GameSyscObjectPool.f_Update(_GameSyscStatic.m_iCurFpsTime);
                _GameSyscStatic.m_AccumilatedTime = _GameSyscStatic.m_AccumilatedTime - _GameSyscStatic.m_iCurFpsTime;

                gameTurnSW.Stop();
                runtime = Convert.ToInt32(iDeltaTime + gameTurnSW.ElapsedMilliseconds);
                _FpsAverage.f_Add(runtime);
                gameTurnSW.Reset();
                
                //MessageBox.DEBUG("RR " + _GameSyscStatic.m_iCurGameFpsNum + "FPS时间 " + _GameSyscStatic.m_iCurFpsTime + " FPS:" + iDeltaTime + " " + runtime);
                _GameSyscStatic.m_iAllGameFrame++;
                _GameSyscStatic.m_iCurGameFpsNum--;
                _GameSyscStatic.m_bIsEnd = true;
                
            }
        }

    }

    private void SendConfirmed()
    {
        StopSocketWatchSW.Reset();
        StopSocketWatchSW.Start();
                
        _GameStepSocket.f_SendPlayerActionConfirm(_iCurGameSyscFrame, StaticValue.m_UserDataUnit.m_PlayerDT.m_iId, _GameSyscStatic.m_iCurGameFpsNum, _FpsAverage.f_GetAverage());
    }
    
    private CreateSocketBuf _CreateSocketBufMain = new CreateSocketBuf();
    private CreateSocketBuf _CreateSocketBuf = new CreateSocketBuf();

    private void Callback_DoAction(object Obj)
    {
        if (!enabled)
        {
            return;
        }

        byte[] aBuf = BuildActionBuf();
        if (aBuf != null)
        {
            _GameStepSocket.f_SendPlayerAction(aBuf);
        }
    }

    private byte[] BuildActionBuf()
    {
        lock (_ActionLocker)
        {
            if (actionsToSend.Count > 0)
            {
                //MessageBox.DEBUG("Action " + actionsToSend.Count);
                _CreateSocketBuf.f_Reset();
                _CreateSocketBufMain.f_Reset();
                _CreateSocketBufMain.f_Add(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId);
                _CreateSocketBufMain.f_Add(_iCurGameSyscFrame);
                //_CreateSocketBufMain.f_Add(_GameSyscStatic.m_iCurGameFpsNum);
                //_CreateSocketBufMain.f_Add(_FpsAverage.f_GetAverage());
                _CreateSocketBufMain.f_Add(actionsToSend.Count);

                while (actionsToSend.Count > 0)
                {
                    GameSysc.Action action = actionsToSend.Dequeue();
                    action.f_Save(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId); //, _iCurGameSyscFrame, _GameSyscStatic.m_iCurGameFpsNum, _FpsAverage.f_GetAverage());
                    byte[] aBytes = ActionTools.Serialize(action);
                    _CreateSocketBufMain.f_Add(aBytes.Length);
                    _CreateSocketBuf.f_Add(aBytes);
                }
                byte[] aUnZipBuf = _CreateSocketBuf.f_GetBuf();
                byte[] aBuf = ZipTools.aaaa3221(aUnZipBuf);
                _CreateSocketBufMain.f_Add(aUnZipBuf.Length);
                _CreateSocketBufMain.f_Add(aBuf);
                return _CreateSocketBufMain.f_GetBuf();
            }
            else
            {
                return null;
            }
         
        }

    }


    /// <summary>
    /// 增加一个新的动作
    /// </summary>
    /// <param name="action"></param>
    public void f_AddMyAction(GameSysc.Action action)
    {
		if (!enabled)
        {
            return;
        }
        lock(_ActionLocker)
        {
            //action.f_Save(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId, _iCurGameSyscFrame);
            actionsToSend.Enqueue(action);
            if (actionsToSend.Count > 100)
            {
                MessageBox.DEBUG(action.m_iType + " AddMyAction " + actionsToSend.Count);
            }
        }
	}


    public void f_AddSyscObject(IPlayerMono tIPlayerMono)
    {
        _GameSyscObjectPool.f_Reg(tIPlayerMono);
    }

    
    ReadBuf _DispPlayerActionBuf = new ReadBuf();
    public void f_ServerBroadCastAction(byte[] aBuf)
    {
        _DispPlayerActionBuf.f_Save(aBuf, aBuf.Length);

        int iUsreId = _DispPlayerActionBuf.f_ReadInt();
        int iActionGameSyscFrame = _DispPlayerActionBuf.f_ReadInt();
        //int iActionGameFpsNum = _DispPlayerActionBuf.f_ReadInt();
        //int iAverage = _DispPlayerActionBuf.f_ReadInt();
        int iActionCount = _DispPlayerActionBuf.f_ReadInt();

        //MessageBox.DEBUG(iUsreId + " " + iActionGameSyscFrame  + " RecvAction " + iActionCount );
        if (iActionCount > 0)
        {
            int[] aActionLen = new int[iActionCount];
            for (int i = 0; i < iActionCount; i++)
            {
                aActionLen[i] = _DispPlayerActionBuf.f_ReadInt();
            }
            int iUnZipPackLen = _DispPlayerActionBuf.f_ReadInt();
            byte[] aZipBuf = _DispPlayerActionBuf.f_ReadBufToEnd();
            byte[] aUnZipBuf = ZipTools.aaa557788(aZipBuf, iUnZipPackLen);
            int iPos = 0;
            for (int i = 0; i < iActionCount; i++)
            {
                byte[] aTTT = new byte[aActionLen[i]];
                Array.Copy(aUnZipBuf, iPos, aTTT, 0, aActionLen[i]);
                GameSysc.Action action = ActionTools.DeSerialize(aTTT);
                action.ProcessAction();
                iPos += aActionLen[i];
            }
        }
    }
    

    public void f_ServerActionConfirm(CTS_ServerActionConfirm tCTS_ServerActionConfirm)
    {
        StopSocketWatchSW.Stop();
        _NetAverage.f_Add((int)StopSocketWatchSW.ElapsedMilliseconds);

    }



}




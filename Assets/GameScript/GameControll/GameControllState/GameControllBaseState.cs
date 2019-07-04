using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllBaseState : ccMachineStateBase
{
    protected GameControllDT _CurGameControllDT = null;
    private bool _bIsRuning = false;
    private bool _bIsEnd = false;

    private float _fTimeOut = 30;
    private int _iRunTimeOutId = 0;
    public GameControllBaseState(int tiId)
        : base(tiId)
    {

    }
    
    public GameControllBaseState f_Clone()
    {
        GameControllBaseState tGameControllBaseState = (GameControllBaseState)MemberwiseClone();
        return tGameControllBaseState;
    }


    protected void StartRun(object Obj = null)
    {
        _bIsEnd = false;
        base.f_Enter(Obj);

        _CurGameControllDT.m_iRunTimes++;
        //MessageBox.DEBUG("StartRun " + this.ToString());

        if (_CurGameControllDT.m_emMissionEndType != GameEM.EM_MissionEndType.None)
        {
            MessageBox.DEBUG("任务的状态错误" + _CurGameControllDT.m_emMissionEndType.ToString());
        }
        
        if (StaticValue.m_bIsMaster)
        {
            if (_iRunTimeOutId == 0)
            {
                if (_CurGameControllDT.fRunTimeOut <= 0)
                {
                    _fTimeOut = GloData.glo_fMaxRunTimeOut + _CurGameControllDT.fStartSleepTime;
                }
                else
                {
                    _fTimeOut = _CurGameControllDT.fRunTimeOut + _CurGameControllDT.fStartSleepTime;
                }
                _iRunTimeOutId = ccTimeEvent.GetInstance().f_RegEvent(1, true, Obj, CallBack_RunTimeOut);
            }
            else
            {
                MessageBox.DEBUG("重複執行動作 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szName);
            }
        }
        if (_CurGameControllDT.fStartSleepTime > 0)
        {
            ccTimeEvent.GetInstance().f_RegEvent(_CurGameControllDT.fStartSleepTime, false, Obj, BaseRun);
        }
        else
        {
            BaseRun(Obj);
        }
    }

    protected void EndRun(object Obj = null)
    {
        //MessageBox.DEBUG("【任務腳本】以上任務結束-----------------------" + iId + " " + this.ToString());
        if (_bIsRuning)
        {
            _bIsRuning = false;
            if (_CurGameControllDT.fEndSleepTime > 0)
            {
                ccTimeEvent.GetInstance().f_RegEvent(_CurGameControllDT.fEndSleepTime, false, Obj, EndCurGoNext);
            }
            else
            {
                EndCurGoNext(Obj);
            }
        }
    }

    private void BaseRun(object Obj)
    {
        _bIsRuning = true;
        Run(Obj);
        if (_CurGameControllDT.iNeedEnd == 0)
        {
            EndCurGoNext(null);
        }
    }

    protected virtual void Run(object Obj)
    {
        
    }

    protected bool IsRuning()
    {
        return _bIsRuning;
    }

    private void EndCurGoNext(object Obj)
    {
        if (_bIsEnd)
        {
            return;
        }
        _bIsEnd = true;
        ClearRunTimeOut();
        _CurGameControllDT.m_emMissionEndType = GameEM.EM_MissionEndType.MissionRunEnd;
        if (StaticValue.m_bIsMaster)
        {
            //MessageBox.DEBUG("EndCurGoNext " + _CurGameControllDT.iId + " " + this.ToString());
            CheckForceGameOver();
            if (_CurGameControllDT.iNeedEnd == 0 &&
                _CurGameControllDT.fEndSleepTime == 0 && 
                _CurGameControllDT.iEndAction == 0) {
                //if (BattleMain.GetInstance().Debug_Object_isNull) {
                //    MessageBox.DEBUG("未有下一个动作整个执行结束 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szName);
                //}
                f_SetComplete((int)EM_GameControllAction.Loop, Obj);
            }
            else
            {
                f_SetComplete((int)EM_GameControllAction.Read, Obj);
            }
        }
        else
        {
            f_SetComplete((int)EM_GameControllAction.Loop, Obj);
        }
    }

    public bool f_IsEnd()
    {
        return _bIsEnd;
    }

    private void CallBack_RunTimeOut(object Obj)
    {
        _fTimeOut--;
        //任務超時
        if (_fTimeOut < 0) {
            EndCurGoNext(null);
            _CurGameControllDT.m_emMissionEndType = GameEM.EM_MissionEndType.MessionTimeOut;
            MessageBox.DEBUG("任務執行超時 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szName);            
        }

    }

    private void ClearRunTimeOut()
    {
        ccTimeEvent.GetInstance().f_UnRegEvent(_iRunTimeOutId);
        _iRunTimeOutId = 0;
    }

    private void CheckForceGameOver()
    {
        //强制检测游戏结束
        if (_CurGameControllDT.iEndAction == -99) {
            MessageBox.DEBUG("CheckForceGameOver ");
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Lost);
        }

    }

}


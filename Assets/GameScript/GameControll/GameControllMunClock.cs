using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllMunClock
{
    private int _iIndex;
    private List<GameControllBaseState> _aList = new List<GameControllBaseState>();
    private object _oLock = new object();



    public void f_Read(object Obj)
    {
        GameControllDT tGameControllDT = (GameControllDT)Obj;
        EM_GameControllAction tEM_GameControllAction = (EM_GameControllAction)tGameControllDT.iStartAction;
        if (GameControllTools.f_CheckStateTypeIsRight(tEM_GameControllAction))
        {
            ServerActionState tServerActionState = new ServerActionState();
            tServerActionState.f_Save(tGameControllDT.iId * 100000);
            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tServerActionState);
        }
        else
        {
            MessageBox.ASSERT("【任务类型错误】[" + tGameControllDT.iId.ToString() + "] " + tGameControllDT.szName);
        }

    }

    public void f_Execute(GameControllDT tGameControllDT)
    {
        if (tGameControllDT == null)
        {
            MessageBox.ASSERT("对应的任务的后续任务未找到 " + tGameControllDT.iId + ">>>" + tGameControllDT.iEndAction);
            return;
        }
        lock (_oLock)
        {
            MessageBox.DEBUG("增加定时器任务 " + tGameControllDT.iId + " " + tGameControllDT.szName);

            EM_GameControllAction tEM_GameControllAction = (EM_GameControllAction)tGameControllDT.iStartAction;
            GameControllBaseState tGameControllBaseState = GameControllTools.f_CreateState(tEM_GameControllAction);
            tGameControllBaseState.f_Enter(tGameControllDT);
            _aList.Add(tGameControllBaseState);
        }
    }
    
    public void f_Update()
    {
        lock (_oLock)
        {
            _iIndex = 0;
            for (int i = 0; i < _aList.Count; i++)
            {
                _aList[i].f_Execute();
                if (_aList[i].f_IsEnd())
                {
                    MessageBox.DEBUG("移除已完成定时器任务 " + _aList[i].iId);
                    _aList.Remove(_aList[i]);
                    return;
                }
            }
        }
    }


    
}
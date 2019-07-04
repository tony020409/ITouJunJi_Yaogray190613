using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using UnityEngine.Playables;


/// <summary>
/// 一次性定时器事件(等待操作对此指令无效)
/// 1303.定时器事件(参数1为定时时间到后执行的下一条指令,参数23无效)
/// </summary>
public class GameControllClock : GameControllBaseState
{
    private GameControllMunClock _GameControllMunClock = null;
    private TimeLineMessageControll _TimeLineMessageControll = null;
    public GameControllClock(GameControllMunClock tGameControllMunClock)
        : base((int)EM_GameControllAction.AutoClock)
    {
        _GameControllMunClock = tGameControllMunClock;
    }

    public override void f_Enter(object Obj)
    {
        if (StaticValue.m_bIsMaster)
        {
            _CurGameControllDT = (GameControllDT)Obj;

            GameControllDT tGameControllDT = (GameControllDT)glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetSC(ccMath.atoi(_CurGameControllDT.szData1));
            if (tGameControllDT == null)
            {
                MessageBox.ASSERT("读取定时器事件对应的下一动作失败 " + _CurGameControllDT.iId + " " + _CurGameControllDT.szData1);
                return;
            }
            _CurGameControllDT.fEndSleepTime = 0;
            if (_CurGameControllDT.fStartSleepTime > 0)
            {
                ccTimeEvent.GetInstance().f_RegEvent(_CurGameControllDT.fStartSleepTime, false, tGameControllDT, Doing);
            }
            else
            {
                Doing(tGameControllDT);
            }
            Run(null);
        }
    }

    protected override void Run(object Obj)
    {
        base.Run(Obj);
    }

    private void Doing(object Obj)
    {
        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming)
        {
            MessageBox.DEBUG("Do Clock " + _CurGameControllDT.iId + ">>" + _CurGameControllDT.szData1);
            _GameControllMunClock.f_Read(Obj);
        }
    }
      

}

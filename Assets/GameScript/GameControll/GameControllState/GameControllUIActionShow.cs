using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllUIActionShow : GameControllBaseState
{
    public GameControllUIActionShow() : base((int)EM_GameControllAction.UIActionShow)
    {
    }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        MessageBox.DEBUG("GameControllUIActionShow " + _CurGameControllDT.iId);

        //所有的出生行为强制修改成必须等待
        _CurGameControllDT.iNeedEnd = 1;

        StartRun();
    }

    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //31.控制相应的UI组件显示 （参数1有效玩家Id -99表示所有玩家同时有效，参数2组件名称，参数3显示时间）
        int[] aPlayer = ccMath.f_String2ArrayInt(_CurGameControllDT.szData1, ";");

        UIActionShow(aPlayer, _CurGameControllDT.szData2, ccMath.atof(_CurGameControllDT.szData3));

        //然後結束
        EndRun();
    }
    
    private void UIActionShow(int[] aPlayer, string strAction, float fTime)
    {
        for(int i = 0; i < aPlayer.Length; i++)
        {
            if (aPlayer[i] == -99)
            {
                BattleMain.GetInstance().f_UIActionShow(strAction, fTime);
                return;
            }
            else if (StaticValue.m_UserDataUnit.m_PlayerDT.m_iId == aPlayer[i])
            {
                BattleMain.GetInstance().f_UIActionShow(strAction, fTime);
                return;
            }

        }
    }

    public override void f_Execute()
    {
        base.f_Execute();

    }

}



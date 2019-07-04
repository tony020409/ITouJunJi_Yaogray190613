using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllShowUIText : GameControllBaseState
{
    public GameControllShowUIText() : base((int)EM_GameControllAction.ShowUIText)
    {

    }

    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        MessageBox.DEBUG("GameControllShowUIText " + _CurGameControllDT.iId);

        //所有的出生行为强制修改成必须等待
        _CurGameControllDT.iNeedEnd = 1;

        StartRun();
    }

    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //30.显示UI文字 （参数1有效玩家Id -99表示所有玩家同时有效，参数2文字内容，参数3文字显示时间）

        int[] aPlayer = ccMath.f_String2ArrayInt(_CurGameControllDT.szData1, ";");

        ShowUIText(aPlayer, _CurGameControllDT.szData2, ccMath.atof(_CurGameControllDT.szData3));

        //然後結束
        EndRun();
    }
    
    private void ShowUIText(int[] aPlayer, string strText, float fTime)
    {
        for(int i = 0; i < aPlayer.Length; i++)
        {
            if (aPlayer[i] == -99)
            {
                BattleMain.GetInstance().f_ShowUIText(strText, fTime);
                return;
            }
            else if (StaticValue.m_UserDataUnit.m_PlayerDT.m_iId == aPlayer[i])
            {
                BattleMain.GetInstance().f_ShowUIText(strText, fTime);
                return;
            }

        }
    }

    public override void f_Execute()
    {
        base.f_Execute();

    }

}



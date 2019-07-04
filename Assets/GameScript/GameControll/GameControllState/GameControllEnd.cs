using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllEnd : GameControllBaseState
{
    public GameControllEnd()
        : base((int)EM_GameControllAction.End)
    {

    }


    public override void f_Enter(object Obj)
    {
        MessageBox.DEBUG("---------主线任务执行完成,等待游戏结束-------------");
        //BattleMain.GetInstance().f_EndGame();
    }

    public override void f_Execute()
    {

    }
}

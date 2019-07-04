using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllServerAction : GameControllBaseState
{

    public GameControllServerAction()
        : base((int)EM_GameControllAction.ServerAction)
    {

    }


    public override void f_Enter(object Obj)
    {
        int iId = (int)Obj;
        _CurGameControllDT = (GameControllDT)glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetSC(iId);
        if (_CurGameControllDT == null) {
            MessageBox.ASSERT("主线任务服务器操作指令任务未找到 " + iId);
        }      
        Disp();
    }


    private void Disp()
    {
        EM_GameControllAction tEM_GameControllAction = (EM_GameControllAction) _CurGameControllDT.iStartAction;
        if (GameControllTools.f_CheckStateTypeIsRight(tEM_GameControllAction))
        {
            //MessageBox.DEBUG("主线任务服务器操作指令 执行动作 " + tEM_GameControllAction.ToString() + "-" + _CurGameControllDT.iId + "-" + _CurGameControllDT.szName);
            //f_SetComplete(_CurGameControllDT.iStartAction, _CurGameControllDT);
            try
            {
                f_ForceChangeState(_CurGameControllDT.iStartAction, _CurGameControllDT);
            }
            catch
            {
                Debug.LogError("指令["+_CurGameControllDT.iId + "]：" + _CurGameControllDT.szName + "執行異常！ 請檢察指令內容 (e.g. 像是參數欄位是否有對應正確...)");
            }
           
        }
        else
        {
            MessageBox.ASSERT("未知的动作事件 " + _CurGameControllDT.iId + " " + _CurGameControllDT.iStartAction);
        }
    }

    public override void f_Execute()
    {

    }


}

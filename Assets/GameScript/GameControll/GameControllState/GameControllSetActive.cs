using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllSetActive : GameControllBaseState
{

    BaseRoleControllV2 _BaseRoleControl;
    GameObject targetObj = null; //解析要開關的物件
    private string path;         //子物件路徑
    private bool state = true;   //開關狀態

    public GameControllSetActive()
        : base((int)EM_GameControllAction.SetActive)
    {
    }


    //8.開關物件事件（参数1为角色在場景上被分配的Id，参数2为子物件路徑(空白則關閉自己)，参数3为開關狀態）
    //例如1:「RoleModel」            →關閉 id=指定 物體下的 RoleModel
    //例如2:「RoleModel/jeep_body_n」→關閉 id=指定 物體下的 RoleModel下的jeep_body_n
    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;                                                               //當前任務
        _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1)); //(參數1) 指定物件的id
        path = _CurGameControllDT.szData2;                            //(參數2) 子物件路徑(填"Self"或"null"或空白表示關閉物件自己)
        state = System.Convert.ToBoolean(_CurGameControllDT.szData3); //(參數3) 希望的開or關狀態

        if (path == "" || path == _BaseRoleControl.name)  //如果參數2是空白或是填id
        { targetObj = _BaseRoleControl.gameObject; }   //開關目標是就指定id的物件                    
        else                                           //如果是填子物件的路徑
        { targetObj = _BaseRoleControl.transform.Find(path).gameObject; } //開關目標是路徑上的子物件

        if (_BaseRoleControl == null)
        {
            MessageBox.ASSERT("【任務腳本】步驟" + _CurGameControllDT.iId + "未找到指定的角色 :" + _CurGameControllDT.szData1);
            EndRun();
            return;
        }
        else
        {
            MessageBox.ASSERT("【任務腳本】步驟" + _CurGameControllDT.iId + "讓角色:" + _CurGameControllDT.szData1 + " 下的 " + _CurGameControllDT.szData2 + "顯示為" + _CurGameControllDT.szData3);
        }
        StartRun();
    }

    
    //public override void f_Execute()
    //{
    //    if (IsRuning())
    //    {
    //        //aPos = ccMath.f_String2ArrayString(_CurGameControllDT.szData2, "/");      //分析物件是否有階層(父物件id-子物件名稱)
    //        //if (_BaseRoleControl != null)
    //        //{
    //        //    if (aPos[1] != null)
    //        //    {
    //        //        for (int i = 0; i < _BaseRoleControl.transform.childCount; ++i)
    //        //        {
    //        //            if (_BaseRoleControl.transform.GetChild(i).name == aPos[1])
    //        //            {
    //        //                targetObj = _BaseRoleControl.transform.GetChild(i).gameObject;
    //        //                targetObj.gameObject.SetActive(state);
    //        //                Debug.LogWarning("【任務腳本】關閉" +_BaseRoleControl.transform.GetChild(i).name);
    //        //                EndRun();
    //        //            }
    //        //        }
    //        //    }
    //        //    else
    //        //    {
    //        //        targetObj = _BaseRoleControl.gameObject;
    //        //        targetObj.gameObject.SetActive(state);
    //        //        EndRun();
    //        //    }
    //        //}
    //    }
    //}

    
    protected override void Run(object Obj)
    {
        if (_BaseRoleControl != null)
        {
            targetObj.gameObject.SetActive(state); //開關目標
            EndRun();
        }
    }


}
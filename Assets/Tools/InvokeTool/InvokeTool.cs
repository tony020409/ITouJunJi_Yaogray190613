using System;
using System.Collections.Generic;
using UnityEngine;

class TimeObj
{
    public BaseRoleControllV2 tarObj;
    public float step;
    public finEvent.FinishEvent UpdateCallback;
    public finEvent.FinishEvent FinishEvent;
}

public class InvokeTool : MonoBehaviour
{
    private bool _bRun = false;
    List<TimeObj> tarObjs = new List<TimeObj>();

  
    /// <summary>
    /// 等待多久
    /// </summary>
    /// <param name="targetObj">需要等待的物件</param>
    /// <param name="finishSec">需要等待的時間</param>
    /// <param name="Callback" > 要執行的事件 </param>
    public static void f_Invoke(BaseRoleControllV2 tBaseRoleControl, float finishSec, finEvent.FinishEvent finishCallback = null)
    {
        if (tBaseRoleControl == null) return;

        InvokeTool tInvokeTool = tBaseRoleControl.gameObject.GetComponent<InvokeTool>();
        if (tInvokeTool == null)
        { tInvokeTool = tBaseRoleControl.gameObject.AddComponent<InvokeTool>(); }

        TimeObj tObj = new TimeObj
        {
            tarObj = tBaseRoleControl,
            step = finishSec,
            UpdateCallback = null,
            FinishEvent = finishCallback
        };
        tInvokeTool.tarObjs.Add(tObj);
        tInvokeTool._bRun = true;
    }


    #region ============== 內部使用函數 ===============
    private void FixedUpdate()
    {
        if (!_bRun)
        { return;}

        for (int i = 0; i < tarObjs.Count; i++)
        {
            if (tarObjs[i].tarObj == null)
            {
                tarObjs.RemoveAt(i);
                i--;
                continue;
            }

            this.Invoke("DoFinish", tarObjs[i].step);
            _bRun = false;
        }
    }

    public void DoFinish()
    {
        for (int i = 0; i < tarObjs.Count; i++)
        {
            if (tarObjs[i].tarObj == null){
                tarObjs.RemoveAt(i);  //反註冊該物件
                i--;
                continue;
            }
            try
            {
                tarObjs[i].FinishEvent(); // 呼叫 CallBack 函數
            }
            catch { }
            tarObjs[i].FinishEvent();   //呼叫 CallBack 函數
            tarObjs.RemoveAt(i);        //反註冊該物件
            Destroy(this);
        }
    }
    #endregion ============================================
}

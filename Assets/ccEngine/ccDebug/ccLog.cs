using ccU3DEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ccLog
{
    private string  _strUserId = "";

    System.DateTime _SleepLogTime;
    private List<string> _aLogText = new List<string>();
    private int _iTimeId = 0;
    public ccLog()
    {

    }

    public void f_SetUserId(string strUserId)
    {
        _strUserId = strUserId;
    }

    public void f_Start()
    {
        if (GloData.m_bDebugLog)
        {
            MessageBox.f_OpenLog();
        }
        _SleepLogTime = System.DateTime.Now;
        if (GloData.glo_iAutoUpdateLog == 0)
        {
            return;
        }
        else if (GloData.glo_iAutoUpdateLog == 1)
        {
            
        }
        else if (GloData.glo_iAutoUpdateLog == 2)
        {
            Application.logMessageReceived += CallBack_SystemLog;
        }

        MessageBox.f_RegLogCall(CallBackMessageBox);
        _iTimeId = ccTimeEvent.GetInstance().f_RegEvent(1, true, null, UpdateSaveLog);
    }

    public void f_Quit()
    {
        UpdateSaveLog(null);
    }

    public void CallBack_SystemLog(string logString, string stackTrace = null, LogType type = LogType.Log)
    {
        _aLogText.Add(logString);
        if (stackTrace != null)
        {
            _aLogText.Add(stackTrace);
        }
    }

    private void CallBackMessageBox(int iMsgType, string strMsg)
    {        
        _aLogText.Add(strMsg);
    }


    /// <summary>
    /// 保存log到txt文本
    /// </summary>
    void UpdateSaveLog(object Obj)
    {
        if (_aLogText.Count > GloData.glo_iMaxLogSize)
        {
            _SleepLogTime = System.DateTime.Now;
            UpdateLog(_strUserId, _aLogText);
            _aLogText.Clear();
        }
        else
        {
            if (_aLogText.Count > 0)
            {
                TimeSpan ttt = System.DateTime.Now - _SleepLogTime;
                if (ttt.TotalSeconds > GloData.glo_iAutoUpdateLogTime)
                {
                    _SleepLogTime = System.DateTime.Now;
                    UpdateLog(_strUserId, _aLogText);
                    _aLogText.Clear();
                }
            }
        }
    }


    private void UpdateLog(string strUserId, List<string> aLog)
    {
        StringBuilder strLog = new StringBuilder();
        for (int i = 0; i < aLog.Count; i++)
        {
            strLog.Append(aLog[i]);
            strLog.Append("\n");
        }

        WWWForm form = new WWWForm();
        form.AddField("iUserId", strUserId);
        form.AddField("strLog", strLog.ToString());
        WWW w = new WWW(GloData.glo_strSaveLog, form);
    }

}
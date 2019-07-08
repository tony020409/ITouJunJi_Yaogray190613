using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;

//記得到Action.cs加
[ProtoContract]
public class ChangeParameterAction : GameSysc.Action
{

    [ProtoMember(10860)]
    public string m_ParameterName; //要變更的參數名稱

    [ProtoMember(10861)]
    public string m_newValue;   //參數要變更的值


    /// <summary>
    /// 設定參數變更的內容
    /// </summary>
    /// <param name="newParameterName"> 要變更的參數名稱 </param>
    /// <param name="newValue"        > 參數要變更的值 </param>
    public void f_SetParameter(string newParameterName, string newValue){
        m_ParameterName = newParameterName;
        m_newValue = newValue;
    }


    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {
        BattleMain.GetInstance().f_SetParamentData(m_ParameterName, m_newValue);
    }


}


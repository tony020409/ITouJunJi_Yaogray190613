using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;
using UnityEngine.AI;

//記得到Action.cs加
[ProtoContract]
public class ChangeAnimatorAction : GameSysc.Action
{

    [ProtoMember(10651)]
    public int m_ild;          //執行的對像

    [ProtoMember(10652)]
    public string m_Parameter; //要設定的參數

    [ProtoMember(10653)]
    public int m_Value;        //要設定的值


    /// <summary>
    /// 設定相關數值
    /// </summary>
    public void f_SetAnimator(int newID, string newParameter, int newValue) {
        m_ild = newID;
        m_Parameter = newParameter;
        m_Value = newValue;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().f_GetRoleControl2(m_ild);
        if (tmpRole == null) {
            MessageBox.ASSERT("找不到編號 " + m_ild + "的角色，所以無法讓它變更 Animator 中的 " + m_Parameter + " 參數值");
            return;
        }
        tmpRole.GetComponent<Animator>().SetInteger(m_Parameter, m_Value);//切換數值

        
        if (tmpRole.GetComponent<NavMeshAgent>() != null) {
            tmpRole.GetComponent<NavMeshAgent>().Warp(tmpRole.transform.position);
            tmpRole.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }


}

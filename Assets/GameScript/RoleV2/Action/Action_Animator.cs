using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;


//記得到Action.cs加
[ProtoContract]
public class Action_Animator : BaseActionV2
{

    public Action_Animator() 
        : base(GameEM.EM_RoleAction.Animator)
    { }

    [ProtoMember(32001)]
    public int m_RoleId;           //要改變動畫的角色id

    [ProtoMember(32002)]
    public int m_ChangeType;       //對 Animator的動作

    [ProtoMember(32003)]
    public string m_nextAnimation; //參數1

    [ProtoMember(32004)]
    public int m_bool;             //bool相關的設定



    /// <summary>
    /// 
    /// </summary>
    /// <param name="newID"       > 執行角色 </param>
    /// <param name="changeType"  > 改變類型 </param>
    /// <param name="newAnimValue"> 改變值   </param>
    public void f_SetAnimation(int newID, GameEM.EM_Animator changeType, string newAnimValue, bool tmpbool = true){
        m_RoleId = newID;
        m_ChangeType = (int)changeType;
        m_nextAnimation = newAnimValue;
        if (tmpbool == true){
            m_bool = 1;
        } else {
            m_bool = 0;
        }
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_RoleId);

        //如果角色不存在
        if (tmpRole == null){
            return;
        }

        //如果角色死了
        if (tmpRole.f_IsDie()) {
            return;
        }


        if (m_ChangeType == (int)GameEM.EM_Animator.Play) {
            tmpRole.GetComponent<Animator>().Play(m_nextAnimation);
        }

        if (m_ChangeType == (int)GameEM.EM_Animator.CrossFade){
            tmpRole.GetComponent<Animator>().CrossFade(m_nextAnimation, 0.25f);
        }

        if (m_ChangeType == (int)GameEM.EM_Animator.SetTrigger) {
            tmpRole.GetComponent<Animator>().SetTrigger(m_nextAnimation);
        }

        if (m_ChangeType == (int)GameEM.EM_Animator.SetBool){
            if (m_bool == 1) {
                tmpRole.GetComponent<Animator>().SetBool(m_nextAnimation, true);
            }
            else {
                tmpRole.GetComponent<Animator>().SetBool(m_nextAnimation, false);
            }
            
        }

    }





}

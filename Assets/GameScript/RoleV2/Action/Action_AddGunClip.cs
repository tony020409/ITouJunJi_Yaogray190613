using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;


//記得到Action.cs加
[ProtoContract]
public class Action_AddGunClip : BaseActionV2
{

    public Action_AddGunClip()
        : base(GameEM.EM_RoleAction.AddGunClip)
    { }

    [ProtoMember(35001)]
    public int m_RoleId;  //執行換子彈的對象

    [ProtoMember(35002)]
    public int m_Value;  //變化量



    /// <summary>
    /// 設定加彈夾 (執行換子彈的對象的ID, 變化量)
    /// </summary>
    /// <param name="newID"   > 執行換子彈的對象的ID </param>
    /// <param name="newValue"> 變化量 </param>
    public void f_Set(int newID, int newValue){
        m_RoleId = newID;
        m_Value = newValue;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_RoleId);

        //如果角色不存在
        if (tmpRole == null) {
            return;
        }

        //如果角色死了
        if (tmpRole.f_IsDie()) {
            return;
        }

        //角色是玩家的話，就補子彈
        MySelfPlayerControll2 _Player = tmpRole.GetComponent<MySelfPlayerControll2>();
        if (_Player != null) {
            _Player.f_AddClip(m_Value);
        }

    }





}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;


//記得到Action加
/// <summary>
/// 更換 OtherPlayer手上的槍模型，假裝 OtherPlayer也換槍了
/// </summary>
[ProtoContract]
public class Action_PlayerChangeGun : BaseActionV2 //GameSysc.Action
{

    public Action_PlayerChangeGun()
        : base(GameEM.EM_RoleAction.PlayerChangeGun)
    { }

    [ProtoMember(38001)]
    public int m_RoleId;  //執行換槍的角色

    [ProtoMember(38002)]
    public int m_GunType; //換的槍枝


    public void f_Set(int newiId, GunEM tGunType) {
        m_RoleId = newiId;
        m_GunType = (int)tGunType;
    }

    

    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {
        OtherPlayerControll2 tmpRole = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_RoleId).GetComponent<OtherPlayerControll2>();

        //如果角色存在
        if (tmpRole != null){
            tmpRole.f_ChangeGun((GunEM)m_GunType);
        }

    }



}


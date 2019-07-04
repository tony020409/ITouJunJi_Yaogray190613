using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

//記得到Action.cs加
[ProtoContract]
public class Action_GetPickable : BaseActionV2
{

    public Action_GetPickable() 
        : base(GameEM.EM_RoleAction.GetPickable)
    { }

    [ProtoMember(33001)]
    public int m_RoleId;  //被撿到的物品

    [ProtoMember(33002)]
    public int m_OwnerId; //撿到的人

    [ProtoMember(33003)]
    public int m_GetState; //是被撿到還是放開 (=0表示放開 / =1表示撿到)



    /// <summary>
    /// 設定使用者 (物件自己的ID, 撿到的人的ID)
    /// </summary>
    /// <param name="newID"     > 物件自己的ID </param>
    /// <param name="newOwnerID"> 撿到的人的ID </param>
    public void f_SetOwner(int newID, int newOwnerID)  {
        m_RoleId = newID;
        m_OwnerId = newOwnerID;
        m_GetState = 1;
    }


    /// <summary>
    /// 同步重置
    /// </summary>
    /// <param name="newID"     > 物件自己的ID </param>
    public void f_Reset(int newID) {
        m_RoleId = newID;
        m_GetState = 0;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
        //拾取品角色
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_RoleId);

        //如果物件不存在
        if (tmpRole == null) {
            return;
        }

        //如果物件死亡 (=使用次數用盡)
        if (tmpRole.f_IsDie()) {
            return;
        }

        //拾取組件
        PickableRoleControl _PickableRoleControl = tmpRole.GetComponent<PickableRoleControl>();

        //如果沒有拾取組件，就不執行事件
        if (_PickableRoleControl == null) {
            return;
        }

        //如果物品是被放開就重置
        if (m_GetState == 0) {
            _PickableRoleControl.f_Reset();
        }

        //如果物品是被撿起來就設定使用者、發動觸發效果
        else {
            if (_PickableRoleControl.canUSE) {
                BaseRoleControllV2 tmpOwner = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_OwnerId);
                _PickableRoleControl._Owner = tmpOwner;  //設定使用者
                _PickableRoleControl.f_PickUp();         //發動物件效果
            }
        }


    }



}

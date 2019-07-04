using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 檢查玩家的手
/// </summary>
public class ConditionAI_CheckPlayerHand : AI_ConditionBaseStateV2
{


    public ConditionAI_CheckPlayerHand()
        : base(AI_EM.EM_AIState.CheckPlayerHand)
    { }



    public override bool f_ConditionTest()
    {
        List<BaseRoleControllV2> aData = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemyAll2(_BaseRoleControl, 10);
        for (int i = 0; i < aData.Count; i++) {
            if (aData[i].GetComponent<MySelfPlayerControll2>() != null) {
                if (Vector3.Distance(aData[i].GetComponent<MySelfPlayerControll2>().m_BulletStart.transform.position, _BaseRoleControl.transform.position) < _BaseRoleControl.f_GetAttackSize())  {
                    return ReturnOwner(_BaseRoleControl.m_iId, aData[i].m_iId);
                }
            }
            else if (aData[i].GetComponent<OtherPlayerControll2>() != null) {
                if (Vector3.Distance(aData[i].GetComponent<OtherPlayerControll2>().m_BulletStart.position, _BaseRoleControl.transform.position) < _BaseRoleControl.f_GetAttackSize()) {
                    return ReturnOwner(_BaseRoleControl.m_iId, aData[i].m_iId);
                }
            }
        }
        return false;
    }



    /// <summary>
    /// 觸碰事件
    /// </summary>
    public bool ReturnOwner(int RoleID, int OwnerID) {
        //如果是 Pickable類型角色，傳遞自己被撿到的消息，然後執行各自在 PickableRoleControl 的設定
        if (_BaseRoleControl.f_GetRoleType() == GameEM.emRoleType.Pickable) {
            if (_BaseRoleControl.GetComponent<PickableRoleControl>() != null) {
                if (!_BaseRoleControl.GetComponent<PickableRoleControl>().canUSE) {
                    return false;
                }
            }
            Action_GetPickable tmpAction = new Action_GetPickable();
            tmpAction.f_SetOwner(RoleID, OwnerID);
            f_DoRunAIState(tmpAction);
            return false; //重複偵測
        }

        //其他類型的腳色，預設切換 AI表上的 RunAI，依情況加 if擴充
        else {
            ChangeAIAction tmpAction = new ChangeAIAction();
            tmpAction.f_GetAI_fromManager(_BaseRoleControl.m_iId, _CharacterAIDT.szRunAI);
            f_DoRunAIState(tmpAction);
            return true; //結束偵測
        }
    }


}
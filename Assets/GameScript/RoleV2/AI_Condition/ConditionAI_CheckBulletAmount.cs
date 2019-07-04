using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 檢查彈藥量
/// </summary>
public class ConditionAI_CheckBulletAmount : AI_ConditionBaseStateV2
{
    private Module_Shoot_TwoHand ModuleShoot2; //開槍模組(FinalIK)

    public ConditionAI_CheckBulletAmount()
        : base(AI_EM.EM_AIState.CheckBulletAmount)
    { }


    public override bool f_ConditionTest()
    {

        ModuleShoot2 = _BaseRoleControl.GetComponent<Module_Shoot_TwoHand>();
        if (ModuleShoot2 != null) {
            if (ModuleShoot2.BulletAmount < ccMath.atoi(_CharacterAIConditionDT.szData1)){
                ChangeAIAction tmpAction = new ChangeAIAction();
                tmpAction.f_GetAI_fromManager(_BaseRoleControl.m_iId, _CharacterAIDT.szRunAI);
                f_DoRunAIState(tmpAction);
                return true;
            }
        }

        return false;
    }


}

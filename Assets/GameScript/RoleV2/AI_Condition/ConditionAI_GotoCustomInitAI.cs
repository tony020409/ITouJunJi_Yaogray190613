using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 前往一個自定義的初始化AI
/// </summary>
public class ConditionAI_GotoCustomInitAI : AI_ConditionBaseStateV2
{
    public ConditionAI_GotoCustomInitAI()
        : base(AI_EM.EM_AIState.GotoCustomInitAI)
    {
    }

    public override bool f_ConditionTest()
    {

        ChangeAIAction tmpAction = new ChangeAIAction();
        tmpAction.f_SetAI(_BaseRoleControl.m_iId, _CharacterAIConditionDT.szData1);
        f_DoRunAIState(tmpAction);
        return true;
    }

}

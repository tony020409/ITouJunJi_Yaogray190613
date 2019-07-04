using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionAI_ChangePoint : AI_ConditionBaseStateV2 {

    private float _CurHp = -99;

    public ConditionAI_ChangePoint ()
        : base(AI_EM.EM_AIState.ChangePoint) {
    }

    public override void f_Enter (object Obj) {
        base.f_Enter(Obj);

        Debug.LogWarning("進入檢測血量");
        _CurHp = -99;
    }



    public override bool f_ConditionTest () {
        IniHP();
        if (_BaseRoleControl.GetComponent<ArcherRoleControl>().CurPatienceCount <= 0) {
            _BaseRoleControl.GetComponent<ArcherRoleControl>().CurPatienceCount = _BaseRoleControl.GetComponent<ArcherRoleControl>().MaxPatienceCount;
            //MessageBox.DEBUG( _BaseRoleControl + "的條件：" + _CurHp + ">" + _BaseRoleControl.f_GetHp() + "達成");
            int rangeRadomNum = 0;

            if (_BaseRoleControl.GetComponent<ArcherRoleControl>().HidePos.Count > 1)
            {
                do
                {
                    rangeRadomNum = Random.Range(0, _BaseRoleControl.GetComponent<ArcherRoleControl>().HidePos.Count);
                } while (_BaseRoleControl.GetComponent<ArcherRoleControl>().CurHidePos == rangeRadomNum);
            } else
            {
                return false;
            }

            RoleChangePointAction tmpAction = new RoleChangePointAction();
            tmpAction.f_ChangePoint(_BaseRoleControl.m_iId, "AI_ChangePoint", rangeRadomNum);
            f_DoRunAIState(tmpAction);
            _CurHp = -99; //條件AI當子條件時，f_Enter()沒執行到，所以這裡先 _CurHp = -99 一次
            return true;
        }
        return false;
    }

    private void IniHP () {
        if (_CurHp == -99) {
            _CurHp = _BaseRoleControl.f_GetHp();
        }
    }




}

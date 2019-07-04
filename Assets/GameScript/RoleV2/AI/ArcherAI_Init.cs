using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// [德軍專案] 士兵的第一個AI：
/// </summary>
public class ArcherAI_Init : AI_RunBaseStateV2
{

    private ArcherRoleControl _ArcherRoleControl;
    private bool _csnStart = true;


    public ArcherAI_Init()
        : base(AI_EM.EM_AIState.ArcherAI_Init)
    {
    }


    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);

        _ArcherRoleControl = _BaseRoleControl.GetComponent<ArcherRoleControl>();

        //如果士兵以外的怪物不小心掉用到這個AI，直接結束AI
        if (_ArcherRoleControl == null) {
            f_RunStateComplete();
        }

        //找出士兵的所有躲藏點中，距離最近的那個
        int HideIndex = 0;
        float HideDistance = 99f;
        _ArcherRoleControl.CurHidePos = HideIndex;
        for (int i = 0; i < _ArcherRoleControl.HidePos.Count; i++) {
            if (Vector3.Distance(transform.position, _ArcherRoleControl.HidePos[i].position) < HideDistance) {
                HideIndex = i;
                HideDistance = Vector3.Distance(transform.position, _ArcherRoleControl.HidePos[i].position);
            }
        }

        //設定士兵最初的躲藏點為距離最近的那個 (ps.士兵有哪些躲藏點是在 ArcherRoleControl.cs 抓取)
        _ArcherRoleControl.CurHidePos = HideIndex;




        //士兵初始化完成
        _ArcherRoleControl.ArcherInit = true;
        f_RunStateComplete();
    }


    public override void f_Execute() {
        base.f_Execute();
    }

    public override void f_Exit() {
        base.f_Exit();
    }


}

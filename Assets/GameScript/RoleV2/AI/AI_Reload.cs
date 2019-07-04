using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 遠攻-換彈
/// 1. 補彈時機由動畫事件去達成
/// 2. 這裡用來變更每隻怪換彈的持續時間，以避免怪物動作太過一致的問題 (同時攻擊、同時換彈)
/// 3. 還有給 ActionController 一個 AI_Reload的狀態去控制動畫
/// </summary>
public class AI_Reload : AI_RunBaseStateV2
{
    public AI_Reload()
        : base(AI_EM.EM_AIState.AI_Reload) { }
    Animator _animator = null;
    ArcherRoleControl _Solider = null;
    Module_Shoot_TwoHand s2 = null;


    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);
        if (_BaseRoleControl.GetComponent<Module_Shoot_TwoHand>() != null){
            s2 = _BaseRoleControl.GetComponent<Module_Shoot_TwoHand>();
            s2.Mv_IKSee(0);
        }

        _Solider = _BaseRoleControl.GetComponent<ArcherRoleControl>();
        PointPart tmp = _Solider.HidePos[_Solider.CurHidePos].GetComponent<PointPart>();
        if (tmp.HideType == EM_Hide.LeftHide){
            _animator.Play("Reload_L");
        }
        else if (tmp.HideType == EM_Hide.RightHide){
            _animator.Play("Reload_R");
        }
        else if (tmp.HideType == EM_Hide.StandHide){
            _animator.Play("Reload");
        }

    }


    public override void f_Execute() {
        base.f_Execute();
    }



    public override void f_Exit(){
        base.f_Exit();
    }



}

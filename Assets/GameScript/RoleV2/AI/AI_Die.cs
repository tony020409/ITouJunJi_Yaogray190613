using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//記得到 AIManager、BaseAIControllV2 註冊
public class AI_Die : AI_RunBaseStateV2
{

    public AI_Die() : 
        base(AI_EM.EM_AIState.Die)
    { }

    public RagdollControl tmpRagdoll = null; //布娃娃工具
    private float tmpTime = 3;

    public override void f_Enter(object Obj)  {
        base.f_Enter(Obj);
        //Debug.LogWarning(_BaseRoleControl.m_iId + " 進入死亡");
        tmpRagdoll = _BaseRoleControl.GetComponent<RagdollControl>();
        if (tmpRagdoll != null) {
            if (tmpRagdoll.Enable) {
                tmpRagdoll.SetRadoll(true);
            }
        }
        if (_CurAction != null) {
            Action_Die tmpAction = (Action_Die)_CurAction;
            tmpTime = tmpAction.m_DieTime;
        }

        _BaseRoleControl.m_bIsComplete = true;
        BattleMain.GetInstance().f_RoleDie2(_BaseRoleControl);
        ccTimeEvent.GetInstance().f_RegEvent(tmpTime, false, null, CallBack_Destory);
    }

    void CallBack_Destory(object Obj) {
        _BaseRoleControl.f_Destory();
    }

    
}

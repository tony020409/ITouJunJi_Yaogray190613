using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ChangePonit_noLookAt : AI_RunBaseStateV2
{

    private BaseRoleControllV2 _ReadyAttackTarget;
    private Vector3 tmpLookAtPos;

    public AI_ChangePonit_noLookAt()
        : base(AI_EM.EM_AIState.AI_ChangePoint) { }



    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);

        if (_CurAction != null) {
            RoleChangePointAction tRoleChangePointAction = (RoleChangePointAction)_CurAction;

            _ReadyAttackTarget = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetViewSize());
            _BaseRoleControl.GetComponent<ArcherRoleControl>().CurHidePos = tRoleChangePointAction.m_Index;

            List<Vector3> path = new List<Vector3>();
            path.Add(transform.position);
            path.Add(_BaseRoleControl.GetComponent<ArcherRoleControl>().HidePos[tRoleChangePointAction.m_Index].position);

            AutoMove(path.ToArray());
        }
    }

    public override void f_Execute() {
        base.f_Execute();

        if (_ReadyAttackTarget != null) {
            tmpLookAtPos = _ReadyAttackTarget.transform.position; //設定面向的位置
            tmpLookAtPos.y = transform.position.y;                //忽略面向的位置的Y軸
            transform.LookAt(tmpLookAtPos);                       //朝向玩家
        }
    }


    public override void f_Exit() {
        base.f_Exit();
        //Debug.LogWarning("離開AI_ChangePoint_NoLookAt");
    }



    private void AutoMove(Vector3[] aArray) {

        float fSingleTime = 3 / _BaseRoleControl.f_GetWalkSpeed() - StaticValue.m_fNetAverage / 1000;
        float m_Speed = aArray.Length * fSingleTime;

        _BaseRoleControl.transform.DOKill();
        TweenParams parms = new TweenParams();
        _BaseRoleControl.transform.DOLocalPath(aArray, m_Speed, PathType.CatmullRom, PathMode.Full3D) // 路徑點數組 / 週期時間 / path type / path mode
                 .SetAs(parms)             //??
                 .SetEase(Ease.Linear)     //速度變化效果 (參考：http://dotween.demigiant.com/documentation.php)
                 .SetOptions(false)        //路徑是否閉合
                 .SetSpeedBased(false)     //基於速度來移動 (false 則表示基於時間(走完路徑要花多久時間))
                 .OnComplete(ReachedEnd);  //如果循環的，每循環完成調用一次。不是循環的則完成執行


        //Hashtable args = new Hashtable();
        //args.Add("path", aArray);
        //args.Add("easeType", iTween.EaseType.linear);
        //args.Add("time", aArray.Length * fSingleTime);
        //args.Add("oncomplete", "ccCallBackMoveComplete");

        ////args.Add("oncompleteparams", "end");
        ////args.Add("oncompletetarget", _BaseRoleControl.gameObject);
        ////args.Add("onupdate", "ccCallBackMoveUpdate");
        ////args.Add("onupdatetarget", _BaseRoleControl.gameObject);
        ////args.Add("onupdateparams", true);

        //iTween tiTween = iTween.MoveTo(_BaseRoleControl.gameObject, args);
        //tiTween.m_ccCallBackMoveUpdate   = ccCallBackMoveUpdate;
        //tiTween.m_ccCallBackMoveComplete = ccCallBackMoveComplete;
    }


    //iTween 用
    protected virtual void ccCallBackMoveUpdate(object Obj) {

    }

    //iTween 用
    protected virtual void ccCallBackMoveComplete(object Obj) {
        f_RunStateComplete();
    }

    //DoTween 用
    protected void ReachedEnd() {
        f_RunStateComplete();
    }



    private void StopWalk() {
        iTween.f_Stop(_BaseRoleControl.gameObject);
        _BaseRoleControl.f_StopWalk();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ccU3DEngine;

public class AI_MoveToHidePos : AI_RunBaseStateV2 {

    private ArcherRoleControl _ArcherRoleControl;
    private bool _csnStart = true;


    public AI_MoveToHidePos ()
        : base(AI_EM.EM_AIState.MoveToHidePos) {
    }

    public override void f_Enter (object Obj) {
        base.f_Enter(Obj);

        _ArcherRoleControl = _BaseRoleControl.GetComponent<ArcherRoleControl>();
        
        if (_CurAction != null) {

            RoleArcherInitAction tRoleChangePointAction = (RoleArcherInitAction)_CurAction;

            int HideIndex = 0;
            float HideDistance = 99f;

            for (int i = 0; i < _ArcherRoleControl.HidePos.Count; i++)
            {
                if (Vector3.Distance(transform.position, _ArcherRoleControl.HidePos[i].position) < HideDistance)
                {
                    HideIndex = i;
                    HideDistance = Vector3.Distance(transform.position, _ArcherRoleControl.HidePos[i].position);
                }
            }

            _ArcherRoleControl.CurHidePos = HideIndex;
            transform.position = _ArcherRoleControl.HidePos[HideIndex].position;     //直接移動位置
            ccTimeEvent.GetInstance().f_RegEvent(0.2f, false, null, ccMoveComplete); //直接執行跳錯，所以延後 0.2秒執行移動結束


            //用 iTween 走過去
            //_BaseRoleControl.transform.LookAt(_ArcherRoleControl.HidePos[HideIndex].position);
            //List<Vector3> path = new List<Vector3>();
            //path.Add(transform.position);
            //path.Add(_ArcherRoleControl.HidePos[HideIndex].position);
            //AutoMove(path.ToArray());
        }
    }

    public override void f_Execute () {
        base.f_Execute();
    }

    public override void f_Exit () {
        base.f_Exit();
    }



    private void ccMoveComplete(object Obj) {
        _ArcherRoleControl.ArcherInit = true;
        f_RunStateComplete();
    }


    /// <summary>
    /// iTween 移動
    /// </summary>
    //private void AutoMove (Vector3[] aArray) {
    //    Hashtable args = new Hashtable();
    //
    //    args.Add("path", aArray);
    //    args.Add("easeType", iTween.EaseType.linear);
    //
    //    float fSingleTime = 0.5f;
    //    args.Add("time", fSingleTime);
    //    args.Add("oncomplete", "ccCallBackMoveComplete");
    //    args.Add("oncompleteparams", "end");
    //    args.Add("oncompletetarget", _BaseRoleControl.gameObject);
    //
    //    //移动中调用，参数和上面类似
    //    args.Add("onupdate", "ccCallBackMoveUpdate");
    //    args.Add("onupdatetarget", _BaseRoleControl.gameObject);
    //    args.Add("onupdateparams", true);
    //    iTween tiTween = iTween.MoveTo(_BaseRoleControl.gameObject, args);
    //    tiTween.m_ccCallBackMoveUpdate = ccCallBackMoveUpdate;
    //    tiTween.m_ccCallBackMoveComplete = ccCallBackMoveComplete;
    //}


    /// <summary>
    /// 移動中執行的項目
    /// </summary>
    //protected virtual void ccCallBackMoveUpdate (object Obj) {
    //
    //}


    /// <summary>
    /// 移動至躲避点完成
    /// </summary>
    //protected virtual void ccCallBackMoveComplete (object Obj) {
    //    _ArcherRoleControl.ArcherInit = true;
    //    f_RunStateComplete();
    //}


    /// <summary>
    /// 停止移動
    /// </summary>
    //private void StopWalk () {
    //    iTween.f_Stop(_BaseRoleControl.gameObject);
    //    _BaseRoleControl.f_StopWalk();
    //}


}





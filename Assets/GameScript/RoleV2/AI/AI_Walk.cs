
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 恐龍移動狀態機
/// </summary>
public class AI_Walk : AI_RunBaseStateV2
{
    private List<TileNode> _aWalkPath = new List<TileNode>();
    TileNode _StayTileNode;
    private int _iLastTileNode = 0;


    public AI_Walk() : base(AI_EM.EM_AIState.Walk)
    {
    }
    
    public override void f_Enter ( object Obj )
    {
        base.f_Enter( Obj );

        if (_CurAction != null)
        {
            RoleWalkAction tRoleWalkAction = (RoleWalkAction)_CurAction;
            List<TileNode> aWalkPath = new List<TileNode>();
            for (int i = 0; i < tRoleWalkAction.m_aPath.Length; i++)
            {
                TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndex(tRoleWalkAction.m_aPath[i]);
                aWalkPath.Add(tTileNode);
            }
            CreateAutoMovePath(aWalkPath);
        }
        MessageBox.DEBUG("Walk Endter " + _BaseRoleControl.m_iId);

    }
    
    protected void CreateAutoMovePath ( List<TileNode> aPathNode )
    {
        int iPathLen = 7;
        //if ( aPathNode.Count < 7 )
        //{
        iPathLen = aPathNode.Count - 1;
        //}
        Vector3[] aArray = new Vector3[ iPathLen ];
        ClearTestTilePos();
        _aWalkPath.Clear();
        for ( int i = 0 ; i < iPathLen ; i++ )
        {
            aArray[ i ] = aPathNode[ i ].transform.position;
            aArray[ i ].y = GloData.glo_fDefaultY;
            //Debug.Log(aPathNode[i]);
            _aWalkPath.Add( aPathNode[ i ] );
        }


        //use Eddy MoveTool
        //_BaseRoleControl.aiAgent.FollowPath( aArray );
        if (_BaseRoleControl.m_ShowTile )
            UpdateTestTilePos();

        _iLastTileNode = 0;
        AutoMove(aArray);
    }

    #region 移动及回调

    private void AutoMove(Vector3[] aArray)
    {
        Hashtable args = new Hashtable();

        args.Add("path", aArray);        
        args.Add("easeType", iTween.EaseType.linear);

        float fSingleTime = 3 / _BaseRoleControl.f_GetWalkSpeed() - StaticValue.m_fNetAverage / 1000;

        MessageBox.DEBUG(_BaseRoleControl.m_iId + " AutoMove " + fSingleTime + " " + StaticValue.m_fNetAverage / 1000);
        
        args.Add("time", aArray.Length * fSingleTime);
        args.Add("oncomplete", "ccCallBackMoveComplete");
        args.Add("oncompleteparams", "end");
        args.Add("oncompletetarget", _BaseRoleControl.gameObject);

        //移动中调用，参数和上面类似
        args.Add("onupdate", "ccCallBackMoveUpdate");
        args.Add("onupdatetarget", _BaseRoleControl.gameObject);
        args.Add("onupdateparams", true);
        iTween tiTween = iTween.MoveTo(_BaseRoleControl.gameObject, args);
        tiTween.m_ccCallBackMoveUpdate = ccCallBackMoveUpdate;
        tiTween.m_ccCallBackMoveComplete = ccCallBackMoveComplete;
    }
            
   
    protected virtual void ccCallBackMoveUpdate(object Obj)
    {
        //if (_iLastTileNode == (int)Obj)
        //{
        //    return;
        //}
        _iLastTileNode = (int)Obj;
        //_BaseRoleControl.m_iWalkIndex = _iLastTileNode;
        if (_aWalkPath.Count > (_iLastTileNode + 1))
        {
            Vector3 Pos = _aWalkPath[_iLastTileNode + 1].transform.position;
            Pos.y = _BaseRoleControl.transform.position.y;

            //_Rotation = Quaternion.LookRotation(Pos - _BaseRoleControl.transform.position);

            //UpdateTestObject(Pos);
            ChangeFace(Pos);
            //_BaseRoleControl.gameObject.transform.LookAt(Pos);
        }
        if (_aWalkPath.Count > 2)
        {
            if (_iLastTileNode >= 2)
            {
                _aWalkPath[_iLastTileNode - 2].f_UnUse(_BaseRoleControl.m_iId);
            }
        }
        //ForceClear();
                
        _BaseRoleControl.f_UpdateWalkInfor();

    }

    protected virtual void ccCallBackMoveComplete(object Obj)
    {
        f_ForceClearPath();

        f_RunStateComplete();
        //if (StaticValue.m_bIsMaster)
        //{
        //    if (!CheckNeedContinueWalk())
        //    {
        //        StopWalk();
        //        f_SetComplete((int)AI_EM.EM_AIState.Idle);
        //    }
        //}
        //else
        //{
        //    f_SetComplete((int)AI_EM.EM_AIState.WaitAction);
        //}
    }

    private void StopWalk()
    {
        //TweenMove.f_Stop(_BaseRoleControl.gameObject);        
        iTween.f_Stop(_BaseRoleControl.gameObject);
        _BaseRoleControl.f_StopWalk();
    }

    private bool CheckNeedContinueWalk()
    {
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetViewSize());
        if (tRoleControl != null)
        {
            List<int> aData = new List<int>();
            aData.Add(_BaseRoleControl.m_iId);
            aData.Add(tRoleControl.m_iId);
            List<TileNode> taWalkPath = BattleMain.GetInstance().m_MapNav.f_GetAroundFreePosForPath(_BaseRoleControl.f_GetPos(), tRoleControl.f_GetPos(), 3, TileNode.TileType.Normal, tRoleControl.m_iId, aData);
            if (taWalkPath != null)
            {
                _aWalkPath = taWalkPath;
                int[] aPath = new int[_aWalkPath.Count];
                for (int i = 0; i < _aWalkPath.Count; i++)
                {
                    aPath[i] = _aWalkPath[i].idx;
                }
                if (aPath.Length > 1)
                {
                    RoleWalkAction tRoleWalkAction = new RoleWalkAction();
                    tRoleWalkAction.f_Walk(_BaseRoleControl.m_iId, _BaseRoleControl.f_GetTeamType(), aPath);
                    _BaseRoleControl.f_AddMyAction(tRoleWalkAction);
                    f_SetComplete((int)AI_EM.EM_AIState.WaitAction);
                    return true;
                }
            }
        }
        return false;
    }

    public void f_ForceClearPath()
    {
        if (_iLastTileNode < 0 || _aWalkPath == null || _aWalkPath.Count == 0)
        {
            return;
        }
        int iStart = _iLastTileNode - 2;
        if (iStart < 0)
        {
            iStart = 0;
        }
        for (int i = iStart; i < _aWalkPath.Count; i++)
        {
            _aWalkPath[i].f_UnUse(_BaseRoleControl.m_iId);
        }
        _iLastTileNode = -99;
        _aWalkPath.Clear();
    }

    #endregion

    void UpdateTestTilePos ( )
    {
        for ( int i = 0 ; i < _aWalkPath.Count ; i++ )
        {
            _aWalkPath[ i ].f_Show();
        }
    }

    void ClearTestTilePos ( )
    {
        for ( int i = 0 ; i < _aWalkPath.Count ; i++ )
        {
            _aWalkPath[ i ].f_UnShow();
        }
    }

    public override void f_Exit()
    {
        base.f_Exit();

        StopWalk();
    }

}

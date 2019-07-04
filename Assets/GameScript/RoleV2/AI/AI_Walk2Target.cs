
using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 恐龍移動狀態機
/// </summary>
public class AI_Walk2Target : AI_RunBaseStateV2
{
    private List<TileNode> _aWalkPath = new List<TileNode>();
    TileNode _StayTileNode;
    private int _iLastTileNode = 0;
    private ccCallback _Callback_WalkComplete;

    public AI_Walk2Target() : base(AI_EM.EM_AIState.Walk2Target)
    {
    }
        
    public void f_TartgetPos(TileNode tTileNode)
    {

        _aWalkPath = BattleMain.GetInstance().m_MapNav.GetPath(_BaseRoleControl.f_GetPos(), tTileNode, TileNode.TileType.Normal, null);
        if (_aWalkPath == null)
        {
            MessageBox.DEBUG("计算寻路失败");
        }
       
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);

        MessageBox.DEBUG("Walk2Target Endter " + _BaseRoleControl.m_iId);
        if (_aWalkPath == null)
        {
            _Callback_WalkComplete(null);
        }
        else
        {           
            //MessageBox.DEBUG( AI_EM.EM_AIState.Move.ToString() );

            //iPos 格子的ID
            //_StayTileNode = _BaseRoleControl.m_MapNav.f_GetNodeForIndex( iPos );
            //DoWalk();
            //if (Obj != null)
            //{
            //    CreateAutoMovePath();
            //}            
        }
    }

    protected void CreateAutoMovePath()
    {
        int iPathLen = 7;
        iPathLen = _aWalkPath.Count - 1;
        Vector3[] aArray = new Vector3[iPathLen];
        ClearTestTilePos();
        //_aWalkPath.Clear();
        for (int i = 0; i < iPathLen; i++)
        {
            aArray[i] = _aWalkPath[i].transform.position;
            aArray[i].y = GloData.glo_fDefaultY;
            //Debug.Log(aPathNode[i]);
            _aWalkPath.Add(_aWalkPath[i]);
        }


        //use Eddy MoveTool
        //_BaseRoleControl.aiAgent.FollowPath( aArray );
        if (_BaseRoleControl.m_ShowTile)
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



    private float fSleepTime = 0;
    List<int> _aPathIgnoreData = new List<int>();
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
        
        //_BaseRoleControl.f_UpdateWalkInfor();

    }

    protected virtual void ccCallBackMoveComplete(object Obj)
    {
        f_ForceClearPath();

        StopWalk();
        if (StaticValue.m_bIsMaster)
        {            
            if (_Callback_WalkComplete != null)
            {
                _Callback_WalkComplete(null);
            }
        }

        f_RunStateComplete();
    }

    private void StopWalk()
    {
        //TweenMove.f_Stop(_BaseRoleControl.gameObject);        
        iTween.f_Stop(_BaseRoleControl.gameObject);
        _BaseRoleControl.f_StopWalk();
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

    void UpdateTestTilePos()
    {
        for (int i = 0; i < _aWalkPath.Count; i++)
        {
            _aWalkPath[i].f_Show();
        }
    }

    void ClearTestTilePos()
    {
        for (int i = 0; i < _aWalkPath.Count; i++)
        {
            _aWalkPath[i].f_UnShow();
        }
    }

    public override void f_Exit()
    {
        base.f_Exit();

        StopWalk();
    }

}


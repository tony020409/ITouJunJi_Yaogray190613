using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Diagnostics;

public class TweenMove : MonoBehaviour, IPlayerMono
{
    private ccCallback _ccCallbackUpdate, _ccCallbackComplete;
    private float _fSpeed;
    private float _fMinDis;
    private Vector3[] _aPath;
    private int _iIndex;
    private bool _bDoing = false;
    private Vector3 Pos;

    private bool _bIsComplete;
    public bool m_bIsComplete
    {
        get
        {
            return _bIsComplete;
        }

        set
        {
            _bIsComplete = value;
        }
    }

    //public Vector3 m_CurTargetPos;
    void Start()
    {
        _bIsComplete = false;
        glo_Main.GetInstance().m_GameSyscManager.f_AddSyscObject(this);
    }

    private Stopwatch StopSocketWatchSW = new Stopwatch();
    public void f_Update(int iDeltaTime)
    {
        if (_bDoing)
        {
            //if (StopSocketWatchSW.IsRunning)
            //{
            //    StopSocketWatchSW.Stop();
            //    MessageBox.DEBUG("TTTT:" + StopSocketWatchSW.ElapsedMilliseconds);
            //    StopSocketWatchSW.Reset();    
            //}
            //StopSocketWatchSW.Start();
            Doing();
        }
    }

    private void Doing()
    {
        Pos = transform.position;
        Pos.y = 0;
        if (Vector3.Distance(Pos, _aPath[_iIndex]) > _fMinDis)
        {
            Pos = Vector3.MoveTowards(Pos, _aPath[_iIndex], ccDeltaTime.deltaTime * _fSpeed);
            //Pos = Vector3.Lerp(Pos, _aPath[_iIndex], Time.deltaTime * _fSpeed);

            Pos.y = transform.position.y;
            transform.position = Pos;
            if (_ccCallbackUpdate != null)
            {
                _ccCallbackUpdate(_iIndex/2);
            }
                //Vector3 lookAtPos = m_EndPos;
                ////lookAtPos.y = transform.position.y;
                //transform.LookAt(lookAtPos);//, Vector3.left);
        }
        else
        {
            _iIndex++;
            if (_iIndex >= _aPath.Length)
            {
                _bDoing = false;
                _bIsComplete = true;
                if (_ccCallbackComplete != null)
                {
                    Destroy(this);
                    _ccCallbackComplete(null);
                }
            }
            //else
            //{
            //    m_CurTargetPos = _aPath[_iIndex];
            //}
        }
    }

    public void f_Stop()
    {
        _bDoing = false;
    }

    public void MoveTo(Vector3[] aArray, float fSpeed, ccCallback ccCallbackUpdate, ccCallback ccCallbackComplete)
    {
        _ccCallbackUpdate = ccCallbackUpdate;
        _ccCallbackComplete = ccCallbackComplete;
        _fSpeed = fSpeed;
        _aPath = aArray;
        _iIndex = 0;
        _bDoing = true;
        _fMinDis = _fSpeed / 2;
        //m_CurTargetPos = _aPath[_iIndex];
    }

    public static void MoveTo(GameObject obj, Vector3[] aArray, float fSpeed, ccCallback ccCallbackUpdate, ccCallback ccCallbackComplete)
    {
        fSpeed = fSpeed * 0.5f;
        TweenMove tTweenMove = obj.GetComponent<TweenMove>();
        if (tTweenMove == null)
        {
            tTweenMove = obj.AddComponent<TweenMove>();
        }
        Vector3[] aPath;
        aPath = new Vector3[aArray.Length * 2];
        Vector3 Pos = obj.transform.position;
        Pos.y = 0;
        int j = 0;
        for (int i = 0; i < aArray.Length; i++)
        {
            Vector3 Center = Pos + (aArray[i] - Pos) * 0.7f;
            aPath[j] = Center;
            aPath[j].y = 0;
            j++;
            aPath[j] = aArray[i];
            aPath[j].y = 0;
            j++;
            Pos = aArray[i];
        }
        tTweenMove.MoveTo(aPath, fSpeed, ccCallbackUpdate, ccCallbackComplete);
    }

    public static void f_Stop(GameObject obj)
    {
        TweenMove tTweenMove = obj.GetComponent<TweenMove>();
        if (tTweenMove != null)
        {
            tTweenMove.f_Stop();
        }
    }
   

}

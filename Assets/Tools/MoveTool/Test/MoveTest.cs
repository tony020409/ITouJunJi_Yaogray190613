using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MoveTest : MonoBehaviour
{

    public Transform target;
    public float speed = 30;

    float time;
    Animator anim;

    private Stopwatch _Stopwatch = new Stopwatch();

    private void Start()
    {
        //time = MoveTools.f_CalcMoveTime(transform, target.position, speed: speed);

		ccTimeEvent.GetInstance().f_RegEvent(3, false, null, FinishEvent);
	}

    private void FinishEvent(object Obj)
    {
        _Stopwatch.Start();
        print("預計需要時間: " + time);
        //MoveTools.f_Moving(transform, target.position, finishSec: time, finishCallback: Reached);
        anim = GetComponent<Animator>();
    }

    // 扺達事件
    void Reached()
    {
        _Stopwatch.Stop();
        MessageBox.DEBUG("------用时 " + _Stopwatch.ElapsedMilliseconds);

        print("到達! 開始攻擊!");
        anim.SetInteger("control", 31);
    }



}
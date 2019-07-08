using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnpathScript : MonoBehaviour
{
    [Header("攻擊頻率")]
    bool isiAttack = false;        //確保攻擊一次用
    public int Attackfrequency=3;  //
    public int NowAttackfrequency; //

    [Header("盤旋圈數")]
    public int Circlingfrequency=1;
    public int NowCirclingfrequency;

    [Header("播放動畫用")]
    public Animator an;

    [Header("當前路徑")]
    public PathScript Nowpath;
    public PathScript Circlingpath;
    public PathScript Attackpath;

    [Header("?????")]
    public int CurrID = 0;
    public float nowSpeed;           //??速度
    public float CirclingSpeed= 30f; //??速度
    public float DiveSpeed= 60f;     //??速度
    public float reachDistance = 5;  //??距離
    public float rotationSpeed = 10; //??速度
    Vector3 last_position;
    Vector3 curr_position;

    public List<PathScript> attackPathScript;
    public List<PathScript> CirclingPathScript;



    // Use this for initialization =====================================================
    void Start()
    {
       InitAIpath();
    }

    public void InitAIpath()
    {
        last_position = transform.position;
        nowSpeed = CirclingSpeed;
        CirclingPathScript= Pathmanagement.INI.CirclingPathScript;
        attackPathScript = Pathmanagement.INI.attackPathScript;
        int ik = Random.Range(0, CirclingPathScript.Count);
        Circlingpath = CirclingPathScript[ik];
        Nowpath = Circlingpath;
        Nowpath.moveOnpathScript = this;
    }

    public Vector3[] f_GetPath(int iPathId)
    {
        PathScript tPathScript = CirclingPathScript[iPathId];
        Vector3[] aPath = new Vector3[Nowpath.path_objs.Count];
        for(int i = 0; i < Nowpath.path_objs.Count; i++)
        {
            aPath[i] = Nowpath.path_objs[i].position;
        }
        return aPath;
    }

    // Update is called once per frame
    void Update()
    {
        #region 按鍵測試用
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    int i = Random.Range(0, attackPathScript.Count);
        //    path = attackPathScript[i];
        //    CurrID = 0;
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    an.SetInteger("control", 0);
        //    nowSpeed = CirclingSpeed;
        //    CurrID++;
        //}
        #endregion
        //routeRotating();
        //if (CurrID >= Nowpath.path_objs.Count)
        //{
        //    if (Nowpath.en == EN_Path.Attack)
        //    {
        //        Nowpath.moveOnpathScript = null;
        //        an.SetInteger("control", 0);
        //        int i = Random.Range(0, CirclingPathScript.Count);
        //        Nowpath = CirclingPathScript[i];
        //        nowSpeed = CirclingSpeed;
        //    }
        //    if (Nowpath.en == EN_Path.CirclingPath)
        //    {
        //        NowCirclingfrequency += 1;
        //        if (NowCirclingfrequency >= Circlingfrequency)
        //        {

        //            print("盤旋次數" + NowCirclingfrequency);
        //            NowCirclingfrequency = 0;
        //            int kk = Random.Range(0, attackPathScript.Count);
        //            Attackpath = attackPathScript[kk];
        //            Attackjudgment();
        //        }
        //    }
        //    CurrID = 0;
        //}
       // f_Update();
    }


    public void f_Update()
    {
        #region 按鍵測試用
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    int i = Random.Range(0, attackPathScript.Count);
        //    path = attackPathScript[i];
        //    CurrID = 0;
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    an.SetInteger("control", 0);
        //    nowSpeed = CirclingSpeed;
        //    CurrID++;
        //}
        #endregion
        routeRotating();
        if (CurrID >= Nowpath.path_objs.Count)
        {
            if (Nowpath.en == EN_Path.Attack)
            {
                Nowpath.moveOnpathScript = null;
                an.SetInteger("control", 0);
                int i = Random.Range(0, CirclingPathScript.Count);
                Nowpath = CirclingPathScript[i];
                nowSpeed = CirclingSpeed;
            }
            if (Nowpath.en == EN_Path.CirclingPath)
            {
                NowCirclingfrequency += 1;
                if (NowCirclingfrequency >= Circlingfrequency)
                {

                  //  print("盤旋次數" + NowCirclingfrequency);
                    NowCirclingfrequency = 0;
                    int kk = Random.Range(0, attackPathScript.Count);
                    Attackpath = attackPathScript[kk];
                    Attackjudgment();
                }
            }
            CurrID = 0;
        }


    }


    public void routeRotating()
    {
        float distance = Vector3.Distance(Nowpath.path_objs[CurrID].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, Nowpath.path_objs[CurrID].position, Time.deltaTime * nowSpeed);
        var rotation = Quaternion.LookRotation(Nowpath.path_objs[CurrID].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        if (distance <= reachDistance)
        {
            if (Nowpath.en == EN_Path.Attack)
            {
                if (isiAttack == false)
                {
                    Attack();
                }
            }
            if (Nowpath.en == EN_Path.CirclingPath)
            {
                CirclingPath();
            }
        }
    }


    public void Attack()
    {
        nowSpeed = DiveSpeed;
        an.SetInteger("control", 1);

        //如果抵達攻擊點
        if (Attackpath.path_objs[CurrID].GetComponent<point>().en == EN_point.Attack){
            nowSpeed = 0;                //停止移動
            an.SetInteger("control", 2); //播放攻擊動畫
            BaseRoleControllV2 _BaseRoleControl = this.GetComponent<BaseRoleControllV2>();                                                          //獲取自己
            BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetAttackSize()); //獲取攻擊範圍內的敵人
            if (tRoleControl != null){                                        //如果攻擊範圍內有敵人
                tRoleControl.f_BeAttack(_BaseRoleControl.f_GetAttackPower(), -99, GameEM.EM_BodyPart.Body); //觸發傷害
                isiAttack = true;
                //Debug.LogAssertion("玩家[" + tRoleControl.m_iId + "] 受到" + _BaseRoleControl.f_GetAttackPower() + "傷害，剩餘血量 = " + tRoleControl.f_GetHp());
            } else {
                Debug.LogWarning(_BaseRoleControl.m_iId +" 找不到敵人！");  //如果沒有敵人就顯示沒敵人
            } 
        }


        if (Attackpath.path_objs[CurrID].GetComponent<point>().en == EN_point.mobile)
        {
            an.SetInteger("control", 1);
            CurrID++;
        }
    }

    public void CirclingPath()
    {
        an.SetInteger("control", 0);
        CurrID++;
    }
    public void AnimatorEvent(string s)
    {
        if (s == "Attack")
        {
            NowAttackfrequency += 1;
           // print("攻擊次數:" + NowAttackfrequency);
            if (NowAttackfrequency >= Attackfrequency)
            {
                NowAttackfrequency = 0;
                an.SetInteger("control", 0);
                nowSpeed = CirclingSpeed;
                CurrID++;
                isiAttack = false;
            }
        }
    }
    public IEnumerator ooo(int i)
    {
        yield return new WaitForSecondsRealtime(i);
        Attackjudgment();
    }
    public void Attackjudgment()
    {
        if (Attackpath.moveOnpathScript == null)
        {
           // print("此攻擊路線無怪物佔用");
            Nowpath = Attackpath;
            Nowpath.moveOnpathScript = this;
            CurrID = 0;
        }
       else if (Attackpath.moveOnpathScript != null)
        {
            Nowpath = Circlingpath;
          //  print("此攻擊路線已有怪物佔用  繼續盤旋");
        }
    }
}

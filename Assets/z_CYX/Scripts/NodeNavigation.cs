using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavigation : MonoBehaviour {

    [HideInInspector]
    public List<Vector3> nodePoints = new List<Vector3>();

    [Rename("導航啟動距離")]
    public float StartTriggerDistance = 0.1f;
    public Color StartTriggerColor;

    [Rename("節點觸發距離")]
    public float TriggerDistance = 0.5f;
    public Color TriggerColor;

    [Rename("編輯節點")]
    public bool inEditor = false;

    [Line()]
    [HelpBox("Tracker (箭頭) 在閒置時間內，\n" + "如果位移一直沒超過閒置距離，\n" + "就顯示導航!", HelpBoxType.Info)]
    [Rename("閒置距離")]
    public float IdleDistance = 0.1f;

    [Rename("閒置時間 (-99表示永久顯示)")]
    public float IdleTime = 5f;

    [Rename("是否是最後一層導航")]
    public bool isLastNode = false;

    /// <summary>
    /// 暫存座標 (閒置動作判斷用)
    /// </summary>
    private Vector2 TempPosition;

    /// <summary>
    /// 當前時間 (閒置時間倒數用)
    /// </summary>
    private float CurTime;


    [Line()]

    [Rename("當前偵測中的觸發點序號")]
    public int TriggerIndex = 0;

    /// <summary>
    /// 暫存節點序號
    /// </summary>
    private int tempTriggerIndex = 0;

    [Rename("當前是否顯示導航")]
    public bool GuideIO;

    [Rename("是否到達目的地")]
    public bool ArrivalDestination;



    [Line()]
    [Rename("玩家(Camera(eye)")]
    public GameObject _Player;

    [Rename("箭頭物件")]
    public GameObject GuideObj;


    [Line()]
    [Rename("使用 LineRender")]
    public bool UseLineRender = false;

    [Rename("LineRenderer")]
    public LineRenderer _dirLine;



    bool InitArrow = false;





    void Start () {

        if (GuideObj == null) {
            ArrivalDestination = true;
            return;
        }

        if (_Player == null) {
            ArrivalDestination = true;
            return;
        }

        // 更新暫存座標
        TempPosition = new Vector2(GuideObj.transform.position.x, GuideObj.transform.position.z);


        //設定 LineRender
        if (_dirLine != null && UseLineRender) {
            // LineRenderer必須是 Use World Space模式，否則位置會顯示不正確
            _dirLine.useWorldSpace = true;

            //設定 LineRenderer的初始位置
            _dirLine.positionCount = nodePoints.Count;
            for (int i = 0; i <= nodePoints.Count - 1; i++) {
                _dirLine.SetPosition(i, nodePoints[i]);
            }
        }
        else {
            UseLineRender = false;
            if (_dirLine != null) {
                _dirLine.enabled = false;
            }
        }


        //閒置時間=-99時表示永久顯示
        if (IdleTime == -99) {
            ShowNode(true);
        }

        //預設關閉導航
        else {
            ShowNode(false);
            if (_dirLine != null) {
                _dirLine.enabled = false;
            }
        }
        
    }



    void Update () {

        if (!StaticValue.m_bIsMaster) {
            return;
        }

        //if (!StaticValue.m_bIsMaster)  {
        //    if (!InitArrow) {
        //        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
        //            if (BattleMain.GetInstance().m_BattleRolePool.f_Get(0) != null) {
        //                GuideObj = BattleMain.GetInstance().m_BattleRolePool.f_Get(0).GetComponent<OtherPlayerControll2>()._GuildObj;
        //                InitArrow = true;
        //            }
        //        }
        //    }
        //    if (GuideObj == null) {
        //        return;
        //    }
        //}


        //如果不在觸發距離內就不執行
        if (Vector2.Distance(IgnoreY(_Player.transform.position), IgnoreY(this.transform.position)) >= StartTriggerDistance) {
            return;
        }


        //抵達目的地後也不執行
        if (ArrivalDestination) {
            if (IdleTime != -99) {
                return;
            }
            else {
                //玩家離開終點
                if (Vector2.Distance(IgnoreY(_Player.transform.position), IgnoreY(nodePoints[nodePoints.Count - 1])) >= TriggerDistance) {
                    ArrivalDestination = false;
                }
            } 
        }

        //偵測玩家現在靠近哪個節點 (不斷更新指引的方向，不能使用LineRender，因為這邊沒對LineRender做處理)
        for (int i=0; i< nodePoints.Count; i++) {

            if (Vector2.Distance(IgnoreY(_Player.transform.position), IgnoreY(nodePoints[i])) <= TriggerDistance) {
                TriggerIndex = i;
                if (TriggerIndex != nodePoints.Count - 1) {
                    tempTriggerIndex = i;
                }
            }

        }

        //抵達終點
        if (TriggerIndex == nodePoints.Count - 1) {
            if (IdleTime != -99) {                //如果不是永久顯示
                ShowNode(false);                  //隱藏導航
                StartCoroutine(ReNode(IdleTime)); //準備重置導航
                return;
            }
            ArrivalDestination = true;           //到達目的地
        }


        if (IdleTime != -99) {
            CheckIdle();
        }


        //玩家沒到達終點前，箭頭指向玩家的下一個節點
        if (TriggerIndex != nodePoints.Count - 1) {
            f_LookAt(nodePoints[TriggerIndex + 1]);
        }         

        //玩家到達終點時，如果暫存點是0，很有可能是終點起點離很近的情形，所以這裡就強制指向節點[1]
        else if(tempTriggerIndex == 0) {
            f_LookAt(nodePoints[1]);
        }

        //玩家到達終點時，如果上一個暫存點不是終點前的那個點，有機率性箭頭方向會壞掉，所以這裡讓箭頭指向暫存點，叫玩家走回去
        else if (tempTriggerIndex != nodePoints.Count - 2) {
            f_LookAt(nodePoints[tempTriggerIndex]);
        }

        //玩家到達終點時，如果是最後一層導航區域，
        else if (isLastNode) {
            f_LookAt(new Vector3(GuideObj.transform.position.x, 0, GuideObj.transform.position.z), 10, false);
            ShowNode(false);      //隱藏導航
            this.enabled = false; //導航結束
        }


    }


    /// <summary>
    /// 取得一個忽略Y軸的座標
    /// </summary>
    /// <param name="tmp"></param>
    public Vector2 IgnoreY(Vector3 tmp) {
        return new Vector2(tmp.x, tmp.z);
    }


    /// <summary>
    /// 箭頭面對目標 (如果發現箭頭旋轉時產生變形，那是因為箭頭上面某個父物件的大小縮放不是 (1,1,1) 造成的)
    /// </summary>
    /// <param name="targetPos"> 要指向的目標 </param>
    /// <param name="tSpeed"   > 指向速度 </param>
    public void f_LookAt (Vector3 targetPos, float tSpeed = 10f, bool is2D = true) {
        if (GuideObj == null) {
            return;
        }
        if (is2D) {
            targetPos.y = GuideObj.transform.position.y;
        }
        GuideObj.transform.rotation = Quaternion.Slerp(GuideObj.transform.rotation, Quaternion.LookRotation(targetPos - GuideObj.transform.position), Time.deltaTime * tSpeed);
    }



    /// <summary>
    /// 檢查玩家是否在待機
    /// </summary>
    public void CheckIdle() {
        if (!ArrivalDestination) {

            //顯示後直到終點都一值顯示
            if (GuideIO == true) {
                return;
            }

            // 當前座標小於等於閒置距離 (沒在動)
            if (Vector2.Distance( IgnoreY(GuideObj.transform.position), TempPosition) <= IdleDistance){

                // 計算站在原地的時間
                if (CurTime <= IdleTime) {
                    CurTime += Time.deltaTime;
                }

                // 如果站太久就顯示導航路線
                else if (CurTime > IdleTime) {
                    CurTime = 0;       //重新計時
                    CheckCurNodePos(); //取得玩家位置，從玩家位子附近開始導航
                    ShowNode(true);    //開啟導航
                }
            }

            //如果玩家在移動，不計算閒置時間，持續更新暫存座標
            else {
                CurTime = 0;
                TempPosition = IgnoreY(GuideObj.transform.position); //更新暫存座標
            }

        }

    }


    /// <summary>
    /// 開關路徑
    /// </summary>
    /// <param name="tmp"> 開/關 </param>
    public void ShowNode(bool tmp) {
        GuideObj.SetActive(tmp);
        if (UseLineRender) {
            _dirLine.enabled = tmp;
        }
        GuideIO = tmp;
    }



    /// <summary>
    /// 取得玩家位置，從玩家位子附近開始導航
    /// </summary>
    public void CheckCurNodePos() {
        // 暫存距離
        float TempDistance = 99999f;

        // 更新最短距離節點序號
        for (int i = 1; i < nodePoints.Count; i++) {
            if (Vector2.Distance( IgnoreY(_Player.transform.position), IgnoreY(nodePoints[i])) < TempDistance) {
                TriggerIndex = i;
                TempDistance = Vector2.Distance( IgnoreY(_Player.transform.position), IgnoreY(nodePoints[i]) );
            }
        }

        //更新 LineRender
        if (UseLineRender) {
            for (int i = 0; i <= nodePoints.Count - 1; i++) {
                _dirLine.SetPosition(i, nodePoints[i]);
            }
            if (TriggerIndex - 1 >= 0) {
                for (int i = 1; i < TriggerIndex; i++) {
                    _dirLine.SetPosition(i, nodePoints[TriggerIndex]);
                }
            }
        }


    }

    /// <summary>
    /// 抵達終點後的重新導航 (避免起點跟終點離很近，或玩家到終點後又亂跑)
    /// </summary>
    /// <param name="tmp"> 到達終點後，等待多久後重新開啟導航 </param>
    /// <returns></returns>
    IEnumerator ReNode(float tmp) {

        //等待一段時間後
        yield return new WaitForSeconds(tmp);

        //如果玩家還在導航範圍
        if (Vector2.Distance(IgnoreY(_Player.transform.position), IgnoreY(this.transform.position)) <= StartTriggerDistance) {

            //且不在終點附近
            if (Vector2.Distance(IgnoreY(_Player.transform.position), IgnoreY(nodePoints[nodePoints.Count - 1])) >= TriggerDistance) {

                //就重置導航
                ArrivalDestination = false; //改判斷還沒到達目的地
                CurTime = 0;                //重新計時
                CheckCurNodePos();          //取得玩家位置，從玩家位子附近開始導航
                ShowNode(true);             //開啟導航
            }

            //如果還在終點附近，就每隔一段時間再檢查一次
            else {
                StartCoroutine(ReNode(tmp));
            }

        }

    }




}

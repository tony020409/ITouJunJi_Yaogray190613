using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PathTool;
using DG.Tweening;
using UnityEngine.Events;


public class GetPath_and_Move_Lite : MonoBehaviour {

    [Header("當前路徑、起始節點")]
    public Path_Stantard Path;
    public int startPoint = 0;      //起始節點
    [HideInInspector]
    public int currentPoint = 0;    //當前節點

    [Header("速度 (從起點到終點的時間)")]
    public float Speed = 5;         //Speed

    /// <summary>
    /// 獲取路徑的所有航點的引用。
    /// <summary>
    [HideInInspector]
    public Vector3[] waypoints;

    [Header("DoTween項目")]
    public bool isClose = false; //路徑是否閉合
    [HideInInspector]
    public Tweener tween;
    [HideInInspector]
    public List<UnityEvent> events = new List<UnityEvent>();
    private Vector3[] wpPos;
    public DG.Tweening.PathType pathType = DG.Tweening.PathType.CatmullRom; // Animation path type, linear or curved.
    public DG.Tweening.PathMode pathMode = DG.Tweening.PathMode.Full3D;     // Whether this object should orient itself to a different Unity axis.
    public DG.Tweening.Ease easeType = DG.Tweening.Ease.Linear;             // Animation easetype on TimeValue type time.
    public LoopType endAction = LoopType.None;                              // 到達終點時做什麼
    public enum LoopType{
        None,
        Loop,
        PingPong,
        Custom
    }


    // Use this for initialization
    void Start () {
        StartMove();
    }
	
    // 將 Start要執行的東西放在 StartMove()，使其可以從其他腳本調用、允許啟動延遲。
    public void StartMove(){

        //檢查路徑是否存在
        if (Path == null){
            Debug.LogWarning(gameObject.name + "沒有選擇路徑!");
            return;
        }

        waypoints = Path.GetPathPoints(false);                         //獲取所有航點的位置
        startPoint = Mathf.Clamp(startPoint, 0, waypoints.Length - 1); //限制起始節點的編號在航點範圍內
        int index = startPoint;                                        //設定起始節點
        //if (reverse){                                                //如果是反向移動
        //    Array.Reverse(waypoints);
        //    index = waypoints.Length - 1 - index;
        //}
        Initialize(index);


        TweenParams parms = new TweenParams();
        tween = transform.DOPath(wpPos, Speed, pathType, pathMode) // 路点数组 / 周期时间 / path type / path mode
                 .SetAs(parms)                 //??
                 .SetOptions(isClose)          //路徑是否閉合
                 .SetLookAt(0.001f)            //數字越小，移動轉向越自然的樣子，1表示不轉向
                 .OnComplete(ReachedEnd);  //如果循環的，每循環完成調用一次。不是循環的則完成執行

        //如果循環的，每循環完成調用一次。不是循環的則完成執行
        //parms.OnStepComplete(ReachedEnd);
    }


    //initialize or update modified waypoint positions
    //fills array with original positions and adds custom height
    //check for message count and reinitialize if necessary
    private void Initialize(int startAt = 0){
        wpPos = new Vector3[waypoints.Length - startAt];
        for (int i = 0; i < wpPos.Length; i++)
            wpPos[i] = waypoints[i + startAt];

        //message count is smaller than waypoint count,
        //add empty message per waypoint
        for (int i = events.Count; i <= Path.GetWaypointCount() - 1; i++)
            events.Add(new UnityEvent());
    }

    /// <summary>
    /// 到達終點後的行為
    /// </summary>
    private void ReachedEnd(){
        switch (endAction){
            //啥都不幹
            case LoopType.None:
                return;

            //走回起點繼續走路徑
            case LoopType.Loop:
                currentPoint = 0;
                StartMove();
                break;

            //原路回頭走回去
            case LoopType.PingPong:

                break;

            //其他自訂
            case LoopType.Custom:
                break;
        }
    }


    /// <summary>
    /// 更改此對象的當前路徑並開始移動。
    /// ex: SetPath(PathTool_Manager.Paths["P2"]);
    /// <summary>
    public void SetPath(Path_Stantard newPath){
        if (tween != null) {
            tween.Kill();
        }
        tween = null;   //重置tween
        Path = newPath; //設定新路徑
        StartMove();    //重新開始移動
    }



}

using DG.Tweening;
using PathTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPath : MonoBehaviour {


    /// <summary>
    /// 要移動的物件
    /// </summary>
    [Rename("要移動的物件")]
    public Transform tmp;

    /// <summary>
    /// 路徑2
    /// </summary>
    [Rename("路徑_1")]
    public Path_Stantard tPath_1;


    /// <summary>
    /// 路徑2
    /// </summary>
    [Rename("路徑_2")]
    public Path_Stantard tPath_2;



    /// <summary>
    /// 路徑航點
    /// </summary>
    private Vector3[] points;

    /// <summary>
    /// 速度
    /// </summary>
    [Rename("速度")]
    public float m_Speed;

    /// <summary>
    /// 原本的位置
    /// </summary>
    private Vector3 m_OriginPos;

    /// <summary>
    /// 原本的朝向
    /// </summary>
    private Quaternion m_OriginRot;



    // Use this for initialization
    void Start () {

        if (tmp != null) {
            m_OriginPos = tmp.position;
            m_OriginRot = tmp.rotation;
        }


    }

	
	// Update is called once per frame
	void Update () {

        //重置
        if (Input.GetKeyDown(KeyCode.R)) {
            tmp.position = m_OriginPos;
            tmp.rotation = m_OriginRot;
        }


        if (Input.GetKeyDown(KeyCode.A)) {
            Debug.Log("按下A");
            if (tmp == null) {
                return;
            }
            if (tPath_1 != null){
                points = tPath_1.GetPathPoints(false);
            } else {
                Debug.Log("找不到路徑1");
                return;
            }
            tmp.DOKill();
            tmp.position = points[0];
            AutoMove();
        }


        if (Input.GetKeyDown(KeyCode.B))  {
            Debug.Log("按下B");
            if (tmp == null){
                return;
            }
            if (tPath_2 != null){
                points = tPath_2.GetPathPoints(false);
            } else {
                Debug.Log("找不到路徑2");
                return;
            }
            tmp.DOKill();
            tmp.position = points[0];
            AutoMove();
        }

    }



    /// <summary>
    /// 執行移動
    /// </summary>
    /// <param name="isClose"> 路徑是否閉合 </param>
    private void AutoMove(bool isClose = false)  {

        //計算路徑距離
        float pathLength = PathTool_Manager.GetPathLength(points);
       

        //計算移動所需時間 = 距離 / 移動速度
        float moveTime = pathLength / m_Speed;

        Debug.Log("移動");
        //新移動
        //TweenParams parms = new TweenParams();
        //tmp.DOLocalPath(points, moveTime, PathType.CatmullRom, PathMode.Full3D) // 路徑點數組 / 週期時間 / path type / path mode
        //         .SetAs(parms)                     //??
        //         .SetEase(Ease.Linear)             //速度變化效果 (參考：http://dotween.demigiant.com/documentation.php)
        //         .SetOptions(isClose)              //路徑是否閉合
        //         .SetSpeedBased(false);             //基於速度來移動 (false 則表示基於時間(走完路徑要花多久時間))
        //         //.SetLookAt(0.0001f);              //(0~1) 數字越小，移動轉向越自然，1表示不轉向 源參數=0.001f



        TweenParams parms = new TweenParams();
        tmp.DOLocalPath(points, m_Speed, PathType.CatmullRom, PathMode.Full3D) // 路徑點數組 / 週期時間 / path type / path mode
                 .SetAs(parms)             //??
                 .SetEase(Ease.Linear)     //速度變化效果 (參考：http://dotween.demigiant.com/documentation.php)
                 .SetOptions(isClose)      //路徑是否閉合
                 .SetSpeedBased(true);     //基於速度來移動 (false 則表示基於時間(走完路徑要花多久時間))
                 

    }

}

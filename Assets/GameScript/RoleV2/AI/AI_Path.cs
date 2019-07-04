using ccU3DEngine;
using DG.Tweening;
using PathTool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AI_Path : AI_RunBaseStateV2
{


    public AI_Path() : base(AI_EM.EM_AIState.Path) {
    }


    private Vector3[] points;    //路徑航點
    private string endAction;    //結束行為
    private Path_Stantard tPath; //路徑


    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);

        //從 Action_Path 接收路徑資料
        if (_CurAction != null) {
            Action_Path tmpAction = (Action_Path) _CurAction;
            f_SetPath(tmpAction.m_PathName, tmpAction.m_EndAction);
        }


        //如果有找到路徑，就直接移動到第一個航點，並開始移動
        if (tPath != null) {
            _BaseRoleControl.transform.position = points[0];
            AutoMove();
        }

        //沒找到路徑的話就結束AI
        else {
            f_RunStateComplete();
        }
    }



    /// <summary>
    /// 設定路徑資訊
    /// </summary>
    /// <param name="iPathId"   > 路徑名稱 </param>
    /// <param name="iEndAction"> 到終點後的行為 </param>
    public void f_SetPath(string iPathId, string iEndAction) {

        //搜尋路徑名單內的路徑 
        for (int i = 0; i < PathTool_Manager.inst.PathList.Length; i++) { 

            //先用路徑名稱去找，找到名稱相符的路徑就設置路徑
            if (PathTool_Manager.inst.PathList[i].name == iPathId) {
                tPath = PathTool_Manager.inst.PathList[i];
                break;
            }

            //如果所有名單內的路徑都找過了，還找不到名稱相符的
            else if (i == PathTool_Manager.inst.PathList.Length - 1 && tPath == null) {
                tPath = PathTool_Manager.inst.PathList[int.Parse(iPathId)];  //就改用編號去找路徑
                if (tPath == null){                                          //如果還是找不到路徑,就回報找不到的訊息
                    MessageBox.ASSERT(" - AI_Path.cs找不到 " + _BaseRoleControl.m_iId + " 要走的路徑，\n"
                        + "看看是不是腳本打錯 或 路徑沒有放到 BattleMain場景裡的路徑名單裡？\n"
                        + "(中文路徑找不到的情況下，可能造成「数据转换时出错,转换数据：xxx」的訊息出現)");
                }
            }

        }

        if (tPath != null)  {
            points = tPath.GetPathPoints(false); //獲取路徑航點
            endAction = iEndAction;              //獲取結束行為
        }

    }



    /// <summary>
    /// 執行移動
    /// </summary>
    /// <param name="isClose"> 路徑是否閉合 </param>
    private void AutoMove(bool isClose = false) {
        float pathLength = PathTool_Manager.GetPathLength(points);       //計算路徑距離
        float moveTime = pathLength / _BaseRoleControl.f_GetWalkSpeed(); //計算移動所需時間 = 距離 / 怪物的移動速度
        TweenParams parms = new TweenParams();                           //???
        _BaseRoleControl.transform.DOLocalPath(points, moveTime, PathType.CatmullRom, PathMode.Full3D) // 路点数组 / 周期时间 / path type / path mode
                 .SetAs(parms)             //??
                 .SetEase(Ease.Linear)     //速度變化效果 (參考：http://dotween.demigiant.com/documentation.php)
                 .SetOptions(isClose)      //路徑是否閉合
                 .SetSpeedBased(true)      //基於速度來移動 (false 則表示基於時間(走完路徑要花多久時間))
                 .SetLookAt(0.001f)        //數字越小，移動轉向越自然的樣子，1表示不轉向
                 .OnComplete(ReachedEnd);  //如果循環的，每循環完成調用一次。不是循環的則完成執行
    }


    
    /// <summary>
    /// 移動結束事件
    /// </summary>
    private void ReachedEnd() {
        //如果設定成 Loop，則回到起點重複跑
        if (endAction == "Loop") {
            _BaseRoleControl.transform.position = points[0]; //直接移動到第一個航點
            AutoMove();                                      //重複路徑
        }

        //如果設定成 Kill，則死亡
        else if (endAction == "Kill") {
            _BaseRoleControl.f_Die();
        }

        //如果設定成 End，則結束AI
        else if (endAction == "End") {
            f_RunStateComplete();
        }

        //其它擴充
        //....
    }

}

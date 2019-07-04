using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;
using PathTool;
using DG.Tweening;

//記得到Action.cs加
[ProtoContract]
public class Action_Path : BaseActionV2
{
    public Action_Path()
        : base(GameEM.EM_RoleAction.Path)
    { }

    [ProtoMember(40001)]
    public string m_Name;

    [ProtoMember(40002)]
    public string m_PathName;

    [ProtoMember(40003)]
    public string m_EndAction;

    [ProtoMember(40004)]
    public float m_Speed;



    //要移動的物件
    Transform tmp;

    //其他參數
    private Vector3[] points;    //路徑航點
    private string endAction;    //結束行為
    private Path_Stantard tPath; //路徑
    private bool isLookAt;       //是否看著路徑點移動


    /// <summary>
    /// 設定路徑
    /// </summary>
    /// <param name="tObjName"  > 要走路徑的角色或物件 </param>
    /// <param name="tPathName" > 路徑的名稱 </param>
    /// <param name="tEndAction"> 走到終點的動作 </param>
    /// <param name="tSpeed"    > 移動速度 </param>
    public void f_Path(string tObjName, string tPathName, float tSpeed = 1, string tEndAction = "End") {
        m_Name      = tObjName;
        m_PathName  = tPathName;
        m_Speed     = tSpeed;
        m_EndAction = tEndAction;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {

        //預設
        tmp = null;
        isLookAt = true;

        //先檢查要移動的物件是不是場景的物件
        GameObject tObj = BattleMain.GetInstance().f_GetGameObj(m_Name);
        if (tObj != null) {
            tmp = tObj.transform;

            //由於場景物件暫時不會看著路徑節點移動(e.g. 電梯只需要位置移動)，
            //看著路徑點反而會造成電梯轉向變得怪怪的，所以這邊直接設定成不看著，
            //之後有需要看著點移動再說，有需要的話.....呵呵
            isLookAt = false;
        }
       
        //如果不是場景的物件就看是不是怪物
        else {
            BaseRoleControllV2 _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(m_Name));
            if (_BaseRoleControl != null) {
                tmp = _BaseRoleControl.transform;
                
                //怪物用怪物自己的移動速度
                m_Speed = _BaseRoleControl.f_GetWalkSpeed();
            }
        }


        //找不到物件就不在執行東西
        if (tmp == null) {
            Debug.LogWarning("找不到物件：" + tmp.name);
            return;
        } 

        //找得到物件就執行路徑移動
        f_SetPath(m_PathName, m_EndAction);
        if (tPath != null) {
            tmp.DOKill();
            tmp.position = points[0];
            AutoMove(isLookAt);
        } else {
            Debug.LogWarning("找不到路徑：" + m_PathName);
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
            else if (i == PathTool_Manager.inst.PathList.Length - 1 && tPath == null)  {
                tPath = PathTool_Manager.inst.PathList[int.Parse(iPathId)]; //就改用編號去找路徑
                if (tPath == null) {                                        //如果還是找不到路徑,就回報找不到的訊息
                    MessageBox.ASSERT(" - Action_Path.cs找不到 " + m_Name + " 要走的路徑，\n"
                        + "看看是不是腳本打錯 或 路徑沒有放到 BattleMain場景裡的路徑名單裡？\n"
                        + "(中文路徑找不到的情況下，可能造成「数据转换时出错,转换数据：xxx」的訊息出現)");
                }
            }
        }


        if (tPath != null) {
            points = tPath.GetPathPoints(false); //獲取路徑航點
            endAction = iEndAction;              //獲取結束行為
        }

    }





    /// <summary>
    /// 路徑是否閉合
    /// </summary>
    /// <param name="isLookAt"> 是否看著路徑節點移動動 </param>
    /// <param name="isClose" > 路徑是否閉合 </param>
    private void AutoMove(bool isLookAt = true, bool isClose = false) {

        //計算路徑距離
        //float pathLength = PathTool_Manager.GetPathLength(points);       

        //計算移動所需時間 = 距離 / 移動速度
        //float moveTime = pathLength / m_Speed;

        if (m_Speed == 0) {
            Debug.LogWarning("速度=0，請檢查表格速度欄位是否忘記填寫，這裡將讓速度強制=1，以讓腳本能順利運作");
            m_Speed = 1;
        }


        //看著路徑點移動
        if (isLookAt) {
            TweenParams parms = new TweenParams();
            tmp.DOLocalPath(points, m_Speed, PathType.CatmullRom, PathMode.Full3D) // 路徑點數組 / 週期時間 / path type / path mode
                     .SetAs(parms)             //??
                     .SetEase(Ease.Linear)     //速度變化效果 (參考：http://dotween.demigiant.com/documentation.php)
                     .SetOptions(isClose)      //路徑是否閉合
                     .SetSpeedBased(false)     //基於速度來移動 (false 則表示基於時間(走完路徑要花多久時間))
                     .SetLookAt(0.001f)        //(0~1) 數字越小，移動轉向越自然
                     .OnComplete(ReachedEnd);  //如果循環的，每循環完成調用一次。不是循環的則完成執行
        }

        //不看著路徑點移動 (像是電梯)
        else {
            TweenParams parms = new TweenParams();
            tmp.DOLocalPath(points, m_Speed, PathType.CatmullRom, PathMode.Full3D) // 路徑點數組 / 週期時間 / path type / path mode
                     .SetAs(parms)             //??
                     .SetEase(Ease.Linear)     //速度變化效果 (參考：http://dotween.demigiant.com/documentation.php)
                     .SetOptions(isClose)      //路徑是否閉合
                     .SetSpeedBased(true)     //基於速度來移動 (false 則表示基於時間(走完路徑要花多久時間))
                     .OnComplete(ReachedEnd);  //如果循環的，每循環完成調用一次。不是循環的則完成執行
        }
    }


    
    /// <summary>
    /// 移動結束事件
    /// </summary>
    private void ReachedEnd() {
        //如果設定成 Loop，則回到起點重複跑
        if (endAction == "Loop") {
            tmp.position = points[0]; //直接移動到第一個航點
            AutoMove();               //重複路徑
        }

        //如果設定成 Kill，則死亡
        else if (endAction == "Kill") {

            //先檢查要移動的物件是不是怪物
            BaseRoleControllV2 _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(m_Name));
            if (_BaseRoleControl != null) {
                _BaseRoleControl.f_Die();
            }

            //如果不是怪物就看是不是場景的物件
            else {
                GameObject tObj = BattleMain.GetInstance().f_GetGameObj(m_Name);
                if (tObj != null)  {
                    tObj.SetActive(false);
                }
            }


        }

        //如果設定成 End，則結束AI
        else if (endAction == "End") {

        }

        //其它擴充
        //....

        tmp = null;

    }



}

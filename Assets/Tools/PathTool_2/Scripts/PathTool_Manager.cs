// 這個文件來自 "Simple Waypoint System" 
// 如果您從Unity資源商店購買了這些資源，則只允許使用這些資源。
// 您不得許可，再許可，出售，轉售，轉讓，分配，分發或以其他方式向任何第三方提供服務或內容。

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //使用 Where運算符

namespace PathTool
{
    public class PathTool_Manager : MonoBehaviour{

        //在 Scene視窗放置新航點的按鍵(預設P)
        public KeyCode placementKey = KeyCode.P;

        //存儲場景中的所有路徑和其名稱。
        //您可以通過調用 PathTool_Manager.Paths [“路徑名稱”]來呼叫它們。
        public static readonly Dictionary<string, Path_Stantard> Paths = new Dictionary<string, Path_Stantard>();

        //或是用 PathList[ "編號" ]來呼叫
        public Path_Stantard[] PathList;

        //讓其他程式獲取用
        public static PathTool_Manager inst;
        private void Awake(){
            inst = this;
        }


        /// <summary>
        /// 如果 GameObject包含路徑組件，則將其添加到路徑字典中。
        /// </summary>
        public static void AddPath(GameObject path){
            //檢查路徑是否已經實例化，然後刪除名稱裡 (Clone)的部分
            string pathName = path.name;
            if (pathName.Contains("(Clone)")) {
                pathName = pathName.Replace("(Clone)", "");
            }

            //嘗試獲取PathManager_Lite組件
            Path_Stantard pathMan = path.GetComponentInChildren<Path_Stantard>();
            if (pathMan == null){
                Debug.LogWarning("你呼叫了 PathTool_Manager的 AddPath()，但物件 " + pathName + " 沒有沒有附加 Path_Stantard");
                return;
            }

            //？？
            CleanUp();

            //檢查我們的字典中是否已經包含相同名稱的路徑
            //如果有撞名的航點存在，我們就在結尾添加一個獨特的編號給他(?)
            if (Paths.ContainsKey(pathName)){
                int i = 1;
                while (Paths.ContainsKey(pathName + "#" + i)){
                    i++;
                }
                pathName += "#" + i;
                Debug.Log("將 " + path.name + " 改名為 " + pathName + " because a path with the same name was found.");
            }

            //重命名路徑並將其添加到字典中
            path.name = pathName;
            Paths.Add(pathName, pathMan);
        }



        // 清除和銷毀 Paths字典中的 路徑(GameObjects)。=======================================================================
        public static void CleanUp(){
            string[] keys = Paths.Where(p => p.Value == null).Select(p => p.Key).ToArray(); //這裡的 p表示 Paths中的每一个元素，右邊的 p.Value == null是條件
            for (int i = 0; i < keys.Length; i++)
                Paths.Remove(keys[i]);
        }



        //靜態Dictionaries(字典) 在場景之間保存它們的數值，
        //我們不希望每當物件被破壞時(例如，變換場景時) 路徑的字典(Dictionary)就會被清除掉。===================================
        void OnDestroy(){
            Paths.Clear();
        }




        /// <summary>
        /// 計算總路徑長度。
        /// <summary>
        /// ==================================================================================================================
        public static float GetPathLength(Vector3[] waypoints){
            float dist = 0f;
            for (int i = 0; i < waypoints.Length - 1; i++)
                dist += Vector3.Distance(waypoints[i], waypoints[i + 1]);
            return dist;
        }




        /// <summary>
        /// 在航點之間繪製筆直的 Gizmo線
        /// <summary>
        /// ==================================================================================================================
        public static void DrawStraight(Vector3[] waypoints){
            for (int i = 0; i < waypoints.Length - 1; i++)
                Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
        }

        /// <summary>
        /// 在航點之間繪製彎曲的 Gizmo線 (這是從 HOTween修改的)
        /// <summary>
        /// ==================================================================================================================
        //http://code.google.com/p/hotween/source/browse/trunk/Holoville/HOTween/Core/Path.cs
        public static void DrawCurved(Vector3[] pathPoints){
            pathPoints = GetCurved(pathPoints);
            Vector3 prevPt = pathPoints[0];
            Vector3 currPt;
            for (int i = 1; i < pathPoints.Length; ++i){
                currPt = pathPoints[i];
                Gizmos.DrawLine(currPt, prevPt);
                prevPt = currPt;
            }
        }

        // ====================================================================================================================
        public static Vector3[] GetCurved(Vector3[] waypoints){
            //曲線路徑的輔助數組，includes control points for waypoint array
            Vector3[] gizmoPoints = new Vector3[waypoints.Length + 2];
            waypoints.CopyTo(gizmoPoints, 1);
            gizmoPoints[0] = waypoints[1];
            gizmoPoints[gizmoPoints.Length - 1] = gizmoPoints[gizmoPoints.Length - 2];

            Vector3[] drawPs;
            Vector3 currPt;

            //存儲繪圖點
            int subdivisions = gizmoPoints.Length * 10;
            drawPs = new Vector3[subdivisions + 1];
            for (int i = 0; i <= subdivisions; ++i){
                float pm = i / (float)subdivisions;
                currPt = GetPoint(gizmoPoints, pm);
                drawPs[i] = currPt;
            }

            return drawPs;
        }

        /// <summary>
        /// Gets the point on the curve at a given percentage (0-1). (這是從 HOTween修改的)
        /// <summary>
        // http://code.google.com/p/hotween/source/browse/trunk/Holoville/HOTween/Core/Path.cs
        // ====================================================================================================================
        public static Vector3 GetPoint(Vector3[] gizmoPoints, float t){
            int numSections = gizmoPoints.Length - 3;
            int tSec = (int)Mathf.Floor(t * numSections);
            int currPt = numSections - 1;
            if (currPt > tSec){
                currPt = tSec;
            }
            float u = t * numSections - currPt;
            Vector3 a = gizmoPoints[currPt];
            Vector3 b = gizmoPoints[currPt + 1];
            Vector3 c = gizmoPoints[currPt + 2];
            Vector3 d = gizmoPoints[currPt + 3];

            return .5f * (
                    (-a + 3f * b - 3f * c + d) * (u * u * u)
                    + (2f * a - 5f * b + 4f * c - d) * (u * u)
                    + (-a + c) * u
                    + 2f * b
                );
        }

        /// <summary>
        /// Smoothes a list of Vector3's based on the number of interpolations. Credits to "Codetastic".
        /// <summary>
        // http://answers.unity3d.com/questions/392606/line-drawing-how-can-i-interpolate-between-points.html
        // ====================================================================================================================
        public static List<Vector3> SmoothCurve(List<Vector3> pathToCurve, int interpolations){
            List<Vector3> tempPoints;
            List<Vector3> curvedPoints;
            int pointsLength = 0;
            int curvedLength = 0;

            if (interpolations < 1)
                interpolations = 1;

            pointsLength = pathToCurve.Count;
            curvedLength = (pointsLength * Mathf.RoundToInt(interpolations)) - 1;
            curvedPoints = new List<Vector3>(curvedLength);

            float t = 0.0f;
            for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++){
                t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);
                tempPoints = new List<Vector3>(pathToCurve);
                for (int j = pointsLength - 1; j > 0; j--){
                    for (int i = 0; i < j; i++){
                        tempPoints[i] = (1 - t) * tempPoints[i] + t * tempPoints[i + 1];
                    }
                }
                curvedPoints.Add(tempPoints[0]);
            }
            return curvedPoints;
        }

    }

}

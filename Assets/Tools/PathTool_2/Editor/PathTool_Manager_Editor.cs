/*  This file is part of the "Simple Waypoint System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using PathTool;


[CustomEditor(typeof(PathTool_Manager))]
public class PathTool_Manager_Editor : Editor
{

    private PathTool_Manager script;     //PathTool_Manager自己
    private string pathName = "";        //新的路徑名稱
    private bool mode2D = false;         //啟用2D模式放置（自動檢測）
    private static bool placing = false; //如果我們在編輯器中放置新的路標

    private static GameObject path;                                  //新路徑物件
    private static Path_Stantard pathMan;                            //Path Manager reference for editing waypoints
    private static List<GameObject> wpList = new List<GameObject>(); //編輯器在創建路徑航點時的暫存列表

    //路徑模式
    private enum PathType{
        standard,
        bezier
    }
    private PathType pathType = PathType.standard;



    // ====================================================================================================================
    public void OnSceneGUI()
    {
        //啟用創建模式後，在按鍵上放置新的路標
        if (Event.current.type != EventType.KeyDown || !placing) return;

        //scene view camera placement
        //if (Event.current.keyCode == KeyCode.C){
        //    Event.current.Use();
        //    Vector3 camPos = GetSceneView().camera.transform.position;
        //
        //    //place a waypoint at the camera
        //    if (pathMan is Path_Bezier)
        //        PlaceBezierPoint(camPos);
        //    else
        //        PlaceWaypoint(camPos);
        //
        //}

        if (Event.current.keyCode == KeyCode.P)
        {
            //投放依據滑鼠位置的射線
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            //2d 模式下
            if (mode2D){
                Event.current.Use();
                //將屏幕轉換為2d位置
                Vector3 pos2D = worldRay.origin;
                pos2D.z = 0;

                //place a waypoint at clicked point
                if (pathMan is Path_Bezier)
                    PlaceBezierPoint(pos2D);
                else
                    PlaceWaypoint(pos2D);
            }

            //3d 模式下
            else{

                //有偵測到碰撞體的話，點放置到碰撞上
                if (Physics.Raycast(worldRay, out hit)){
                    Event.current.Use();
                    if (pathMan is Path_Bezier)
                        PlaceBezierPoint(hit.point); //放置點
                    else
                        PlaceWaypoint(hit.point); //放置點
                }

                //沒偵測到碰撞體的話，點的位置以網路的神奇公式去計算點的位置
                else{
                    //下面8行的位置在某些角度會跑掉故註解
                    //float t1 = -worldRay.origin.x / worldRay.direction.x;
                    //float t2 = -worldRay.origin.y / worldRay.direction.y;
                    //float t3 = -worldRay.origin.z / worldRay.direction.z;
                    //Vector3 mouseWorldPos = worldRay.origin + t1 * t2 * t3 * worldRay.direction / 2000;
                    //float k1 = ( (int)(mouseWorldPos.x * 100) ) / 100; //小數點只留二位數
                    //float k2 = ( (int)(mouseWorldPos.y * 100) ) / 100; //小數點只留二位數
                    //float k3 = ( (int)(mouseWorldPos.z * 100) ) / 100; //小數點只留二位數
                    //mouseWorldPos = new Vector3(k1,k2,k3);
                    Vector3 mouseWorldPos = worldRay.origin;
                    if (pathMan is Path_Bezier)
                        PlaceBezierPoint(mouseWorldPos); //放置點
                    else
                        PlaceWaypoint(mouseWorldPos);    //放置點
                }
            }
        }
    }


    // ====================================================================================================================
    public override void OnInspectorGUI(){

        //show default variables of manager
        //DrawDefaultInspector();

        //get manager reference
        script = (PathTool_Manager) target;
        EditorGUILayout.Space();
        script.placementKey = (KeyCode) EditorGUILayout.EnumPopup("路徑點設置鍵", script.placementKey);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //get sceneview to auto-detect 2D mode
        SceneView view = GetSceneView();
        mode2D = view.in2DMode;

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        //draw path text label
        GUILayout.Label("輸入路徑名稱: ", GUILayout.Height(15));
        //display text field for creating a path with that name
        pathName = EditorGUILayout.TextField(pathName, GUILayout.Height(15));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        //draw path type selection enum
        GUILayout.Label("Select Path Type: ", GUILayout.Height(15));
        pathType = (PathType)EditorGUILayout.EnumPopup(pathType);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //顯示Unity現在是2D還3D模式 (因為用不到這個資訊故解掉)
        //if (mode2D)
        //    GUILayout.Label("2D Mode Detected.", GUILayout.Height(15));
        //else
        //    GUILayout.Label("3D Mode Detected.", GUILayout.Height(15));
        //EditorGUILayout.Space();

        //開始按鈕及其相關動作
        EditorGUILayout.Space();
        GUILayout.Label("輸入好路徑名稱後，按下開始按紐即可創造路徑.", GUILayout.Height(15));
        if (!placing && GUILayout.Button("開始設置路徑", GUILayout.Height(40)))
        {
            if (pathName == ""){
                EditorUtility.DisplayDialog("No Path Name", "請先輸入路徑名稱 再開始設置路徑！", "好好好，我知道了");
                return;
            }

            if (script.transform.Find(pathName) != null){
                if (EditorUtility.DisplayDialog("Path Exists Already","啊哦, 這個路徑名稱有人用過了,\n\n你要編輯它嗎?", "編輯它", "這個路徑名稱我不要了")){
                    Selection.activeTransform = script.transform.Find(pathName);
                }
                return;
            }

            //create a new container transform which will hold all new waypoints
            path = new GameObject(pathName);

            //reset position and parent container gameobject to this manager gameobject
            path.transform.position = script.gameObject.transform.position;
            path.transform.parent = script.gameObject.transform;
            StartPath();

            //we passed all prior checks, toggle waypoint placement
            placing = true;
            //focus sceneview for placement
            view.Focus();
        }


        GUI.backgroundColor = Color.yellow;
        //finish path button
        if (placing && GUILayout.Button("完成編輯，按此創造路徑", GUILayout.Height(40))){
            FinishPath();
        }

        GUI.backgroundColor = Color.white;
        EditorGUILayout.Space();
        GUILayout.TextArea("使用方式:  \n "
                         + "1. 先輸入路徑的名稱, \n"
                         + "2. 按下'Start Path' 開啟路徑編輯模式. \n"
                         + "3. 把滑鼠移動到Scene視窗中, \n"
                         + "4. 在要設置路徑點的地方依序按下'P'鍵即可設置. \n"
                         + "5. 路徑設置完成後，按 'Finish Editing' 即可完成路徑.");


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("路徑清單", GUILayout.Height(15));
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PathList"),true); //找尋叫PathList的數組
        serializedObject.ApplyModifiedProperties();                                    //允許修改數組
    }



    //when losing editor focus (離開 Editor後) ===================================================================================
    void OnDisable(){
        FinishPath();
    }



    //根據選擇的路徑模式，給路徑掛上不同腳本 =====================================================================================
    void StartPath(){
        switch (pathType){
            case PathType.standard:
                pathMan = path.AddComponent<Path_Stantard>();
                pathMan.waypoints = new Transform[0];
                break;
            case PathType.bezier:
                pathMan = path.AddComponent<Path_Bezier>();
                Path_Bezier thisPath = pathMan as Path_Bezier;
                thisPath.showHandles = true;
                thisPath.bPoints = new List<BezierPoint>();
                break;
        }
    }


    public static void ContinuePath(Path_Stantard p)
    {
        path = p.gameObject;
        pathMan = p;
        placing = true;

        wpList.Clear();
        if (p is Path_Bezier)
        {
            for (int i = 0; i < (p as Path_Bezier).bPoints.Count; i++)
                wpList.Add((p as Path_Bezier).bPoints[i].wp.gameObject);
        }
        else
        {
            for (int i = 0; i < p.waypoints.Length; i++)
                wpList.Add(p.waypoints[i].gameObject);
        }

        GetSceneView().Focus();
    }


    //path manager placement
    void PlaceWaypoint(Vector3 placePos)
    {
        //產生路徑航點
        GameObject wayp = new GameObject("Waypoint");

        //每增加一個新的航點，我們的航點陣列數應該要增加1
        //but arrays gets erased on resize, so we use a classical rule of three //但是在調整大小時數組會被擦除，所以我們使用三經典規則(?)(這句翻譯看不懂)
        Transform[] wpCache = new Transform[pathMan.waypoints.Length];
        System.Array.Copy(pathMan.waypoints, wpCache, pathMan.waypoints.Length);

        pathMan.waypoints = new Transform[pathMan.waypoints.Length + 1];
        System.Array.Copy(wpCache, pathMan.waypoints, wpCache.Length);
        pathMan.waypoints[pathMan.waypoints.Length - 1] = wayp.transform;


        //這是在放置第一個航點時執行的:
        //我們將路徑容器轉換為第一個航點位置，
        //so the transform (and grab/rotate/scale handles) aren't out of sight //所以變換（和抓取/旋轉/縮放手柄）並不在視線之外(?)(這句翻譯看不懂)
        if (wpList.Count == 0){
            //pathMan.transform.position = placePos;
            pathMan.transform.position = new Vector3(0, 0, 0); //改放在(0,0,0)
        }

        //當前航點的位置 = 在 Scene視窗中點擊的位置
        if (mode2D) placePos.z = 0f;
        wayp.transform.position = placePos;
        wayp.transform.rotation = Quaternion.Euler(-90, 0, 0);

        wayp.transform.parent = pathMan.transform;    //parent it to the defined path 
        wpList.Add(wayp);                             //add waypoint to temporary list
        wayp.name = "Waypoint " + (wpList.Count - 1); //rename waypoint to match the list count
    }


    //bezier path placement
    void PlaceBezierPoint(Vector3 placePos)
    {
        //create new bezier point property class
        BezierPoint newPoint = new BezierPoint();

        //instantiate waypoint gameobject
        Transform wayp = new GameObject("Waypoint").transform;
        //assign waypoint to the class
        newPoint.wp = wayp;

        //same as above
        if (wpList.Count == 0)
            pathMan.transform.position = placePos;

        //position current waypoint at clicked position in scene view
        if (mode2D) placePos.z = 0f;
        wayp.position = placePos;
        wayp.transform.rotation = Quaternion.Euler(-90, 0, 0);
        //parent it to the defined path
        wayp.parent = pathMan.transform;

        Path_Bezier thisPath = pathMan as Path_Bezier;
        //create new array with bezier point handle positions
        Transform left = new GameObject("Left").transform;
        Transform right = new GameObject("Right").transform;
        left.parent = right.parent = wayp;

        //initialize positions and last waypoint
        Vector3 handleOffset = new Vector3(2, 0, 0);
        Vector3 targetDir = Vector3.zero;
        int lastIndex = wpList.Count - 1;

        //position handles to the left/right of the waypoint respectively
        left.position = wayp.position + wayp.rotation * handleOffset;
        right.position = wayp.position + wayp.rotation * -handleOffset;
        newPoint.cp = new[] { left, right };

        //position first handle in direction of the second waypoint
        if (wpList.Count == 1)
        {
            targetDir = (wayp.position - wpList[0].transform.position).normalized;
            thisPath.bPoints[0].cp[1].localPosition = targetDir * 2;
        }
        //always position last handle to look at the previous waypoint 
        else if (wpList.Count >= 1)
        {
            targetDir = (wpList[lastIndex].transform.position - wayp.position);
            wayp.transform.rotation = Quaternion.LookRotation(targetDir) * Quaternion.Euler(0, -90, 0);
        }

        //position handle direction to the center of both last and next waypoints
        //takes into account 2D mode
        if (wpList.Count >= 2)
        {
            //get last point and center direction
            BezierPoint lastPoint = thisPath.bPoints[lastIndex];
            targetDir = (wayp.position - wpList[lastIndex].transform.position) +
                                (wpList[lastIndex - 1].transform.position - wpList[lastIndex].transform.position);

            //rotate to the center 2D/3D
            Quaternion lookRot = Quaternion.LookRotation(targetDir);
            if (mode2D)
            {
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg + 90;
                lookRot = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            lastPoint.wp.rotation = lookRot;

            //cache handle and get previous of last waypoint
            Vector3 leftPos = lastPoint.cp[0].position;
            Vector3 preLastPos = wpList[lastIndex - 1].transform.position;

            //calculate whether right or left handle distance is greater to last waypoint
            //left handle should point to the last waypoint, so reposition if necessary
            if (Vector3.Distance(leftPos, preLastPos) > Vector3.Distance(lastPoint.cp[1].position, preLastPos))
            {
                lastPoint.cp[0].position = lastPoint.cp[1].position;
                lastPoint.cp[1].position = leftPos;
            }
        }

        thisPath.bPoints.Add(newPoint);                  //add waypoint to the list of waypoints
        thisPath.segmentDetail.Add(thisPath.pathDetail); //
        wpList.Add(wayp.gameObject);                     //add waypoint to temporary list
        wayp.name = "Waypoint " + (wpList.Count - 1);    //重新命名 waypoint 的名稱，好讓名稱與 list 的數量一致
        thisPath.CalculatePath();                        //recalculate bezier path
    }


    //完成路徑 =================================================================================================
    void FinishPath()
    {
        if (!placing) return;

        if (wpList.Count < 2)
        {
            //如果我們已經創建了一條路徑，請再次銷毀它
            if (path) DestroyImmediate(path);
            Debug.LogWarning("取消創建路徑，因為路徑航點不足(沒有航點或只有一個)。");
        }


        placing = false;                   //toggle placement off
        wpList.Clear();                    //清空航點的暫存列表，我們這邊只需要用來獲得路點數量
        pathName = "";                     //reset path name input field
        Selection.activeGameObject = path; //make the new path the active selection
    }


    /// <summary>
    /// Gets the active SceneView or creates one. (獲取活動的SceneView或創建一個。)
    /// </summary>
    public static SceneView GetSceneView()
    {
        SceneView view = SceneView.lastActiveSceneView;
        if (view == null)
            view = EditorWindow.GetWindow<SceneView>();
        return view;
    }
}

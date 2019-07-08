using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(NodeNavigation))]
[CanEditMultipleObjects]
public class NodeNavigationEditor : Editor {

    NodeNavigation NN;
    ReorderableList RL;


    //參考資料：https://forum.unity.com/threads/onscenegui-stop-been-called-after-a-generic-script-is-updated.492130/
    //由於 OnSceneGUI 的部分常常無法正常顯示，經查詢後看到上文，
    //讓 Editor 在每次變更時重新刷新
    public void OnEnable() {
        NN = (NodeNavigation)target;
    }

    public void OnDisable() {
        NN = null;
    }


    public override void OnInspectorGUI () {
        base.OnInspectorGUI();

        serializedObject.Update();
        
        if (RL == null) {
            RL = new ReorderableList(NN.nodePoints, typeof(Vector3), true, true, true, true);
        }
        
        RL.drawHeaderCallback = (Rect rect) => {
        
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.x = 35f;
            // 欄位寬度
            rect.width = 100f;
            //// 欄位間距
            //var spacing = 5f;
        
            EditorGUI.LabelField(rect, "路徑節點");
        };


        //列表顯示頭表
        RL.drawElementCallback  = (Rect rect, int index, bool isActive, bool isFocused) => {
        
            rect.height -= 2f;  //欄位高度
            rect.width = 50f;   //欄位寬度
            EditorGUI.LabelField(rect, "Node" + index);
            rect.x = 100f;
            rect.width = 200f; //欄位寬度
            NN.nodePoints[index] = EditorGUI.Vector3Field(rect, GUIContent.none, NN.nodePoints[index]);
        };


        //列表添加時回調：新產生的點的位置會在上一個點的附近
        RL.onAddCallback = (ReorderableList list) => {

            //產生一個新元素
            ReorderableList.defaultBehaviours.DoAddButton(list);

            //第一顆的位子 (上面已經產生一顆，所以沒有0顆的情況)
            if (NN.nodePoints.Count == 1){
                NN.nodePoints[0] = new Vector3(NN.transform.position.x + 1, NN.transform.position.y, NN.transform.position.z);
            }

            //第二顆(含)以後的位子
            else if (NN.nodePoints.Count >= 2) {
                int tmpIndex = NN.nodePoints.Count - 1;
                NN.nodePoints[tmpIndex] = new Vector3(NN.nodePoints[tmpIndex - 1].x + 1, NN.nodePoints[tmpIndex - 1].y, NN.nodePoints[tmpIndex - 1].z + 1);
            }

        };
  
        
        RL.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        
        SceneView.RepaintAll();

        //在編輯模式中，紀錄變更
        if (!Application.isPlaying) {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }


    }

    private void OnSceneGUI () {
        if (NN != null) {

            //顯示啟動範圍
            Handles.color = NN.StartTriggerColor;
            Handles.DrawSolidDisc(NN.transform.position, Vector3.up, NN.StartTriggerDistance);


            for (int i = 0; i < NN.nodePoints.Count; i++) {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                Handles.Label(NN.nodePoints[i], "Node" + i, style);

                //
                if (NN.inEditor) {
                    NN.nodePoints[i] = Handles.PositionHandle(NN.nodePoints[i], Quaternion.identity);
                }
               

                //顯示觸發範圍
                Handles.color = NN.TriggerColor;
                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), NN.nodePoints[i], Quaternion.identity, NN.TriggerDistance*2, EventType.Repaint);
                //Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), NN.transform.position, Quaternion.identity, NN.StartTriggerDistance, EventType.Repaint);
            }

            //將每個點用線連起來
            Handles.color = Color.white;
            Handles.DrawPolyLine(NN.nodePoints.ToArray());
            //Handles.DrawBezier(NN.nodePoints[0], NN.nodePoints[1], Vector3.zero, Vector3.zero, Color.red, null, 1f);
        }
    }



}

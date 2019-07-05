using UnityEngine;
using UnityEditor;




namespace UnityFileDebug
{

    [CustomEditor(typeof(UnityFileDebug))]
    public class UnityFileDebugEditor : Editor {

        UnityFileDebug instance;

        //是否輸出 Log文本
        SerializedProperty useLogOutput;

        //檔案名稱
        SerializedProperty fileName;
        GUIContent fileNameContent;

        //輸出類型
        SerializedProperty fileType;

        //輸出路徑類型
        SerializedProperty showRelative;
        SerializedProperty absolutePath;
        SerializedProperty relativeFolder;
        SerializedProperty relativePathType;

        //輸出路徑
        SerializedProperty filePath;
        SerializedProperty filePathFull;

        //通用參數名稱用
        GUIContent tText;


        string copyPath;

        void OnEnable()  {


            instance = (UnityFileDebug)target;
            fileNameContent = new GUIContent {
                text    = "輸出的檔案 名稱",
                tooltip = "您希望的日誌名稱 (不帶.txt之類的擴展名)"
            };
            tText = new GUIContent();

            // Update references to serialized objects
            useLogOutput = serializedObject.FindProperty("useLogOutput");

            fileName = serializedObject.FindProperty("fileName");
            fileType = serializedObject.FindProperty("fileType");

            showRelative = serializedObject.FindProperty("useRelativePath"); 
            absolutePath = serializedObject.FindProperty("absolutePath");
            relativeFolder   = serializedObject.FindProperty("relativeFolder");
            relativePathType = serializedObject.FindProperty("relativePathType");

            filePath         = serializedObject.FindProperty("filePath");
            filePathFull     = serializedObject.FindProperty("filePathFull");
        }


        public override void OnInspectorGUI() {

            EditorGUILayout.Separator();

            serializedObject.Update();
            instance.UpdateFilePath();

            // 是否啟用保留 Log的功能 (由外部檔案決定)
            tText.text = "是否啟用文本輸出的功能 (由外部檔案決定)";
            EditorGUILayout.PropertyField(useLogOutput, tText);


            // 檔案名稱
            EditorGUILayout.PropertyField(fileName, fileNameContent);

            // 輸出類型
            tText.text = "輸出的檔案 類型";
            EditorGUILayout.PropertyField(fileType, tText);


            // 輸出路徑
            tText.text = "啟用 [相對路徑]";
            EditorGUILayout.PropertyField(showRelative, tText);


            //輸出路徑 - 使用絕對路徑
            if ( !showRelative.boolValue) {
                tText.text = "[絕對路徑] 位置";
                EditorGUILayout.PropertyField(absolutePath, tText);
            }

            //輸出路徑 - 使用相對路徑
            else {
                tText.text = "[相對路徑] 類型";
                EditorGUILayout.PropertyField(relativePathType, tText);
                tText.text = "[相對路徑] 資料夾名稱";
                EditorGUILayout.PropertyField(relativeFolder, tText);
                EditorGUILayout.LabelField("路徑位置：\t" + instance.GetRelativePath(instance.relativePathType));
            }


            // 打開輸出資料夾、copy html to output path
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("開啟輸出資料夾")) {
                OpenInFileBrowser.OpenFilePath(filePath.stringValue);
            }

            if (GUILayout.Button("Copy HTML to Output Path")) {
                copyPath = filePath.stringValue.Replace('\\', '/');
                if (!copyPath.EndsWith("/")) {
                    copyPath += "/";
                }
                copyPath += "UnityFileDebugViewer.html";
                if (System.IO.Directory.Exists("Assets/UnityFileDebug/HtmlViewer/UnityFileDebugViewer.html")) {
                    FileUtil.ReplaceFile("Assets/ccYBO/3rdLib/LogOutputTool/UnityFileDebugViewer.html", copyPath);
                } else {
                    Debug.LogError("找不到網址路徑，你是不是有修改插件資料夾的名稱或位置呢？ \n" + 
                                   "有的話請到 UnityFileDebugEditor.cs修改 html的預設位置。");
                }
                
            }
            EditorGUILayout.EndHorizontal();




            // If running, show full output path and count
            if (Application.isPlaying) {
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("複製輸出文件的路徑(含檔名)")) {
                    EditorGUIUtility.systemCopyBuffer = filePathFull.stringValue;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(filePathFull.stringValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("讀取到的 Log 數量 = " + instance.count);
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            // 告訴使用者關於這個插件的事
            EditorGUILayout.HelpBox(" Unity File Debug \n 由 Sacred Seed Studio製作，\n 並且使用 MIT Licensed。\n 請隨意使用，修改，貢獻，報告錯誤和建議功能", MessageType.Info, true);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Github 網頁")) {
                Application.OpenURL("https://github.com/Sacred-Seed-Studio/Unity-File-Debug");
            }
            if (GUILayout.Button("使用說明")) {
                Application.OpenURL("https://github.com/Sacred-Seed-Studio/Unity-File-Debug/blob/master/README.md");
            }
            if (GUILayout.Button("Bugs 或 功能建議")) {
                Application.OpenURL("https://github.com/Sacred-Seed-Studio/Unity-File-Debug/issues");
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
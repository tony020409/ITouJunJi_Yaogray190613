using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class EditorTest : MonoBehaviour {

  [MenuItem("Assets/Copy Asset Path to ClipBoard")]
  static void CopyAssetPath2Clipboard()
    {
#if UNITY_EDITOR
    //string path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
    string path = Application.dataPath;
    string path2 = AssetDatabase.GetAssetPath(Selection.activeInstanceID);  
    path2 = path2.Remove(0, 6);
    path = path + path2;
    path = path.Replace('/', '\\');
    //path = path.Replace('!', '');
    //string path = Debug.Log(AssetDatabase.GetAssetPath(material));
    TextEditor text2Editor = new TextEditor();
        text2Editor.text = path;
        text2Editor.OnFocus();
        text2Editor.Copy();
#endif
    }
}

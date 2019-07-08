using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;


//本程式複製自：
//http://tsubakit1.hateblo.jp/entry/2017/04/28/005237

//修改熱鍵參閱：
//https://unity3d.com/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items

// ％ = CTRL
// ＃ = Shift
// ＆ = Alt
// LEFT/RIGHT/UP/DOWN = 箭頭鍵
// F1 ~ F12
// HOME，END，PGUP，PGDN
// _小寫英文 = 單按某顆英文鍵

//更改排序位置參閱 (SiblingIndex)；
//http://kan-kikuchi.hatenablog.com/entry/SiblingIndex


/// <summary>
/// 特殊的物件複製功能
/// </summary>
public class ObjectDuplicate {

    /// <summary>
    /// Ctrl + Shift + D = 複製物件至原物件下方，且名稱不變
    /// </summary>
    [MenuItem("Edit/DummyDuplicate %#d", false, -1)]
    static void CreateEmptyObjec2t() {
        foreach (Object obj in Selection.objects) {
            var path = AssetDatabase.GetAssetPath(obj);
            if (path == string.Empty) {
                GameObject gameObject = obj as GameObject;
                GameObject copy = GameObject.Instantiate(gameObject, gameObject.transform.parent);
                copy.name = obj.name;
                copy.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex() + 1);
                Undo.RegisterCreatedObjectUndo(copy, "deplicate");
            }
            else {
                var newPath = AssetDatabase.GenerateUniqueAssetPath(path);
                AssetDatabase.CopyAsset(path, newPath);
            }
        }
    }



    /// <summary>
    /// Ctrl + Shift + Alt + D = 複製物件至原物件下方，名稱保留括弧編號
    /// </summary>
    [MenuItem("Edit/DummyDuplicate2 %#&d", false, -1)]
    static void CreateEmptyObjec3t() {

        int index = 0;                                    //記錄相同名稱物件的編號
        string tmpName = "";                              //記錄複製物件的名稱
        GameObject tmpObj = null;                         //記錄複製出來的物件
        List<GameObject> goList = new List<GameObject>(); //記錄含有相同名稱的物件有哪些

        foreach (Object obj in Selection.objects) {
            var path = AssetDatabase.GetAssetPath(obj);

            //複製物件
            if (path == string.Empty) {
                GameObject gameObject = obj as GameObject;
                tmpObj = GameObject.Instantiate(gameObject, gameObject.transform.parent);
                tmpObj.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex() + 1);
                Undo.RegisterCreatedObjectUndo(tmpObj, "deplicate");

                //記錄複製物件的名稱
                try {
                    //去編號
                    string[] tmp = obj.name.Split('('); 
                    tmpName = tmp[0];
                }
                catch {
                    tmpName = obj.name; 
                }

                 //找尋場景含有相同名稱的物件
                 goList.Clear();
                 foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject))) {
                     if (go.name.Contains(tmpName)) {
                         goList.Add(go);
                     }
                 }
                
                 //分析含有相同名稱的物件中，編號最大的數值是多少
                 for (int i = 0; i < goList.Count; i++) {
                     if (goList[i].name.Contains(")")) {
                         try {
                             string[] tmp = goList[i].name.Split('(')[1].Split(')');
                             int tmpIndex = int.Parse(tmp[0]);
                             if (index <= tmpIndex) {
                                 index = tmpIndex;
                             }
                         }
                         catch {
                             index = 0;
                         }
                     }
                 }
                
                 //改名，給物件名稱加上括弧編號
                 tmpObj.name = tmpName + " (" + (index + 1) + ")";
                
                 //如果是從名稱有編號的物件去做複製，編號前的空格會變成兩個，所以這裡把兩個空格變成一個空格
                 tmpObj.name = tmpObj.name.Replace("  ", " ");
            }

            //複製檔案(?)
            else {
                var newPath = AssetDatabase.GenerateUniqueAssetPath(path);
                AssetDatabase.CopyAsset(path, newPath);
            }
        }
    }

}
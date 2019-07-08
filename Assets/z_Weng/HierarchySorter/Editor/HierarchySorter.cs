//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@@@@%,,*&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@(,,,,,,,,*(@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@(,,,,,,,,,,,,,,,(@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@%*,,*/////,,,*(#%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@&(///////#####%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@&(((((########%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@&(((((((######%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@@@@#(((%@@@@%#%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,#&@@@((((#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*/((((((((@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(((((@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(##((@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(######%@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(########%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(########%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(########%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(########%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(########%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(########%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(########%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@/,,,*****(########%@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@&*,,,*****(#########&@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@&%###*,,,,****(############%&@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@%#######*,,,,,,**(###############%&@@@@@@@@@@@@@@
//@@@@@@@@@@@&%####(///*,,,,,,(#######((######%@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@&////////**,,(###((((((###@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@&(///////*(((((((((#@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@%//((((((((#&@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@@@@((((%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@[WAS THIS USEFUL? CONSIDER FOLLOWING US ON TWITTER]@@@@@
//@@@@@@@@@@@@https://twitter.com/WinteractiveSE@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;


[ExecuteInEditMode]
public class HierarchySorter : MonoBehaviour {


    [MenuItem("GameObject/排序 (Sort)/A_to_Z", priority = 0)]
    public static void HierarchySort_A_to_Z() {

        List<GameObject> allObjects    = new List<GameObject>(Selection.gameObjects);

        allObjects = allObjects.OrderBy(x => x.name).ToList();
        foreach (GameObject item in allObjects) {
            item.transform.SetAsLastSibling();
        }
    }


    [MenuItem("GameObject/排序 (Sort)/Z_to_A", priority = 0)]
    public static void HierarchySort_Z_to_A() {

        List<GameObject> allObjects    = new List<GameObject>(Selection.gameObjects);

        allObjects = allObjects.OrderBy(x => x.name).ToList();
        foreach (GameObject item in allObjects) {
            item.transform.SetAsFirstSibling();
        }
    }



    //[MenuItem("GameObject/排序 (Sort)/A_to_Z (All)", priority = 0)]
    //public static void HierarchySort_A_to_Z_All() {
    //    
    //    List<Canvas> canvases          = new List<Canvas>(FindObjectsOfType<Canvas>());
    //    List<GameObject> canvasObjects = new List<GameObject>();
    //    List<GameObject> allObjects    = new List<GameObject>(FindObjectsOfType<GameObject>());
    //
    //    foreach (Canvas canvas in canvases) {
    //        foreach (Transform canvasObj in canvas.GetComponentsInChildren<Transform>()) {
    //            canvasObjects.Add(canvasObj.gameObject);
    //        }
    //    }
    //
    //    foreach (GameObject obj in canvasObjects) {
    //        allObjects.Remove(obj);
    //    }
    //
    //
    //    allObjects = allObjects.OrderBy(x => x.name).ToList();
    //    foreach (GameObject item in allObjects)  {
    //        item.transform.SetAsLastSibling();
    //    }
    //}





}

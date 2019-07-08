using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//參考自：
//https://stackoverflow.com/questions/37943729/get-all-children-children-of-children-in-unity3d


public class PositionManager : MonoBehaviour {


    //[HelpBox("PositionManager的子物件會自動加入清單", HelpBoxType.Info)]
    private Dictionary<string, GameObject> _dirPosObject = new Dictionary<string, GameObject>();

    private void Awake()
    {
        GetChildRecursive(transform);
    }

    
    /// <summary>
    /// 獲取指定物件A下的所有子物件 (包含子物件的子物件)
    /// </summary>
    /// <param name="obj"> 指定物件A </param>
    private void GetChildRecursive(Transform obj)
    {
        if (obj == null)
        {
            return;
        }
        foreach (Transform child in obj)
        {
            if (child == null)
            {
                continue;
            }
            Save(child.name, child.gameObject);
            GetChildRecursive(child);
        }
    }


    private void Save(string strName, GameObject Obj)
    {
        if (_dirPosObject.ContainsKey(strName))
        {
            MessageBox.ASSERT("PosManager下面存在同名的物件，" + strName);
        }
        else
        {
            _dirPosObject.Add(strName, Obj);
        }
    }


    ///// <summary>
    ///// 獲取指定物件A下的所有子物件 (不含子物件的子物件)
    ///// </summary>
    ///// <param name="obj"> 指定物件A </param>
    //private void GetChild(Transform obj){
    //    for (int i = 0; i < obj.childCount; i++){
    //        CreatePos[i] = obj.GetChild(i);
    //    }
    //}

    public GameObject f_GetPosManagerObject(string strName)
    {
        if (_dirPosObject.ContainsKey(strName))
        {
            return _dirPosObject[strName];
        }
        return null;
    }
    
}

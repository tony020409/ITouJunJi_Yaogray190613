using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour {



    [Header("物件捉取")]
    public Transform[] Obj;


    //讓其他程式獲取用
    public static GameObjectManager inst;
    private void Awake(){
        inst = this;

        //將子物件加到清單
        Obj = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            Obj[i] = transform.GetChild(i);
        }
    }


    // Use this for initialization
    void Start() {
    }


}

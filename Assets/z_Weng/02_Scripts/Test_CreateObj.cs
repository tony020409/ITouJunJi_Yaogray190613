using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CreateObj : MonoBehaviour {

    [Header("要產生的物件")]
    public GameObject G1;               //要生產的物件

    [Header("是否要延遲產生、要延遲多久")]
    public bool useTime = false; 
    public float cTime = 0.0f;

    [Header("是否使用按鍵 (若不使用則自動召喚一次)")]
    public bool isKey = true;           //使用按鍵
    public KeyCode Obj1Key = KeyCode.K; //按鍵
    private bool canCreate = true;      //讓自動模式只產生一次爆炸

    [Header("你按了多少次鍵盤了(測試用)")]
    public int count1;                  //生產過幾次了



    // Start ===========================================================
    void Start(){
    }

    // Update ==========================================================
    void Update(){

        if (Input.GetKeyDown(Obj1Key) && isKey){
            if (!useTime)                   //如果不延遲產生物件
            { CreateObj(); }                //就直接產生物件
            else                            //如果要延遲產生物件
            { Invoke("CreateObj", cTime); } //就在指定要延遲的時間後產生物件
        }

        if (!isKey && canCreate){
            if (!useTime)                   //如果不延遲產生物件
            { CreateObj(); }                //就直接產生物件
            else                            //如果要延遲產生物件
            { Invoke("CreateObj", cTime); } //就在指定要延遲的時間後產生物件
            canCreate = false;              //只產生一次
        }


    }

    // 創造物件 ======================================================
    void CreateObj(){
        count1++;
        Instantiate(G1, transform.position, Quaternion.identity);
    }


}

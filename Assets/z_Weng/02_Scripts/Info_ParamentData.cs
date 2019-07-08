using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info_ParamentData : MonoBehaviour {

    [Header("【參數1】")]
    [Rename(" - 名稱")] public string ParamentA = "RoomKeyA";
    [Rename(" - 數值")] public string KeyA;

    [Header("【參數2】")]
    [Rename(" - 名稱")] public string ParamentB = "RoomKeyB";
    [Rename(" - 數值")] public string KeyB;

    //測試時動態修改用的參數
    private int tempKeyA = 0;
    private int tempKeyB = 0;

    [Header("參數情況 (輸出後觀測用的Text)")]
    public Text textKeyA;
    public Text textKeyB;

    [Rename("觀測模式")]
    public bool isDebugMode;



    void Start(){
        KeyA = "-99"; //給個初始值避掉 Update裡的  if (KeyA != "") 判斷
        KeyB = "-99"; //給個初始值避掉 Update裡的  if (KeyA != "") 判斷

        //預設不顯示
        isDebugMode = false;
        if (textKeyA != null) {
            textKeyA.gameObject.SetActive(false);
        }
        if (textKeyB != null)  {
            textKeyB.gameObject.SetActive(false);
        }


    }



    void Update () {

        #if UNITY_EDITOR
        if (BattleMain.GetInstance() == null) {
            return;
        }

        GetParamentData();
        ShowParamentData();


        //測試用
        if (Input.GetKey(KeyCode.KeypadPeriod) && Input.GetKeyDown(KeyCode.Keypad1)) {
            tempKeyA++;
            BattleMain.GetInstance().f_SetParamentData(ParamentA, tempKeyA.ToString());
        }

        //測試用
        if (Input.GetKey(KeyCode.KeypadPeriod) && Input.GetKeyDown(KeyCode.Keypad2)) {
            tempKeyB++;
            BattleMain.GetInstance().f_SetParamentData(ParamentB, tempKeyB.ToString());
        }
        #endif



        //輸出後按「橫排數字2」開/關參數檢測模式
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            //if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
                isDebugMode = !isDebugMode;
                if (isDebugMode) {
                    textKeyA.gameObject.SetActive(true);
                    textKeyB.gameObject.SetActive(true);
                } else {
                    textKeyA.gameObject.SetActive(false);
                    textKeyB.gameObject.SetActive(false);
                }
            //}

        }


    }




    private void FixedUpdate() {

        if (isDebugMode) {
            if (BattleMain.GetInstance() == null) {
                return;
            }
            if (glo_Main.GetInstance() == null) {
                return;
            }
            else if (glo_Main.GetInstance().m_EM_GameStatic != EM_GameStatic.Gaming) {
                return;
            }
            GetParamentData();
            ShowParamentData();
        }

    }



    /// <summary>
    /// 嘗試取得參數
    /// </summary>
    private void GetParamentData() {

        try
        {
            if (KeyA != "")
            {
                KeyA = BattleMain.GetInstance().f_GetParamentData(ParamentA);
            }
        }
        catch { }


        try
        {
            if (KeyB != "")
            {
                KeyB = BattleMain.GetInstance().f_GetParamentData(ParamentB);
            }
        }
        catch
        { }
    }


    /// <summary>
    /// 顯示參數
    /// </summary>
    private void ShowParamentData() {
        if (textKeyA != null) {
            textKeyA.text = "參數： " + ParamentA + " = " + KeyA;
        }
        if (textKeyB != null) {
            textKeyB.text = "參數： " + ParamentB + " = " + KeyB;
        }

    }



}

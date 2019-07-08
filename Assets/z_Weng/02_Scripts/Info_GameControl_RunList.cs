using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info_GameControl_RunList : MonoBehaviour {

    [Rename("觀測模式")]
    public bool isDebugMode;

    [Rename("開關顯示用的節點")]
    public GameObject Root;

    [Rename("用來顯示的 Text")]
    public Text _aList;

    [Rename("強制執行的 Button")]
    public Button _button;


    private List<int> temp = new List<int>();

    [HideInInspector]
    public int tempCount = 0;

    // Use this for initialization
    void Start () {

        //清空資訊
        _aList.text = "";

        //註冊按鈕強制執行的事件
        //_button.onClick.AddListener( BattleMain.GetInstance().f_ForceGameControlRun );

        //預設不顯示
        isDebugMode = false;
        Root.SetActive(isDebugMode);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            isDebugMode = !isDebugMode;
            Root.SetActive(isDebugMode);
        }
	}


    private void FixedUpdate() {

        if (isDebugMode) { 
            if (BattleMain.GetInstance() == null) {
                return;
            }
            temp = BattleMain.GetInstance().GCA_reserveList;
            if (tempCount != temp.Count) {
                Refresh();
            }
            for (int i = 0; i < temp.Count; i++) {
                if (tempCount == temp.Count) {
                    return;
                }
                _aList.text += temp[i].ToString() + " / ";
                tempCount++;
            }
        }

    }


    /// <summary>
    /// 刷新待執行清單
    /// </summary>
    void Refresh() {
        tempCount = 0;
        _aList.text = "";
    }




}

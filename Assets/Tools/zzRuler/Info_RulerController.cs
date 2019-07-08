using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info_RulerController : MonoBehaviour {


    [HelpBox("右Alt+K顯示玩家定位點與比例尺，方便定位", HelpBoxType.Info)]
    [Rename("當前開關狀態")] public bool tmpState;
    [Rename("比例尺模型")]   public GameObject   Ruler;
    [Rename("玩家位置清單")] public GameObject[] PlayerPosList;



	// Use this for initialization
	void Start () {
        tmpState = false;
        SetInfo(tmpState);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.K)) {
            tmpState = !tmpState;
            SetInfo(tmpState);
        }
	}


    /// <summary>
    /// 設定開關狀態
    /// </summary>
    /// <param name="tmp"> 設定目標 </param>
    void SetInfo(bool tmp) {
        if (Ruler != null) {
            Ruler.SetActive(tmp);
        }
        for (int i = 0; i < PlayerPosList.Length; i++) {
            PlayerPosList[i].SetActive(tmp);
        }
    }




}

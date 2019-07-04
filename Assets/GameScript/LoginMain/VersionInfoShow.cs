using UnityEngine;
using UnityEngine.UI;

public class VersionInfoShow : MonoBehaviour {


    public Text VerifyText;
    public Text GameText;
    public Image BackGround;
    [Line(2)]
    [HelpBox("版號=遊戲名稱+輸出年.月.日.流水號\n" + "範例：異星任務 180918_01\n" + "【 改完記得儲存場景再輸出！】",HelpBoxType.Info)]
    [Rename("驗證版號")]           public string Info_VerifyVersion = "w180713";
    [Rename("遊戲名稱")]           public string Info_GameName      = "Agent";
    [Rename("輸出日期")]           public string Info_BuildDay      = "181120";
    [Rename("當日流水號(0~99)")]   public string Info_BuildNumber   = "01";
    [Rename("其它備註事項")]       public string Info_Description = "";
    [Rename("背景圖 (1920x1080)")] public Sprite newBackGround;
    private string szDsah = "_";

    // Use this for initialization
    void Start () {

        //底線組合
        if (Info_BuildDay!= "") {
            //Info_BuildDay = szDsah + Info_BuildDay;
        }
        if (Info_BuildNumber != "") {
            Info_BuildNumber = szDsah + Info_BuildNumber;
        }
        if (Info_Description != "") {
            Info_Description = szDsah + Info_Description;
        }



        //驗證版號
        if (VerifyText != null) {
            VerifyText.text = Info_VerifyVersion;
        }

        //遊戲版號 = 遊戲名稱 + 輸出日期 + 流水號 + 其他備註
        if (GameText != null) {
            GameText.text = Info_GameName + Info_BuildDay + Info_BuildNumber + Info_Description;
        }

        //背景圖
        if (BackGround != null) {
            if (newBackGround != null) {
                BackGround.sprite = newBackGround;
            }
        }
        
    }



}

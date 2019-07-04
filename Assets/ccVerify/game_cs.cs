using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccVerifySDK;
public class game_cs : MonoBehaviour {

    public GameObject disconect_icon;

    private int a;

	// Use this for initialization
	void Start () {
        disconect_icon.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //驗證開始
    public void On_StarGameBtn()
    {
        //上傳序號
        a = Random.Range(0, 999999);
        ccVerityService.GetInstance().f_StartGame(a.ToString("000000"));

    }



    //驗證次數
    public void On_GameTimeGameBtn()
    {
        ccVerityService.GetInstance().f_SubmitData(a.ToString("000000"), 45, Callback_Submit);
    }

    private void Callback_Submit(eVerifyMsgOperateResult teMsgOperateResult, string strKey = "")
    {
        //服务器确认数据后返回的提交数据的KEY
        if (teMsgOperateResult != eVerifyMsgOperateResult.OR_Succeed)
        {
            Debug.Log("SubmitData 游戏开始数据提交失败 " + ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult));
            disconect_icon.SetActive(true);
        }
        else
        {
            Debug.Log("数据提交成功 " + ccVerityService.GetInstance().f_GetSubmitDataCount());
        }
    }


    //驗證結束
    public void On_StopGameBtn()
    {
        ccVerityService.GetInstance().f_StopGame(a.ToString("000000"));
    }
}

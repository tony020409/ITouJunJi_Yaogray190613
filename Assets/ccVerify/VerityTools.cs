using ccVerifySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerityTools
{
  
    /// <summary>
    /// 驗證：計次
    /// </summary>
    /// <returns></returns>
    public static string f_SubmitData() {
        return ccVerityService.GetInstance().f_SubmitData(GetData(), GloData.glo_iSubmitDataTimeOut, VerifyCallBack_SubmitData);
    }


    static void OpenSocketErrorUI(string strErrorInfor) {
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SOCKETERROR, strErrorInfor);
    }


    public static void VerifyCallBack_SubmitData(eVerifyMsgOperateResult teMsgOperateResult, string strKey = "") {
        if (teMsgOperateResult == eVerifyMsgOperateResult.OR_Succeed) {

        }
        else {//FAIL
            OpenSocketErrorUI(ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult));
        }
    }


    /// <summary>
    /// 驗證：開始
    /// </summary>
    /// <returns></returns>
    public static string f_StartGame() {
        return ccVerityService.GetInstance().f_StartGame(GetData());
    }


    /// <summary>
    /// 驗證：結束
    /// </summary>
    /// <returns></returns>
    public static string f_StopGame() {
        return ccVerityService.GetInstance().f_StopGame(GetData());
    }



    private static string GetData() {
        return string.Format("{0}{1}", UnityEngine.Random.Range(11111111, 99999999), UnityEngine.Random.Range(11111111, 99999999));
    }




}
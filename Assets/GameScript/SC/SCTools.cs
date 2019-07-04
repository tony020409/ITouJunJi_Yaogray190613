using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCTools
{

    /// <summary>
    /// 手工设置当前工作的VVV几
    /// </summary>
    /// <param name="strVVV"></param>
    public static void f_SetVVV(string strVVV)
    {
        if (strVVV.Length > 0) {
            GloData.glo_ProName = strVVV;
            GloData.glo_strLoadAllSC   = "http://" + GloData.glo_strHttpServerIP + "/" + GloData.glo_ProName + "/ver/";
            GloData.glo_strLoadVer     = "http://" + GloData.glo_strHttpServerIP + "/" + GloData.glo_ProName + "/ver/LoadVer.php";
            GloData.glo_strSaveLog     = "http://" + GloData.glo_strHttpServerIP + "/" + GloData.glo_ProName + "/Log/SaveLog.php";
            GloData.glo_strCDNResource = "http://" + GloData.glo_strCDNServer    + "/" + GloData.glo_ProName + "/assetbundles/";
            Debug.Log("VVV 通道設置完成 = " + GloData.glo_ProName);
        }
        //glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.LOADSCSUC);
    }

}
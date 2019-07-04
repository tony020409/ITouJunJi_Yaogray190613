using UnityEngine;
using System.Collections;



public class ResourceTools
{

    public static string f_GetLocalMainPath()
    {
        //if (GloData.glo_iPackType == 0)
        //{
        //    return Application.persistentDataPath + "/" + GloData.glo_ProName;
        //}
        //else
        //{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            Debug.Log(GloData.glo_ProName + "(ResourceTools.cs)");
            return Application.streamingAssetsPath + "/" + GloData.glo_ProName;
#elif UNITY_IPHONE
        return Application.streamingAssetsPath + "/" + GloData.glo_ProName;
#elif UNITY_ANDROID
        return Application.dataPath + "!assets/" + GloData.glo_ProName;
#endif
        //}
    }


    public static string f_GetServerMainPath()
    {
        string ppSQL = "";
#if UNITY_WEBPLAYER
        ppSQL = "WebPlayer";
#elif UNITY_ANDROID
        ppSQL = "Android";
#elif UNITY_IPHONE
        ppSQL = "IOS";
#else
        ppSQL = "Windows";
#endif
        return GloData.glo_strCDNResource + ppSQL;
    }


    public static void f_UpdateURL()
    {


    }
    
}
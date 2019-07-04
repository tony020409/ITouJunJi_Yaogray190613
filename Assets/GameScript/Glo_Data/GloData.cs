

//#define  VER_LAN      //使用 #define  VER_LAN   表示使用外部脚本 (外部腾讯云服务器)
//#define  LOCAL_LAN  //使用 #define  LOCAL_LAN 表示使用网内测试脚本，

using UnityEngine;
using System.Collections;

/// <summary>
/// 项目说明：
/// 项目名称：
/// 工程开始时间                          2017-02-01
/// </summary>
public class GloData// : MonoBehaviour
{
    public static int glo_iVer = 1000001;
    public static string glo_strVer = "r.";
       

    /// <summary>
    /// 控制台是否输出Log
    /// </summary>
    public static bool m_bDebugLog = true;

    // <summary>
    // 是否为单机版本 true单机 false联网
    // </summary>
    //public static bool m_bCloseSocket = true;


    public static string glo_strSvrIP = "192.168.0.106";
    public static int glo_iSvrPort = 9600;
    public static int glo_iPingTime = 45;
    public static int glo_iGameId = 90;
    public static int glo_iGamePlayerTime = 600;
    public static string glo_strPcName = "";

    public static string glo_strCDNServer    = "123.207.87.187";
    public static string glo_strHttpServerIP = "123.207.87.187";

    public static int glo_iSubmitDataTimeOut = 10;

    

#if vvv2
    public static string glo_ProName = "vvv2";

#elif vvv3
    public static string glo_ProName = "vvv3";

#elif vvv4
    public static string glo_ProName = "vvv4";

#elif vvv5
    public static string glo_ProName = "vvv5";

#elif vvv6
    public static string glo_ProName = "vvv6";

#elif vvv7
    public static string glo_ProName = "vvv7";

#elif vvv8
    public static string glo_ProName = "vvv8";

#elif vvv9
    public static string glo_ProName = "vvv9";

#elif vvv10
    public static string glo_ProName = "vvv10";

#elif vvv11
    public static string glo_ProName = "vvv11";

#elif vvv12
    public static string glo_ProName = "vvv12";

#elif vvv13
    public static string glo_ProName = "vvv13";

#elif vvv14
    public static string glo_ProName = "vvv14";

#elif vvv15
    public static string glo_ProName = "vvv15";

#elif vvv30
    public static string glo_ProName = "vvv30";

#elif vvv31
    public static string glo_ProName = "vvv31";

#elif vvv32
    public static string glo_ProName = "vvv32";

#elif vvv33
    public static string glo_ProName = "vvv33";

#elif vvv34
    public static string glo_ProName = "vvv34";

#elif vvv35
    public static string glo_ProName = "vvv35";

#elif vvv36
    public static string glo_ProName = "vvv36";

#elif vvv37
    public static string glo_ProName = "vvv37";

#elif vvv38
    public static string glo_ProName = "vvv38";

#elif vvv39
    public static string glo_ProName = "vvv39";

#elif vvv40
    public static string glo_ProName = "vvv40";

#elif vvv41
    public static string glo_ProName = "vvv41";

#elif vvv42
    public static string glo_ProName = "vvv42";

#elif vvv43
    public static string glo_ProName = "vvv43";

#elif vvv44
    public static string glo_ProName = "vvv44";

#elif vvv45
    public static string glo_ProName = "vvv45";

#elif vvv46
    public static string glo_ProName = "vvv46";

#elif vvv47
    public static string glo_ProName = "vvv47";

#elif vvv48
    public static string glo_ProName = "vvv48";

#elif vvv49
    public static string glo_ProName = "vvv49";

#elif vvv50
    public static string glo_ProName = "vvv50";

#elif vvv51
    public static string glo_ProName = "vvv51";

#elif L1
    //本地测试设置
    public static string glo_ProName = "vvv2";


#else
#endif

    /// <summary>
    /// 游戏模式，-99无效  0玩家模式 1管理员模式
    /// </summary>
    public static int glo_iGameModel = -99;


    public static int glo_iSvrControllPort = 9601;
    public static string glo_strLoadVer   = "http://" + glo_strHttpServerIP + "/" + glo_ProName + "/ver/LoadVer.php";
    public static string glo_strLoadAllSC = "http://" + glo_strHttpServerIP + "/" + glo_ProName + "/ver/";
    public static string glo_strSaveLog   = "http://" + glo_strHttpServerIP + "/" + glo_ProName + "/Log/SaveLog.php";

   
    public static string glo_strCDNResource = "http://" + glo_strCDNServer + "/" + glo_ProName + "/assetbundles/";

    public static float glo_fCatchBufSleepTime = 0.1f;
    public static float glo_fAutoReLoginSleepTime = 10f;


    public static int glo_iRecvPingTime = 300;

    public static string glo_Team = "A";

    /// <summary>
    /// 校正坐标
    /// </summary>
    public static float glo_fGameXYPos = 0.01f;


    /// <summary>
    /// 资源密码
    /// </summary>
    public static string glo_strResourcePwd = "Night4";


    /// <summary>
    /// 
    /// </summary>
    public static float glo_fDefaultY = 9.75f;

    /// <summary>
    /// 玩家基准地面
    /// </summary>
    public static float glo_fPlayerDefaultY = 10f;

    /// <summary>
    /// 光球爆炸范围
    /// </summary>
    public static float glo_fBombSize = 3f;

    /// <summary>
    /// 光球爆炸伤害
    /// </summary>
    public static int glo_iBombAttackHp = 50;

    /// <summary>
    /// 护盾爆炸范围
    /// </summary>
    public static float glo_fShieldSize = 3f;

    /// <summary>
    /// 护盾增加HP
    /// </summary>
    public static int glo_iShieldHp = 50;

    /// <summary>
    /// 弓箭射中的伤害
    /// </summary>
    public static int glo_iArrowAttackHp = 5;
    
        
    /// <summary>
    /// 是否可以射杀自己人
    /// </summary>
    public static bool glo_bCanShootMySelf = false;

    /// <summary>
    /// 最大日志存储量
    /// </summary>
    public static int glo_iMaxLogSize = 100;

    /// <summary>
    /// 0不自动上传LOG， 1自动上传游戏LOG, 自动上传游戏LOG和UnityLog
    /// </summary>
    public static int glo_iAutoUpdateLog = 0;

    /// <summary>
    /// 自动上传LOG时间
    /// </summary>
    public static int glo_iAutoUpdateLogTime = 60;

    public static float glo_fMaxControllFrameTime = 1 / 15f;
    public static float glo_fActionFPS = 1/30f;
    public static float glo_fTestFPS = 1 / 10f;

    /// <summary>
    /// 脚本设置玩家的位置
    /// </summary>
    public static int glo_iPos = 0;
    /// <summary>
    /// 脚本设置玩家的队伍
    /// </summary>
    public static int glo_iTeamId = 0;
    public static float glo_fMaxRunTimeOut = 1;
    /// <summary>
    /// 保存玩家自己的身高
    /// </summary>
    public static float glo_fMyselfHeight = 0;


}
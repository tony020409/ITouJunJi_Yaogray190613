using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using ccU3DEngine;
using System.Collections.Generic;
using ccGameControllSDK;


public class glo_Main : FLEventDispatcherMono
{
    public GameObject m_TestObj;
    public GameObject[] m_aMonsterModel;


    public EM_GameStatic m_EM_GameStatic = EM_GameStatic.Waiting;

    public GameSyscManager m_GameSyscManager;
	
    [HideInInspector]
    public ccMessagePoolV2 m_GameMessagePool = new ccMessagePoolV2();
    public ccMessagePoolV2 m_UIMessagePool = new ccMessagePoolV2();

    ccLog m_ccLog = new ccLog();
    public SC_Pool m_SC_Pool;


    /// <summary>
    /// 游戏资源管理器
    /// </summary>
    public ResourceManager m_ResourceManager;



    /// <summary>
    /// 控制台是否啟用
    /// </summary>
    public bool Controller_bool;


    private static glo_Main _Instance = null;
    public static glo_Main GetInstance()
    {
        if (null == _Instance)
        {
            _Instance = (glo_Main)FindObjectOfType(typeof(glo_Main));
            if (null == _Instance)
            {
                MessageBox.ASSERT("init glo_Main Fail");
				return null;
            }
            _Instance.m_GameSyscManager = _Instance.gameObject.AddComponent<GameSyscManager>();
                      
        }
        return _Instance;
    }


    void Awake()
    {
        //接收是否使用控制台，以之後傳給 BattleMain接收
        Controller_bool = ccVerifyMain2.GetInstance().Controller_bool;

        m_ccLog.f_SetUserId(SystemInfo.deviceUniqueIdentifier);
        m_ccLog.f_Start();

        //GloData.glo_iVer = ccMath.atoi(Application.version);
        GloData.glo_strVer = GloData.glo_strVer + GloData.glo_iVer;
        ResourceTools.f_UpdateURL();

        m_SC_Pool = new SC_Pool();
        m_ResourceManager = new ResourceManager();
        ReadGameModel();

    }


    void Start()
    {
        Application.targetFrameRate = 90;
        MessageBox.DEBUG("初始游戏");
        MessageBox.DEBUG(Application.persistentDataPath);

        DontDestroyOnLoad(this);    //自己不消失

        InitMessage();
        InitEvent();
        InitAudio();
        InitResManager();

    }

    private void InitMessage()
    {
        ccTimeEvent.GetInstance().f_ChangePingTime(0.02f);
        //m_GameMessagePool.f_AddListener(MessageDef.LOADSCSUC, CallBack_InitResManager);
    }


    private void InitEvent()
    {

    }

    private void InitAudio()
    {

    }

    /// <summary>
    /// 初始化资源管理
    /// 开始加载资源
    /// </summary>
    private void InitResManager()
    {
        MessageBox.DEBUG("InitResManager...");
        //m_GameMessagePool.f_RemoveListener(MessageDef.LOADSCSUC, CallBack_InitResManager);

        startLoadResourceTime = Time.realtimeSinceStartup;
        LoadResource _LoadResource = new LoadResource();
        _LoadResource.f_StartLoad(Callback_LoadResSuc);
    }


    /// <summary>
    /// 
    /// </summary>
    private float startLoadResourceTime = 0;

    /// <summary>
    /// 1. 资源成功加载完回调、
    /// 2. 初始化玩家数据结构、
    /// 3. 广播资源加载完成（通知显示logo页面可以进入游戏）
    /// </summary>
    void Callback_LoadResSuc(object Obj) {
        //m_ccLog.f_Start();
        //string resourceTimeHint = "加载资源、表格花费" + (Time.realtimeSinceStartup - startLoadResourceTime) + "秒.";
        //MessageBox.DEBUG(resourceTimeHint);
        //Data_Pool.f_InitPool();
        //
        //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_RESOURCECOMPLETE);
        //
        //
        //#if Verify
        ////新的驗證裡沒有 f_Start()
        ////ccVerifyMain2.GetInstance().f_Start();
        //
        ////登入畫面顯示 Glo_Main 情況
        //ccVerifyMain2.GetInstance().f_Show_GloMainStatic();
        //#endif

        m_ccLog.f_Start();
        Data_Pool.f_InitPool();
        if (Controller_bool) {
            GameControllLogin();
        }
        else {
            //登入畫面顯示 Glo_Main 情況
            ccVerifyMain2.GetInstance().f_Show_GloMainStatic();
        }
    }



    #region 控制台相关

    /// <summary>
    /// 开始控制台登陆
    /// </summary>
    void GameControllLogin() {
        //int iGameId                                   游戏Id
        //string strName                                登陆帐号
        //string strPwd                                 登陆密码
        //ccSimpleCallback tCallBackLogin               登陆结果回调
        //ccSimpleCallbackGameCMD CallBackPlayGame      接收到控制台发来的开始游戏的指令
        //ccSimpleCallback tCallBackNetState            网络波动回调
        ccGameControll.GetInstance().f_Login(GloData.glo_iGameId, "111", "111", GameControllSDKLoginResult, CallBackStartGame, CallBackNetState);
    }

    /// <summary>
    /// 接收到控制台发来的开始游戏的指令
    /// </summary>
    void CallBackStartGame() {
        MessageBox.DEBUG("控制台接收开始游戏指令");
        BattleMain.GetInstance().f_StartGame();
    }

    /// <summary>
    /// 控制台网络发生波动回调
    /// </summary>
    /// <param name="teSocketResult"></param>
    void CallBackNetState(eSocketResult teSocketResult) {
        MessageBox.DEBUG("控制台网络发生波动 " + teSocketResult.ToString());
    }



    /// <summary>
    /// 登陆结果回调
    /// </summary>
    /// <param name="teSocketResult"></param>   
    void GameControllSDKLoginResult(eSocketResult teSocketResult) {

        if (teSocketResult != eSocketResult.OR_Succeed) {
            MessageBox.DEBUG("控制台登陆失败" + teSocketResult.ToString());
            return;
        }

        Debug.Log("GameControll.登陆成功");
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_RESOURCECOMPLETE);
    }
    #endregion



    private bool ReadGameModel()
    {

        string path = Application.streamingAssetsPath + "/Config/Config.txt";

        if (!File.Exists(path))
        {
            MessageBox.ASSERT("Load Init Fail");
            return false;
        }
        StreamReader sr = File.OpenText(path);

        string strIp = sr.ReadToEnd();
        sr.Close();

        if (!string.IsNullOrEmpty(strIp))
        {
            //以"-"區分數據
            string[] aData = ccMath.f_String2ArrayString(strIp, "-");

            //給 GlodData資料
            GloData.glo_iPos = ccMath.atoi(aData[0]);
            GloData.glo_iTeamId = ccMath.atoi(aData[1]);
            GloData.glo_strSvrIP = aData[2];
            GloData.glo_iSvrPort = ccMath.atoi(aData[3]);
            GloData.glo_iGameModel = ccMath.atoi(aData[4]);
            GloData.glo_fMaxControllFrameTime = ccMath.atof(aData[5]);
            GloData.glo_fActionFPS = ccMath.atof(aData[6]);
            GloData.glo_fTestFPS = ccMath.atof(aData[7]);
            string strVVV = aData[8];
            SCTools.f_SetVVV(strVVV);
            return true;
        }

        return false;
    }


    void Update()
    {
        GameSocket.GetInstance().f_Update();
        ControllSocket.GetInstance().f_Update();

        m_GameMessagePool.f_Update();
        m_UIMessagePool.f_Update();

    }

    private void OnDestroy()
    {
        GameSocket.GetInstance().f_Close();
        GameSocket.GetInstance().Destroy();

        ControllSocket.GetInstance().f_Close();
        ControllSocket.GetInstance().Destroy();
    }

    public void f_Destroy()
    {
        MessageBox.DEBUG("强制结束游戏 QuitGame");
        MissionLog.f_Out();

        m_GameSyscManager.f_Stop();
        GameSocket.GetInstance().f_Close();
        GameSocket.GetInstance().Destroy();

        ControllSocket.GetInstance().f_Close();
        ControllSocket.GetInstance().Destroy();

        MessageBox.DEBUG("................................................");

        ccTimeEvent.GetInstance().f_RegEvent(1, false, null, ApplicationQuit);
    }

    //void OnApplicationQuit()
    void ApplicationQuit(object Obj)
    {
        m_ccLog.f_Quit();
        Application.Quit(); 
    }


    /// <summary>
    /// 登陆游戏成功，初始游戏数据准备进入游戏
    /// </summary>
    public void f_InitGame()
    {
        
       
    }


    public Coroutine f_StartCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }


    public void f_Reset()
    {
        m_GameSyscManager.f_Reset();
    }

    public GameObject f_CreateTestObject()
    {
        GameObject Obj = Instantiate(m_TestObj);
        return Obj;
    }

}

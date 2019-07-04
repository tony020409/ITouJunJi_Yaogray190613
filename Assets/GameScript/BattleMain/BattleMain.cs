using ccU3DEngine;
using GameControllAction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using DG.Tweening;
using PathologicalGames;
using System.Text.RegularExpressions;
using ccGameControllSDK;

public class BattleMain : UIFramwork, IPlayerMono
{

    private bool Controller_bool;
    private DateTime m_tStartTimeTime;

    /// <summary>
    /// 保存游戏子彈Pool
    /// </summary>
    public BulletPool m_BulletPool = new BulletPool();

    /// <summary>
    /// 保存游戏角色Pool
    /// </summary>
    public BattleRolePool m_BattleRolePool = new BattleRolePool();


    public GameMission _GameMission = new GameMission();
    private AudioSource _audioSource;
    private BattleAI _BattleAI = new BattleAI();
    private GameControllV2 _GameControllV2 = new GameControllV2();
    private PositionManager _PositionManager = null;

    /// <summary>
    /// 遊戲結果
    /// </summary>
    private EM_GameResult _EM_GameResult = EM_GameResult.Default;


    [Header("---------------------")]
    public MapNav m_MapNav;
    [Rename("玩家 (CameraRig)")]    public GameObject m_oMySelfPlayer_VR;
    [Rename("玩家 (MySelfPlayer)")] public GameObject m_oMySelfPlayer;
    [Rename("怪物物件節點")]        public GameObject m_oRole;
    private GameObject[] m_aPlayerPos;


    /// <summary>
    /// (物件池) 粒子特效
    /// </summary>
    [Rename("(物件池) 粒子特效")]
    public SpawnPool m_ParticlePool;


    [Header("UI項目---------------")]
    [Rename("遊戲勝利時顯示的物件")] public GameObject[] ui_Win;
    [Rename("遊戲失敗時顯示的物件")] public GameObject[] ui_Lost;

    [Header("UI：場景淡入淡出用")]
    [Rename("淡入淡出時的音樂")]   public AudioClip _FadeClip;
    [Rename("淡入淡出用的黑畫面")] public Image UI_SceneFade;

    [Header("躲藏点---------")]
    public Transform[] HidePos;


    [Header("門清單---------")]
    public GameObject[] Door;
    /// <summary>
    /// 門字典
    /// </summary>
    private Dictionary<string, DoorAnimation> _dicDoor = new Dictionary<string, DoorAnimation>();


    [Header("導航清單---------")]
    public List<NodeNavigation> NodeNavigatioList;


    [Header("網路相關-----------")]
    public bool _bGameSocketEor = false;
    [Rename("網路斷線遮罩")]   public GameObject Canvas_Internet_WARING;
    [Rename("伺服器斷線遮罩")] public GameObject Canvas_Server_WARING;

    /// <summary>
    /// 偵測到要執行的 GameControl_Condition ID 列表
    /// </summary>
    [Rename("預計執行的 條件表 ID 清單")]
    public List<int> GCC_reserveList = new List<int>();

    /// <summary>
    /// 預計執行的 GameControl ID 列表
    /// </summary>
    [Rename("預計執行的 執行表 ID 清單")]
    public List<int> GCA_reserveList = new List<int>();


    /// <summary>
    /// (Debug) 是否顯示腳本讀取的 Log
    /// </summary>
    [HideInInspector]
    public bool Debug_GameControlRead = true;


    /// <summary>
    /// (Debug) 是否顯示 物件=Null 的 Log
    /// </summary>
    [HideInInspector]
    public bool Debug_Object_isNull = false;


    /// <summary>
    /// (Debug) 是否執行測試動作
    /// </summary>
    [HideInInspector]
    public bool Debug_Mode = false;


    private static BattleMain _Instance = null;
    public static BattleMain GetInstance() {
        return _Instance;
    }

    #region 内部消息
    void Start() {
        _Instance = this;

        //外讀讀取一個叫 Config_LogOutput.txt的文件，決定要顯示的日誌內容
        string ConfigDebugFilePath = Application.streamingAssetsPath + "/" + "Config_LogOutput.txt"; //外部文檔路徑
        Debug_GameControlRead = CheckOutputDebug(ConfigDebugFilePath, "GameControl_Read", "1");      //決定是否顯示「腳本讀取內容」
        Debug_Object_isNull = CheckOutputDebug(ConfigDebugFilePath, "Null", "1");                    //決定是否顯示「找不到物件(物件=Null) 的提示訊息」
        Debug_Mode = CheckOutputDebug(ConfigDebugFilePath, "Debug_Mode", "1");                       //決定是否執行測試動作

        //接收是否使用控制台
        Controller_bool = glo_Main.GetInstance().Controller_bool;

        InitComponent();
        InitPlayerPos();
        InitGame();
        InitGameRes();
        Data_Pool.m_PlayerPool.f_RequestPlayerList();
        //ccTimeEvent.GetInstance().f_RegEvent(3, false, null, ReadyStartGame);

        if (glo_Main.GetInstance() != null) {
            glo_Main.GetInstance().m_GameSyscManager.f_AddSyscObject(this);
        }

        ccTimeEvent.GetInstance().f_RegEvent(1, false, null, InitPlayerPool);

        _audioSource = this.GetComponent<AudioSource>();
    }


    void Update() {

        //開/關 1~8F
        //if (Input.GetKeyDown(KeyCode.F1)) {
        //    f_GetGameObj("1F").SetActive( !f_GetGameObj("1F").activeInHierarchy);
        //}
        //if (Input.GetKeyDown(KeyCode.F2)) {
        //    f_GetGameObj("2F").SetActive(!f_GetGameObj("2F").activeInHierarchy);
        //}
        //if (Input.GetKeyDown(KeyCode.F3)){
        //    f_GetGameObj("3F").SetActive(!f_GetGameObj("3F").activeInHierarchy);
        //}
        //if (Input.GetKeyDown(KeyCode.F4))  {
        //    f_GetGameObj("4F").SetActive(!f_GetGameObj("4F").activeInHierarchy);
        //}
        //if (Input.GetKeyDown(KeyCode.F5))  {
        //    f_GetGameObj("5F").SetActive(!f_GetGameObj("5F").activeInHierarchy);
        //}
        //if (Input.GetKeyDown(KeyCode.F6)) {
        //    f_GetGameObj("6F").SetActive(!f_GetGameObj("6F").activeInHierarchy);
        //}
        //if (Input.GetKeyDown(KeyCode.F7)){
        //    f_GetGameObj("7F").SetActive(!f_GetGameObj("7F").activeInHierarchy);
        //}
        //if (Input.GetKeyDown(KeyCode.F8)){
        //    f_GetGameObj("8F").SetActive(!f_GetGameObj("8F").activeInHierarchy);
        //}
        //
        ////強制勝利
        //if (Input.GetKeyDown(KeyCode.F9)) {
        //    if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
        //        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Win);
        //    }
        //}
       
    }


    private void InitGameRes() {
        if (glo_Main.GetInstance() == null) {
            return;
        }
        List<NBaseSCDT> aData = glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetAll();
        for (int i = 0; i < aData.Count; i++) {
            GameControllDT tGameControllDT = (GameControllDT)aData[i];
            EM_GameControllAction tEM_GameControllAction = (EM_GameControllAction)tGameControllDT.iStartAction;
            if (tEM_GameControllAction == EM_GameControllAction.RoleCreate) {
                CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(ccMath.atoi(tGameControllDT.szData2));
                glo_Main.GetInstance().m_ResourceManager.f_CreateRole2(tCharacterDT, false);
            }
        }

        _GameControllV2.f_Init();
    }


    private void InitComponent() {
        //位置管理器
        GameObject tObj = f_GetObject("PosManager");
        if (tObj == null) {
            MessageBox.ASSERT("PosManager 组件未找到");
        }
        _PositionManager = tObj.GetComponent<PositionManager>();

        //門管理
        SaveDoor();
    }


    private void InitGame() {
        if (glo_Main.GetInstance()!=null) {
            glo_Main.GetInstance().f_Reset();
        }

        if (GloData.glo_iGameModel == 1) {
            GraphicRaycaster tGraphicRaycaster = transform.GetChild(0).GetComponent<GraphicRaycaster>();
            tGraphicRaycaster.enabled = true;
        }
        else {
            //f_GetObject("Master").SetActive(false);
        }
    }


    /// <summary>
    /// 取得玩家位置列表
    /// </summary>
    private void InitPlayerPos() {
        GameObject PlayerPos = f_GetObject("PlayerPos");
        m_aPlayerPos = new GameObject[PlayerPos.transform.childCount];
        for (int i = 0; i < PlayerPos.transform.childCount; i++){
            m_aPlayerPos[i] = PlayerPos.transform.GetChild(i).gameObject;
        }
    }


    private void InitPlayerPool(object Obj = null)  {
        if (StaticValue.m_UserDataUnit.m_PlayerDT == null) {
            ccTimeEvent.GetInstance().f_RegEvent(1, false, null, InitPlayerPool);
        }
        else {
            Data_Pool.m_PlayerPool.f_InitPlayerPool();

            //控制台狀態切換為準備
            if (Controller_bool) {
                GameReadying();
            }
        }
    }


    /// <summary>
    /// 控制台狀態切換為準備
    /// </summary>
    private void GameReadying() {
        ccGameControll.GetInstance().f_UpdateGameStatic(EM_GameState.Readying);
    }


    private int _iDDD = 0;
    public void f_Update(int iDeltaTime){

        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Waiting) {
            //if (Input.GetKeyDown(KeyCode.K)) {
            //    //UI_BtnStartGame();
            //}
        }


        else if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
            TestKey();
            _iDDD--;
            if (StaticValue.m_bIsMaster) {
                _BattleAI.f_Update();
                //_GameMission.f_CheckAllPlayerIsDie();
            }

            if (_GameControllV2 != null) {
                _GameControllV2.f_Update();
            }          
        }
    }
    #endregion


    #region 同步相关

    private bool _bIsComplete;
    public bool m_bIsComplete
    {
        get
        {
            return _bIsComplete;
        }

        set
        {
            _bIsComplete = value;
        }
    }

    #endregion



    #region 消息相关
    protected override void f_InitMessage() {
        base.f_InitMessage();
        if (glo_Main.GetInstance() == null) {
            return;
        }
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.PlayerJionGame, On_BattleUIUpdate);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.PlayerLeaveGame, On_BattleUIUpdate);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.DOORDIE, OnDoorDie, null);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.GAMEOVER, Callback_GameOver, this);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SOCKETERROR, On_SocketError, this);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.GAMESOCKETERO, On_GameSocketError, this);
    }


    private void DoDestory() {
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.PlayerJionGame, On_BattleUIUpdate);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.PlayerLeaveGame, On_BattleUIUpdate);
        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.DOORDIE, OnDoorDie, null);
        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.GAMEOVER, Callback_GameOver, this);
        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.SOCKETERROR,On_SocketError,this);
        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.GAMESOCKETERO, On_GameSocketError, this);
        glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Waiting;
        StaticValue.m_UserDataUnit.m_PlayerDT = null;
        StaticValue.m_PlayerSelTeam = GameEM.TeamType.Ero;
        StaticValue.m_PlayerSelJob = GameEM.PlayerJob.Ero;
        m_BattleRolePool.f_Clear();
        _BattleAI.f_Clear();
        Data_Pool.m_TeamPool.f_Destory();
    }



    private void On_SocketError(object obj)  {
        MessageBox.DEBUG("與網路連接錯誤");
        string strErrorInfor = (string)obj;
        //跳出斷網UI
        Canvas_Internet_WARING.SetActive(true);
    }


    private void On_GameSocketError(object obj) {
        if (_bGameSocketEor) {
            return;
        }
        MessageBox.DEBUG("與遊戲伺服器連接錯誤");
        _bGameSocketEor = true;
        string strErrorInfor = (string)obj;
        //跳出斷伺服器UI
        Canvas_Server_WARING.SetActive(true);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool f_CheckGameSocketEro() {
        return _bGameSocketEor;
    }



    private void On_BattleUIUpdate(object Obj) {
        return;

        List<PlayerDT> aData = Data_Pool.m_PlayerPool.f_GetAll();

        GameObject oPlayerList = f_GetObject("PlayerList");
        Transform tItem = oPlayerList.transform.GetChild(0);
        int iNum = aData.Count - oPlayerList.transform.childCount;
        if (oPlayerList.transform.childCount < aData.Count) {
            for (int i = 0; i < iNum; i++) {
                GameObject oNewItem = Instantiate(tItem.gameObject);
                oNewItem.transform.parent = oPlayerList.transform;
            }
        }
        for (int i = 0; i < oPlayerList.transform.childCount; i++) {
            oPlayerList.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < aData.Count; i++) {
            PlayerDT tPlayerDT = (PlayerDT)aData[i];
            tItem = oPlayerList.transform.GetChild(i);
            tItem.gameObject.SetActive(true);
            Vector3 Pos = tItem.gameObject.transform.localPosition;
            Pos.z = 0;
            tItem.gameObject.transform.localPosition = Pos;
            tItem.transform.localScale = new Vector3(1, 1, 1);
            UpdateItem(tPlayerDT, tItem);
        }
        f_GetObject("PlayerList").SetActive(false);
        f_GetObject("PlayerList").SetActive(true);
    }



    private void UpdateItem(PlayerDT tPlayerDT, Transform tItem) {
        UITools.f_SetText(tItem.gameObject, "PlayerId:" + tPlayerDT.m_iId + " Pos:" + tPlayerDT.f_GetPos());
    }


    private void OnDoorDie(object Obj) {
        MessageBox.DEBUG("OnDoorDie");

        return;

        if (StaticValue.m_bIsMaster) {
            GameSocket.GetInstance().f_GameOver();
        }

        GameEM.TeamType emTeamType = (GameEM.TeamType)Obj;
        if (StaticValue.m_UserDataUnit.m_PlayerDT.f_GetTeamType() == emTeamType) {
            MessageBox.DEBUG("Lose");
            f_GetObject("Lose").SetActive(true);
            glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Lost;
        }
        else {
            MessageBox.DEBUG("Win");
            SoundPool.GetInstance().f_PlaySound("Sound/勝利");
            f_GetObject("Win").SetActive(true);
            glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Win;
        }
        f_GetObject("BtnLeaveBattle").SetActive(true);
        //}

        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.DOORDIE, OnDoorDie, null);
    }

    #endregion



    #region 游戏控制(鍵盤測試區) ================================================================
    private int _iDoKeyDown = -99;
    private int iTestBulletId;

    /// <summary>
    /// 创建测试怪物鍵盤
    /// </summary>
    private void TestKey() {
        if (Input.GetKeyDown(KeyCode.X)) {
            //GameObject obj = f_GetGameObj("tmpPos (22)");
            //int iBulletId = 4;
            //BulletDT tBulletDT = (BulletDT)glo_Main.GetInstance().m_SC_Pool.m_BulletSC.f_GetSC(iBulletId);
            //BaseRoleControllV2 tBaseRoleControllV2 = f_GetRoleControl2(0);
            //tBulletDT.fSpeed = 0.01f;
            //BaseBullet tBaseBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(tBulletDT);
            //tBaseBullet.gameObject.transform.position = obj.transform.position;
            //tBaseBullet.transform.LookAt(tBaseRoleControllV2.transform);
            //iTestBulletId = ccMath.f_CreateKeyId();
            //tBaseBullet.f_Fired(iTestBulletId, iBulletId, GameEM.TeamType.B, 0);
            //return;
        }
        else if (Input.GetKeyDown(KeyCode.Z)) {
            //BaseBullet tBaseBullet = m_BulletPool.f_Get(iTestBulletId);
            //tBaseBullet.f_BeAttack(2);

            //CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
            //tCreateSocketBuf.f_Add(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId);
            //tCreateSocketBuf.f_Add(1);
            //tCreateSocketBuf.f_Add(1);
            //tCreateSocketBuf.f_Add(2);
            //
            //for (int i = 0; i < 16; i++)
            //{
            //    //public int m_iShot;
            //    //public int m_iShotHit;
            //    //public int m_iHeadShot;
            //    //public int m_iHeadShotDie;
            //    //public int m_iShotDie;
            //
            //    tCreateSocketBuf.f_Add(i * 10 + 0);
            //    tCreateSocketBuf.f_Add(i*10 + 1);
            //    tCreateSocketBuf.f_Add(i * 10 + 2);
            //    tCreateSocketBuf.f_Add(i * 10 + 3);
            //    tCreateSocketBuf.f_Add(i * 10 + 4);
            //    tCreateSocketBuf.f_Add(i * 10 + 5);
            //    tCreateSocketBuf.f_Add(i * 10 + 6);
            //}          
            //向验证服务器提交一份游戏结果
            //ccVerifyMain2.GetInstance().f_SubmitGameOverData(tCreateSocketBuf.f_GetBuf());
        }


        if (_iDoKeyDown == -99)
        {
            KeyCode tKeyCode = KeyCode.None;
            if (Input.GetKeyDown(KeyCode.A))
            { //A
                tKeyCode = KeyCode.A;
            }
            if (Input.GetKeyDown(KeyCode.B))
            { //B
                tKeyCode = KeyCode.B;
            }
            if (Input.GetKeyDown(KeyCode.C))
            { //C
                tKeyCode = KeyCode.C;
            }
            if (Input.GetKeyDown(KeyCode.D))
            { //D
                tKeyCode = KeyCode.D;
            }
            if (Input.GetKeyDown(KeyCode.E))
            { //E
                tKeyCode = KeyCode.E;
            }
            if (Input.GetKeyDown(KeyCode.I))
            { //I
                tKeyCode = KeyCode.I;
            }
            if (Input.GetKeyDown(KeyCode.J))
            { //J
                tKeyCode = KeyCode.J;
            }
            if (Input.GetKeyDown(KeyCode.M))
            { //M
                tKeyCode = KeyCode.M;
            }
            if (Input.GetKeyDown(KeyCode.N))
            { //N
                tKeyCode = KeyCode.N;
            }
            if (Input.GetKeyDown(KeyCode.O))
            { //O
                tKeyCode = KeyCode.O;
            }
            if (Input.GetKeyDown(KeyCode.P))
            { //P
                tKeyCode = KeyCode.P;
            }
            if (Input.GetKeyDown(KeyCode.S))
            { //S
                tKeyCode = KeyCode.S;
            }
            if (Input.GetKeyDown(KeyCode.T))
            { //T
                tKeyCode = KeyCode.T;
            }
            if (Input.GetKeyDown(KeyCode.X))
            { //X
                tKeyCode = KeyCode.X;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            { //Z
                tKeyCode = KeyCode.Z;
            }
            if (Input.GetKeyDown(KeyCode.K))
            { //K
                tKeyCode = KeyCode.K;
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            { //右邊數字1
                tKeyCode = KeyCode.Keypad1;
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            { //右邊數字2
                tKeyCode = KeyCode.Keypad2;
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            { //右邊數字3
                tKeyCode = KeyCode.Keypad3;
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            { //右邊數字4
                tKeyCode = KeyCode.Keypad4;
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            { //右邊數字5
                tKeyCode = KeyCode.Keypad5;
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            { //右邊數字6
                tKeyCode = KeyCode.Keypad6;
            }
            if (Input.GetKeyDown(KeyCode.Keypad7))
            { //右邊數字7
                tKeyCode = KeyCode.Keypad7;
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            { //右邊數字8
                tKeyCode = KeyCode.Keypad8;
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            { //右邊數字9
                tKeyCode = KeyCode.Keypad9;
            }
            if (tKeyCode != KeyCode.None)
            {
                _iDoKeyDown = ccTimeEvent.GetInstance().f_RegEvent(0.5f, true, tKeyCode, CallBack_TestKey);
            }
        }
    }

    private void CallBack_TestKey(object Obj) {
        KeyCode tKeyCode = (KeyCode)Obj;
        if (tKeyCode == KeyCode.O) {
           
        }
        ccTimeEvent.GetInstance().f_UnRegEvent(_iDoKeyDown);
        _iDoKeyDown = -99;
    }


    /// <summary>
    /// 管理員開始遊戲
    /// </summary>
    /// <param name="Obj"></param>
    private void UI_BtnStartGame() {
        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Waiting) {
            f_StartGame();
        }
        else {
            //f_EndGame();
        }
    }


    /// <summary>
    /// 接收到 SDK 过来的开始游戏命令后，主动向同步服务器发送开始游戏指令
    /// </summary>
    public void f_StartGame() {

        if (StaticValue.m_bIsMaster) {

            if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Waiting) {
                GameSocket.GetInstance().f_StartPlayGame();
                UITools.f_SetText(f_GetObject("BtnStartGame"), "StopGame");
            }
            else  {
                MessageBox.ASSERT("游戏状态错误不能执行开始游戏");
            }
        }
    }


    /// <summary>
    /// 结束游戏命令
    /// </summary>
    private void Callback_GameOver(object Obj)
    {
        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming)
        {
            if (StaticValue.m_bIsMaster)
            {
                //Data_Pool.m_PlayerPool.f_GameOver();
                int iGameTime = (int)(System.DateTime.Now - m_tStartTimeTime).TotalSeconds;
                int tEM_GameResult = (int)Obj;
                _GameMission.f_Reset();
                glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Waiting;
                List<PlayerDT> aPlayer = Data_Pool.m_PlayerPool.f_GetAll();

                CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
                tCreateSocketBuf.f_Add(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId);
                tCreateSocketBuf.f_Add(tEM_GameResult);
                tCreateSocketBuf.f_Add(iGameTime);
                tCreateSocketBuf.f_Add(aPlayer.Count);
                
                for(int i = 0; i < aPlayer.Count; i++) {
                    PlayerScorePool tPlayerScorePool = aPlayer[i].m_PlayerScorePool;
                    tCreateSocketBuf.f_Add(aPlayer[i].m_iId);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iShot);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iShotHit);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iHeadShot);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iHeadShotDie);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iShotDie);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iDie);
                }
                for (int i = aPlayer.Count; i < 16; i++) {
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                }

                GameSocket.GetInstance().f_SendBuf2Force((int)SocketCommand.CTS_GameOver, tCreateSocketBuf.f_GetBuf());

            }
            MissionLog.f_Out();
        }

    }



    /// <summary>
    /// 正式开始游戏 (按下 3v3GameServer 開始按鈕後執行的地方)
    /// </summary>
    /// <param name="tCTS_StartGame"></param>
    public void On_Start(CTS_StartGame tCTS_StartGame) {

        //驗證：開始遊戲
        #if Verify
        VerityTools.f_StartGame();
        MessageBox.DEBUG("[驗證] 遊戲開始");
        #endif

        //通知控制台当前游戏的状态为进行游戏中
        if (Controller_bool) {
            ccGameControll.GetInstance().f_UpdateGameStatic(EM_GameState.Gaming);
        }

        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
            return;
        }       
        glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Gaming;
        _BattleAI.f_Start();
        _GameControllV2.f_Start();

        MessageBox.DEBUG("戰鬥開始");
        m_tStartTimeTime = System.DateTime.Now;
    }




    /// <summary>
    /// 游戏结束返回大厅
    /// </summary>
    /// <param name="tCTS_GameOver"></param>
    public void On_GameOver(CTS_GameOver tCTS_GameOver) {

        if (_EM_GameResult != EM_GameResult.Default) {
            return;
        }

        //驗證：遊戲結束
        #if Verify
        VerityTools.f_StopGame();
        MessageBox.DEBUG("[驗證] 遊戲結束");
        #endif

            
        //通知控制台当前游戏的状态为进行结束状态
        if (Controller_bool) {
            ccGameControll.GetInstance().f_UpdateGameStatic(EM_GameState.GameOver);
        }


        glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Waiting;
        _BattleAI.f_Stop();
        _GameControllV2.f_Stop();
        _GameMission.f_Reset();

        _EM_GameResult = (EM_GameResult) tCTS_GameOver.m_iGameResult;
        if (_EM_GameResult == EM_GameResult.Win) {
            OpenUI_Win();
        }
        else if (_EM_GameResult == EM_GameResult.Lost) {
            MySelfPlayerControll2.staticMySelfPlayerControll2._Lutify.Blend = 1;
            MySelfPlayerControll2.staticMySelfPlayerControll2.DeathreciprocalGameObject.gameObject.SetActive(false);
            OpenUI_Lost();
        }

        //排行榜 (只在勝利時打開)
        if (_EM_GameResult == EM_GameResult.Win) {
            ScoreRank.GetInstance().Receive_GameOverData(tCTS_GameOver);
        }
        
        MessageBox.DEBUG("游戏结束， " + _EM_GameResult.ToString());
        MessageBox.DEBUG("游戏结束返回大厅");
        //ccTimeEvent.GetInstance().f_RegEvent(15, false, null, QuitGame);
    }


    void QuitGame(object Obj) {
        MessageBox.DEBUG("强制结束游戏");
        glo_Main.GetInstance().f_Destroy();
    }


    
    /// <summary>
    /// 显示胜利UI
    /// </summary>
    public void OpenUI_Win() {
        if (ui_Win.Length > 0) {
            for (int i = 0; i < ui_Win.Length; i++) {
                ui_Win[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// 显示失败UI
    /// </summary>
    public void OpenUI_Lost() {
        if (ui_Lost.Length > 0) {
            for (int i = 0; i < ui_Lost.Length; i++) {
                ui_Lost[i].SetActive(true);
            }
        }
    }


    public void f_RegRoleDieMission(BaseRoleControllV2 tBaseRoleControl, EM_GameResult tEM_GameResult) {
        _GameMission.f_RegRole(tBaseRoleControl, tEM_GameResult);
    }
#endregion


    #region 玩家控制 ============================================================================
    /// <summary>
    /// 取得玩家出生位置
    /// </summary>
    public GameObject f_GetPlayerPos(PlayerDT tPlayerDT) {
        int iPos = tPlayerDT.f_GetPos();
        if (iPos > m_aPlayerPos.Length) {
            MessageBox.DEBUG("场景里没有空余位置。");
            return null;
        }
        return m_aPlayerPos[iPos];
    }
    #endregion



    #region 玩家及其它角色相關 ==================================================================
    /// <summary>
    /// 創建 MySelfPlayer
    /// </summary>
    /// <param name="tPlayerDT"></param>
    public void f_CreateMyselfPlayer(PlayerDT tPlayerDT) {
        GameObject Obj = BattleMain.GetInstance().f_GetPlayerPos(tPlayerDT);
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(Obj.transform.position);
        MySelfPlayerControll2 tMySelfPlayerControll2 = m_oMySelfPlayer.GetComponent<MySelfPlayerControll2>();
        tPlayerDT.f_SetPlayerInfor(tMySelfPlayerControll2, null);
        CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(1000);
        tMySelfPlayerControll2.f_Init(tPlayerDT.m_iId, new PlayerActionController(tMySelfPlayerControll2), tPlayerDT.f_GetTeamType(), tCharacterDT, tTileNode, tPlayerDT.f_GetHeight());
        tMySelfPlayerControll2.f_SetPos(new Vector3(Obj.transform.position.x, GloData.glo_fPlayerDefaultY, Obj.transform.position.z));
        m_BattleRolePool.f_Save(tMySelfPlayerControll2);
    }

    /// <summary>
    /// 創建 OtherPlayer
    /// </summary>
    /// <param name="tPlayerDT"></param>
    public void f_CreateOtherPlayer(PlayerDT tPlayerDT)  {
        if (f_GetRoleControl2(tPlayerDT.m_iId) != null) {
            return;
        }
        GameObject Obj = BattleMain.GetInstance().f_GetPlayerPos(tPlayerDT);
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(Obj.transform.position);
        OtherPlayerControll2 tOtherPlayerControll2 = glo_Main.GetInstance().m_ResourceManager.f_CreateOtherPlayer(tPlayerDT.f_GetHeight());
        tPlayerDT.f_SetPlayerInfor(tOtherPlayerControll2, null);
        CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(1000);
        tOtherPlayerControll2.f_Init(tPlayerDT.m_iId, new PlayerActionController(tOtherPlayerControll2), tPlayerDT.f_GetTeamType(), tCharacterDT, tTileNode, tPlayerDT.f_GetHeight());
        tOtherPlayerControll2.transform.position = new Vector3(Obj.transform.localPosition.x, GloData.glo_fPlayerDefaultY, Obj.transform.localPosition.z);
        m_BattleRolePool.f_Save(tOtherPlayerControll2);
    }
    

    /// <summary>
    /// 將角色加進角色列表
    /// </summary>
    /// <param name="tRoleControl"> 角色 </param>
    public void f_SaveRole(BaseRoleControllV2 tRoleControl) {
        if (tRoleControl != null) {
            m_BattleRolePool.f_Save(tRoleControl);
        }
        else {
            MessageBox.ASSERT("f_SaveRole == null");
        }
    }

    public void f_DestoryOtherPlayer(PlayerDT tPlayerDT) {

    }
    #endregion



    #region 老版本功能

    public BaseRoleControllV2 f_GetRoleControl2(int iRoleKeyId) {
        return m_BattleRolePool.f_Get(iRoleKeyId);
    }
    

    /// <summary>
    /// 角色死亡
    /// </summary>
    /// <param name="tRoleControl"></param>
    public void f_RoleDie2(BaseRoleControllV2 tRoleControl) {
        //從角色列表中移除
        m_BattleRolePool.f_Die(tRoleControl);

        //檢查這個角色的死亡與遊戲勝利或失敗是否有關連
        if (StaticValue.m_bIsMaster)  {
            _GameMission.f_RoleDie(tRoleControl);
        }
    }
    #endregion


    #region 资源接口----------------------------------------------------------
    private void CreateRole(int iId) {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.A, iId, 1126);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreatePlayerMoster(int iId) {
        GameObject TTT = f_GetGameObj("TTT");
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(TTT.transform.position);
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.A, iId, tTileNode.idx);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }


    private void CreateComputerMonter(int iId) {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.Computer, iId, 396);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    public void CreateComputerMonter2(int iId) {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.Computer, iId, 425);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreatePlayerMonter(int iId) {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.A, iId, 396);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    public void f_CreateMonster(int iUserId, int iCharacterDTId, int iTileNodeId) {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.Computer, iCharacterDTId, iTileNodeId);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }


    /// <summary>
    /// 玩家自己创建怪物
    /// </summary>
    /// <param name="tCharacterDT"></param>
    /// <param name="Pos"></param>
    public void f_CreateRole(int iUserId, CharacterDT tCharacterDT, Vector3 Pos) {
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(Pos);
        f_CreateMonster(iUserId, tCharacterDT.iId, tTileNode.idx);
    }
    #endregion



    #region 房間門項目 --------------------------------------------------

    /// <summary>
    /// 門字典取得門資訊
    /// </summary>
    private void SaveDoor() {
        for (int i = 0; i < Door.Length; i++) {
            if (_dicDoor.ContainsKey(Door[i].name)) {
                MessageBox.ASSERT("BattleMain 的門清單裡有相同名字的門：" + Door[i].name);
            }
            else {
                _dicDoor.Add(Door[i].name, Door[i].GetComponent<DoorAnimation>());
            }
        }
    }


    /// <summary>
    /// 獲取指定名稱的門
    /// </summary>
    /// <param name="index"> 要取得的物品名稱 </param>
    public GameObject GetDoor (string tmpName){
        for (int i = 0; i < Door.Length; i++) {
            if (Door[i].name == tmpName){
                return Door[i].gameObject;
            }
        }
        return null;
    }


    /// <summary>
    /// 獲取指定名稱的門 (用字典取得)
    /// </summary>
    /// <param name="tmpName"> 要取得的門的名稱 </param>
    /// <returns></returns>
    public DoorAnimation GetDoor2(string tmpName) {
        if (_dicDoor.ContainsKey(tmpName)) {
            return _dicDoor[tmpName];
        }
        return null;
    }
    #endregion




    //---------------------------------------------------------

    /// <summary>
    /// "FPS:" + iData + " R:" + tGameSyscStatic.m_iRecvAction + " S:" + tGameSyscStatic.m_iSendAction + " C:" + tGameSyscStatic.m_fCurFrameTime + " N:" + tGameSyscStatic.m_iNetWorkTime;
    /// </summary>
    public void f_UpdateFps(string ppSQL) {
        //MessageBox.DEBUG(ppSQL);
        //Text tText = f_GetObject("FpsText").GetComponent<Text>();
        //tText.text = ppSQL;
        //Text tText2 = f_GetObject("FpsText2").GetComponent<Text>();
        //tText2.text = ppSQL;
    }


    /// <summary>
    /// 其它玩家的坐标信息更新动作
    /// </summary>
    /// <param name="tPlayerTransformAction"></param>
    public void f_UpdatePlayerAction(PlayerTransformAction tPlayerTransformAction) {
        if (tPlayerTransformAction.m_iUserId == StaticValue.m_UserDataUnit.m_PlayerDT.m_iId) {
            return;
        }
        PlayerDT tPlayerDT = Data_Pool.m_PlayerPool.f_GetPlayer(tPlayerTransformAction.m_iUserId);
        if (tPlayerDT != null) {
            ((OtherPlayerControll2)tPlayerDT.m_PlayerControll).f_UpdatePlayerTransform(tPlayerTransformAction);
        }
    }


    #region UI相关

    /// <summary>
    /// 显示UI文字
    /// </summary>
    /// <param name="aPlayer"></param>
    /// <param name="strText"></param>
    /// <param name="fTime"></param>
    public void f_ShowUIText(string strText, float fTime) {
        GameObject tUIText = f_GetObject("UIText");
        tUIText.SetActive(true);
        Text tText = tUIText.GetComponent<Text>();
        tText.text = strText;

        ccTimeEvent.GetInstance().f_RegEvent(fTime, false, tUIText, Callback_CloseUIText);
    }

    private void Callback_CloseUIText(object Obj){
        GameObject tUIText = (GameObject)Obj;
        tUIText.SetActive(false);
    }

    /// <summary>
    /// 打开显示UI组件
    /// </summary>
    /// <param name="aPlayer"></param>
    /// <param name="strText"></param>
    /// <param name="fTime"></param>
    public void f_UIActionShow(string strUIAction, float fTime) {
        GameObject tUIAction = f_GetObject(strUIAction);
        tUIAction.SetActive(true);
        ccTimeEvent.GetInstance().f_RegEvent(fTime, false, tUIAction, Callback_CloseUIActionShow);
    }

    private void Callback_CloseUIActionShow(object Obj) {
        GameObject tUIAction = (GameObject)(Obj);
        tUIAction.SetActive(false);
    }



    /// <summary>
    /// 場景淡入淡出
    /// </summary>
    /// <param name="value"> 透明度 </param>
    /// <param name="time" > 淡入淡出花費的時間 </param>
    public void SceneFadeTo(int value, float time = 0.5f) {
        UI_SceneFade.DOFade(value, time);
        if (_audioSource == null) {
            return;
        }
        if (_FadeClip != null && value == 1) {
            _audioSource.PlayOneShot(_FadeClip, 1);
        }
    }

    #endregion


    #region 任务公共相关接口

    /// <summary>
    /// (接口) 同步執行 GameControl 動作
    /// </summary>
    public void f_RunServerActionState(int iConditionId, int iId) {
        GCA_reserveList.Remove(iId);
        GCC_reserveList.Remove(iConditionId);
        _GameControllV2.f_RunServerActionState(iConditionId, iId);
    }



    /// <summary>
    /// (接口) 同步執行 GameControl 動作
    /// </summary>
    public void f_Force_RunServerActionState(int iConditionId, int iId) {
        GCA_reserveList.Remove(iId);
        GCC_reserveList.Remove(iConditionId);
        GameControll tmp = new GameControll(iConditionId);
        tmp.f_RunServerActionState(iId);
    }




    /// <summary>
    /// 取得參數值
    /// </summary>
    /// <param name="strParament"> 要取得的參數名稱 </param>
    public string f_GetParamentData(string strParament){
        return _GameControllV2.f_GetParamentData(strParament);
    }

    /// <summary>
    /// 設定參數值
    /// </summary>
    /// <param name="strParament"> 要設定的參數名稱 </param>
    /// <param name="strData"    > 新參數值 </param>
    public void f_SetParamentData(string strParament, string strData){
        _GameControllV2.f_SetParamentData(strParament, strData);
    }


    /// <summary>
    /// 強制再次讀取沒執行到的腳本
    /// </summary>
    public void f_ForceGameControlRun() {
        for (int i=0; i< GCA_reserveList.Count; i++) {
            if (GCA_reserveList.Count-1 >= i) {
                //GameControllThread tmp = new GameControllThread(GCC_reserveList[i], GCA_reserveList[i]);
                //tmp.f_Start();

                //GameControllRead tmp = new GameControllRead(GCC_reserveList[i]);
                //tmp.f_Enter(GCA_reserveList[i]);

                Force_ServerActionState tServerActionState = new Force_ServerActionState();
                tServerActionState.f_Save(GCC_reserveList[i], GCA_reserveList[i]);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tServerActionState);
                Debug.LogWarning("嘗試執行手動觸發：網路漏執行的腳本[" + GCA_reserveList[i] + "] (這訊息不代表一定執行成功)");
            }
        }
    }

    #endregion


    #region BattleMain场景物件相关

    public GameObject f_GetGameObj(string strName){
        GameObject oGameObj = f_GetObject(strName);
        if (oGameObj == null) {
            oGameObj = _PositionManager.f_GetPosManagerObject(strName);
            if (oGameObj != null) {
                return oGameObj;
            } else {
                if (Debug_Object_isNull) {
                    Debug.LogWarning("【警告】游戏场景里的对象未找到！" + strName);
                }
            }
        }
        return oGameObj;
    }
       

    public void f_SetChildForGameObj(GameObject Obj) {
        Transform oGameObj = f_GetObject("PosManager").transform;
        Obj.transform.parent = oGameObj;
    }

    #endregion




    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"> 檔案路徑(含檔名與.txt) </param>
    /// <param name="tmp"     > 參考字 </param>
    /// <param name="trueValue"   > 決定項 </param>
    bool CheckOutputDebug(string filePath, string tmp, string trueValue, bool defaultValue = false) {
        if (System.IO.File.Exists(filePath)) {
            string[] strs = System.IO.File.ReadAllLines(filePath);
            for (int i = 0; i < strs.Length; i++) {
                if (strs[i].Contains(tmp)) {
                    if (f_GetSplitTextIndex(strs[i], "：", 1) == trueValue) {
                        return true;
                    }
                    else {
                        continue;
                    }
                }
            }
        }
        //找不到文檔預設關閉不輸出
        return defaultValue;
    }


    /// <summary>
    /// 取得某段文字以特殊字元拆分後 第 Index項的內容
    /// </summary>
    /// <param name="tmp"> 要分析的內容(string) </param>
    /// <param name="value" > 特殊字元 </param>
    /// <param name="index" > 拆分後的第幾項 </param>
    public string f_GetSplitTextIndex(string tmp, string value, int index) {
        string[] tmpContent = Regex.Split(tmp, value, RegexOptions.IgnoreCase); //根據換行符劃分出多個行文本
        if (tmpContent.Length == 1) {
            return "-99";
        }
        return tmpContent[index];                                                   //回傳解析結果
    }

}


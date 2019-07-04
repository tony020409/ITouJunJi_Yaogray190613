using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ccVerifySDK;
using y_Network.Launcher;
using ccGameControllSDK;
using System.Text.RegularExpressions;

/*
//协议操作结果
    public enum eVerifyMsgOperateResult
    {

        /// <summary>
        /// 操作成功
        /// </summary>
        OR_Succeed = 0, // 成功

        OR_LoginEro = 1,
        /// <summary>
        /// 帐号失效
        /// </summary>
        OR_AccountDisable,

        /// <summary>
        /// 帐号错误
        /// </summary>
        OR_AccountEro,

        /// <summary>
        /// 登陆DLL失败
        /// </summary>
        OR_LoginDLLEro,

        /// <summary>
        /// 未登陆
        /// </summary>
        OR_NoLogin = 5,

        /// <summary>
        /// 电脑硬件信息错误
        /// </summary>
        OR_PcError,

        /// <summary>
        /// 电脑信息已经注册
        /// </summary>
        OR_PcIsRegedited,

        /// <summary>
        /// 未登陆注册电脑信息失败
        /// </summary>
        OR_LoginEroPcError,

        /// <summary>
        /// 数据验证失败
        /// </summary>
        OR_DataVerifyFail,

        /// <summary>
        /// 电脑信息未注册，注册电脑信息
        /// </summary>
        OR_NeedRegPcInfor = 10,

        /// <summary>
        /// 游戏次数已使用完
        /// </summary>
        OR_GameTimesIsLimit,

        /// <summary>
        /// 未找到游戏信息
        /// </summary>
        OR_NoFindGameInfor = 12,

        /// <summary>
        /// 未找到电脑信息
        /// </summary>
        OR_NoFindPcInfor,

        /// <summary>
        /// 授权电脑已满
        /// </summary>
        OR_RegeditPcFull = 14,

        /// <summary>
        /// 授权模式错误
        /// </summary>
        OR_GameTimesNoName,

        /// <summary>
        /// 与授权游戏不一致
        /// </summary>
        OR_GameVerifyGameIdEro,

        //客户端专用提示
        OR_Error_WIFIConnectTimeOut = 993, //WIFI网络未开
        OR_Error_ConnectTimeOut = 994, //连接超时
        OR_Error_CreateAccountTimeOut = 995, //注册超时
        OR_Error_LoginTimeOut = 996, //登陆超时
        OR_Error_ExitGame = 997, //游戏出错，强制玩家离开
        OR_Error_ServerOffLine = 998, //服务器未开启
        OR_Error_Disconnect = 999, //游戏断开连接
        OR_Error_Default = 10000, //操作失败

    }
*/

public class ccVerifyMain2 : UIFramwork
{
    Text _StaticText;
    [Rename("遊戲id")]
    public int _iGameId;

    [Rename("遊戲id欄位")]
    public InputField IdField;

    /// <summary>
    /// 
    /// </summary>
    public int iEventId;

    [Rename("帳號輸入欄")]
    public InputField Account;

    [Rename("密碼輸入欄")]
    public InputField Password;

    [Rename("下方小圓點")]
    public Text _GloMainStaticText;

    [Rename("自動登入")]
    public bool useAutologin;

    [Rename("是否啟用控制台")]
    public bool Controller_bool;

    /// <summary>
    /// 登入是否有回調
    /// </summary>
    private bool isLoginRet_callback = false;

    /// <summary>
    /// 註冊時間
    /// </summary>
    private int Regedit_time;


    /// <summary>
    /// 要進入的場景名稱 
    /// </summary>
    public string game_scene = "BattleMain";


    private static ccVerifyMain2 _Instance = null;
    public static ccVerifyMain2 GetInstance() {
        return _Instance;
    }


    protected override void f_CustomAwake()  {
        base.f_CustomAwake();

        //修改遊戲id
        IdField.text = _iGameId.ToString();

        //決定是否啟用控制台
        string ConfigDebugFilePath = Application.streamingAssetsPath + "/" + "Config_GameControl.txt"; //外部文檔路徑
        Controller_bool = CheckOutputDebug(ConfigDebugFilePath, "使用控制台", "1");


        ccVerityService.GetInstance();
        _Instance = this;
        MessageBox.f_OpenLog();
    }

   

    #region UI消息
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        
        //開關 UI
        _StaticText = f_GetObject("StaticText").GetComponent<Text>();
        f_GetObject("VerifyPcGameBtn").SetActive(false);
        f_GetObject("RegeditPcGameBtn").SetActive(false);
        f_GetObject("StarGameBtn").SetActive(false);
        f_GetObject("StopGameBtn").SetActive(false);

        //註冊按鈕事件
        f_RegClickEvent("LoginBtn", On_LoginBtn);
        f_RegClickEvent("RegeditPcGameBtn", On_RegeditPcGameBtn);
        f_RegClickEvent("StarGameBtn", On_StarGameBtn);
        f_RegClickEvent("StopGameBtn", On_StopGameBtn);
        f_RegClickEvent("GetGamesBtn", On_GetGamesBtn);
        f_RegClickEvent("CheckCanStartGameBtn", On_CheckCanStartGameBtn);
        f_RegClickEvent("GetPcName", On_GetPcName);

        Account.text = PlayerPrefs.GetString("Account");
        Password.text = PlayerPrefs.GetString("Password");

        //控制台
        if (Controller_bool) {
            glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_RESOURCECOMPLETE, On_LoadVerityInfor);
            InvokeRepeating("On_LoginBtn", 3, 5);
        }
        else {
            Account.text  = PlayerPrefs.GetString("帳號");
            Password.text = PlayerPrefs.GetString("密碼");
        }

        //自動登入
        if (useAutologin && !Controller_bool) {
            StartCoroutine("Autologin");
        }


    }
    #endregion


    /// <summary>
    /// 讀取記錄過的帳號密碼
    /// </summary>
    private void On_LoadVerityInfor(object Obj) {
        LoadAccountPassword();
    }


    /// <summary>
    /// 自動登入
    /// </summary>
    IEnumerator Autologin() {
        yield return new WaitForSeconds(5);
        On_LoginBtn();
    }


    /// <summary>
    /// 登入
    /// </summary>
    public void On_LoginBtn() {
        Log("Login");
        if (CheckAndUdpateGameId()) {
            Text LoginName = f_GetObject("LoginName").transform.Find("Text").GetComponent<Text>();
            Text LoginPwd  = f_GetObject("LoginPwd").transform.Find("Text").GetComponent<Text>();
            if (LoginName.text != "" && LoginPwd.text != "") {
                ccVerityService.GetInstance().f_Login(10, Account.text, Password.text, GloData.glo_iGameId, Callback_LoginRet);
            }
            else {
                if (Controller_bool) {
                    ccGameControll.GetInstance().f_UpdateGameStatic(EM_GameState.VerifyPwdError);
                    CancelInvoke("On_LoginBtn");
                }
            }
            iEventId = ccTimeEvent.GetInstance().f_RegEvent(10, true, null, Login_Over);
        }
    }


    /// <summary>
    /// 登入超時，重新執行登入
    /// </summary>
    public void Login_Over(object obj) {
        Log("登入超時，重新登入");
        On_LoginBtn();
    }


    /// <summary>
    /// 登陆认证系统回调参考实例,
    /// </summary>
    /// <param name="Obj"></param>
    private void Callback_LoginRet(eVerifyMsgOperateResult teMsgOperateResult) {

        Debug.Log("登入結束結果 + " + teMsgOperateResult);

        //登入成功
        if (teMsgOperateResult == eVerifyMsgOperateResult.OR_Succeed) {
            Log("可以开始游戏");

            // UI 開或關
            f_GetObject("LoginPanel").SetActive(false);
            f_GetObject("StarGameBtn").SetActive(true);
            f_GetObject("GetGamesBtn").SetActive(true);
            f_GetObject("CheckCanStartGameBtn").SetActive(true);
            f_GetObject("GetPcName").SetActive(true);
            f_GetObject("StopGameBtn").SetActive(true);
            f_GetObject("VerifyPcGameBtn").SetActive(false);
            f_GetObject("RegeditPcGameBtn").SetActive(false);


            //自動確認是否可開始遊戲
            On_CheckCanStartGameBtn();

            //登入成功後，記錄使用的帳號密碼
            SaveAccountPassword();

            //確定有回傳
            //isLoginRet_callback = true;

        }

        //未找到遊戲訊息
        else if (teMsgOperateResult == eVerifyMsgOperateResult.OR_NoFindGameInfor) {

            if (Controller_bool) {
                //停止自動登入帳號
                CancelInvoke("On_LoginBtn");
                ccGameControll.GetInstance().f_SendErroInfor(EM_ErrorType.VerifyError, ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult));
            }
            Log("未找到遊戲信息");
        }

        //未找到電腦訊息
        else if (teMsgOperateResult == eVerifyMsgOperateResult.OR_NoFindPcInfor) {

            f_GetObject("LoginPanel").SetActive(false);
            f_GetObject("RegeditPcGameBtn").SetActive(true);
            f_GetObject("VerifyPcGameBtn").SetActive(false);

            //自動註冊
            On_RegeditPcGameBtn();

            //如果使用控制台
            if (Controller_bool) {
                //第二次檢查到未註冊 送出驗證失敗訊息
                if (Regedit_time >= 1) {
                    ccGameControll.GetInstance().f_SendErroInfor(EM_ErrorType.VerifyError, ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult));
                }

                //停止自動登入帳號
                CancelInvoke("On_LoginBtn");
                Regedit_time++;
            }

            Debug.Log("未找到遊戲訊息");
            Log(ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult) + " (未找到遊戲訊息)");
        }

        //電腦註冊已滿
        else if (teMsgOperateResult == eVerifyMsgOperateResult.OR_RegeditPcFull) {

            f_GetObject("LoginPanel").SetActive(false);
            f_GetObject("RegeditPcGameBtn").SetActive(true);
            f_GetObject("VerifyPcGameBtn").SetActive(false);

            if (Controller_bool) {
                ccGameControll.GetInstance().f_SendErroInfor(EM_ErrorType.VerifyError, ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult));

                //停止自動登入帳號
                CancelInvoke("On_LoginBtn");
            }

            Debug.Log("電腦註冊已滿");
            Log(ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult) + " (電腦註冊已滿)");
        }


        else {
            if (Controller_bool) {
                ccGameControll.GetInstance().f_SendErroInfor(EM_ErrorType.VerifyError, ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult));
            }

            Log("登入失敗 " + teMsgOperateResult.ToString());
            ccVerityService.GetInstance().f_Close();
        }


        //確定有回傳
        ccTimeEvent.GetInstance().f_UnRegEvent(iEventId);


    }

    /// <summary>
    /// 記錄使用的帳號密碼
    /// </summary>
    private void SaveAccountPassword() {

        //使用控制台
        if (Controller_bool) {
            //从控制台SDK读取帐号密码，不再需要本地保存
        }

        //不使用控制台時，記錄使用的帳號密碼
        else {
            PlayerPrefs.SetString("帳號", Account.text);
            PlayerPrefs.SetString("密碼", Password.text);
        }

    }

    public void On_GetPcName() {
        Log("On_GetPcName");
        f_GetPcName(Callback_VerifyPcInfor);
    }

    public void On_RegeditPcGameBtn() {
        Log("On_RegeditPcGameBtn");
        ccVerityService.GetInstance().f_RegPcInfor(Callback_LoginRet);
    }

    public void On_CheckCanStartGameBtn() {
        Log("On_CheckCanStartGameBtn");
        ccVerityService.GetInstance().f_CheckCanStartGame(Callback_CheckCanStartGame);
    }

    private void Callback_CheckCanStartGame(eVerifyMsgOperateResult teMsgOperateResult) {
        if (teMsgOperateResult == eVerifyMsgOperateResult.OR_Succeed) {
            Log("讀取場景中...");
            MessageBox.DEBUG("ccVerityService 可以開始遊戲");
            Connect();
        }
        else {
            Log("不能開始遊戲" + teMsgOperateResult.ToString());
        }

    }



    #region 登入流程 (原本寫在 Launch.cs)

    public void Connect() {
        MessageBox.DEBUG("Connect");
        GameSocket.GetInstance().f_Login(Callback_LoginSuc);
        ControllSocket.GetInstance().f_Login(Callback_GameControllLoginSuc);
    }

    private void Callback_GameControllLoginSuc(object Obj) {
        MessageBox.DEBUG("GameControll登陆");
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult) Obj;
        if (teMsgOperateResult == (int)eMsgOperateResult.OR_Succeed) {
            MessageBox.DEBUG("GameControll登陆成功");
        }
        else {
            MessageBox.DEBUG("登陆失败 " + teMsgOperateResult.ToString());
        }

    }


    private void Callback_LoginSuc(object Obj) {
        MessageBox.DEBUG("GameStep登陆");
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)Obj;
        if (teMsgOperateResult == (int)eMsgOperateResult.OR_Succeed) {
            MessageBox.DEBUG("GameStep登陆成功");
            if (GloData.glo_iGameModel == 1) {
                StaticValue.m_bIsMaster = true;
            }
            else {
                StaticValue.m_bIsMaster = false;
            }

            //進入 BattleMain 場景
            SceneManager.LoadScene(GameEM.GameScene.BattleMain.ToString());
        }
        else {
            MessageBox.DEBUG("登陆失败 " + teMsgOperateResult.ToString());
        }
    }



    /// <summary>
    /// glo_Main 讀取完畢的提示 (給 glo_Main.cs 的 Callback_LoadResSuc 調用 )
    /// </summary>
    public void f_Show_GloMainStatic() {
        _GloMainStaticText.color = Color.green;
    }

    #endregion


    void Update() {
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.Alpha0)) {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.DeleteAll();
            Account.text = "";
            Password.text = "";
            Log("帳密記憶已清除");
        }
    }


    private void Log(string strText) {
        if (_StaticText != null) {
            _StaticText.text = strText;
        }
        MessageBox.DEBUG(strText);
    }



    #region 測試區
    [HideInInspector]
    public int testa = 0;

    /// <summary>
    /// 測試按鍵：開始遊戲
    /// </summary>
    public void On_StarGameBtn() {
        Log("On_StarGameBtn");
        testa = Random.Range(0, 999999);
        ccVerityService.GetInstance().f_StartGame(testa.ToString("000000"));
    }


    /// <summary>
    /// 測試按鍵：結束遊戲
    /// </summary>
    public void On_StopGameBtn() {
        ccVerityService.GetInstance().f_StopGame(testa.ToString("000000"));
    }


    /// <summary>
    /// 測試按鍵：計次(?)
    /// </summary>
    public void On_GameTimeGameBtn() {
        ccVerityService.GetInstance().f_SubmitData(testa.ToString("000000"), 5, Callback_Submit);
    }

    private void Callback_Submit(eVerifyMsgOperateResult teMsgOperateResult, string strKey = "") {
        //服务器确认数据后返回的提交数据的KEY
        if (teMsgOperateResult != eVerifyMsgOperateResult.OR_Succeed) {
            Log("SubmitData 游戏开始数据提交失败 " + ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult));
        }
        else {
            Log("数据提交成功 " + ccVerityService.GetInstance().f_GetSubmitDataCount());
            //ccVerityService.GetInstance().f_StopGame(Callback_StopReturn);
        }        
    }
          

    public void On_GetGamesBtn() {
        Log("On_GetGamesBtn");
        ccVerityService.GetInstance().f_GetGamePlayTimes(Callback_GetGamePlayTimes);
    }

    private void Callback_GetGamePlayTimes(object Obj) {
        int iTimes = (int)Obj;
        Log("电脑可玩次数 " + iTimes);
    }



    /// <summary>
    /// 測試按鍵：開始+計次+結束
    /// </summary>
    public void test_verify_button() {
        StartCoroutine(MyCoroutineFunction2());
    }
    IEnumerator MyCoroutineFunction2() {
        for (var i = 0; i < 1000; i++) {
            On_StarGameBtn();
            Debug.Log("開始");
            yield return new WaitForSeconds(0.5f);
            On_GameTimeGameBtn();
            Debug.Log("計次");
            yield return new WaitForSeconds(0.5f);
            On_StopGameBtn();
            Debug.Log("結束");
            yield return new WaitForSeconds(0.5f);
        }

    }
    #endregion


    #region 更新GameId
    /// <summary>
    /// 取得遊戲id
    /// </summary>
    /// <returns></returns>
    private bool CheckAndUdpateGameId() {

        if (f_GetObject("GameId") != null) {
            Text GameIdText = f_GetObject("GameId").transform.Find("Text").GetComponent<Text>();
            int iGameId = ccU3DEngine.ccMath.atoi(GameIdText.text);
            if (iGameId > 0) {
                GloData.glo_iGameId = iGameId;
                return true;
            }
            MessageBox.ASSERT("GameId 错误" + GameIdText.text);
            return false;
        }
        return false;
    }
    #endregion



    #region 記錄遊戲的帳號密碼
    /// <summary>
    /// 取得上次使用的帳號密碼
    /// </summary>
    private void LoadAccountPassword() {
        string strServerIp = "";
        string strVerifyName = "";
        string strVerifyPwd = "";
        int iPos = 0;
        ccGameControll.GetInstance().f_GetGameRuning(out iPos, out strServerIp, out strVerifyName, out strVerifyPwd);
        GloData.glo_iPos = iPos;
        GloData.glo_strSvrIP = strServerIp;
        Account.text = strVerifyName;
        Password.text = strVerifyPwd;

    }
    #endregion




    #region 获得机器名

    public void f_GetPcName(ccCallback tccCallback) {
        VerifyGameSocket.GetInstance().f_GetPcName(GloData.glo_iGameId, tccCallback);
    }


    private void Callback_VerifyPcInfor(object Obj) {
        string strName = (string)Obj;
        GloData.glo_strPcName = strName;
        Log("机器名：" + strName);
    }


    #endregion



    #region 讀取外部文件
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
    #endregion

}
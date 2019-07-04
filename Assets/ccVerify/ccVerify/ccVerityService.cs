using ccU3DEngine;
using ccVerifySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ccVerityService : MonoBehaviour
{
    private static ccVerityService _Instance = null;
    public static ccVerityService GetInstance()
    {
        if (_Instance == null)
        {
            GameObject tGameObject = new GameObject();
            tGameObject.name = "ccVerityService";
            _Instance = tGameObject.AddComponent<ccVerityService>();
        }

        return _Instance;
    }

    #region 内部机制

    private ErroCode _ErroCode = new ErroCode();

    void Awake()
    {
        Application.runInBackground = true;
        DontDestroyOnLoad(gameObject);
    }

    void FixedUpdate()
    {
        VerifyGameSocket.GetInstance().f_Update();

    }

    /// <summary>
    /// 程序退出时事件处理
    /// </summary>
    void OnApplicationQuit()
    {
        f_Close();
    }

    public void f_Close()
    {
        VerifyGameSocket.GetInstance().f_Close();
    }

    #endregion

    #region 登陆认证接口

    /// <summary>
    /// 游戏登陆认证系统
    /// </summary>
    /// <param name="iGameDuration">最小游戏时长,此参数值不能小于10秒（单位秒）</param>
    /// <param name="strIp">服务器IP</param>
    /// <param name="iPort">服务器工作Port</param>
    /// <param name="strName">用户名</param>
    /// <param name="strPwd">用户密码</param>
    /// <param name="iGameId">游戏Id</param>
    /// <param name="tccCallback">登陆认证结果消息回调</param>
    public void f_Login(int iGameDuration, string strName, string strPwd, int iGameId, ccSocketCallback CallBackLogin)
    {
        VerifyGameSocket.GetInstance().f_Login(iGameDuration, strName, strPwd, iGameId, CallBackLogin);
    }

    /// <summary>
    /// 注销
    /// </summary>
    public void f_LoginOut()
    {
        MessageBox.DEBUG("f_LoginOut");
        VerifyGameSocket.GetInstance().f_Close();
    }
    
    #endregion

    /// <summary>
    /// 获取错误对应的说明
    /// </summary>
    /// <param name="iErroCode"></param>
    /// <returns></returns>
    public string f_GetErroCode(eVerifyMsgOperateResult teMsgOperateResult)
    {
        return _ErroCode.f_GetErroCode((int)teMsgOperateResult);
    }
    
    #region 注册电脑信息
    
    /// <summary>
    /// 注册电脑信息
    /// </summary>
    /// <param name="strName"></param>
    /// <param name="strPwf"></param>
    /// <param name="tccCallback">登陆认证结果消息回调</param>
    public void f_RegPcInfor(ccSocketCallback tccSocketCallback)
    {
        if (VerifyGameSocket.GetInstance().f_CheckIsLoginSuc())
        {
            VerifyGameSocket.GetInstance().f_RegPcInfor(GloData.glo_iGameId, tccSocketCallback);
        }
        else
        {
            tccSocketCallback(eVerifyMsgOperateResult.OR_NoLogin);
        }        
    }

   
    #endregion

    #region 开始结束游戏

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="strName"></param>
    /// <param name="strPwf"></param>
    /// <param name="tccCallback">登陆认证结果消息回调</param>
    public string f_StartGame(string strData)
    {
        if (VerifyGameSocket.GetInstance().f_CheckIsLoginSuc())
        {
            return VerifyGameSocket.GetInstance().f_StartGame(strData);
        }
        return "ERO";
    }

   

    /// <summary>
    /// 结束游戏
    /// </summary>
    /// <param name="strName"></param>40
    /// 
    /// <param name="strPwf"></param>
    /// <param name="tccCallback">登陆认证结果消息回调</param>
    public string f_StopGame(string strData)
    {
        if (VerifyGameSocket.GetInstance().f_CheckIsLoginSuc())
        {
            return VerifyGameSocket.GetInstance().f_StopGame(strData);
        }
        return "ERO";
    }


    #endregion


    #region 游戏数据提交接口

    /// <summary>
    ///  /// <summary>
    /// 游戏数据提交接口
    /// </summary>
    /// <param name="strData">需要提示的游戏数据</param>
    /// <param name="SocketCallbackDT">标准成功失败返回接口</param>
    public string f_SubmitData(string strData, int iTimeOut, ccVerifyCallBack tccSocketCallback = null)
    {                    
        return VerifyGameSocket.GetInstance().f_SubmitData(strData, iTimeOut, tccSocketCallback);
    }

    /// <summary>
    /// 游戏数据检测接口
    /// </summary>
    /// <returns></returns>
    public string f_GetSubmitData()
    {
        return VerifyGameSocket.GetInstance().f_GetSubmitData();
    }

    public int f_GetSubmitDataCount()
    {
        return VerifyGameSocket.GetInstance().f_GetSubmitDataCount();
    }


    #endregion


    #region 检测是否可以开始游戏

    /// <summary>
    ///  /// <summary>
    /// 游戏数据提交接口
    /// </summary>
    /// <param name="strData">需要提示的游戏数据</param>
    /// <param name="SocketCallbackDT">标准成功失败返回接口</param>
    public void f_CheckCanStartGame(ccSocketCallback tccSocketCallback)
    {
        if (VerifyGameSocket.GetInstance().f_CheckIsLoginSuc())
        {
            VerifyGameSocket.GetInstance().f_CheckCanStartGame(GloData.glo_iGameId, tccSocketCallback);
        }
        else
        {
            tccSocketCallback(eVerifyMsgOperateResult.OR_NoLogin);
        }        
    }
        
    #endregion

    #region 游戏可玩次数

    /// <summary>
    /// 游戏可玩次数查询
    /// </summary>
    /// <param name="strName"></param>
    /// <param name="strPwf"></param>
    /// <param name="tccCallback">登陆认证结果消息回调</param>
    public void f_GetGamePlayTimes(ccCallback tccCallback)
    {
        if (VerifyGameSocket.GetInstance().f_CheckIsLoginSuc())
        {
            VerifyGameSocket.GetInstance().f_GetGamePlayTimes(GloData.glo_iGameId, tccCallback);
        }
        else
        {
            tccCallback(eVerifyMsgOperateResult.OR_NoLogin);
        }        
    }

 
    #endregion





}

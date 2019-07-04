using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErroCode
{
    Dictionary<int, string> _aErroCode = new Dictionary<int, string>();


    public ErroCode()
    {
        _aErroCode.Add(0, "操作成功");
        _aErroCode.Add(1, "登陆失败");
        _aErroCode.Add(2, "帐号失效");
        _aErroCode.Add(3, "帐号错误");
        _aErroCode.Add(4, "登陆DLL失败");
        _aErroCode.Add(5, "未登陆");
        _aErroCode.Add(6, "电脑硬件信息错误");
        _aErroCode.Add(7, "电脑信息已经注册");
        _aErroCode.Add(8, "未登陆注册电脑信息失败");
        _aErroCode.Add(9, "数据验证失败");
        _aErroCode.Add(10, "电脑信息未注册，注册电脑信息");
        _aErroCode.Add(11, "游戏次数已使用完");
        _aErroCode.Add(12, "未找到游戏信息");
        _aErroCode.Add(13, "未找到电脑信息");
        _aErroCode.Add(14, "授权电脑已满");
        _aErroCode.Add(15, "授权模式错误");
        _aErroCode.Add(16, "授权游戏不一致");
        _aErroCode.Add(17, "当次游戏未进行GameStart授权");

        _aErroCode.Add(18, "游戏已经开始，并且未结束，不要重复开始请求");
        _aErroCode.Add(19, "游戏开始时间点错误");
        _aErroCode.Add(20, "strData数据超过255长度");



        _aErroCode.Add(993, "WIFI网络未开");
        _aErroCode.Add(994, "连接超时");
        _aErroCode.Add(995, "注册超时");
        _aErroCode.Add(996, "登陆超时");
        _aErroCode.Add(997, "游戏出错，强制玩家离开");
        _aErroCode.Add(998, "服务器未开启");
        _aErroCode.Add(999, "游戏断开连接");
        _aErroCode.Add(10000, "操作失败");
        
    }

    public string f_GetErroCode(int iErroCode)
    {
        string strEroCode = "";
        if (_aErroCode.TryGetValue(iErroCode, out strEroCode))
        {
            return strEroCode;
        }
        return "未知错误码 " + iErroCode;
    }

}

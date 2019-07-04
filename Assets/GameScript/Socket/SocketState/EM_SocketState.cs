using UnityEngine;
using System.Collections;

/// <summary>
/// Socket状态
/// </summary>
public enum EM_Socket
{
    //OffLine,
    Connect = 1,//连接
    Regedit,//注册
    Login,//登录
    Loop,
    Wait,//等待

    ControllLogin,//Controll登录
}
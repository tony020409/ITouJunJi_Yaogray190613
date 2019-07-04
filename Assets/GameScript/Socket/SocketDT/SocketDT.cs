using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct basicNode1 : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int value1;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stGameCommandReturn : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iCommand;
    /// <summary>
    /// 操作结果
    /// </summary>
    public int iResult;
    public int iData2;
    public int iData3;
};


/// <summary>
/// 登陆封包
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_CTG_AccountEnter
{
    /// <summary>
    /// 0：正常登陆 1：重新连接
    /// </summary>
    public int m_state;									// 服务器UID

    /// <summary>
    /// 账户名
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strAccount;                      //// 机器码		

    /// <summary>
    /// 密码
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strPassword;                      //// 机器码		
    //服务器ID： 默认1
    public int m_dwServerID;									// 服务器UID
    //GamePlayer标示，服务器用
    public long m_GamePlayerPoint;
}

/// <summary>
/// 创建账户封包
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_CTG_AccountCreate
{
    /// <summary>
    /// 账户名
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strAccount;                      //// 机器码		

    /// <summary>
    /// 密码
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strPassword;   

}


/// <summary>
/// 创建账户封包
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_AccountCreateRelt : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    /// <summary>
    /// OR_Succeed = 0, // 成功 OR_Error_AccountRepetition = 20, // 注册：账号重复
    /// </summary>
    public int m_result;	

    /// <summary>
    /// 账户名
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strAccount;                      //// 机器码		

    	
}



#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_LoginRelt : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }    
    /// <summary>
    /// 账户ID
    /// </summary>
    public int m_PlayerId;                                  // 账户id
    /// <summary>
    /// OR_Succeed = 0, // 成功 OR_Error_NoAccount = 21, // 登陆：账号不存在 OR_Error_Password = 22, // 登陆：密码错误
    /// </summary>
    public int m_result;									// 账户id
    
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_AccountLoginRelt : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17, ArraySubType = UnmanagedType.SysInt)]       //eUserAttr_Enum.eUserAttr_INVALID - 1)]
    public int[] attr;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string szRoleName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_UserAttr : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public char attrEnum;
    public int iValue;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct sCharacter : SocketDataBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int GetNodeType()
    {
        return m_node_type;
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    public int m_node_type;//

    public int iId;
    public int m_iExp;
    public int m_iCharacterId;
    public int m_iTempleteId;
    public int m_iCityId;


}




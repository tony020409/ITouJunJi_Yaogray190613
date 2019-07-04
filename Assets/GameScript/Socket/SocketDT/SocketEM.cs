using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

//协议操作类型
public enum eMsgOperateType
{
    OT_NULL = -99,      //不存在错误码

    OT_CreateAccount = 0, // 创建帐号
    OT_LoginGame = 1, // 登陆游戏
};

//协议操作结果
public enum eMsgOperateResult
{
    OR_Succeed = 0, // 成功
    OR_Fail = 1, //未知原因失败
    OR_SocketConnectFail = 2, //网格无法连接     
    OR_VerFail = 3, //获取版本失败 
    OR_ScFail = 4, //获取脚本失败 
    OR_ResourceFail = 5, //加载资源失败

    OR_Error_AccountRepetition = 20, // 注册：账号重复
    OR_Error_NoAccount = 21, // 登陆：账号不存在
    OR_Error_Password = 22, // 登陆：密码错误
    OR_Error_AccountOnline = 24, // 登陆：账号在线
    OR_Error_NameRepetition = 23, // 改名：名称重复

    OR_Error_VersionNotMatch = 71, //版本不匹配 2016-7-8 
    OR_Error_ElseWhereLogin = 72, //异地登录 2016-7-8 
    OR_Error_SeverMaintain = 73, //服务器维护 2016-7-8 

    OR_Error_PosIsHavePlayer = 74, //位置上已经有玩家，操作失败


    OR_Error_WIFIConnectTimeOut = 993, //WIFI网络未开
    OR_Error_ConnectTimeOut = 994, //连接超时
    OR_Error_CreateAccountTimeOut = 995, //注册超时
    OR_Error_LoginTimeOut = 996, //登陆超时
    OR_Error_ExitGame = 997, //游戏出错，强制玩家离开
    OR_Error_ServerOffLine = 998, //服务器未开启
    OR_Error_Disconnect = 999, //游戏断开连接
    OR_Error_Default = 10000, //操作失败
};

//数据节点更新类型枚举
public enum eUpdateNodeType
{
    node_default,//默认，第一次进入游戏
    node_add,//添加
    node_update,
    node_delete,
}


//角色属性枚举
public enum eChangeRoleDataType
{
    eDefault = 0,
    eAccountId = 1000,      //账号ID AccountId=UserId

    eGUID = 1001,			//uid
    eLevel = 1002,       //等级	
    eRank = 1003,		//玩家rank
    eLastTime = 1004,	//Last_Login 最后离线时间	
    eCityId = 1005,	//看板娘模板Id

    eExp = 1007,		//经验

    eGold = 1008,		//金幣
    eToken = 1009,		//魔法石

    eAdvanecPP = 1010,			//AP
    eBp = 1011,			//BP

    eNoobStep = 1015,
    eBan = 1016,

    eActive = 1017,
    eVisitor = 1018,
    eMoney = 1019,




}

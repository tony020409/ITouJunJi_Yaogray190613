
//============================================
//
//    GameControll来自GameControll.xlsx文件自动生成脚本
//    2018/7/6 16:27:34
//    
//
//============================================
using System;
using System.Collections.Generic;

public class GameControllDT : NBaseSCDT
{
    public GameControllDT()
    {
        m_iRunTimes = 0;
        m_emMissionEndType = GameEM.EM_MissionEndType.None;
    }

    /// <summary>
    /// 说明
    /// </summary>
    public string szName;
    /// <summary>
    /// 段落
    /// </summary>
    public int iSection;
    /// <summary>
    /// 等待多少时间后开始执行（单位秒）
    /// </summary>
    public float fStartSleepTime;
    /// <summary>
    /// 开始时执行指令动作
    /// </summary>
    public int iStartAction;
    /// <summary>
    /// 所属阵营
    /// </summary>
    public int iTeam;
    /// <summary>
    /// 动作参数1
    /// </summary>
    public string szData1;
    /// <summary>
    /// 动作参数2
    /// </summary>
    public string szData2;
    /// <summary>
    /// 动作参数3
    /// </summary>
    public string szData3;
    /// <summary>
    /// 动作参数3
    /// </summary>
    public string szData4;
    /// <summary>
    /// 有效攻击部位
    /// </summary>
    public string szBeAttackPos;
    /// <summary>
    /// 有效攻击阵营
    /// </summary>
    public int iBeAttackTeam;
    /// <summary>
    /// 是否需要等待结束
    /// </summary>
    public int iNeedEnd;
    /// <summary>
    /// 结束事件后等待多少时间后执行（单位秒）
    /// </summary>
    public float fEndSleepTime;
    /// <summary>
    /// 结束时执行指令动作
    /// </summary>
    public int iEndAction;
    /// <summary>
    /// 角色出生时对游戏结果影响0未影响1死亡游戏失败2死亡游戏胜利
    /// </summary>
    public int iGameResult;
    /// <summary>
    /// 运行超时时间，指定时间脚本没有结束就被自动结束不填写默认10秒超时
    /// </summary>
    public float fRunTimeOut = 1;


    #region  附加缓存信息

    /// <summary>
    /// 执行次数
    /// </summary>
    public int m_iRunTimes;

    /// <summary>
    /// 结束类型
    /// </summary>
    public GameEM.EM_MissionEndType m_emMissionEndType;


    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态机管理器
/// 所有的AI注册都通过此管理器来进行
/// </summary>
public class AIManager
{
    Dictionary<string, AI_ConditionBaseStateV2> _dirConditionData = new Dictionary<string, AI_ConditionBaseStateV2>();
    Dictionary<string, AI_RunBaseStateV2> _dirRunData = new Dictionary<string, AI_RunBaseStateV2>();

    private static AIManager _Instance;
    public static AIManager GetInstance()
    {
        if (null == _Instance)
        {
            _Instance = new AIManager();
        }
        return _Instance;
    }

    public AIManager()
    {
        RegConditionAI();
        RegRunAI();
    }

    #region 条件状态机
    /// <summary>
    /// 注册现有的AI状态机
    /// </summary>
    private void RegConditionAI()
    {
        RegCondition("1000", new AI_Search());
        RegCondition("1001", new AI_CheckCanAttack());
        RegCondition("1003", new ConditionAI_ViewSize_In());
        RegCondition("1004", new ConditionAI_ViewSize_Out());
        RegCondition("1005", new ConditionAI_AttackSize_In());
        RegCondition("1006", new ConditionAI_AttackSize_Out());
        RegCondition("1007", new ConditionAI_ChangePoint());
        RegCondition("1008", new ConditionAI_ArcherInit());
        RegCondition("1009", new ConditionAI_CheckPlayerHand());
        RegCondition("1010", new ConditionAI_CheckPlayerTracker());
        RegCondition("1011", new ConditionAI_GotoCustomInitAI());
        //注册AI脚本里需要用到的AI状态机
    }

    /// <summary>
    /// 状态机注册
    /// </summary>
    /// <param name="szAI">状态机名称，与CharacterAI脚本里的szAI栏位名相对应</param>
    /// <param name="tAI_BaseStateV2">程序实现对应的AI状态机</param>
    private void RegCondition(string szAI, AI_ConditionBaseStateV2 tAI_BaseStateV2)
    {
        if (_dirConditionData.ContainsKey(szAI))
        {
            MessageBox.ASSERT("已存在同名的AI状态机 " + szAI);
        }
        else
        {
            _dirConditionData.Add(szAI, tAI_BaseStateV2);
        }
    }


    AI_ConditionBaseStateV2 f_GetCondigionAI(string strAI)
    {
        AI_ConditionBaseStateV2 tAI_BaseStateV2 = null;
        if (!_dirConditionData.TryGetValue(strAI, out tAI_BaseStateV2))
        {
            MessageBox.ASSERT("未找到指令的AI模块,檢查 AIManager里是否有注册AI状态机" + strAI);
        }
        return tAI_BaseStateV2;
    }


    public AI_ConditionBaseStateV2 f_CreateConditionAI(BaseRoleControllV2 tRoleControl, CharacterAIDT tCharacterAIDT, CharacterAIConditionDT tCharacterAIConditionDT)
    {
        if (tRoleControl == null || tCharacterAIDT == null)
        {
            MessageBox.ASSERT("BaseRoleControllV2或tCharacterAIDT为Null，创建AI失败 " + tCharacterAIDT.szMainAI);
            return null;
        }
        AI_ConditionBaseStateV2 tAI_BaseStateV2 = f_GetCondigionAI(tCharacterAIDT.szMainAI);
        if (tAI_BaseStateV2 != null)
        {
            AI_ConditionBaseStateV2 tNewAI_BaseStateV2 = tAI_BaseStateV2.f_Clone();
            tNewAI_BaseStateV2.f_Init(tRoleControl, tCharacterAIDT, tCharacterAIConditionDT);
            return tNewAI_BaseStateV2;
        }
        return null;
    }

    #endregion


    #region 执行状态机

    private void RegRunAI()
    {
        RegRun("1000", new AI_PlayerDie());         //玩家死亡
        RegRun("1001", new AI_PlayerIdle());        //玩家待機

        RegRun("2000", new AI_Walk());
        RegRun("2002", new AI_Attack());
        RegRun("2003", new ZombieAI2_Idle());       //殭屍AI：待機
        RegRun("2004", new ZombieAI2_Chase());      //殭屍AI：追擊
        RegRun("2005", new ZombieAI2_NearAttack()); //殭屍AI：近戰
        RegRun("2006", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)
        RegRun("2007", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)
        RegRun("2008", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)
        RegRun("2009", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)
        RegRun("2010", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)
        RegRun("2011", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)
        RegRun("2012", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)
        RegRun("2013", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)
        RegRun("2014", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)
        RegRun("2015", new ZombieAI2_FarAttack());  //士兵AI：遠攻 (子彈數不同)

        RegRun("2016", new AI_Die());                  //怪物死亡
        RegRun("2017", new AI_Path());                 //走路徑

        RegRun("2019", new AI_ChangePonit_LookAt());   // 躲避點 (有LookAt)
        RegRun("2020", new AI_ChangePonit_noLookAt()); // 躲避點 (沒有LookAt)
        RegRun("2021", new AI_MoveToHidePos());        // 遠攻初始
        RegRun("2022", new AI_Throw());                // 丟東西
        RegRun("2300", new AI_StoneAttack());          // 丟東西

        RegRun("2600", new ArcherAI_Init());           // 士兵AI_初始化
    }




    private void RegRun(string szAI, AI_RunBaseStateV2 tAI_BaseStateV2)
    {
        if (_dirRunData.ContainsKey(szAI))
        {
            MessageBox.ASSERT("已存在同名的AI状态机 " + szAI);
        }
        else
        {
            _dirRunData.Add(szAI, tAI_BaseStateV2);
        }
    }


    public AI_RunBaseStateV2 f_GetRunAI(string strAI)
    {
        AI_RunBaseStateV2 tAI_BaseStateV2 = null;
        if (!_dirRunData.TryGetValue(strAI, out tAI_BaseStateV2))
        {
            MessageBox.ASSERT("未找到指令的AI模块: " + strAI + " (RunAI)");
        }
        return tAI_BaseStateV2;
    }



    public AI_RunBaseStateV2 f_CreateRunAI(BaseRoleControllV2 tRoleControl, CharacterAIDT tCharacterAIDT, CharacterAIRunDT tCharacterAIRunDT)
    {
        if (tRoleControl == null || tCharacterAIDT == null)
        {
            MessageBox.ASSERT("BaseRoleControllV2或tCharacterAIDT为Null，创建AI失败 " + tCharacterAIDT.szRunAI);
            return null;
        }
        AI_RunBaseStateV2 tAI_BaseStateV2 = f_GetRunAI(tCharacterAIDT.szRunAI);
        if (tAI_BaseStateV2 != null)
        {
            AI_RunBaseStateV2 tNewAI_BaseStateV2 = tAI_BaseStateV2.f_Clone();
            tNewAI_BaseStateV2.f_Init(tRoleControl, tCharacterAIDT, tCharacterAIRunDT);
            return tNewAI_BaseStateV2;
        }
        return null;
    }

    #endregion

}

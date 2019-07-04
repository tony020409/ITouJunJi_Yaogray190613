using ccU3DEngine;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI控制器基类
/// </summary>
public class BaseAIControllV2
{
    //List<AI_RunBaseStateV2> _aRunAIAction = new List<AI_RunBaseStateV2>();
    protected ccMulMachineManager _ConditionAIMachineManger = null;
    protected ccMulMachineManager _RunAIMachineManger = null;
    private BaseRoleControllV2 _RoleControl;

    public BaseAIControllV2(BaseRoleControllV2 tRoleControl)
    {
        _RoleControl = tRoleControl;

        InitDefaultAI();
        CharacterAIConditionDT tCharacterAIConditionDT = null;
        CharacterAIRunDT tCharacterAIRunDT = null;
        if (_RoleControl.f_GetAI() != null)
        {
            int[] aAI = ccMath.f_String2ArrayInt(_RoleControl.f_GetAI(), ";");
            for (int i = 0; i < aAI.Length; i++)
            {
                if (aAI[i] <= 0)
                {
                    continue;
                }
                CharacterAIDT tCharacterAIDT = (CharacterAIDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAISC.f_GetSC(aAI[i]);
                tCharacterAIConditionDT = (CharacterAIConditionDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAIConditionSC.f_GetSC(ccMath.atoi(tCharacterAIDT.szMainAI));
                AI_ConditionBaseStateV2 tAI_ConditionBaseStateV2 = AIManager.GetInstance().f_CreateConditionAI(_RoleControl, tCharacterAIDT, tCharacterAIConditionDT);
                f_RegConditionAIState(tAI_ConditionBaseStateV2);

                tCharacterAIRunDT = (CharacterAIRunDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAIRunSC.f_GetSC(ccMath.atoi(tCharacterAIDT.szRunAI));
                AI_RunBaseStateV2 tAI_RunBaseStateV2 = AIManager.GetInstance().f_CreateRunAI(_RoleControl, tCharacterAIDT, tCharacterAIRunDT);
                f_RegRunAIState(tAI_RunBaseStateV2);

                //注册子任务相关
                if (tCharacterAIDT.szSubAI != null && tCharacterAIDT.szSubAI.Length > 0)
                {
                    int[] aSubAI = ccMath.f_String2ArrayInt(tCharacterAIDT.szSubAI, ";");
                    for (i = 0; i < aSubAI.Length; i++)
                    {
                        if (aSubAI[i] <= 0)
                        {
                            continue;
                        }
                        CharacterAIDT tSubCharacterAIDT = (CharacterAIDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAISC.f_GetSC(aSubAI[i]);
                        if (tSubCharacterAIDT.szSubAI != null && tSubCharacterAIDT.szSubAI.Length > 0)
                        {
                            MessageBox.ASSERT("子条件AI不能再包含子条件，创建子条件AI失败 Id = " + tSubCharacterAIDT.iId);
                        }
                        tCharacterAIConditionDT = (CharacterAIConditionDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAIConditionSC.f_GetSC(ccMath.atoi(tSubCharacterAIDT.szMainAI));
                        AI_ConditionBaseStateV2 tAI_SubConditionBaseStateV2 = AIManager.GetInstance().f_CreateConditionAI(_RoleControl, tSubCharacterAIDT, tCharacterAIConditionDT);
                        tAI_ConditionBaseStateV2.f_RegSubState(tAI_SubConditionBaseStateV2);

                        tCharacterAIRunDT = (CharacterAIRunDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAIRunSC.f_GetSC(ccMath.atoi(tSubCharacterAIDT.szRunAI));
                        AI_RunBaseStateV2 tAI_SubRunBaseStateV2 = AIManager.GetInstance().f_CreateRunAI(_RoleControl, tSubCharacterAIDT, tCharacterAIRunDT);
                        f_RegRunAIState(tAI_SubRunBaseStateV2);
                    }
                }

            }
        }
       
    }

    /// <summary>
    /// 默认AI，必须AI自动增加
    /// </summary>
    void InitDefaultAI() {

        //玩家AI
        if (_RoleControl.f_GetRoleType() == GameEM.emRoleType.Player) {
            CharacterAIRunDT tCharacterAIRunDT = null;

            AI_PlayerDie tmpPlayerAI_1 = new AI_PlayerDie();
            tCharacterAIRunDT = (CharacterAIRunDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAIRunSC.f_GetSC(1000);
            tmpPlayerAI_1.f_Init(_RoleControl, null, tCharacterAIRunDT);
            f_RegRunAIState(tmpPlayerAI_1);

            AI_PlayerIdle tmpPlayerAI_2 = new AI_PlayerIdle();
            tmpPlayerAI_2.f_Init(_RoleControl, null, null);
            f_RegRunAIState(tmpPlayerAI_2);
        }

        //怪物默认AI
        else {
            AI_WaitActionRun tAI_Wait = new AI_WaitActionRun();
            tAI_Wait.f_Init(_RoleControl, null, null);
            f_RegRunAIState(tAI_Wait);

            AI_Die tAI_Die = new AI_Die();
            tAI_Die.f_Init(_RoleControl, null, null);
            f_RegRunAIState(tAI_Die);
        }
    }

    #region 条件状态机
    /// <summary>
    /// 向控制器增加新的AI动作
    /// </summary>
    void f_RegConditionAIState(AI_ConditionBaseStateV2 tAI_BaseState)
    {
        if (_ConditionAIMachineManger == null)
        {
            bool bRun = StaticValue.m_bIsMaster ? false : true;
            _ConditionAIMachineManger = new ccMulMachineManager(new AI_WaitAction2(_RoleControl), bRun, true);
        }     
        _ConditionAIMachineManger.f_RegState(tAI_BaseState);
    }

    private AI_RunBaseStateV2 f_GetAI(GameEM.EM_RoleAction tRoleAction)
    {
        return null;
        //return _aRunAIAction.Find(delegate (AI_RunBaseStateV2 p) { return p.f_GetRoleAction() == tRoleAction; });
    }

    #endregion


    #region 执行状态机
    public void f_RegRunAIState(AI_RunBaseStateV2 tAI_BaseState)
    {
        if (_RunAIMachineManger == null)
        {
            bool bRun = StaticValue.m_bIsMaster ? false : true;
            _RunAIMachineManger = new ccMulMachineManager(new AI_WaitAction2(_RoleControl), bRun);
        }
        _RunAIMachineManger.f_RegState(tAI_BaseState);
        //_aRunAIAction.Add(tAI_BaseState.f_GetDoAction());
    }

    /// <summary>
    /// 强制执行AI动作。
    /// </summary>
    /// <param name="tAIState"></param>
    /// <param name="Obj"></param>
    public void f_RunAIState(AI_EM.EM_AIState tAIState, BaseActionV2 tBaseActionV2)
    {
        ccMulMachineStateBase tccMulMachineStateBase = _RunAIMachineManger.f_Get((int)tAIState);
        if (tccMulMachineStateBase != null)
        {
            AI_RunBaseStateV2 tAI_BaseStateV2 = (AI_RunBaseStateV2)tccMulMachineStateBase;
            tAI_BaseStateV2.f_SaveParament(tBaseActionV2);
            _RunAIMachineManger.f_ChangeState(tAI_BaseStateV2);
        }
    }


    public void f_RunStateComplete() {

        if (_ConditionAIMachineManger!=null) {
            _ConditionAIMachineManger.f_ForceCompleteCurState();
        }

        if (_RunAIMachineManger!=null) {
            _RunAIMachineManger.f_ForceCompleteCurState();
        }
        
    }

    #endregion

    public void f_Update()
    {
        if (_ConditionAIMachineManger != null)
        {
            _ConditionAIMachineManger.f_Update();
        }
        if (_RunAIMachineManger != null)
        {
            _RunAIMachineManger.f_Update();
        }
    }

    public void f_ProcessAction(BaseActionV2 tAction)
    {
        AI_RunBaseStateV2 tAI_BaseStateV2 = f_GetAI((GameEM.EM_RoleAction)tAction.m_iType);
        if (tAI_BaseStateV2 != null)
        {
            tAI_BaseStateV2.f_SaveParament(tAction);
            _RunAIMachineManger.f_ChangeState(tAI_BaseStateV2);
        }
        else
        {
            MessageBox.ASSERT("未找到同步动作对应的AI状态机 " + ((GameEM.EM_RoleAction)tAction.m_iType).ToString());
        }
    }



    #region AI动作
    /// <summary>
    /// 取得當前的RunAI
    /// </summary>
    public AI_EM.EM_AIState GetCurRunAI() {
        if (_RunAIMachineManger.f_GetCurMachineState() == null) {
            return AI_EM.EM_AIState.None;
        }
        return (AI_EM.EM_AIState)_RunAIMachineManger.f_GetCurMachineState().f_GetId();
    }


    /// <summary>
    /// 
    /// </summary>
    public AI_EM.EM_AIState GetCurRunAIList() {
        if (_RunAIMachineManger.f_GetCurMachineState() == null) {
            return AI_EM.EM_AIState.None;
        }
        return (AI_EM.EM_AIState)_RunAIMachineManger.f_GetCurMachineState().f_GetId();
    }


    /// <summary>
    /// 確認當前AI是否為Walk
    /// </summary>
    public bool f_CheckAIIsWalk(){
        return _RunAIMachineManger.f_GetCurMachineState().f_GetId() == (int)AI_EM.EM_AIState.Walk ? true : false;
    }


    public virtual void f_Attack()
    {
        //ZombieAI2_NearAttack tAI_Attack = (ZombieAI2_NearAttack)_RunAIMachineManger.f_GetCurMachineState();
        //tAI_Attack.f_Attack();
        //UpdateLog();
    }

    public virtual void f_AttackComplete()
    {
        //_RunAIMachineManger.f_ChangeState((int)AI_EM.EM_AIState.Idle);
        //UpdateLog();
    }

    #endregion


}

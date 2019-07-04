/***************************************************************

/***************************************************************

f_ConditionTest 进行条件检测，当条件满足时就会启动状态机的操作 返回值说明：true满足条件 false不满足条件，


f_SaveParament 设置状态机的执行参数,当次执行有效结束时会自动清除

f_Enter状态机开始执行时触发
f_Execute  状态机循环执行类似Update
f_Exit 状态机被结束时触发


***************************************************************/


using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AI条件状态机基类
/// </summary>
public abstract class AI_ConditionBaseStateV2 : ccMulMachineStateBase
{
    protected CharacterAIConditionDT _CharacterAIConditionDT;
    protected CharacterAIDT _CharacterAIDT;
    protected BaseRoleControllV2 _BaseRoleControl;
    protected Transform transform;

    private bool _bIsRuning = false;
    protected BaseActionV2 _CurAction;
    protected object _CurObj;
    protected ccCallback _ccCompleteCallback = null;

    /// <summary>
    /// 保存最后执行的Action动作方便调试用
    /// </summary>
    protected BaseActionV2 _LastRunAction = null;

    List<AI_ConditionBaseStateV2> _SubConditionState = null;

    /// <summary>
    /// 保存当前正在运行的条件状态机，有可能是当前主状态机也有可能是子条件状态机
    /// </summary>
    private AI_ConditionBaseStateV2 _CurRunConditionState = null;
    private BaseRoleControllV2 _TargetRole = null;

    public AI_ConditionBaseStateV2 f_Clone()
    {
        return (AI_ConditionBaseStateV2)MemberwiseClone();
    }

    /// <summary>
    /// 初始AI状态机
    /// </summary>
    /// <param name="tiId">状态机Id</param>
    /// <param name="emRoleAction">状态机对应的动作Action</param>
    public AI_ConditionBaseStateV2(AI_EM.EM_AIState emAIState)
        : base((int)emAIState)
    {

    }

    public void f_Init(BaseRoleControllV2 tBaseRoleControllV2, CharacterAIDT tCharacterAIDT, CharacterAIConditionDT tCharacterAIConditionDT)
    {
        _BaseRoleControl = tBaseRoleControllV2;
        transform = tBaseRoleControllV2.transform;
        _CharacterAIDT = tCharacterAIDT;
        _CharacterAIConditionDT = tCharacterAIConditionDT;
    }

    public void f_RegSubState(AI_ConditionBaseStateV2 tAI_ConditionBaseStateV2)
    {
        if (_SubConditionState == null)
        {
            _SubConditionState = new List<AI_ConditionBaseStateV2>();
        }
        _SubConditionState.Add(tAI_ConditionBaseStateV2);
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        SaveCurRunConditonState(this);
    }

    public sealed override void f_Execute()
    {
        base.f_Execute();

        if (CheckTargetRoleIsDie())
        {
            return;
        }
        if (_CurRunConditionState == null || _CurRunConditionState == this)
        {
            if (_SubConditionState != null)
            {
                for (int i = 0; i < _SubConditionState.Count; i++)
                {
                    if (_SubConditionState[i].f_ConditionTest())
                    {
                        SaveCurRunConditonState(_SubConditionState[i]);
                        return;
                    }
                }
            }
        }
        else
        {
            if (_CurRunConditionState.f_CheckIsComplete())
            {//子条件结束，主条件主动启动结束
                f_SetComplete();
            }
        }
    }
    public override void f_Exit()
    {
        base.f_Exit();
        if (_CurRunConditionState != null && _CurRunConditionState != this)
        {
            _CurRunConditionState.f_Exit();
        }
        _CurRunConditionState = null;
        _TargetRole = null;
    }

    protected void SaveTargetRole(BaseRoleControllV2 tBaseRoleControllV2)
    {
        _TargetRole = tBaseRoleControllV2;
    }

    private bool CheckTargetRoleIsDie()
    {
        if (_TargetRole != null)
        {
            if (_TargetRole.f_IsDie())
            {
                f_SetComplete();
                return true;
            }
        }
        return false;
    }
    
    private void SaveCurRunConditonState(AI_ConditionBaseStateV2 tAI_ConditionBaseStateV2)
    {
        _CurRunConditionState = tAI_ConditionBaseStateV2;
    }

    #region 同步相关

    /// <summary>
    /// 广播动作给其它玩家进行同步
    /// </summary>
    /// <param name="tRoleAttackAction"></param>
    protected void f_DoRunAIState(BaseActionV2 tBaseActionV2)
    {
        tBaseActionV2.f_SaveStateId(f_GetId());
        _LastRunAction = tBaseActionV2;
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tBaseActionV2);
    }

    #endregion
       

    #region 公共缓存内容
    /// <summary>
    /// 保存一个名为strParamentName变量，变量值为Obj
    /// </summary>
    /// <param name="strParamentName">变量名</param>
    /// <param name="Obj">变量值</param>
    //public void f_MemoryParament_Save(string strParamentName, object Obj)
   
    /// <summary>
    /// 获取strParamentName变量的变量值，如果不存在则返回null
    /// </summary>
    /// <param name="strParamentName">变量名</param>
    /// <returns></returns>
    //public object f_MemoryParament_Get(string strParamentName)
   
    /// <summary>
    /// 清除所有的变量
    /// </summary>
    //public void f_MemoryParament_Clear()
    
    #endregion

}
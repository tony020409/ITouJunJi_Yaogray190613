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
/// AI状态机基类
/// </summary>
public abstract class AI_RunBaseStateV2 : ccMulMachineStateBase
{
    private bool _bInitOK = false;
    protected CharacterAIRunDT _CharacterAIRunDT;
    protected CharacterAIDT _CharacterAIDT;
    protected BaseRoleControllV2 _BaseRoleControl;
    protected Transform transform;

    private bool _bIsRuning = false;
    protected BaseActionV2 _CurAction;
       
    public GameEM.EM_RoleAction f_GetRoleAction() {
        return RoleTools.f_GetAIState2RoleAction((AI_EM.EM_AIState)_iId);
    }


    public AI_RunBaseStateV2 f_Clone(){
        return (AI_RunBaseStateV2)MemberwiseClone();
    }

    /// <summary>
    /// 初始AI状态机
    /// </summary>
    /// <param name="tiId">状态机Id</param>
    /// <param name="emRoleAction">状态机对应的动作Action</param>
    public AI_RunBaseStateV2(AI_EM.EM_AIState emAIState)
        : base((int)emAIState)
    {    
    }

    #region 内部处理

    public sealed override bool f_ConditionTest()
    {
        return false;
    }
    
    public void f_Init(BaseRoleControllV2 tBaseRoleControllV2, CharacterAIDT tCharacterAIDT, CharacterAIRunDT tCharacterAIRunDT)
    {
        _BaseRoleControl = tBaseRoleControllV2;
        transform = tBaseRoleControllV2.transform;
        _CharacterAIDT = tCharacterAIDT;
        _CharacterAIRunDT = tCharacterAIRunDT;
        _bInitOK = true;
    }

    #endregion

    /// <summary>
    /// 设置状态机的执行参数,当次执行有效结束时会自动清除
    /// </summary>
    /// <param name="tBaseActionV2"> 状态执行的RoleAction </param>
    /// <param name="Obj"          > 参数 </param>
    /// <param name="tccCallback"  > 结束回调 </param>
    public void f_SaveParament(BaseActionV2 tBaseActionV2) {
        if (_bIsRuning) {
            if (BattleMain.GetInstance().Debug_GameControlRead) {
                MessageBox.ASSERT("角色:" + _BaseRoleControl.m_iId + "的" + (AI_EM.EM_AIState)_iId + "(" + _iId + ") 状态机运行中不能进行参数设置");
            }
        }
        _CurAction = tBaseActionV2;
    }

    public override void f_Enter(object Obj)
    {        
        if (!_bInitOK)
        {
            MessageBox.ASSERT("状态机没有调用f_Init进行初始设置，请去BaseAIControllV2中进行初始设置");
        }
        base.f_Enter(Obj);
       
        _bIsRuning = true;
        _BaseRoleControl.f_PlayAction((AI_EM.EM_AIState)f_GetId());
    }
    
    public override void f_Exit()
    {
        base.f_Exit();
        _bIsRuning = false;
        _CurAction = null;
    }

    /// <summary>
    /// 执行AI状态机结束，之后主控制器进入新的条件检测状态
    /// </summary>
    protected void f_RunStateComplete()
    {
        f_SetComplete();

        // 修改动作结束机制，未测试确认代码WLB [11/5/2018 Administrator]
        if (StaticValue.m_bIsMaster)
        {
            _BaseRoleControl.f_RunStateComplete();
        }
    }
    
    #region 同步相关
    
    /// <summary>
    /// 广播动作给其它玩家进行同步
    /// </summary>
    /// <param name="tRoleAttackAction"></param>
    protected void f_BroadCastAction(BaseActionV2 tBaseActionV2)
    {
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tBaseActionV2);
    }

    #endregion


    protected void ChangeFace(Vector3 Pos)
    {
        iTween.LookTo(_BaseRoleControl.gameObject, Pos, 1.5f);
    }


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
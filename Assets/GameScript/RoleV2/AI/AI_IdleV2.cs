using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 系统保存状态机，不需要外部重写
/// </summary>
public class AI_IdleV2 : AI_RunBaseStateV2
{
    
    public AI_IdleV2()
        : base(AI_EM.EM_AIState.Idle)
    {
        
    }

  

}

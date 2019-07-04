using UnityEngine;
using ccU3DEngine;
using System.Collections.Generic;

public class AI_Search : AI_ConditionBaseStateV2
{
    private BaseRoleControllV2 _ReadyAttackTarget;
    private List<TileNode> _aWalkPath = null;

    private float fSleepTime = 0;
    List<int> _aPathIgnoreData = new List<int>();
    public AI_Search()
        : base(AI_EM.EM_AIState.Search)
    {
    }

    public override bool f_ConditionTest()
    {
        _ReadyAttackTarget = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetViewSize());
        if (_ReadyAttackTarget != null)
        {//发现目标，走向目标
            List<int> aData = new List<int>();
            aData.Add(_BaseRoleControl.m_iId);
            _aWalkPath = BattleMain.GetInstance().m_MapNav.f_GetAroundFreePosForPath(_BaseRoleControl.f_GetPos(), _ReadyAttackTarget.f_GetPos(), (int)(_BaseRoleControl.f_GetAttackSize() + 0.5f), TileNode.TileType.Normal, _ReadyAttackTarget.m_iId, aData);
            if (_aWalkPath == null)
            {
                MessageBox.DEBUG("AI_Search找到目标但是无路可走，取消操作");
            }
            else
            {
                int[] aPath = new int[_aWalkPath.Count];
                for (int i = 0; i < _aWalkPath.Count; i++)
                {
                    aPath[i] = _aWalkPath[i].idx;
                }
                if (aPath.Length > 1)
                {
                    RoleWalkAction tRoleWalkAction = new RoleWalkAction();
                    tRoleWalkAction.f_Walk(_BaseRoleControl.m_iId, _BaseRoleControl.f_GetTeamType(), aPath);
                    f_DoRunAIState(tRoleWalkAction);
                    return true;
                }
            }
        }
        return false;
    }
    

}

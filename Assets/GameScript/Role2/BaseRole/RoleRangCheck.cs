using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleRangCheck
{
    private bool _bOpen = false;
    private float _fRang = 0;
    private int _iRunAction = 0;
    private int _iPeopleCount = 0;
    private BaseRoleControllV2 _BaseRoleControl;
    private ccCallback _ccCallback = null;
    public RoleRangCheck (BaseRoleControllV2 tBaseRoleControl)
    {
        _BaseRoleControl = tBaseRoleControl;
    }

    /// <summary>
    /// 打开检测功能
    /// </summary>
    public void f_Open(float fRang, int iRunAction, ccCallback tccCallBack = null, int iPeopleCount = 1)
    {
        _bOpen = true;
        _fRang = fRang;
        _iRunAction = iRunAction;
        _ccCallback = tccCallBack;
        _iPeopleCount = iPeopleCount;
    }

    /// <summary>
    /// 关闭检测功能
    /// </summary>
    public void f_Close()
    {
        _bOpen = false;
    }

    public void f_Update()
    {
        if (!_bOpen)
        {
            return;
        }

        List<BaseRoleControllV2> tBaseRoleControl = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemyAll2(_BaseRoleControl, _fRang);
        if (tBaseRoleControl.Count >= _iPeopleCount)
        {
            f_Close();
            MessageBox.DEBUG("发现目标执行后续触发任务Action " + _iRunAction);
            if (_ccCallback != null && _iRunAction == -99)
            {
                _ccCallback(null);
                _ccCallback = null;
            }
            else
            {
                ServerActionState tServerActionState = new ServerActionState();
                tServerActionState.f_Save(_iRunAction * 100000);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tServerActionState);
            }            
        }


    }


}

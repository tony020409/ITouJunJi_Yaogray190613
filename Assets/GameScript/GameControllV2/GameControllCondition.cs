using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllCondition
{
    class GameControllConditionPoolDT
    {
        public GameControllConditionPoolDT(int iId, GameControllPara tGameControllPara, GameControll_ConditionDT tGameControll_ConditionDT)
        {
            _iId = iId;
            _GameControll_ConditionDT = tGameControll_ConditionDT;
            _GameControllPara = tGameControllPara;
        }

        private int _iId;
        private GameControllThread _GameControllThread;
        private GameControll_ConditionDT _GameControll_ConditionDT;
        private GameControllPara _GameControllPara;

        private ConditionState_Base _ConditionState_Base = null;
        private bool _bConditionIsOk = false;

        public void f_Init()
        {
            //3000. 無條件執行
            if (EM_GameControllAction.V3_Init == (EM_GameControllAction)_GameControll_ConditionDT.iConditionId) {
                _ConditionState_Base = new GameControllV3_Init(_iId, _GameControllPara);
            }

            //2002. ???
            else if (EM_GameControllAction.V3_RoleRangCheckAsync == (EM_GameControllAction)_GameControll_ConditionDT.iConditionId) {
                _ConditionState_Base = new ConditionState_RoleRangCheck(_iId, _GameControllPara);
            }

            //2007. 指定阵营的角色，进入指定「怪物」的指定範圍內，触发指令任务
            else if (EM_GameControllAction.V3_RoleRangCheckAsyncForTeam == (EM_GameControllAction)_GameControll_ConditionDT.iConditionId) {
                _ConditionState_Base = new ConditionState_RoleRangCheckForTeam(_iId, _GameControllPara);
            }

            //2008. 指定阵营的角色的「頭」，进入指定「怪物」的指定範圍內，触发指令任务 (方形偵測)
            else if (EM_GameControllAction.V3_RoleRangCheckAsyncForTeam_verRect == (EM_GameControllAction)_GameControll_ConditionDT.iConditionId) {
                _ConditionState_Base = new ConditionState_RoleRangCheckForTeam_verRect(_iId, _GameControllPara);
            }

            //2009. 指定阵营的玩家的手，进入指定「怪物」的指定範圍內，触发指令任务 (方形偵測)
            else if (EM_GameControllAction.V3_CheckHand == (EM_GameControllAction)_GameControll_ConditionDT.iConditionId) {
                _ConditionState_Base = new ConditionState_RoleRangCheckForHand(_iId, _GameControllPara);
            }

            //2010. 指定阵营的角色，进入指定「物件」的指定範圍內，触发指令任务
            else if (EM_GameControllAction.V3_ObjectRangCheckForTeam == (EM_GameControllAction)_GameControll_ConditionDT.iConditionId) {
                _ConditionState_Base = new ConditionState_ObjectRangCheckForTeam (_iId, _GameControllPara);
            }

            //2011. 指定阵营的角色，进入指定「物件」的指定範圍內，触发指令任务 (方形偵測) (暫時沒有需求，所以還是用用怪物偵測的)
            else if (EM_GameControllAction.V3_ObjectRangCheckForTeam_verRect == (EM_GameControllAction)_GameControll_ConditionDT.iConditionId) {
                _ConditionState_Base = new ConditionState_RoleRangCheckForTeam_verRect (_iId, _GameControllPara);
            }

            // 4
            else if (EM_GameControllAction.RoleHp == (EM_GameControllAction)_GameControll_ConditionDT.iConditionId) {
                _ConditionState_Base = new ConditionState_RoleHp(_iId, _GameControllPara);
            }

            // 遇到無法解析的指令
            else {
                MessageBox.ASSERT("无法解析的条件指令请查对 Id:" + _GameControll_ConditionDT.iId + " 条件:" + _GameControll_ConditionDT.iConditionId);
            }
            _ConditionState_Base.f_Init(_GameControll_ConditionDT.szParament, _GameControll_ConditionDT.szParamentData, _GameControll_ConditionDT.szData1, _GameControll_ConditionDT.szData2, _GameControll_ConditionDT.szData3, _GameControll_ConditionDT.szData4);
            _GameControllThread = new GameControllThread(_iId, _GameControll_ConditionDT.iRunAction);
        }

        public void f_Update()
        {
            if (!_bConditionIsOk)
            {
                if (_ConditionState_Base.f_Check())
                {
                    _bConditionIsOk = true;
                    _GameControllThread.f_Start();
                }
            }
            else
            {
                _GameControllThread.f_Update();
            }
        }

        public int f_GetId()
        {
            return _iId;
        }

        public void f_RunServerActionState(int iId)
        {
            _GameControllThread.f_RunServerActionState(iId);
        }
    }

    private GameControllPara _GameControllPara = null;
    List<GameControllConditionPoolDT> _aData = new List< GameControllConditionPoolDT>();

    public GameControllCondition (GameControllPara tGameControllPara)
    {
        _GameControllPara = tGameControllPara;
    }

    public void f_Init()
    {
        List<NBaseSCDT> aData = glo_Main.GetInstance().m_SC_Pool.m_GameControll_ConditionSC.f_GetAll();
        for (int i = 0; i < aData.Count; i++)
        {
            GameControll_ConditionDT tGameControll_ConditionDT = (GameControll_ConditionDT)aData[i];
            GameControllConditionPoolDT tGameControllConditionPoolDT = new GameControllConditionPoolDT(tGameControll_ConditionDT.iId, _GameControllPara, tGameControll_ConditionDT);
            tGameControllConditionPoolDT.f_Init();
            _aData.Add(tGameControllConditionPoolDT);
        }
    }
    
    public void f_Update()
    {
        for (int i = 0; i < _aData.Count; i++)
        {
            GameControllConditionPoolDT tGameControllConditionPoolDT = _aData[i];
            tGameControllConditionPoolDT.f_Update();
        }

    }


    private GameControllConditionPoolDT GetGameControllConditionPoolDT(int iConditionId)
    {
        GameControllConditionPoolDT tGameControllConditionPoolDT = _aData.Find(delegate (GameControllConditionPoolDT tItem)
                                                                                {
                                                                                    if (tItem.f_GetId() == iConditionId)
                                                                                    {
                                                                                        return true;
                                                                                    }
                                                                                    return false;
                                                                                } );
        return tGameControllConditionPoolDT;
    }

    public void f_RunServerActionState(int iConditionId, int iId)
    {
        GameControllConditionPoolDT tGameControllConditionPoolDT = GetGameControllConditionPoolDT(iConditionId);        
        if (tGameControllConditionPoolDT != null)
        {
            tGameControllConditionPoolDT.f_RunServerActionState(iId);
        }
        else
        {
            MessageBox.ASSERT("未找到对应的事件处理线程，" + iConditionId);
        }
    }



}
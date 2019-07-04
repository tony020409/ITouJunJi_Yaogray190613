using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllV2
{
    private GameControllPara _GameControllPara = null;
    private GameControllCondition _GameControllCondition = null;
    private bool _bIsRuning = false;

    public void f_Init()
    {
        _GameControllPara = new GameControllPara();
        _GameControllCondition = new GameControllCondition(_GameControllPara);

        _GameControllPara.f_Init();
        _GameControllCondition.f_Init();
    }

    public void f_Start()
    {
        _bIsRuning = true;
    }

    public void f_Stop()
    {
        _bIsRuning = true;
    }

    public void f_Update()
    {
        if (StaticValue.m_bIsMaster)
        {
            if (!_bIsRuning)
            {
                return;
            }
            _GameControllCondition.f_Update();
        }
    }

    public void f_RunServerActionState(int iConditionId, int iId)
    {
        _GameControllCondition.f_RunServerActionState(iConditionId, iId);
    }


    public string f_GetParamentData(string strParament){
        return _GameControllPara.f_GetParamentData(strParament);
    }


    public void f_SetParamentData(string strParament, string strData) {
        _GameControllPara.f_SetParamentData(strParament, strData);
    }

}
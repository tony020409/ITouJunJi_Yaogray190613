using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionState_Base
{

    private string _szParament;
    private string _szParamentData;
    private GameControllPara _GameControllPara;
    private int _iId;


    public ConditionState_Base(int iId, GameControllPara tGameControllPara) {
        _iId = iId;
        _GameControllPara = tGameControllPara;
    }

    public virtual void f_Init(string szParament, string szParamentData, string szData1, string szData2, string szData3, string szData4) {
        _szParament = szParament;
        _szParamentData = szParamentData;
    }


    public virtual bool f_Check() {
        string szData = _GameControllPara.f_GetParamentData(_szParament);
        if (szData.Equals(_szParamentData)) {
            return true;
        }
        return false;
    }


}
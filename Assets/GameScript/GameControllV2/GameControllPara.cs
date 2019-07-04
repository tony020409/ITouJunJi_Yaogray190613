using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllPara
{
    class GameControllParaPoolDT
    {
        public GameControllParaPoolDT(GameControll_ParameterDT tGameControll_ParameterDT)
        {
            m_szParamentName = tGameControll_ParameterDT.szParamentName;
            m_szData = tGameControll_ParameterDT.szData;
            m_GameControll_ParameterDT = tGameControll_ParameterDT;
        }
        public string m_szParamentName;
        public string m_szData;
        public GameControll_ParameterDT m_GameControll_ParameterDT;
    }

    Dictionary<string, GameControllParaPoolDT> _aData = new Dictionary<string, GameControllParaPoolDT>();

    public void f_Init()
    {
        List<NBaseSCDT> aData = glo_Main.GetInstance().m_SC_Pool.m_GameControll_ParameterSC.f_GetAll();
        for (int i = 0; i < aData.Count; i++)
        {
            GameControll_ParameterDT tGameControll_ParameterDT = (GameControll_ParameterDT)aData[i];
            GameControllParaPoolDT tGameControllParaDT = new GameControllParaPoolDT(tGameControll_ParameterDT);
            _aData.Add(tGameControllParaDT.m_szParamentName, tGameControllParaDT);
        }
    }

    public string f_GetParamentData(string szParamentName)
    {
        GameControllParaPoolDT tGameControllParaDT = null;
        if (_aData.TryGetValue(szParamentName, out tGameControllParaDT))
        {
            return tGameControllParaDT.m_szData;
        }
        else
        {
            MessageBox.ASSERT("获取变量「" + szParamentName + "」失败，请查对变量是否存在，或是 Excel表格填寫变量的欄位 是不是有空格漏填的");
        }
        return "";
    }

    public void f_SetParamentData(string szParamentName, string szData)
    {
        GameControllParaPoolDT tGameControllParaDT = null;
        if (_aData.TryGetValue(szParamentName, out tGameControllParaDT))
        {
            tGameControllParaDT.m_szData = szData;
        }
        else
        {
            MessageBox.ASSERT("设置变量失败，未找到变量，请查对 " + szParamentName + " 是否存在");
        }
    }

}
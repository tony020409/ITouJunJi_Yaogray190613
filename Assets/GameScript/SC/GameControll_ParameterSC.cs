
//============================================
//
//    GameControll_Parameter来自GameControll_Parameter.xlsx文件自动生成脚本
//    2018/7/29 12:00:35
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GameControll_ParameterSC : NBaseSC
{
    public GameControll_ParameterSC()
    {
        Create("GameControll_ParameterDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GameControll_ParameterDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
                    MessageBox.DEBUG(m_strRegDTName + ".csv 腳本存在空紀錄：id = " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new GameControll_ParameterDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT.szReadme = tData[a++];
                DataDT.szParamentName = tData[a++];
                DataDT.szData = tData[a++];
                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
                continue;
            }
        }
    }

}

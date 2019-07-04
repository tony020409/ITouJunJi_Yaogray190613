
//============================================
//
//    GameControll_Condition来自GameControll_Condition.xlsx文件自动生成脚本
//    2018/7/29 22:46:37
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GameControll_ConditionSC : NBaseSC
{
    public GameControll_ConditionSC()
    {
        Create("GameControll_ConditionDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GameControll_ConditionDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "") {
                    MessageBox.DEBUG(m_strRegDTName + ".csv 腳本存在空紀錄：id = " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new GameControll_ConditionDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT.szName = tData[a++];
                DataDT.szParament = tData[a++];
                DataDT.szParamentData = tData[a++];
                DataDT.iConditionId = ccMath.atoi(tData[a++]);
                DataDT.szData1 = tData[a++];
                DataDT.szData2 = tData[a++];
                DataDT.szData3 = tData[a++];
                DataDT.szData4 = tData[a++];
                DataDT.iRunAction = ccMath.atoi(tData[a++]);
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

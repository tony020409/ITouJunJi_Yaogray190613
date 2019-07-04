
//============================================
//
//    CharacterAI来自CharacterAI.xlsx文件自动生成脚本
//    2018/9/2 23:19:04
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CharacterAISC : NBaseSC
{
    public CharacterAISC()
    {
        Create("CharacterAIDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CharacterAIDT DataDT;
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
                DataDT = new CharacterAIDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT.szReadme = tData[a++];
                DataDT.szMainAI = tData[a++];
                DataDT.szSubAI = tData[a++];
                DataDT.szRunAI = tData[a++];
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

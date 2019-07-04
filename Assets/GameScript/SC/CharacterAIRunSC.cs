
//============================================
//
//    CharacterAIRun来自CharacterAIRun.xlsx文件自动生成脚本
//    2018/9/7 15:29:32
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CharacterAIRunSC : NBaseSC
{
    public CharacterAIRunSC()
    {
        Create("CharacterAIRunDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CharacterAIRunDT DataDT;
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
                DataDT = new CharacterAIRunDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id错误");
                }
                DataDT.szReadme = tData[a++];
                DataDT.szAI = tData[a++];
                DataDT.szData1 = tData[a++];
                DataDT.szData2 = tData[a++];
                DataDT.szData3 = tData[a++];
                DataDT.szData4 = tData[a++];
                DataDT.szData5 = tData[a++];
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

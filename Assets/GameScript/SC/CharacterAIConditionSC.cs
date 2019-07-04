
//============================================
//
//    CharacterAICondition来自CharacterAICondition.xlsx文件自动生成脚本
//    2018/9/2 23:19:16
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CharacterAIConditionSC : NBaseSC
{
    public CharacterAIConditionSC()
    {
        Create("CharacterAIConditionDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CharacterAIConditionDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try {
                if (tFoddScData[i] == "") {
                    MessageBox.DEBUG(m_strRegDTName + ".csv 腳本存在空紀錄：id = " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new CharacterAIConditionDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT.szReadme = tData[a++];
                DataDT.szAI = tData[a++];
                DataDT.szData1 = tData[a++];
                DataDT.szData2 = tData[a++];
                DataDT.szData3 = tData[a++];
                DataDT.szData4 = tData[a++];
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

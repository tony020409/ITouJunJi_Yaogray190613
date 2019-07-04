
//============================================
//
//    GameControll来自GameControll.xlsx文件自动生成脚本
//    2018/7/30 19:19:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GameControllSC : NBaseSC
{
    public GameControllSC()
    {
        Create("GameControllDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GameControllDT DataDT;
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
                DataDT = new GameControllDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id錯誤，Id不應該小於0");
                }
                DataDT.szName = tData[a++];
                DataDT.iSection = ccMath.atoi(tData[a++]);
                DataDT.fStartSleepTime = ccMath.atof(tData[a++]);
                DataDT.iStartAction = ccMath.atoi(tData[a++]);
                DataDT.iTeam = ccMath.atoi(tData[a++]);
                DataDT.szData1 = tData[a++];
                DataDT.szData2 = tData[a++];
                DataDT.szData3 = tData[a++];
                DataDT.szData4 = tData[a++];
                DataDT.szBeAttackPos = tData[a++];
                DataDT.iBeAttackTeam = ccMath.atoi(tData[a++]);
                DataDT.iNeedEnd = ccMath.atoi(tData[a++]);
                DataDT.fEndSleepTime = ccMath.atof(tData[a++]);
                DataDT.iEndAction = ccMath.atoi(tData[a++]);
                DataDT.iGameResult = ccMath.atoi(tData[a++]);
                DataDT.fRunTimeOut = ccMath.atof(tData[a++]);
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

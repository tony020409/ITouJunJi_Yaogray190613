
//============================================
//
//    Gun来自Gun.xlsx文件自动生成脚本
//    2018/10/9 10:27:00
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GunSC : NBaseSC
{
    public GunSC()
    {
        Create("GunDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GunDT DataDT;
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
                DataDT = new GunDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id錯誤，Id不應該小於0");
                }
                DataDT.szName = tData[a++];
                DataDT.szBulletResName = tData[a++];
                DataDT.fSpeed = ccMath.atof(tData[a++]);
                DataDT.iMaxBullet = ccMath.atoi(tData[a++]);
                DataDT.szBulletStartPos = tData[a++];
                DataDT.iBulletId = ccMath.atoi(tData[a++]);
                DataDT.iClipNum = ccMath.atoi(tData[a++]);
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

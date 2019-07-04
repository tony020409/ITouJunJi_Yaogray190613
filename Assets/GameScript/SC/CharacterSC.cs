
//============================================
//
//    Character来自Character.xlsx文件自动生成脚本
//    2018/5/23 20:30:20
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CharacterSC : NBaseSC
{
    public CharacterSC()
    {
        Create("CharacterDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CharacterDT DataDT;
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
                DataDT = new CharacterDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0) {
                    MessageBox.ASSERT("Id錯誤，Id不應該小於0");
                }
                DataDT.szName = tData[a++];
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.szResName = tData[a++];
                DataDT.strReadme = tData[a++];
                DataDT.iHp = ccMath.atoi(tData[a++]);
                DataDT.iAttackPower = ccMath.atoi(tData[a++]);
                DataDT.fAttackSize = ccMath.atof(tData[a++]);
                DataDT.fMoveSpeed = ccMath.atof(tData[a++]);
                DataDT.fViewSize = ccMath.atof(tData[a++]);
                DataDT.szAttackType = tData[a++];
                DataDT.szPos = tData[a++];
                DataDT.fBodySize = ccMath.atof(tData[a++]);
                DataDT.iNoFind = ccMath.atoi(tData[a++]);
                DataDT.iInvincible = ccMath.atoi(tData[a++]);
                DataDT.iReBirth = ccMath.atoi(tData[a++]);
                DataDT.szAI = tData[a++];
                DataDT.szGun = tData[a++];
                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
                continue;
            }
        }
    }


    public int f_GetScId(MonsterType tMonsterType)
    {
        if (tMonsterType == MonsterType.FAT_OGRE_PBR)
        {
            return 1000;
        }
        else if (tMonsterType == MonsterType.Goblin_Arch)
        {
            return 1003;
        }
        else if (tMonsterType == MonsterType.Goblin_Ranger)
        {
            return 1001;
        }
        else if (tMonsterType == MonsterType.Goblin_Suicide)
        {
            return 1002;
        }
        //else if (tMonsterType == MonsterType.Prop_Jar_Explosion)
        //{
        //    return 1000;
        //}
        //else if (tMonsterType == MonsterType.Shield)
        //{
        //    return 1000;
        //}
        return -99;
    }


}

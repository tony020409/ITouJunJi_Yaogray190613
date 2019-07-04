using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionLog
{

    public static void f_Out()
    {
        string ppSQL = "--------------------------------------------------------------\n";
        List<NBaseSCDT> aData = glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetAll();
        //for(int i = 0; i < aData.Count; i++)
        int i = 0;
        foreach(NBaseSCDT tNBaseSCDT in aData )
        {
            GameControllDT tGameControllDT = (GameControllDT)tNBaseSCDT;
            ppSQL = ppSQL + " " + tGameControllDT.iId + " 执行次数:" + tGameControllDT.m_iRunTimes + " 结束状态:" + (int)tGameControllDT.m_emMissionEndType + "\n";
            if (i == 100)
            {
                MessageBox.DEBUG(ppSQL);
                //ppSQL += "--------------------------------------------------------------\n";
                i = 0;
            }
        }
        ppSQL += "--------------------------------------------------------------\n";
        MessageBox.DEBUG(ppSQL);
    }


}
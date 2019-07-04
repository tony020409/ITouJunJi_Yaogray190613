using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllPlayMvTools
{


    public static void f_Play(int iGameControllDTId)
    {
        GameControllDT tGameControllDT = (GameControllDT)glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetSC(iGameControllDTId);

        //开始播放相应的动画        
        GameObject oMv = glo_Main.GetInstance().m_ResourceManager.f_Animator(tGameControllDT.szData1);
        if (oMv == null)
        {
            MessageBox.ASSERT("创建动画失败 " + tGameControllDT.szData1);
        }

        //GameEM.TeamType tTeamType = (GameEM.TeamType)tGameControllDT.iTeam;
        ////5.播放动画（参数1为动画对象名称，参数2为角色分配的指定KeyId 参数2为角色模板Id，不填写无效）
        //MvObj2BaseRoleControll(oMv, tGameControllDT.szData2, tGameControllDT.szData3, tTeamType);
        //if (tGameControllDT.iNeedEnd == 1)
        //{
        //    _TimeLineMessageControll = oMv.AddComponent<TimeLineMessageControll>();
        //    _TimeLineMessageControll.f_RegCompleteCallBack(CallBack_PlayMvComplete);
        //}
    }

}
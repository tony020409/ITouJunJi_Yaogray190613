using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using UnityEngine.Playables;

public class GameControllPlayMv : GameControllBaseState
{

    private TimeLineMessageControll _TimeLineMessageControll = null;
    public GameControllPlayMv()
        : base((int)EM_GameControllAction.PlayMv)
    {


    }

    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
        
    }

    protected override void Run(object Obj)
    {
        base.Run(Obj);
        
        //开始播放相应的动画        
        GameObject oMv = glo_Main.GetInstance().m_ResourceManager.f_Animator(_CurGameControllDT.szData1);
        if (oMv == null)
        {
            MessageBox.ASSERT("创建动画失败 " + _CurGameControllDT.szData1);
        }
        //OpenTimeLine(oMv);

        GameEM.TeamType tTeamType = (GameEM.TeamType)_CurGameControllDT.iTeam;
        //5.播放动画（参数1为动画对象名称，参数2为角色分配的指定KeyId 参数2为角色模板Id，不填写无效）
        MvObj2BaseRoleControll(oMv, _CurGameControllDT.szData2, _CurGameControllDT.szData3, tTeamType, 0);
        if (_CurGameControllDT.iNeedEnd == 1)
        {
            _TimeLineMessageControll = oMv.AddComponent<TimeLineMessageControll>();
            _TimeLineMessageControll.f_RegCompleteCallBack(CallBack_PlayMvComplete);
        }
        

    }

    //动画播放结束回调事件
    private void CallBack_PlayMvComplete(object Obj)
    {
        string strMessage = (string)Obj;
        if (strMessage == "OnPlayComplete")
        {
            if (_TimeLineMessageControll != null)
            {
                //CloseTimeLine(_TimeLineMessageControll.gameObject);
                GameObject.Destroy(_TimeLineMessageControll);
                _TimeLineMessageControll = null;
            }
            EndRun();
        }
        else
        {
            MessageBox.DEBUG("无效的TimeLine消息 " + strMessage);
        }
    }

    private void MvObj2BaseRoleControll(GameObject oMv, string strKeyId, string strRoleTemp, GameEM.TeamType tTeamType, float fHeight)
    {        
        CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(ccMath.atoi(strRoleTemp));
        if (tCharacterDT == null)
        {
            MessageBox.ASSERT("无法解析的RoleControll " + strKeyId + " " + strRoleTemp);
            return;
        }
        int iKeyId = ccMath.atoi(strKeyId);
        BaseRoleControllV2 tBaseRoleControl = RoleTools.f_CreateRoleControll(oMv, iKeyId, tTeamType, tCharacterDT, null, fHeight);
        if (tBaseRoleControl == null)
        {
            MessageBox.ASSERT("RoleControll控制对象为空，保存失败");
        }
    }

    private void OpenTimeLine(GameObject oMv)
    {
        PlayableDirector tPlayableDirector = oMv.GetComponent<PlayableDirector>();
        tPlayableDirector.enabled = true;
    }

    private void CloseTimeLine(GameObject oMv)
    {
        PlayableDirector tPlayableDirector = oMv.GetComponent<PlayableDirector>();
        tPlayableDirector.enabled = false;
    }

}

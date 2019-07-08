using ProtoBuf;
using System.Collections;
using System.Collections.Generic;

[ProtoContract]
public class RoleWaitAction :BaseActionV2
{    
    [ProtoMember(10403)]
    public float m_fSleepTime;
    

    public RoleWaitAction()
            : base(GameEM.EM_RoleAction.Wait)
    {
    }

    public void f_Wait(int iRoleId, float fSleepTime)
    {
        m_iRoleId = iRoleId;
        m_fSleepTime = fSleepTime;
    }


    public override void ProcessAction()
    {
        //MessageBox.DEBUG("Wait " + m_iRoleId);
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl != null)
        {
            //MessageBox.DEBUG("Wait " + m_iRoleId);
            tRoleControl.f_RunAIState(AI_EM.EM_AIState.WaitAction, this);           // tRoleControl.f_ChangeAI2Wait(m_fSleepTime);
        }
        else
        {
            MessageBox.ASSERT("Wait 未找到目标 " + m_iRoleId);
        }
    }


}


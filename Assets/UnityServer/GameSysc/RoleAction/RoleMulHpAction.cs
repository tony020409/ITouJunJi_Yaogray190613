
using ProtoBuf;
using System.Collections;
using System.Collections.Generic;

[ProtoContract]
public class RoleMulHpAction : GameSysc.Action
{
    [ProtoMember(11001)]
    public int m_iRoleId;

    /// <summary>
    /// 加血还是减血  0减血 1加血 2
    /// </summary>
    [ProtoMember(11002)]
    public int m_iHpType;

    [ProtoMember(11003)]
    public int[] m_aRoleId;

    [ProtoMember(11004)]
    public int[] m_aRoleData;


    public RoleMulHpAction()
            : base()
    {
        m_iType = (int)GameEM.EM_RoleAction.RoleMulHpAction;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iType">加血还是减血  0减血 1加血 2</param>
    /// <param name="aRoleId"></param>
    /// <param name="aRoleData"></param>
    public void f_SetData(int iType, int[] aRoleId, int[] aRoleData)
    {
        m_aRoleId = aRoleId;
        m_iHpType = iType;
        m_aRoleData = aRoleData;
    }


    public override void ProcessAction()
    {
        for (int i = 0; i < m_aRoleId.Length; i++)
        {
            //MessageBox.DEBUG("Attack " + m_iRoleId);
            BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_aRoleId[i]);
            if (tRoleControl != null)
            {
                MessageBox.DEBUG("MulHp " + m_iRoleId);
                DispRoleData(tRoleControl, m_aRoleData[i]);
            }
            else
            {
                MessageBox.ASSERT("MulHp 未找到目标 " + m_aRoleId[i]);
            }
        }
    }

    private void DispRoleData(BaseRoleControllV2 tRoleControl, int iNum)
    {
        if (m_iHpType == 0)
        {
            tRoleControl.f_BeAttack(iNum, -99, GameEM.EM_BodyPart.Body);
        }
        else if (m_iHpType == 1)
        {
            tRoleControl.f_AddHp(iNum);
        }

    }


}
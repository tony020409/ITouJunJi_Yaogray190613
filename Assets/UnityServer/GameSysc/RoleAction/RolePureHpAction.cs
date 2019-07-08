using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using UnityEngine;


/// <summary>
/// 純對血量進行控制
/// </summary>
[ProtoContract]
public class RolePureHpAction : GameSysc.Action
{
    /// <summary> 0=减血 1=加血 </summary>
    [ProtoMember(11651)]
    public int m_iHpType;

    /// <summary> 加扣血對象 </summary>
    [ProtoMember(11652)]
    public int m_iRoleId;

    /// <summary> 血量變化量 </summary>
    [ProtoMember(11653)]
    public int m_iHp;


    public RolePureHpAction(): base(){
        m_iType = (int)GameEM.EM_RoleAction.PureHp;
    }

    /// <summary>
    /// 加扣血
    /// </summary>
    /// <param name="iRoleId"> 加扣血對象 </param>
    /// <param name="iHp"    > 血量變化量 </param>
    public void f_Hp(int iRoleId, int iHp){
        m_iRoleId = iRoleId;
        m_iHp = iHp;
    }


    /// <summary>
    /// 給予傷害
    /// </summary>
    /// <param name="iRoleId"> 扣血對象 </param>
    /// <param name="iHp"    > 扣多少　　</param>
    public void f_BeAttack(int iRoleId, int iHp){
        m_iRoleId = iRoleId;
        m_iHp = iHp * -1;
    }


    /// <summary>
    /// 增加血量
    /// </summary>
    /// <param name="iRoleId"> 扣血對象 </param>
    /// <param name="iHp"    > 扣多少　　</param>
    public void f_AddHp(int iRoleId, int iHp) {
        m_iRoleId = iRoleId;
        m_iHp = iHp;
    }


    public override void ProcessAction()
    {
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl != null){

            if (m_iHp < 0){
                tRoleControl.f_BeAttack(m_iHp * -1, -99, GameEM.EM_BodyPart.Body); //負負得正
            }
            else if (m_iHp > 0){
                tRoleControl.f_AddHp(m_iHp);
            }

        }
        else{
            MessageBox.ASSERT("Hp 未找到目标 " + m_iRoleId);
        }
    }


}

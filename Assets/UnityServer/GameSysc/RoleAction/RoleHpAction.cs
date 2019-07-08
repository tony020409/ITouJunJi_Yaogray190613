using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using UnityEngine;

[ProtoContract]
public class RoleHpAction : GameSysc.Action
{
    [ProtoMember(10701)]
    public int m_iBeRoleId;

    [ProtoMember(10702)]
    public int m_iHp;
    
    [ProtoMember(10703)]
    public float m_fPosX;

    [ProtoMember(10704)]
    public float m_fPosY;

    [ProtoMember(10705)]
    public float m_fPosZ;

    [ProtoMember(10706)]
    public int m_iBodypart;

    public RoleHpAction(): base(){
        m_iType = (int)GameEM.EM_RoleAction.HP;
    }


    public void f_Hp(int iRoleId, int iBeRoleId, int iHp, Vector3 v3Data = default(Vector3), GameEM.EM_BodyPart tEM_BodyPart = GameEM.EM_BodyPart.Body) {
        m_iRoleId = iRoleId;
        m_iBeRoleId = iBeRoleId;
        m_iHp = iHp;
        m_fPosX = v3Data.x;
        m_fPosY = v3Data.y;
        m_fPosZ = v3Data.z;
        m_iBodypart = (int)tEM_BodyPart;
    }

    public override void ProcessAction() {

        //對怪物傷害
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iBeRoleId);
        if (tRoleControl != null) {
            tRoleControl.f_BeAttack(m_iHp, m_iRoleId, (GameEM.EM_BodyPart)m_iBodypart);
        }

        //如果找不到指定id的怪物，就查找是否是子弹之间的伤害
        else {
            BaseBullet tBaseBullet = BattleMain.GetInstance().m_BulletPool.f_Get(m_iBeRoleId);
            if (tBaseBullet != null) {
                tBaseBullet.f_BeAttack(m_iHp);
            } else {
                //Debug.LogWarning("【警告】RoleHpAction 未找到id編號: " + m_iRoleId + "的怪物或子彈!");
            }
        }
    }


}

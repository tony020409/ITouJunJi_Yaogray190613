using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ProtoContract]
public class RoleThrowAttackAction : GameSysc.Action
{
    [ProtoMember(27101)]
    public int m_iBulletId;

    [ProtoMember(27102)]
    public int m_iTeamType;

    [ProtoMember(27103)]
    public float m_fPosX;

    [ProtoMember(27104)]
    public float m_fPosY;

    [ProtoMember(27105)]
    public float m_fPosZ;

    [ProtoMember(27106)]
    public float m_fQutnX;

    [ProtoMember(27107)]
    public float m_fQutnY;

    [ProtoMember(27108)]
    public float m_fQutnZ;

    [ProtoMember(27109)]
    public float m_fQutnW;

    [ProtoMember(27110)]
    public int m_iBulletDT;


    public RoleThrowAttackAction() : base()
    {
        m_iType = (int)GameEM.EM_RoleAction.ThrowAttack;
    }

    /// <summary>
    /// 攻击
    /// </summary>
    /// <param name="iBulletId">子弹分配的Id</param>
    /// <param name="iRoleId"></param>
    /// <param name="iBulletDT">子弹的模板Id</param>
    /// <param name="tTeamType"></param>
    /// <param name="iBeAttackRoleId"></param>
    /// <param name="Pos"></param>
    /// <param name="tQuaternion"></param>
    /// <param name="Rifleeffectswitch"></param>
    public void f_Attack(int iBulletId, int iRoleId, int iBulletDT, GameEM.TeamType tTeamType,
         Vector3 Pos, Quaternion tQuaternion)
    {
        m_iBulletId = iBulletId;
        m_iRoleId = iRoleId;
        m_iTeamType = (int)tTeamType;
        m_iBulletDT = iBulletDT;
        m_fPosX = Pos.x;
        m_fPosY = Pos.y;
        m_fPosZ = Pos.z;
        m_fQutnX = tQuaternion.x;
        m_fQutnY = tQuaternion.y;
        m_fQutnZ = tQuaternion.z;
        m_fQutnW = tQuaternion.w;
    }


    public override void ProcessAction()
    {
        BaseRoleControllV2 tmpPlayer = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);        //攻擊發動者

        Vector3 v3StartPos = new Vector3(m_fPosX, m_fPosY, m_fPosZ);
        Quaternion rotation = new Quaternion(m_fQutnX, m_fQutnY, m_fQutnZ, m_fQutnW);

        //GameObject oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(); //產生子彈
        BulletDT tBulletDT = (BulletDT)glo_Main.GetInstance().m_SC_Pool.m_BulletSC.f_GetSC(m_iBulletDT);
        BaseBullet tBaseBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(tBulletDT);
        tBaseBullet.transform.position = v3StartPos;                                        //設定子彈位置
        tBaseBullet.transform.rotation = rotation;                                          //設定子彈朝向
        tBaseBullet.f_Fired(m_iBulletId, m_iBulletDT, (GameEM.TeamType)m_iTeamType, tmpPlayer.m_iId);

    }

}
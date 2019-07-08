using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 传输玩家丢出动作
/// </summary>
[ProtoContract]
public class PlayerPushAction : GameSysc.Action
{
    [ProtoMember(10901)]
    public int m_iPushType;       

    [ProtoMember(10904)]
    public float m_fPosX;

    [ProtoMember(10905)]
    public float m_fPosY;

    [ProtoMember(10906)]
    public float m_fPosZ;

    [ProtoMember(10907)]
    public float m_fQutnX;

    [ProtoMember(10908)]
    public float m_fQutnY;

    [ProtoMember(10909)]
    public float m_fQutnZ;

    [ProtoMember(10910)]
    public float m_fQutnW;

    [ProtoMember(10911)]
    public float m_fVelocityX;

    [ProtoMember(10912)]
    public float m_fVelocityY;

    [ProtoMember(10913)]
    public float m_fVelocityZ;

    [ProtoMember(10914)]
    public float m_fAngularVelocityX;

    [ProtoMember(10915)]
    public float m_fAngularVelocityY;

    [ProtoMember(10916)]
    public float m_fAngularVelocityZ;

    [ProtoMember(10917)]
    public float m_fMaxAngularVelocity;



    public PlayerPushAction() : base()
    {
        m_iType = (int)GameEM.EM_RoleAction.PlayerPush;
    }
    
    public void f_PushObject(int iType, Vector3 Pos, Quaternion tQuaternion, Vector3 v3Velocity, Vector3 v3AngularVelocity, float fMaxAngularVelocity)
    {
        m_iPushType = iType;
        m_fVelocityX = v3Velocity.x;
        m_fVelocityY = v3Velocity.y;
        m_fVelocityZ = v3Velocity.z;
        m_fAngularVelocityX = v3AngularVelocity.x;
        m_fAngularVelocityY = v3AngularVelocity.y;
        m_fAngularVelocityZ = v3AngularVelocity.z;
        m_fMaxAngularVelocity = fMaxAngularVelocity;

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
        MessageBox.DEBUG(m_iUserId + "PlayerPushAction " + m_iPushType);

        Vector3 Pos = new Vector3(m_fPosX, m_fPosY, m_fPosZ);
        Quaternion tQuaternion = new Quaternion(m_fQutnX, m_fQutnY, m_fQutnZ, m_fQutnW);
        Vector3 v3Velocity = new Vector3(m_fVelocityX, m_fVelocityY, m_fVelocityZ);
        Vector3 v3AngularVelocity = new Vector3(m_fAngularVelocityX, m_fAngularVelocityY, m_fAngularVelocityZ);
        if (m_iUserId != StaticValue.m_UserDataUnit.m_PlayerDT.m_iId)
        {
            PlayerDT tPlayerDT = Data_Pool.m_TeamPool.f_GetPlayerDT(m_iUserId);
            GameObject tCreateObj = null;
            if ((MonsterType)m_iPushType == MonsterType.Archer_Arrow)
            {
                GameObject oModel = glo_Main.GetInstance().m_aMonsterModel[m_iPushType];
                tCreateObj = (GameObject)GameObject.Instantiate(oModel, Pos, tQuaternion);
                //tCreateObj.GetComponent<PlayArrowWeapenControll>().Fired(tPlayerDT.f_GetTeamType());               
            }
            else
            {
                //tCreateObj = SummonerTools.f_CreateSummon(m_iUserId, (MonsterType)m_iPushType, Pos, tQuaternion);                
            }
            var rigidbody = tCreateObj.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = tCreateObj.AddComponent<Rigidbody>();
            }
            rigidbody.velocity = v3Velocity;
            //rigidbody.angularVelocity = v3AngularVelocity;
            //rigidbody.maxAngularVelocity = m_fMaxAngularVelocity;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }


    }


}

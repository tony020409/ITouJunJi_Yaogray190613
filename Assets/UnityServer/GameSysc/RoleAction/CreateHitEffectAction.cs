using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

//記得到Action.cs加
[ProtoContract]
public class CreateHitEffectAction : GameSysc.Action
{
    [ProtoMember(10850)]
    public int m_FxNumber;      //要創造的特效編號

    //(因為[ProtoMember]不支援 Vector3、Quaternion，所以要拆分開來)
    [ProtoMember(10851)]
    public float m_CreatePosX; //要產生的位置X 
    [ProtoMember(10852)]
    public float m_CreatePosY; //要產生的位置Y
    [ProtoMember(10853)]
    public float m_CreatePosZ; //要產生的位置Z

    //(因為[ProtoMember]不支援 Vector3、Quaternion，所以要拆分開來)
    [ProtoMember(10854)]
    public float m_CreateRotX; //要產生的朝向X
    [ProtoMember(10855)]
    public float m_CreateRotY; //要產生的朝向Y
    [ProtoMember(10856)]
    public float m_CreateRotZ; //要產生的朝向Z
    [ProtoMember(10857)]
    public float m_CreateRotW; //要產生的朝向W



    //設定產生的擊中特效物件
    public void f_SetCreate(int tmpNumber, Vector3 createPos, Quaternion createRot){
        m_FxNumber = tmpNumber;

        m_CreatePosX = createPos.x;
        m_CreatePosY = createPos.y;
        m_CreatePosZ = createPos.z;

        m_CreateRotX = createRot.x;
        m_CreateRotY = createRot.y;
        m_CreateRotZ = createRot.z;
        m_CreateRotW = createRot.w;
    }


    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction(){

        GameObject oBullet = null;
        Vector3 tmpPos = new Vector3(m_CreatePosX, m_CreatePosY, m_CreatePosZ);
        Quaternion tmpRot = new Quaternion(m_CreateRotX, m_CreateRotY, m_CreateRotZ, m_CreateRotW);

        if (m_FxNumber == 0){
            oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateFX_Blood_Small();
        }
        else if (m_FxNumber == 1){
            oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateFX_Blood_Big();
        }
        else if (m_FxNumber == 2){
            oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateFX_Terrain();
        }
        else if (m_FxNumber == 3){
            oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateFX_Wood();
        }
        else if (m_FxNumber == 4){
            oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateFX_Metal();
        }

        if (oBullet != null){
            oBullet.transform.position = tmpPos;
            oBullet.transform.rotation = tmpRot;
        }
        
    }


}

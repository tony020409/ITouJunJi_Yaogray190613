using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;

//記得到Action.cs加
[ProtoContract]
public class CreateResourceAction : BaseActionV2 //GameSysc.Action
{

    [ProtoMember(24001)]
    public string resPath;      //要創造的特效編號

    [ProtoMember(24002)]
    public string resName;      //要創造的特效編號

    //(因為[ProtoMember]不支援 Vector3、Quaternion，所以要拆分開來)
    [ProtoMember(24003)]
    public float m_CreatePosX; //要產生的位置X 
    [ProtoMember(24004)]
    public float m_CreatePosY; //要產生的位置Y
    [ProtoMember(24005)]
    public float m_CreatePosZ; //要產生的位置Z

    //(因為[ProtoMember]不支援 Vector3、Quaternion，所以要拆分開來)
    [ProtoMember(24006)]
    public float m_CreateRotX; //要產生的朝向X
    [ProtoMember(24007)]
    public float m_CreateRotY; //要產生的朝向Y
    [ProtoMember(24008)]
    public float m_CreateRotZ; //要產生的朝向Z
    [ProtoMember(24009)]
    public float m_CreateRotW; //要產生的朝向W


    public CreateResourceAction(): base(GameEM.EM_RoleAction.CreateResource)
    {
    }

    /// <summary>
    /// 設定要創造的資源路徑與名稱
    /// </summary>
    /// <param name="newPath"> 資源在 Resource下的路徑 (e.g. Model/Bullet/) </param>
    /// <param name="newName"> 資源在 Resource下的名稱 (e.g. BusseBullet) </param>
    public void f_GetResource(string newPath, string newName){
        resPath = newPath;
        resName = newName;
    }


    /// <summary>
    /// 設定產生的物件位置與朝向
    /// </summary>
    /// <param name="createPos"></param>
    /// <param name="createRot"></param>
    public void f_SetResource(Vector3 createPos, Quaternion createRot){
        m_CreatePosX = createPos.x; //位置x
        m_CreatePosY = createPos.y; //位置y
        m_CreatePosZ = createPos.z; //位置z
        m_CreateRotX = createRot.x; //朝向x
        m_CreateRotY = createRot.y; //朝向y
        m_CreateRotZ = createRot.z; //朝向z
        m_CreateRotW = createRot.w; //朝向w
    }


    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {
        GameObject oBullet = null;
        oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateResource(resPath, resName);                   //產生資源
        if (oBullet != null) {                                                                                   //如果資源存在
            oBullet.transform.position = new Vector3(m_CreatePosX, m_CreatePosY, m_CreatePosZ);                  //設定資源位置
            oBullet.transform.rotation = new Quaternion(m_CreateRotX, m_CreateRotY, m_CreateRotZ, m_CreateRotW); //設定資源朝向
        }
    }
}

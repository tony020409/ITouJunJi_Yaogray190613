using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;

//記得到Action.cs加
[ProtoContract]
public class Action_Shoot : BaseActionV2 //GameSysc.Action
{

    public Action_Shoot()
        : base(GameEM.EM_RoleAction.CreateResource)
    { }

    [ProtoMember(25001)]
    public string resPath;      //要創造的資源路徑

    [ProtoMember(25002)]
    public string resName;      //要創造的資源名稱

    //(因為[ProtoMember]不支援 Vector3、Quaternion，所以要拆分開來)
    [ProtoMember(25003)]
    public float m_CreatePosX; //要產生的位置X 
    [ProtoMember(25004)]
    public float m_CreatePosY; //要產生的位置Y
    [ProtoMember(25005)]
    public float m_CreatePosZ; //要產生的位置Z

    //(因為[ProtoMember]不支援 Vector3、Quaternion，所以要拆分開來)
    [ProtoMember(25006)]
    public float m_CreateRotX; //要產生的朝向X
    [ProtoMember(25007)]
    public float m_CreateRotY; //要產生的朝向Y
    [ProtoMember(25008)]
    public float m_CreateRotZ; //要產生的朝向Z
    [ProtoMember(25009)]
    public float m_CreateRotW; //要產生的朝向W

    //腳色參數
    [ProtoMember(25010)] public int m_RoleId;     //攻擊發動者的id
    [ProtoMember(25011)] public float m_iEnemyId; //被攻擊者的id

    //子彈的參數
    [ProtoMember(25012)] public int m_TeamType;      //攻擊發動者的隊伍
    [ProtoMember(25013)] public float m_BulletSpeed; //攻擊發動者的子彈生命
    [ProtoMember(25014)] public float m_BulletLife;  //攻擊發動者的id
    [ProtoMember(25015)] public int m_AttackPower;   //攻擊發動者的攻擊力 
    [ProtoMember(25016)] public int m_BulletAmount;  //攻擊發動者的彈藥量
    [ProtoMember(25017)] public int m_BulletID;      //子彈種類

    //目標參數
    [ProtoMember(25018)] public float m_TargetPosX;      //目標座標X
    [ProtoMember(25019)] public float m_TargetPosY;      //目標座標X
    [ProtoMember(25020)] public float m_TargetPosZ;      //目標座標X

    /// <summary>
    /// 設定要創造的資源路徑與名稱
    /// </summary>
    /// <param name="newPath"> 資源在 Resource下的路徑 (e.g. Model/Bullet/) </param>
    /// <param name="newName"> 資源在 Resource下的名稱 (e.g. BusseBullet) </param>
    public void f_GetResource(string newPath, string newName)
    {
        resPath = newPath;
        resName = newName;
    }


    /// <summary>
    /// 設定產生的物件位置與朝向
    /// </summary>
    /// <param name="createPos"></param>
    /// <param name="createRot"></param>
    public void f_SetResource(Vector3 createPos, Quaternion createRot)
    {
        m_CreatePosX = createPos.x; //位置x
        m_CreatePosY = createPos.y; //位置y
        m_CreatePosZ = createPos.z; //位置z
        m_CreateRotX = createRot.x; //朝向x
        m_CreateRotY = createRot.y; //朝向y
        m_CreateRotZ = createRot.z; //朝向z
        m_CreateRotW = createRot.w; //朝向w
    }


    /// <summary>
    /// 設定子彈數值
    /// </summary>
    /// <param name="newTeam" > 攻擊發動者的隊伍 </param>
    /// <param name="newSpeed"> 攻擊發動者的子彈速度 </param>
    /// <param name="newLife" > 攻擊發動者的子彈生命 </param>
    /// <param name="newiId"  > 攻擊發動者的id </param>
    /// <param name="newPower"> 攻擊發動者的攻擊力 </param>
    public void f_SetFire(GameEM.TeamType newTeam, float newSpeed, float newLife, int newiId, int newPower)
    {
        m_TeamType = (int)newTeam;
        m_BulletSpeed = newSpeed;
        m_BulletLife = newLife;
        m_RoleId = newiId;
        m_AttackPower = newPower;
    }


    /// <summary>
    /// 設定開槍的對象 (換彈時用)
    /// </summary>
    /// <param name="newiId"> 攻擊發動者的id </param>
    public void f_SetRoleId(int newiId)
    {
        m_RoleId = newiId;
    }




    /// <summary>
    /// 設定子彈
    /// </summary>
    /// <param name="newBulletID"></param>
    /// <param name="createPos"></param>
    /// <param name="createRot"></param>
    public void f_SetBullet(int newBulletID, Vector3 createPos, Quaternion createRot) {
        m_BulletID = newBulletID;   //子彈種類
        m_CreatePosX = createPos.x; //位置x
        m_CreatePosY = createPos.y; //位置y
        m_CreatePosZ = createPos.z; //位置z
        m_CreateRotX = createRot.x; //朝向x
        m_CreateRotY = createRot.y; //朝向y
        m_CreateRotZ = createRot.z; //朝向z
        m_CreateRotW = createRot.w; //朝向w
    }


    /// <summary>
    /// 設定彈藥變化量
    /// </summary>
    /// <param name="value"> 變化量 </param>
    public void f_SetBulletAmount(int value) {
        m_BulletAmount = value;
    }


    /// <summary>
    /// 設定子彈強制擊中的目標點
    /// </summary>
    /// <param name="tmpPos"></param>
    public void f_SetTarget(Vector3 tmpPos) {
        m_TargetPosX = tmpPos.x;
        m_TargetPosY = tmpPos.y;
        m_TargetPosZ = tmpPos.z;
    }


    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_RoleId);

        //如果角色死了(不存在)
        if (tmpRole == null) {
            return;
        }

        //如果彈藥變化量大於0，表示補彈
        if (m_BulletAmount >= 0) {
            if (tmpRole.GetComponent<Module_Shoot_TwoHand>() != null) {
                tmpRole.GetComponent<Module_Shoot_TwoHand>().BulletAmount = m_BulletAmount; //彈藥量變更
            }

        }

        //否則損失子彈
        else
        {
            if (tmpRole.GetComponent<Module_Shoot_TwoHand>() != null) {
                tmpRole.GetComponent<Module_Shoot_TwoHand>().BulletAmount += m_BulletAmount; //彈藥量變更 (e.g. 加-1)
            }

            if (m_BulletID == 0) {
                MessageBox.ASSERT("角色 " + m_RoleId + "的 " + tmpRole.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name + " 動作召喚非法子彈id-" + m_BulletID);
                return;
            }
            

            BulletDT tBulletDT = (BulletDT)glo_Main.GetInstance().m_SC_Pool.m_BulletSC.f_GetSC(m_BulletID);      //獲取子彈資料(攻擊、速度、存活時間)
            BaseBullet tBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(tBulletDT);             //產生子彈
            tBullet.transform.position = new Vector3(m_CreatePosX, m_CreatePosY, m_CreatePosZ);                  //設定子彈位置
            tBullet.transform.rotation = new Quaternion(m_CreateRotX, m_CreateRotY, m_CreateRotZ, m_CreateRotW); //設定子彈朝向
            tBullet.f_Fired(ccMath.f_CreateKeyId(), m_BulletID, tmpRole.f_GetTeamType(), tmpRole.m_iId);         //子彈擊出

            //GameObject oBullet = null;
            //oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateResource(resPath, resName);                                     //產生資源
            //if (oBullet != null) {                                                                                                     //如果資源存在
            //    oBullet.transform.position = new Vector3(m_CreatePosX, m_CreatePosY, m_CreatePosZ);                                    //設定資源位置
            //    oBullet.transform.rotation = new Quaternion(m_CreateRotX, m_CreateRotY, m_CreateRotZ, m_CreateRotW);                   //設定資源朝向
            //    BaseBullet tBaseBullet = oBullet.GetComponent<BaseBullet>();                                                           //取得子彈元件
            //    //tBaseBullet.f_Fired(, m_BulletSpeed, m_BulletLife, m_RoleId, m_AttackPower);                //子彈發動攻擊
            //    tBaseBullet.f_Fired(ccMath.f_CreateKeyId(), BulletID, (GameEM.TeamType)m_TeamType, tmpRole.m_iId); //子彈擊出
            //}
        }
    }





}

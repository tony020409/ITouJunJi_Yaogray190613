using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ProtoContract]
public class Action_PlayerShoot : GameSysc.Action
{

    //108的編號要改

    [ProtoMember(30001)]
    public int m_iBulletId;

    [ProtoMember(30002)]
    public int m_iTeamType;

    [ProtoMember(30003)]
    public float m_fPosX;

    [ProtoMember(30004)]
    public float m_fPosY;

    [ProtoMember(30005)]
    public float m_fPosZ;

    [ProtoMember(30006)]
    public float m_fQutnX;

    [ProtoMember(30007)]
    public float m_fQutnY;

    [ProtoMember(30008)]
    public float m_fQutnZ;

    [ProtoMember(30009)]
    public float m_fQutnW;

    [ProtoMember(30010)]
    public int m_iBulletDT;

    [ProtoMember(30011)]
    public int m_iGunType;

    [ProtoMember(30012)]
    public int m_iRifleState; //步枪特效开关  0关闭 1开启 2无作用

    [ProtoMember(30013)]
    public int m_iTartgetNumber;



    public Action_PlayerShoot() : base() {
        m_iType = (int)GameEM.EM_RoleAction.ArrowAttack;
    }


    /// <summary>
    /// 玩家攻擊
    /// </summary>
    /// <param name="iRoleId"     > 玩家Id </param>
    /// <param name="tRifleState" > 開火狀態 (0=停火 / 1=開火) </param>
    /// <param name="tGunType"    > 槍枝類型 </param>
    public void f_AttackInit( int iRoleId, int tRifleState, GameEM.GunEM tGunType) {
        m_iRoleId = iRoleId;
        m_iRifleState = tRifleState;
        m_iGunType = (int)tGunType;
    }


    /// <summary>
    /// 設定子彈
    /// </summary>
    /// <param name="iBulletId"  > 子彈名稱(可以使用ccMath.f_CreateKeyId()來隨機產生) </param>
    /// <param name="iBulletDT"  > 子彈類型 </param>
    /// <param name="tPos"       > 子彈產生位置 </param>
    /// <param name="tQuaternion"> 子彈產生朝向 </param>
    public void SetBulletPos( int iBulletId, int iBulletDT,  Vector3 tPos, Quaternion tQuaternion) {
        m_iBulletId = iBulletId;
        m_iBulletDT = iBulletDT;
        m_fPosX = tPos.x;
        m_fPosY = tPos.y;
        m_fPosZ = tPos.z;
        m_fQutnX = tQuaternion.x;
        m_fQutnY = tQuaternion.y;
        m_fQutnZ = tQuaternion.z;
        m_fQutnW = tQuaternion.w;
    }




    public override void ProcessAction()
    {
        BaseRoleControllV2 tmpPlayer = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);         //攻擊發動者

        if (tmpPlayer != null) {
            m_iTeamType = (int)tmpPlayer.f_GetTeamType();                                                    //取得玩家陣營
            Vector3 MuzzlePos = new Vector3(m_fPosX, m_fPosY, m_fPosZ);                                      //取得子彈發射位置
            Quaternion MuzzleRot = new Quaternion(m_fQutnX, m_fQutnY, m_fQutnZ, m_fQutnW);                   //取得子彈發射朝向

            ////步槍
            if (m_iGunType == (int)GameEM.GunEM.Rifle) {
                if (tmpPlayer != null) {
                    MySelfPlayerControll2 _MySelfPlayerControll2 = tmpPlayer.gameObject.GetComponent<MySelfPlayerControll2>();

                    //如果是玩家自己
                    if (_MySelfPlayerControll2 != null) {
                        if (m_iRifleState == 1) {
                            BulletDT tBulletDT = (BulletDT)glo_Main.GetInstance().m_SC_Pool.m_BulletSC.f_GetSC(m_iBulletDT); //子彈數值
                            BaseBullet tBaseBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(tBulletDT);     //產生子彈
                            tBaseBullet.transform.position = MuzzlePos;                                                      //設定子彈位置
                            tBaseBullet.transform.rotation = MuzzleRot;                                                      //設定子彈朝向
                            tBaseBullet.f_Fired(m_iBulletId, m_iBulletDT, (GameEM.TeamType)m_iTeamType, tmpPlayer.m_iId);    //子彈發射
                            _MySelfPlayerControll2.f_AutoGun((GunEM)m_iGunType); //相關特效開啟
                        }
                        else if (m_iRifleState == 0) {
                            _MySelfPlayerControll2.f_StopGun((GunEM)m_iGunType); //相關特效關閉
                            return;
                        }
                    }



                    //如果是其他玩家
                    else {
                        OtherPlayerControll2 tOtherPlayerControll2 = tmpPlayer.gameObject.GetComponent<OtherPlayerControll2>();
                        if (tOtherPlayerControll2 != null) {
                            if (m_iRifleState == 1) {
                                tOtherPlayerControll2.f_AutoGun((GunEM)m_iGunType); //相關特效開啟
                            }
                            else if (m_iRifleState == 0) {
                                tOtherPlayerControll2.f_StopGun((GunEM)m_iGunType); //相關特效關閉
                                return;
                            }
                        }
                    }
                }
            }


            ////激光枪
            //else if (m_iGunType == (int)GameEM.GunEM.Lasergun) {
            //    GameObject tNeedlegunObj = glo_Main.GetInstance().m_ResourceManager.f_NeedlegunBullet();
            //    tNeedlegunObj.transform.position = v3StartPos;
            //    tNeedlegunObj.transform.rotation = rotation;
            //    //if (tBeRoleControl.f_GetRoleType() == GameEM.emRoleType.Trex)  {
            //    //    Needlegun tNeedlegun = tNeedlegunObj.GetComponent<Needlegun>();
            //    //    Enemy tEnemy = tBeRoleControl.gameObject.GetComponent<Enemy>();
            //    //    tNeedlegun.f_Fired(tBeRoleControl, GameEM.TeamType.A, 50, 5, tRoleControl.f_GetAttackPower(), tEnemy.f_GetCurTargetObj(m_iTartgetNumber));
            //    //}
            //}

            ////导弹攻击
            //else if (m_iGunType == (int)GameEM.GunEM.Needlegun) {
            //    //var N = glo_Main.GetInstance().m_ResourceManager.f_NeedlegunBullet();
            //    //N.transform.position = v3StartPos;
            //    //N.transform.rotation = rotation;
            //    //var T = tRoleControl.gameObject.GetComponent<Enemy>();
            //    ////detectionAims.target = GameObject.Find("Role/" + AimsID).GetComponent<Enemy>();
            //    //// detectionAims.target.getAttackSite(ccMath.atoi(AttackSite) - 1);
            //    ////m_fTargetX
            //    //N.GetComponent<Needlegun>().getTarget(T.ArrowArray[T._iTargetIndex]);
            //    //N.GetComponent<Needlegun>().f_Fired(m_iTeamType, 50, 5, tRoleControl.f_GetAttackPower());
            //}
        }


        else {
            MessageBox.ASSERT("ArrowAttack 未找到目标 " + m_iRoleId);
        }
    }


}

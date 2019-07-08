using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ProtoContract]
public class RoleArrowAttackAction : GameSysc.Action
{
    [ProtoMember(10801)]
    public int m_iBulletId;

    [ProtoMember(10802)]
    public int m_iTeamType;

    [ProtoMember(10803)]
    public int m_iBeAttackRoleId;

    [ProtoMember(10804)]
    public float m_fPosX;

    [ProtoMember(10805)]
    public float m_fPosY;

    [ProtoMember(10806)]
    public float m_fPosZ;

    [ProtoMember(10807)]
    public float m_fQutnX;

    [ProtoMember(10808)]
    public float m_fQutnY;

    [ProtoMember(10809)]
    public float m_fQutnZ;

    [ProtoMember(10810)]
    public float m_fQutnW;

    [ProtoMember(10811)]
    public int m_iBulletDT;

    [ProtoMember(10812)]
    public int m_iRifleeffectswitch; //步枪特效开关  0关闭 1开启 2无作用

    [ProtoMember(10813)]
    public int m_iTartgetNumber;


    [ProtoMember(10814)]
    public int m_iGunType; //槍枝類型


    public RoleArrowAttackAction() : base() {
        m_iType = (int)GameEM.EM_RoleAction.ArrowAttack;
    }



    /// <summary>
    /// 玩家子彈射擊
    /// </summary>
    /// <param name="iBulletId"        > 子彈被分配到的名稱 </param>
    /// <param name="iRoleId"          > 玩家id(攻擊發動者) </param>
    /// <param name="iBulletDT"        > 子彈模板id </param>
    /// <param name="tTeamType"        > 玩家隊伍 </param>
    /// <param name="Pos"              > 玩家槍口位置 </param>
    /// <param name="tQuaternion"      > 玩家槍口角度 </param>
    /// <param name="tGunType"         > 玩家槍枝類型 </param>
    /// <param name="Rifleeffectswitch"> 開火狀態 (1=開 / 0=關) </param>
    /// <param name="iBeAttackRoleId"  > 被攻擊者id(目前沒用到) </param>
    public void f_Attack(int iBulletId, int iRoleId, int iBulletDT, GameEM.TeamType tTeamType,
        Vector3 Pos, Quaternion tQuaternion, GunEM tGunType, int Rifleeffectswitch, int iBeAttackRoleId = 0)
    {
        m_iBulletId = iBulletId;
        m_iRoleId = iRoleId;
        m_iTeamType = (int)tTeamType;
        m_iBulletDT = iBulletDT;
        m_iBeAttackRoleId = iBeAttackRoleId;
        m_fPosX = Pos.x;
        m_fPosY = Pos.y;
        m_fPosZ = Pos.z;
        m_fQutnX = tQuaternion.x;
        m_fQutnY = tQuaternion.y;
        m_fQutnZ = tQuaternion.z;
        m_fQutnW = tQuaternion.w;
        m_iGunType = (int)tGunType;
        m_iRifleeffectswitch = Rifleeffectswitch;
    }

    ///// <summary>
    ///// 制式攻擊
    ///// </summary>
    //public void f_Attack( int iRoleId, GameEM.GunEM tGunEM, GameEM.TeamType tTeamType, 
    //    int iBeAttackRoleId, Vector3 Pos, Quaternion tQuaternion, int Rifleeffectswitch)
    //{
    //    m_iRoleId = iRoleId;
    //    m_iTeamType = (int)tTeamType;
    //    m_iGunType = (int)tGunEM;
    //    m_iBeAttackRoleId = iBeAttackRoleId;
    //    m_fPosX = Pos.x;
    //    m_fPosY = Pos.y;
    //    m_fPosZ = Pos.z;
    //    m_fQutnX = tQuaternion.x;
    //    m_fQutnY = tQuaternion.y;
    //    m_fQutnZ = tQuaternion.z;
    //    m_fQutnW = tQuaternion.w;
    //    m_iRifleeffectswitch = Rifleeffectswitch;
    //}


    ///// <summary>
    ///// 雷射槍攻擊
    ///// </summary>
    //public void f_LaserGunAttack ( int iRoleId,  GameEM.GunEM tGunEM,  GameEM.TeamType tTeamType, 
    //    int iBeAttackRoleId, Vector3 Pos, Quaternion tQuaternion, int _fTartgetNumber)
    //{
    //    m_iRoleId = iRoleId;
    //    m_iTeamType = (int)tTeamType;
    //    m_iGunType = (int)tGunEM;
    //    m_iBeAttackRoleId = iBeAttackRoleId;
    //    m_fPosX = Pos.x;
    //    m_fPosY = Pos.y;
    //    m_fPosZ = Pos.z;
    //    m_fQutnX = tQuaternion.x;
    //    m_fQutnY = tQuaternion.y;
    //    m_fQutnZ = tQuaternion.z;
    //    m_fQutnW = tQuaternion.w;
    //    m_iTartgetNumber = _fTartgetNumber;
    //}



    public override void ProcessAction()
    {
        BaseRoleControllV2 tmpEnemy = BattleMain.GetInstance().f_GetRoleControl2(m_iBeAttackRoleId); //被攻擊者
        BaseRoleControllV2 tmpPlayer = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);       //攻擊發動者
        MySelfPlayerControll2 _MySelfPlayerControll2 = tmpPlayer.GetComponent<MySelfPlayerControll2>();
        OtherPlayerControll2 _OtherPlayerControll2 = tmpPlayer.gameObject.GetComponent<OtherPlayerControll2>();

        //射出子彈
        if (tmpPlayer != null && m_iRifleeffectswitch != 0) {

            Vector3 v3StartPos = new Vector3(m_fPosX, m_fPosY, m_fPosZ);                                     //取得子彈起點
            Quaternion rotation = new Quaternion(m_fQutnX, m_fQutnY, m_fQutnZ, m_fQutnW);                    //取得子彈朝向

            BulletDT tBulletDT = (BulletDT)glo_Main.GetInstance().m_SC_Pool.m_BulletSC.f_GetSC(m_iBulletDT); //取得子彈資訊
            BaseBullet tBaseBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(tBulletDT);     //產生子彈
            tBaseBullet.transform.position = v3StartPos;                                                     //設定子彈位置
            tBaseBullet.transform.rotation = rotation;                                                       //設定子彈朝向
            tBaseBullet.f_Fired(m_iBulletId, m_iBulletDT, (GameEM.TeamType)m_iTeamType, tmpPlayer.m_iId);    //子彈擊出

            //排行榜
            Data_Pool.m_PlayerPool.f_Shot(m_iRoleId);

            ////步槍
            //if (m_iGunType == (int)GameEM.GunEM.Rifle) {
            //    if (tmpPlayer != null)
            //    {
            //        if (tmpPlayer.f_GetRoleType() == GameEM.emRoleType.Player) {
            //            MySelfPlayerControll2 _MySelfPlayerControll2 = tmpPlayer.gameObject.GetComponent<MySelfPlayerControll2>();

            //            //如果是其他玩家
            //            if (_MySelfPlayerControll2 == null)   {
            //                OtherPlayerControll2 tOtherPlayerControll2 = tmpPlayer.gameObject.GetComponent<OtherPlayerControll2>();
            //                if (tOtherPlayerControll2 != null) {
            //                    if (m_iRifleeffectswitch == 1) {
            //                        tOtherPlayerControll2.f_AutoGun();
            //                    }
            //                    else if (m_iRifleeffectswitch == 0) {
            //                        tOtherPlayerControll2.f_StopGun();
            //                        return;
            //                    }
            //                }
            //            }

            //            //如果是玩家自己
            //            else  {
            //                if (m_iRifleeffectswitch == 1){
            //                    GameObject oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(); //產生子彈
            //                    oBullet.transform.position = v3StartPos;                                        //設定子彈位置
            //                    oBullet.transform.rotation = rotation;                                          //設定子彈朝向
            //                    BaseBullet tBaseBullet = oBullet.GetComponent<BaseBullet>();                    //取得子彈元件
            //                    tBaseBullet.f_Fired((GameEM.TeamType)m_iTeamType, _MySelfPlayerControll2.BulletSpeed, _MySelfPlayerControll2.BulletLife, tmpPlayer.m_iId, tmpPlayer.f_GetAttackPower());
            //                    _MySelfPlayerControll2.f_AutoGun();
            //                }
            //                else if (m_iRifleeffectswitch == 0)  {
            //                    _MySelfPlayerControll2.f_StopGun();
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}


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


        //其他玩家的情形
        if (_OtherPlayerControll2 != null)  {
            if (m_iRifleeffectswitch == 1)  {
                _OtherPlayerControll2.f_AutoGun((GunEM)m_iGunType);
            }
            else if (m_iRifleeffectswitch == 0){
                _OtherPlayerControll2.f_StopGun((GunEM)m_iGunType);
                return;
            }
        }

    }

}
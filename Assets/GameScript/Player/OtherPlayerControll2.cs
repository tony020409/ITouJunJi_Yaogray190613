using GameControllAction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerControll2 : BaseRoleControllV2
{
    public GameObject m_oLeftHand;
    public GameObject m_oRightHand;
    public GameObject m_oHead;
    public GameObject m_oFootL;
    public GameObject m_oFootR;
    public GameObject m_oOther1;
    public GameObject m_oOther2;

    [Line()]
    [Rename("補給品偵測點")] public Transform m_BulletStart;
    [Line()]
    [Rename("玩家當前持有的槍")] public GameObject _CurGunObject;
    [Line()]
    [Rename("手槍模型")]                      public GameObject RifleGameObject;
    [Rename("手槍開火特效 (物件)")]           public GameObject Rifleeffect;
    [Rename("手槍開火特效 (vp_MuzzleFlash)")] public vp_MuzzleFlash _vp_MuzzleFlash;
    [Rename("手槍開槍聲 (AimSound)")]         public AimSound.GunSoundSource Ani_Attack1;
    [Line()]
    [Rename("衝鋒槍模型")]     public GameObject SubmachineGameObject;
    [Rename("衝鋒槍開火特效")] public GameObject SubmachineFx;
    [Line()]
    [Rename("正常的模型")]   public GameObject _BodyAlive;
    [Rename("死掉時的模型")] public GameObject _BodyDie;
    [Line()]
    [Rename("導航箭頭")] public GameObject _GuildObj;

    /// <summary>
    /// OtherPlayer持有的槍支列表
    /// </summary>
    private Dictionary<GunEM, GameObject> oGunList = new Dictionary<GunEM, GameObject>();
    private void Awake() {
        f_GetBodyCollider();
        RegOtherPlayerGun();
    }


    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true){
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight, bUpdatePos);
        if (this.name != "0") {
            m_oOther1.SetActive(false);
            m_oOther2.SetActive(false);
        }
    }



    /// <summary>
    /// 註冊 OtherPlayer持有哪些槍、槍對應的模型
    /// </summary>
    private void RegOtherPlayerGun() {
        Save(GunEM.Rifle, RifleGameObject);
        Save(GunEM.Submachine, SubmachineGameObject);
        _CurGunObject = RifleGameObject;
    }


    #region 玩家动作数据
    /// <summary>
    /// 接收来至玩家动作Socket的动作数据
    /// </summary>
    /// <param name="tPlayerTransformAction"></param>
    public void f_UpdatePlayerTransform(PlayerTransformAction tPlayerTransformAction){
        UpdateHead(tPlayerTransformAction);
        UpdateArmL(tPlayerTransformAction);
        UpdateArmR(tPlayerTransformAction);
        UpdateFootL(tPlayerTransformAction);
        UpdateFootR(tPlayerTransformAction);
        UpdateOther1(tPlayerTransformAction);
        UpdateOther2(tPlayerTransformAction);
        UpdateTransform(tPlayerTransformAction);
        //UpdateBody(tPlayerTransformAction);
    }


    Vector3 Pos = new Vector3();
    private void UpdateHead(PlayerTransformAction tPlayerTransformAction) {
        Pos.x = tPlayerTransformAction.m_fHeadPosX;
        Pos.y = tPlayerTransformAction.m_fHeadPosY;
        Pos.z = tPlayerTransformAction.m_fHeadPosZ;
        m_oHead.transform.position = Pos;
        //iTween.MoveTo(gameObject, Pos, GloData.glo_fMaxControllFrameTime);

        Vector3 v3Quaternion = transform.rotation.eulerAngles;
        Quaternion tQuaternion = Quaternion.Euler(transform.rotation.eulerAngles.x, tPlayerTransformAction.m_fHeadQutnY, transform.rotation.eulerAngles.z);
        //tQuaternion.x = tPlayerTransformAction.m_fHeadQutnX;
        //v3Quaternion.y = tPlayerTransformAction.m_fHeadQutnY;
        //tQuaternion.z = tPlayerTransformAction.m_fHeadQutnZ;
        //tQuaternion.w = tPlayerTransformAction.m_fHeadQutnW;
        transform.rotation = tQuaternion;
        //iTween.RotateTo(gameObject, v3Quaternion, GloData.glo_fMaxControllFrameTime);

    }


    private void UpdateArmL(PlayerTransformAction tPlayerTransformAction) {
        //Vector3 Pos = new Vector3(tPlayerTransformAction.m_fArmLPosX, tPlayerTransformAction.m_fArmLPosY, tPlayerTransformAction.m_fArmLPosZ);
        Pos.x = tPlayerTransformAction.m_fArmLPosX;
        Pos.y = tPlayerTransformAction.m_fArmLPosY;
        Pos.z = tPlayerTransformAction.m_fArmLPosZ;
        m_oLeftHand.transform.position = Pos;
        Quaternion tQuaternion = Quaternion.Euler(tPlayerTransformAction.m_fArmLQutnX, tPlayerTransformAction.m_fArmLQutnY, tPlayerTransformAction.m_fArmLQutnZ);
        m_oLeftHand.transform.rotation = tQuaternion;
        //iTween.MoveTo(m_oArmL, Pos, GloData.glo_fMaxControllFrameTime);
    }


    private void UpdateArmR(PlayerTransformAction tPlayerTransformAction) {
        //Vector3 Pos = new Vector3(tPlayerTransformAction.m_fArmRPosX, tPlayerTransformAction.m_fArmRPosY, tPlayerTransformAction.m_fArmRPosZ);
        Pos.x = tPlayerTransformAction.m_fArmRPosX;
        Pos.y = tPlayerTransformAction.m_fArmRPosY;
        Pos.z = tPlayerTransformAction.m_fArmRPosZ;
        m_oRightHand.transform.position = Pos;
        Quaternion tQuaternion = Quaternion.Euler(tPlayerTransformAction.m_fArmRQutnX, tPlayerTransformAction.m_fArmRQutnY, tPlayerTransformAction.m_fArmRQutnZ);
        m_oRightHand.transform.rotation = tQuaternion;
        //iTween.MoveTo(m_oArmR, Pos, GloData.glo_fMaxControllFrameTime);
    }


    private void UpdateTransform(PlayerTransformAction tPlayerTransformAction) {
        Pos.x = tPlayerTransformAction.m_TransformX;
        Pos.y = tPlayerTransformAction.m_TransformY;
        Pos.z = tPlayerTransformAction.m_TransformZ;
        transform.position = Pos;
    }

    private void UpdateFootL(PlayerTransformAction tPlayerTransformAction){
        if (m_oFootL == null) {
            return;
        }
        //Vector3 Pos = new Vector3(tPlayerTransformAction.m_fArmLPosX, tPlayerTransformAction.m_fArmLPosY, tPlayerTransformAction.m_fArmLPosZ);
        Pos.x = tPlayerTransformAction.m_fFootLPosX;
        Pos.y = tPlayerTransformAction.m_fFootLPosY;
        Pos.z = tPlayerTransformAction.m_fFootLPosZ;
        m_oFootL.transform.position = Pos;
        Quaternion tQuaternion = Quaternion.Euler(tPlayerTransformAction.m_fFootLQutnX, tPlayerTransformAction.m_fFootLQutnY, tPlayerTransformAction.m_fFootLQutnZ);
        m_oFootL.transform.rotation = tQuaternion;
        //iTween.MoveTo(m_oArmL, Pos, GloData.glo_fMaxControllFrameTime);
    }


    private void UpdateFootR(PlayerTransformAction tPlayerTransformAction) {
        if (m_oFootR == null) {
            return;
        }
        //Vector3 Pos = new Vector3(tPlayerTransformAction.m_fArmLPosX, tPlayerTransformAction.m_fArmLPosY, tPlayerTransformAction.m_fArmLPosZ);
        Pos.x = tPlayerTransformAction.m_fFootRPosX;
        Pos.y = tPlayerTransformAction.m_fFootRPosY;
        Pos.z = tPlayerTransformAction.m_fFootRPosZ;
        m_oFootR.transform.position = Pos;
        Quaternion tQuaternion = Quaternion.Euler(tPlayerTransformAction.m_fFootRQutnX, tPlayerTransformAction.m_fFootRQutnY, tPlayerTransformAction.m_fFootRQutnZ);
        m_oFootR.transform.rotation = tQuaternion;
        //iTween.MoveTo(m_oArmL, Pos, GloData.glo_fMaxControllFrameTime);
    }

    private void UpdateOther1(PlayerTransformAction tPlayerTransformAction) {
        if (m_oOther1 == null) {
            return;
        }
        //Vector3 Pos = new Vector3(tPlayerTransformAction.m_fArmLPosX, tPlayerTransformAction.m_fArmLPosY, tPlayerTransformAction.m_fArmLPosZ);
        Pos.x = tPlayerTransformAction.m_fOtherPos1X;
        Pos.y = tPlayerTransformAction.m_fOtherPos1Y;
        Pos.z = tPlayerTransformAction.m_fOtherPos1Z;
        m_oOther1.transform.position = Pos;
        Quaternion tQuaternion = Quaternion.Euler(tPlayerTransformAction.m_fOtherRQutn1X, tPlayerTransformAction.m_fOtherRQutn1Y, tPlayerTransformAction.m_fOtherRQutn1Z);
        m_oOther1.transform.rotation = tQuaternion;
    }

    private void UpdateOther2(PlayerTransformAction tPlayerTransformAction){
        if (m_oOther2 == null) {
            return;
        }
        //Vector3 Pos = new Vector3(tPlayerTransformAction.m_fArmLPosX, tPlayerTransformAction.m_fArmLPosY, tPlayerTransformAction.m_fArmLPosZ);
        Pos.x = tPlayerTransformAction.m_fOtherPos2X;
        Pos.y = tPlayerTransformAction.m_fOtherPos2Y;
        Pos.z = tPlayerTransformAction.m_fOtherPos2Z;
        m_oOther2.transform.position = Pos;
        Quaternion tQuaternion = Quaternion.Euler(tPlayerTransformAction.m_fOtherRQutn2X, tPlayerTransformAction.m_fOtherRQutn2Y, tPlayerTransformAction.m_fOtherRQutn2Z);
        m_oOther2.transform.rotation = tQuaternion;
    }
    #endregion


    #region 其他玩家槍隻相關 (換槍、開火、停火特效)
    /// <summary>
    /// 保存OtherPlayer持有槍的訊息
    /// </summary>
    /// <param name="emName"> 類型 </param>
    /// <param name="Obj"   > 對應的模型 </param>
    private void Save(GunEM emName, GameObject Obj)  {
        if (oGunList.ContainsKey(emName)) {
            MessageBox.DEBUG("OtherPlayer下面存在同類型的槍枝，" + emName);
        } else {
            oGunList.Add(emName, Obj);
        }
    }


    /// <summary>
    /// (OtherPlayer) 換槍 (只換模型)
    /// </summary>
    public void f_ChangeGun(GunEM tGunType) { 
        if (oGunList.ContainsKey(tGunType)) {   //如果玩家持有對應類型的槍隻
            _CurGunObject.SetActive(false);     //原本的槍關閉
            _CurGunObject = oGunList[tGunType]; //更新槍枝模型
            _CurGunObject.SetActive(true);      //顯示更新後的槍枝模型
        }
    }


    /// <summary>
    /// (OtherPlayer) 開火
    /// </summary>
    /// <param name="tGunType"> 槍枝類型 </param>
    public override void f_AutoGun(GunEM tGunType) {
        if (tGunType == GunEM.Rifle) {
            Ani_Attack1.Play(1);
            _vp_MuzzleFlash.Shoot();
            //Rifleeffect.gameObject.SetActive(true);
        }
        else if (tGunType == GunEM.Submachine){
            Ani_Attack1.Play();
            SubmachineFx.SetActive(true);
        }
        else if (tGunType == GunEM.Sniper) {

        }
    }

    /// <summary>
    /// (OtherPlayer) 停火
    /// </summary>
    /// <param name="tGunType"> 槍枝類型 </param>
    public override void f_StopGun(GunEM tGunType){
        if (tGunType == GunEM.Rifle) {
            Ani_Attack1.Stop();
            //Rifleeffect.gameObject.SetActive(false);
        }
        else if (tGunType == GunEM.Submachine){
            Ani_Attack1.Stop();
            SubmachineFx.SetActive(false);
        }
        else if (tGunType == GunEM.Sniper) {

        }
    }
    #endregion


    #region 其他玩家的復活與死亡 (更改模型、開關碰撞)
    public override void f_Rebirth() {
        base.f_Rebirth();
        f_SetBodyCollider(true);    //打開碰撞
        _BodyAlive.SetActive(true); //打開活著時的模型
        _BodyDie.SetActive(false);  //關閉死掉時的模型
    }


    public override void f_Die(){
        base.f_Die();
        f_SetBodyCollider(false);    //關閉碰撞
        _BodyAlive.SetActive(false); //關閉活著時的模型
        _BodyDie.SetActive(true);    //打開死掉時的模型
    }
    #endregion


    #region 玩家Y向上弹起
    /// <summary>
    /// 玩家受到向上弹起的力
    /// </summary>
    /// <param name="fPower">弹起来花费的时间</param>
    /// /// <param name="fHeight">被弹起来高度</param>
    public override void f_Spring(float fPower, float fHeight)
    {
        //Vector3 Pos = transform.position;
        //Pos.y += fHeight;
        //iTween.MoveTo(this.gameObject, iTween.Hash(
        //                "position", Pos,
        //                "speed", fPower,
        //                //"orienttopath", true,
        //                "axis", "y",
        //                "easetype", "linearTween",
        //                "oncomplete", "ccCallBackSpringComplete",
        //                "oncompletetarget", gameObject
        //                ));

        //_bIsSpring = true;

        ////向其它玩家发送弹起来的消息
        ////RoleSpringAction tRoleSpringAction = new RoleSpringAction();
        ////tRoleSpringAction.f_Send2OtherPlayer(m_iId, fPower, fHeight);
        ////f_AddMyAction(tRoleSpringAction);

    }

    /// <summary>
    /// 受到来此外部的坐标变更，用来实现类似被抓起
    /// </summary>
    public override void f_SpringPush(float fY)
    {
        //Vector3 Pos = transform.position;
        //Pos.y = fY;
        //transform.position = Pos;

        //_bIsSpring = true;
    }

    /// <summary>
    /// 从弹起的空中回到地面
    /// </summary>
    /// <param name="fPower"></param>
    public override void f_SpringReset(float fPower)
    {
        //Vector3 Pos = transform.position;
        //Pos.y = GloData.glo_fPlayerDefaultY;
        //iTween.MoveTo(this.gameObject, iTween.Hash(
        //                "position", Pos,
        //                "speed", fPower,
        //                //"orienttopath", true,
        //                "axis", "y",
        //                "easetype", "easeInOutExpo",
        //                "oncomplete", "ccCallBackSpringResetComplete",
        //                "oncompletetarget", gameObject
        //                ));
    }

    /// <summary>
    /// 被弹起结束回调
    /// </summary>
    private void ccCallBackSpringComplete()
    {
        f_SpringReset(1);
    }

    /// <summary>
    /// 返回到地面回调
    /// </summary>
    private void ccCallBackSpringResetComplete()
    {
        //_bIsSpring = false;
    }

    #endregion


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllAction;
using ccU3DEngine;
using UnityEngine.UI;
using Sam;
using DG.Tweening;
using HTC.UnityPlugin.Vive;

/// <summary>
/// 開槍類型 (單發與連發)
/// </summary>
public enum RifleEM
{
    single,
    continuous,
}


public class MySelfPlayerControll2 : BaseRoleControllV2
{
    [HideInInspector]
    public GunControll _GunControll = null;
    public static MySelfPlayerControll2 staticMySelfPlayerControll2;

    [Header("--------【身體組件】--------")] //同步給OtherPlayer的
    public GameObject m_oLeftHand;    //左手控制器 - Controller (left)
    public GameObject m_oRightHand;   //右手控制器 - Controller (right)
    public GameObject m_oHead;        //頭盔 - Camera (head)
    public GameObject m_oIKRightHand; //玩家IK的右手控制物件
    public GameObject m_oIKHead;      //玩家IK的頭部控制物件
    public GameObject m_oVRBody;      //Player[CameraRig]
    public GameObject m_oOther1;      //其他物件1
    public GameObject m_oOther2;      //其他物件2
    PlayerTransformAction _PlayerTransformAction = new PlayerTransformAction();


    #region 小胖加入的参数
    [Header("--------【武器參數】--------")]
    [Rename("開槍槍口")]           public GameObject m_BulletStart;
    [Rename("目前槍類型")]         public GunEM GunEM;
    public bool RifleEMbool;
    public bool Riflebool = true;
    [Rename("開槍模式")] public RifleEM REM;

    [Header("--------【手槍參數】--------")]
    [Rename("手槍模型")] public GameObject RifleObj;
    [Rename("手槍模型")] public GameObject RifledGunObj;
    public LineRenderer Rifle_Laser;
    public GameObject Rifleeffect;
    public vp_MuzzleFlash _vp_MuzzleFlash;
    [Rename("手槍動畫機")] public Animator Rifle_animator;
    [Rename("手槍噴的彈殼")] public GameObject pistolShell;
    [Rename("手槍噴彈殼的位置")] public GameObject pistolShellPoint;
    [Rename("手槍沒子彈聲")]   public AudioClip HG_EmptySound;

    [Header("--------【雷射槍參數】--------")]
    [Rename("雷射槍模型")]
    public GameObject LasergunGameObject;
    public LineRenderer Lasergunlaser;
    public LayerMask LM;
    public Color TrackColor;
    public Color LasergunlaserTargetedColor;
    public Color LasergunlaserNotaimingColor;
    public float LasergunlaserBulletMaxCDTime = 5;
    public float LasergunlaserBulletNowCDTime = 0;
    public bool  LasergunlaserBulletbool = false;
    [Rename("雷射槍CD跑條")]        public Image LasergunlaserBulletImage;
    [Rename("雷射槍彈藥數 (文字)")] public Text LasergunlaserBullettext;

    [Header("--------【刺針槍參數】--------")]
    [Rename("刺針槍模型")]
    public GameObject   NeedlegunGameObject;
    public AudioSource  NeedlegunAudioSource;
    public LineRenderer Needlegunlaser;

    [Header("--------【炸彈參數】--------")]
    [Rename("炸彈模型")]       public GameObject BombObj;
    [Rename("炸彈安裝指示線")] public LineRenderer BombLaser;
    [Rename("炸彈位置正確顏色")] public Color BombLaser_CatchColor;
    [Rename("炸彈位置錯誤顏色")] public Color BombLaser_NullColor;
    [Rename("炸彈追瞄 UI")] public Image Bomb_TrackImage;
    [Rename("炸彈偵測點")]  public Transform Bomb_Trigger;
    [Rename("炸彈安裝時間")] public float BombPlaceTime = 3.0f;
    [Rename("安裝完成語音")] public AudioClip BombCompleteAudio;

    [Header("--------【狙擊槍參數】--------")]
    [Rename("狙擊槍模型")] public GameObject SniperObj;
    [Rename("狙擊槍槍口")] public GameObject SniperGun_fire_ins;
    [Rename("狙擊槍animator")] public Animator SniperGun_animator;
    public LineRenderer Sniper_Laser;
    public GameObject Snipereffect;
    public vp_MuzzleFlash Sniper_vp_MuzzleFlash;
    public AudioSource Sniper_m_AudioSource;

    [Header("--------【衝鋒槍參數】--------")]
    [Rename("衝鋒槍模型")]         public GameObject SubmachineObj;
    [Rename("衝鋒槍槍口")]         public GameObject SubmachineGun_fire_ins;
    [Rename("衝鋒槍火光特效")]     public GameObject SubmachineGun_effect;
    [Rename("衝鋒槍噴彈殼的位置")] public GameObject SubmachineShellPoint;
    [Rename("衝鋒槍沒子彈聲")]     public AudioClip  SMG_EmptySound;


    [Header("--------【UI物件】--------")]
    [Rename("UI節點 (開關顯示用)")] public GameObject UI_Root;
    [Rename("追瞄或讀取UI (開關顯示用)")] public GameObject trackImageGameObject;
    [Rename("追瞄或讀取UI (跑條)")]       public Image trackImage;
    [Rename("血量 (跑條)")] public Image HPImage;
    [Rename("血量 (文字)")] public Text HPtext;
    [Rename("子彈數 (跑條)")] public Image BulletImage;
    [Rename("子彈數 (文字)")] public Text Bullettext;
    [Rename("彈夾數 (文字)")] public Text ClipText;
    [Rename("復活數 (文字)")] public Text HaveLifeText;
    [Rename("死亡且復活數0的通知")] public GameObject DieTipObj;

    [Header("--------【未分類】--------")]
    [Rename("m_AudioSource")]  public AudioSource m_AudioSource;
    [Rename("開槍的音效插件")] public AimSound.GunSoundSource Ani_Attack1;
    public static bool Damageswitch;
    public DetectionAims detectionAims;
    [Rename("任務目標")]       public string NowAimsID;
    [Rename("菜單鍵開關狀態")] public bool MenuPressedState;
    private float alpha = 1.0f;      //文字閃爍用
    private bool alphaFinish = true; //文字閃爍用



    [Header("--------【驗證參數】--------")]
    [Rename("是否進行驗證計次")]     public bool Verify = false;
    #endregion

    [Header("--------死亡與受傷效果--------")]
    [Rename("受傷時的效果")]          public Image screen_Hit;
    [Rename("Lutify (死亡黑畫面用)")] public Lutify _Lutify;
    /// <summary>
    /// false=重生倒數開始, true=不重生倒數
    /// </summary>
    public bool _Lutifybool;
    public float fDelayTime; //重生倒數的進度
    [Rename("死亡倒數物件")] public GameObject DeathreciprocalGameObject;
    [Rename("死亡倒數跑條")] public Image Deathreciprocal;

    [Header("------玩家被彈飛的參數------")]
    [Rename("彈飛上去的時間")]   public float fPower = 1;
    [Rename("彈飛時的最高高度")] public float flyHeight = 4;

    [Header("------玩家 Tracker的參數------")]
    public Transform Tracker_1;
    public Transform Tracker_2;


    void Awake() {
        staticMySelfPlayerControll2 = this;
    }

    /// <summary>
    /// 角色初始化
    /// </summary>
    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true) {
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight, bUpdatePos);
        _PlayerTransformAction.f_Save(iId, 0);
        ccTimeEvent.GetInstance().f_RegEvent(GloData.glo_fMaxControllFrameTime, true, null, CallBack_TransPosInfor);
        InitGun();
        _Lutifybool = true;
        DeathreciprocalGameObject.SetActive(false);
        f_GetBodyCollider();
        HaveLifeText.text = "Life:" + f_GetHaveLife();
        HPtext.text = f_GetHp().ToString(); //血條變化(文字)
        //_GunControll.f_StopShoot();       //停止射擊
    }

    protected void InitGun() {
        _GunControll = new GunControll(this);
        _GunControll.f_ChangeGun(GunEM.Rifle);
        _GunControll.f_PushBullet();
        //_GunControll.f_StartShoot();
        Bullettext.text = _GunControll.f_GetNowBullet();
        ClipText.text = _GunControll.f_GetClip() + "";
    }


    //玩家位置跟隨頭盔位置
    private Vector3 _v3Initos = Vector3.zero;
    public void f_SetPos(Vector3 Pos) {
        transform.position = Pos;
        m_oVRBody.transform.position = Pos;
        _v3Initos = Pos;
    }


    //強制旋轉頭盔位置
    public void f_SetRot(Vector3 Rot) {
        transform.localRotation = Quaternion.Euler(Rot);
        m_oVRBody.transform.localRotation = Quaternion.Euler(Rot);
    }


    void Update() {
        //按下菜單鍵
        //if (ControllerEvents_R.index != SteamVR_TrackedObject.EIndex.None 
        //    && SteamVR_Controller.Input((int)ControllerEvents_R.index).GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
        //    OnRightMenuPressed();
        //}
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Menu)) {
            OnRightMenuPressed();
        }

        //按下左方數字8
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            f_AddClip(10);
        }

        //按下左方數字9
        if (Input.GetKeyDown(KeyCode.Alpha9)){
            if (StaticValue.m_bIsMaster){
                RolePureHpAction tmpAction = new RolePureHpAction();
                tmpAction.f_Hp(m_iId, f_GetMaxHp());
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
            }
        }
    }

    //方向圓盤最好配合這個使用 圓盤的.GetAxis()會檢測返回一個二位向量，可用角度劃分圓盤按鍵數量
    //這個函數輸入兩個二維向量會返回一個夾角 180 到 -180 
    float VectorAngle(Vector2 from, Vector2 to) {
        float angle;
        Vector3 cross = Vector3.Cross(from, to);
        angle = Vector2.Angle(from, to);
        return cross.z > 0 ? -angle : angle;
    }


    void FixedUpdate() {
        if (!_bInitOK) {
            return;
        }

        //玩家位置移動
        if (_v3Initos != Vector3.zero) {
            Vector3 tChangePos = _v3Initos - m_oHead.transform.position;
            tChangePos.y = 0;
            m_oVRBody.transform.position = m_oVRBody.transform.position + tChangePos;
            _v3Initos = Vector3.zero;
        } else {
            transform.position = m_oHead.transform.position;
        }


        //死亡倒數開關
        if (_Lutifybool == false && glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
            Deathre();
        }
        else if (_Lutifybool == true) {
            DeathreciprocalGameObject.SetActive(false);
        }


        //if (LasergunlaserBulletbool == false) {
        //    LasergunlaserBulletCD();
        //}

        _GunControll.f_Update();
        //TransPosInfor();


        #region 手把按鍵控制 (換槍枝, 發射子彈, ...) (ViveInput版)
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger)) {
            if (REM == RifleEM.continuous) {
                OnRightTriggerPressed();
            }
        }

        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)) {
            OnRightTriggerPressed();
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger)){
            OnRightTriggerReleased();
        }


        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Pad)){
            Vector2 cc = ViveInput.GetPadAxis(HandRole.RightHand);
            float jiaodu = VectorAngle(new Vector2(1, 0), cc);

            //下  
            if (jiaodu > 45 && jiaodu < 135){
                //_GunControll.f_ChangeGun(GunEM.Submachine);
                //Action_PlayerChangeGun tmpAction = new Action_PlayerChangeGun();
                //tmpAction.f_Set(m_iId, GunEM.Submachine);
                //f_AddMyAction(tmpAction);
            }

            //上  
            if (jiaodu < -45 && jiaodu > -135) {
                _GunControll.f_ChangeGun(GunEM.Rifle);
                Action_PlayerChangeGun tmpAction = new Action_PlayerChangeGun();
                tmpAction.f_Set(m_iId, GunEM.Rifle);
                f_AddMyAction(tmpAction);
            }

            //左  
            if ((jiaodu < 180 && jiaodu > 135) || (jiaodu < -135 && jiaodu > -180)) {
                //_GunControll.f_ChangeGun(GunEM.Sniper);
            }

            //右  
            if ((jiaodu > 0 && jiaodu < 45) || (jiaodu > -45 && jiaodu < 0)) {
            }
        }
        #endregion


        #region 手把按鍵控制 (換槍枝, 發射子彈, ...) (原版)
        //右手把 - 按住板機
        //if (ControllerEvents_R.index != SteamVR_TrackedObject.EIndex.None
        //    && SteamVR_Controller.Input((int)ControllerEvents_R.index).GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
        //    if (REM == RifleEM.continuous) {
        //        OnRightTriggerPressed();
        //    }
        //}
        //
        ////右手把 - 按下板機
        //if (ControllerEvents_R.index != SteamVR_TrackedObject.EIndex.None
        //    && SteamVR_Controller.Input((int)ControllerEvents_R.index).GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
        //    OnRightTriggerPressed();
        //}
        //
        //
        ////右手把 - 放開版機
        //if (ControllerEvents_R.index != SteamVR_TrackedObject.EIndex.None
        //    && SteamVR_Controller.Input((int)ControllerEvents_R.index).GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
        //    OnRightTriggerReleased();
        //}
        //
        //
        ////右手把 - 按住兩旁
        //if (ControllerEvents_R.index != SteamVR_TrackedObject.EIndex.None
        //    && SteamVR_Controller.Input((int)ControllerEvents_R.index).GetPress(SteamVR_Controller.ButtonMask.Grip))  {
        //}
        //
        ////右手把 - 放開兩旁
        //if (ControllerEvents_R.index != SteamVR_TrackedObject.EIndex.None
        //    && SteamVR_Controller.Input((int)ControllerEvents_R.index).GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
        //}
        //
        //
        ////右手觸控板
        //if (ControllerEvents_R.index != SteamVR_TrackedObject.EIndex.None
        //    && SteamVR_Controller.Input((int)ControllerEvents_R.index).GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        //{
        //    //方法返回一個坐標 接觸圓盤位置
        //    Vector2 cc = SteamVR_Controller.Input((int)ControllerEvents_R.index).GetAxis(); 
        //    float jiaodu = VectorAngle(new Vector2(1, 0), cc);
        //
        //    //下  
        //    if (jiaodu > 45 && jiaodu < 135) {
        //        //_GunControll.f_ChangeGun(GunEM.Submachine);
        //        //Action_PlayerChangeGun tmpAction = new Action_PlayerChangeGun();
        //        //tmpAction.f_Set(m_iId, GunEM.Submachine);
        //        //f_AddMyAction(tmpAction);
        //    }
        //
        //    //上  
        //    if (jiaodu < -45 && jiaodu > -135){
        //        _GunControll.f_ChangeGun(GunEM.Rifle);
        //        Action_PlayerChangeGun tmpAction = new Action_PlayerChangeGun();
        //        tmpAction.f_Set(m_iId, GunEM.Rifle);
        //        f_AddMyAction(tmpAction);
        //    }
        //
        //    //左  
        //    if ((jiaodu < 180 && jiaodu > 135) || (jiaodu < -135 && jiaodu > -180)) {
        //        //_GunControll.f_ChangeGun(GunEM.Sniper);
        //    }
        //
        //    //右  
        //    if ((jiaodu > 0 && jiaodu < 45) || (jiaodu > -45 && jiaodu < 0)) {
        //
        //    }
        //}
        #endregion


        #region (鍵盤區)--------------------------
        //Z鍵盤=模型槍的開槍鍵 (對應手把板機)
        if (Input.GetKey(KeyCode.Z)) {
            if (REM == RifleEM.continuous) {
                OnRightTriggerPressed();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            OnRightTriggerPressed();
        }

        if (Input.GetKeyUp(KeyCode.Z)) {
            OnRightTriggerReleased();
        }

        //滑鼠左鍵=模型槍二代的開槍鍵 (對應手把板機)
        if (Input.GetKey(KeyCode.Mouse0)) {
            if (REM == RifleEM.continuous) {
                OnRightTriggerPressed();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)){
            OnRightTriggerPressed();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            OnRightTriggerReleased();
        }
        #endregion
    }



    #region 換槍接口
    /// <summary>
    /// 換槍接口1 (參數用 Enum)
    /// </summary>
    /// <param name="stGunEM"></param>
    public void f_ChangeWeapon(GunEM etGunEM) {
        _GunControll.f_ChangeGun(etGunEM);
    }

    /// <summary>
    /// 換槍接口2 (主要給csv腳本用，參數用 string)
    /// </summary>
    /// <param name="stGunEM"></param>
    public void f_ChangeWeapon(string stGunEM) {
        _GunControll.f_ChangeGun2(stGunEM);
    }

    /// <summary>
    /// 获得当前枪里安装的子弹的数据结构
    /// </summary>
    /// <returns></returns>
    public BulletDT f_GetCurBulletDT() {
        return _GunControll.f_GetCurBulletDT();
    }

    #endregion

    #region 任務目標UI、(恐龍2)雷射槍CD
    /// <summary>
    /// 
    /// </summary>
    /// <param name="AimsID"    > </param>
    /// <param name="AttackSite"> </param>
    public void f_GetAims(string AimsID, string AttackSite){
        NowAimsID = AimsID;
        BaseRoleControllV2 tBaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(AimsID));
        if (tBaseRoleControl != null) {
            detectionAims.target = tBaseRoleControl.gameObject.GetComponent<Enemy>();
            detectionAims.target.getAttackSite(ccMath.atoi(AttackSite) - 1);
            detectionAims.target._iTargetIndex = ccMath.atoi(AttackSite) - 1;
        }
    }


    public void LasergunlaserBulletCD(){
        if (_GunControll == null){
            return;
        }
        LasergunlaserBulletNowCDTime += Time.deltaTime;
        LasergunlaserBulletImage.fillAmount = (float)LasergunlaserBulletNowCDTime / LasergunlaserBulletMaxCDTime;
        LasergunlaserBullettext.text = 0 + "";

        if (LasergunlaserBulletImage.fillAmount >= 1) {
            _GunControll.f_ChangeGun(GunEM.Lasergun);
            if (_GunControll.Gem == GunEM.Lasergun) {
                _GunControll.f_PushBullet();
                LasergunlaserBullettext.text = _GunControll.f_GetNowBullet();
                LasergunlaserBulletbool = true;
                _GunControll.f_ChangeGun(GunEM.Rifle);
            }
        }
    }
    #endregion


    #region 手把對應事件 =============================================================
    /// <summary> 右手手把 - 開槍鍵 - 按下 </summary>
    public void OnRightTriggerPressed() {
        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Lost){
            return;
        }
        if (_Lutifybool == true) {
            if (f_GetHp() == 0) {
                return;
            }
            _GunControll.f_StartShoot(); //开始射击
            Damageswitch = true;
        }
        trackImageGameObject.gameObject.SetActive(false);
    }

    /// <summary> 右手手把 - 開槍鍵 - 放開 </summary>
    public void OnRightTriggerReleased() {
        RifleEMbool = true;
        _GunControll.f_StopShoot();
        Damageswitch = false;
    }


    /// <summary> 右手手把 - 左按鍵 - 按下 </summary>
    public void OnRightTouchpadPressed()
    {
        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Lost)
        {
            return;
        }
        //MessageBox.DEBUG("追瞄開始");
        //_GunControll.f_ChangeGun(GunEM.Lasergun);
        //trackImageGameObject.gameObject.SetActive(true);
        //if (_Lutifybool == true)  {
        //    _GunControll.f_StartShoot();
        //}
    }

    /// <summary> 右手手把 - 左按鍵 - 放開 </summary>
    public void OnRightTouchpadReleased()
    {
        //MessageBox.DEBUG("追瞄結束");
        //_GunControll.f_StopShoot();
        //Lasergunlaser.enabled = false;
        //trackImageGameObject.gameObject.SetActive(false);
    }




    /// <summary> 右手手把 - 菜單鍵盤 </summary>
    public void OnRightMenuPressed()
    {
        if (MenuPressedState == false)
        {
            HaveLifeText.gameObject.SetActive(true);
            HPtext.gameObject.SetActive(true);
            MenuPressedState = true;
            //_GunControll.f_ChangeGun(GunEM.Bomb);
            //MenuPressedState = true;
            //BombObj.SetActive(true);
            //RifleObj.SetActive(false);
            //BombLaser.enabled = true;
            //Rifle_Laser.enabled = false;
            //_GunControll.f_StartShoot(); //开始射击
        }
        else
        {
            HaveLifeText.gameObject.SetActive(false);
            HPtext.gameObject.SetActive(false);
            MenuPressedState = false;
            //MenuPressedState = false;
            //BombObj.SetActive(false);
            //RifleObj.SetActive(true);
            //BombLaser.enabled = false;
            //Rifle_Laser.enabled = true;
            //_GunControll.f_StopShoot(); //停止射击
            //_GunControll.f_ChangeGun(GunEM.Rifle);
        }

    }

    #endregion


    #region 玩家位置和動作
    private void CallBack_TransPosInfor(object Obj) {
        //玩家死亡后不再发送坐标信息
        //if (f_IsDie()) {
        //    return;
        //}
        CreateAction();

        #region 被註解的內容
        //#region  玩家頭部的位置及旋轉角度與頭盔一樣  玩家右手位置及角度與右手把一樣  玩家左手位置及角度與左手把一樣  ViveManager腳本 在場景中[ViveManager]物件上 
        //Avatar_Head.transform.position = ViveManager.inst.Head.transform.position;
        //Avatar_Head.transform.rotation = ViveManager.inst.Head.transform.rotation;
        //Avatar_RightHand.transform.position = ViveManager.inst.RightHand.transform.position;
        //Avatar_RightHand.transform.rotation = ViveManager.inst.RightHand.transform.rotation;
        //Avatar_LeftHand.transform.position = ViveManager.inst.LeftHand.transform.position;
        //Avatar_LeftHand.transform.rotation = ViveManager.inst.LeftHand.transform.rotation;
        //#endregion
        #endregion

    }

    /// <summary>
    /// 發送角色身上的座標等訊息(?)
    /// </summary>
    private void CreateAction() {
        SaveHead(_PlayerTransformAction);
        SaveArmL(_PlayerTransformAction);
        SaveArmR(_PlayerTransformAction);
        SaveTransform(_PlayerTransformAction);
        SaveOther1(_PlayerTransformAction);
        SaveOther2(_PlayerTransformAction);
        ControllSocket.GetInstance().f_SendAction(_PlayerTransformAction);
    }


    private void SaveHead(PlayerTransformAction tPlayerTransformAction) {
        tPlayerTransformAction.m_fHeadPosX = m_oHead.transform.position.x;
        tPlayerTransformAction.m_fHeadPosY = m_oHead.transform.position.y;
        tPlayerTransformAction.m_fHeadPosZ = m_oHead.transform.position.z;
        //Quaternion tQuaternion = m_oHead.transform.rotation.eulerAngles.y;
        //tPlayerTransformAction.m_fHeadQutnX = m_oHead.transform.rotation.x;
        tPlayerTransformAction.m_fHeadQutnY = m_oHead.transform.rotation.eulerAngles.y;
        //tPlayerTransformAction.m_fHeadQutnZ = m_oHead.transform.rotation.z;
        //tPlayerTransformAction.m_fHeadQutnW = m_oHead.transform.rotation.w;
    }

    private void SaveArmL(PlayerTransformAction tPlayerTransformAction){
        tPlayerTransformAction.m_fArmLPosX = m_oLeftHand.transform.position.x;
        tPlayerTransformAction.m_fArmLPosY = m_oLeftHand.transform.position.y;
        tPlayerTransformAction.m_fArmLPosZ = m_oLeftHand.transform.position.z;
        Vector3 v3Rotation = m_oLeftHand.transform.rotation.eulerAngles;
        tPlayerTransformAction.m_fArmLQutnX = v3Rotation.x;
        tPlayerTransformAction.m_fArmLQutnY = v3Rotation.y;
        tPlayerTransformAction.m_fArmLQutnZ = v3Rotation.z;
    }

    private void SaveArmR(PlayerTransformAction tPlayerTransformAction) {
        tPlayerTransformAction.m_fArmRPosX = m_oRightHand.transform.position.x;
        tPlayerTransformAction.m_fArmRPosY = m_oRightHand.transform.position.y;
        tPlayerTransformAction.m_fArmRPosZ = m_oRightHand.transform.position.z;
        Vector3 v3Rotation = m_oRightHand.transform.rotation.eulerAngles;
        tPlayerTransformAction.m_fArmRQutnX = v3Rotation.x;
        tPlayerTransformAction.m_fArmRQutnY = v3Rotation.y;
        tPlayerTransformAction.m_fArmRQutnZ = v3Rotation.z;
    }

    private void SaveTransform(PlayerTransformAction tPlayerTransformAction) {
        tPlayerTransformAction.m_TransformX = transform.position.x;
        tPlayerTransformAction.m_TransformY = transform.position.y;
        tPlayerTransformAction.m_TransformZ = transform.position.z;
    }


    private void SaveOther1(PlayerTransformAction tPlayerTransformAction){
        tPlayerTransformAction.m_fOtherPos1X = m_oOther1.transform.position.x;
        tPlayerTransformAction.m_fOtherPos1Y = m_oOther1.transform.position.y;
        tPlayerTransformAction.m_fOtherPos1Z = m_oOther1.transform.position.z;
        Vector3 v3Rotation = m_oOther1.transform.rotation.eulerAngles;
        tPlayerTransformAction.m_fOtherRQutn1X = v3Rotation.x;
        tPlayerTransformAction.m_fOtherRQutn1Y = v3Rotation.y;
        tPlayerTransformAction.m_fOtherRQutn1Z = v3Rotation.z;
    }


    private void SaveOther2(PlayerTransformAction tPlayerTransformAction){
        tPlayerTransformAction.m_fOtherPos2X = m_oOther2.transform.position.x;
        tPlayerTransformAction.m_fOtherPos2Y = m_oOther2.transform.position.y;
        tPlayerTransformAction.m_fOtherPos2Z = m_oOther2.transform.position.z;
        Vector3 v3Rotation = m_oOther2.transform.rotation.eulerAngles;
        tPlayerTransformAction.m_fOtherRQutn2X = v3Rotation.x;
        tPlayerTransformAction.m_fOtherRQutn2Y = v3Rotation.y;
        tPlayerTransformAction.m_fOtherRQutn2Z = v3Rotation.z;
    }

    #endregion


    #region 玩家被伤害、死亡、重生
    public override void f_BeAttack(int iHP, int iRoleId, GameEM.EM_BodyPart tBodyPart) {
        base.f_BeAttack(iHP, iRoleId, tBodyPart);
        Gothurt();
    }

    /// <summary>
    /// 被伤害時的畫面
    /// </summary>
    public void Gothurt() {
        PlayerEffect.ScreenEffect HitScreen = new PlayerEffect.ScreenEffect(ScreenEM.hit, this);                   //紅血濾鏡
        PlayerEffect.SoundEffect PlayerSoundEffect = new PlayerEffect.SoundEffect(SoundEM.hit, ref m_AudioSource); //慘叫聲
        HPImage.fillAmount = (float)(f_GetHp()) / f_GetMaxHp();                                                    //血條變化(圖片)
        //HPtext.text = (int)(HPImage.fillAmount * 100) + "%";                                                     //血條變化(文字%)
        HPtext.text = f_GetHp().ToString();                                                                        //血條變化(文字)
    }


    /// <summary>
    /// 重生UI (?)
    /// </summary>
    public void Deathre()  {
        fDelayTime += Time.deltaTime;                //
        Deathreciprocal.fillAmount = fDelayTime / 2; //
        DeathreciprocalGameObject.SetActive(true);   //死亡倒數物件開啟
        _Lutify.Blend = 1;                           //畫面變黑
        _vp_MuzzleFlash.ShootLightOnly();            //??
        Rifleeffect.gameObject.SetActive(false);     //手槍開火特效關閉
        SubmachineGun_effect.SetActive(false);       //衝鋒槍開火特效關閉
        Ani_Attack1.Stop();                          //開火聲音停止
    }


    /// <summary>
    /// 重生
    /// </summary>
    public override void f_Rebirth() {
        base.f_Rebirth();
        f_SetBodyCollider(true); //打開碰撞
    }


    /// <summary>
    /// 死亡
    /// </summary>
    public override void f_Die() {
        base.f_Die();
        f_SetBodyCollider(false); //關閉碰撞
    }
    #endregion


    #region 槍枝UI

    #endregion


    #region 補彈藥、補彈夾、補血
    /// <summary>
    /// 補子彈
    /// </summary>
    public void f_PushBullet() {
        _GunControll.f_PushBullet();
        Bullettext.text = _GunControll.f_GetNowBullet();
    }


    /// <summary>
    /// 補彈夾
    /// </summary>
    public void f_AddClip(int value){
        _GunControll.f_AddClip(value);
        ClipText.text = _GunControll.f_GetClip().ToString();
    }


    /// <summary>
    /// 補血的地方 (override增加血條更新)
    /// </summary>
    public override void f_AddHp(int iHP) {
        base.f_AddHp(iHP);
        HPImage.fillAmount = (float)(f_GetHp()) / f_GetMaxHp(); //血條變化(圖片)
        //HPtext.text = (int)(HPImage.fillAmount * 100) + "%";  //血條變化(文字%)
        HPtext.text = f_GetHp().ToString();                     //血條變化(文字)
    }
    #endregion


    #region 玩家Y向上弹起
    /// <summary>
    /// 玩家受到向上弹起的力
    /// </summary>
    /// <param name="fPower" > 弹起来花费的时间 </param>
    /// <param name="fHeight"> 被弹起来高度     </param>
    public override void f_Spring(float fPower, float fHeight) {
        Vector3 Pos = transform.position;
        Pos.y += fHeight;
        //iTween.MoveTo(this.gameObject, iTween.Hash(
        //                "position", Pos,
        //                "speed", fPower,
        //                //"orienttopath", true,
        //                "axis", "y",
        //                "easetype", "linearTween",
        //                "oncomplete", "ccCallBackSpringComplete",
        //                "oncompletetarget", gameObject
        //                ));

        this.transform.DOMove(Pos, fPower)        //移動到玩家上方
           .SetEase(Ease.OutQuad)                 //速度變化模式
           .OnComplete(ccCallBackSpringComplete); //到達上方後宣告抓到目標了

        _bIsSpring = true;

        //向其它玩家发送弹起来的消息
        //RoleSpringAction tRoleSpringAction = new RoleSpringAction();
        //tRoleSpringAction.f_Send2OtherPlayer(m_iId, fPower, fHeight);
        //f_AddMyAction(tRoleArrowAttackAction);
    }


    /// <summary>
    /// 受到来此外部的坐标变更，用来实现类似被抓起
    /// </summary>
    public override void f_SpringPush(float fY){
        Vector3 Pos = transform.position;
        Pos.y = fY;
        transform.position = Pos;
        _bIsSpring = true;
    }

    /// <summary>
    /// 从弹起的空中回到地面
    /// </summary>
    /// <param name="fPower"></param>
    public override void f_SpringReset(float fPower) {
        Vector3 Pos = transform.position;
        Pos.y = GloData.glo_fPlayerDefaultY;
        //  iTween.MoveTo(this.gameObject, iTween.Hash(
        //                  "position", Pos,
        //                  "speed", fPower,
        //                  //"orienttopath", true,
        //                  "axis", "y",
        //                  "easetype", "easeInOutExpo",
        //                  "oncomplete", "ccCallBackSpringResetComplete",
        //                  "oncompletetarget", gameObject
        //                  ));
        this.transform.DOMove(Pos, fPower)
            .SetEase(Ease.InCirc)                       //速度變化模式
            .OnComplete(ccCallBackSpringResetComplete); //到達上方後宣告抓到目標了
    }

    /// <summary>
    /// 被弹起结束回调
    /// </summary>
    private void ccCallBackSpringComplete() {
        f_SpringReset(1);
    }

    /// <summary>
    /// 返回到地面回调
    /// </summary>
    private void ccCallBackSpringResetComplete() {
        _bIsSpring = false;
    }

    #endregion




}
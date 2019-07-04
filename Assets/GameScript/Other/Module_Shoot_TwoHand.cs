using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using RootMotion.FinalIK;

public class Module_Shoot_TwoHand : MonoBehaviour
{
    [HelpBox("動畫調用 Mv_AttackEvent() 發射子彈\n" +
             "動畫調用 Mv_Reload()         補充子彈\n"+
             "____________________________________\n" +
             "動畫調用 Mv_SetInv() 開啟無敵\n" +
             "(0=關 / 1=開)\n" +
             "預設:沒彈藥時開啟無敵\n" +
             "預設:下發攻擊時關閉\n" +
             "____________________________________\n" +
             "動畫調用 Mv_SetHand() 選擇使用的手\n" +
             "(R=右手 / L=左手)\n" +
             "____________________________________\n" +
             "動畫調用 Mv_IKSee() 進行瞄準\n" +
             "(1=瞄準 / 0=結束瞄準)\n" +
             "____________________________________\n" +
             "動畫調用 Mv_ChangeTarget() 隨機切換目標"
             , HelpBoxType.Info, height = 2)]



    [Line(0)]
    [Rename("是否初始化了")]   public bool _bInit;
    [Rename("是否在瞄準狀態")] public bool _bSee;
    [Rename("是否瞄到敵人")]   public bool _bCanShoot;
    [Line(0)]
    [Rename("開啟偵測 (牆壁)")] public bool _bDebug_WallCast;
    [Rename("開啟偵測 (槍口)")] public bool _bDebug_MizzlePos;
    [Rename("開啟偵測 (血量)")] public bool _bDebug_HP;
    [Rename("偵測血量用的UI")]  public Text _DebugUI_HP;
    [Rename("開啟偵測 (彈藥)")] public bool _bDebug_BulletAmount;
    [Rename("彈藥偵測用文字")]  public Text _DebugUI_BulletAmount;
    [Line(0)]
    [Rename("攻擊目標 (程式會自動給)")] public Transform TargetPos;
    [Rename("牆壁偵測Layer層")]         public LayerMask WallMask;
    private RaycastHit hit;               //Raycast用
    private Animator _animator;           //自己的動畫狀態機
    private BaseRoleControllV2 _BaseRole; //自己的角色模板
    private Vector3 tmpLookAtPos;         //怪物朝向位置 (目標去掉Y軸)
    private Quaternion tmpRotation;       //緩衝LookAt用
    [Line()]
    [Rename("如果不看玩家的話，這裡打勾 (像是從頭到尾只用動畫去做掃射時)")] public bool OnlyAnimator;
    [Line()]
    [Rename("[右手使用]")]   public bool useR;
    [Rename("右手IK")]       public AimIK AimIk_R;
    [Rename("右手瞄準點")]   public Transform AimPos_R;
    [Rename("右手槍口")]     public Transform MuzzlePos_R;
    [Rename("右手開火特效")] public GameObject MuzzleFx_R;
    [Rename("(特效時間)")]   public float MuzzleFxTime_R = 0.1f;
    [Rename("(開火音效)")]   public AudioClip FireSound_R;
    [Rename("瞄準位置調整")] public Vector3 Offset_R = new Vector3(0,0,0);
    [Line()]
    [Rename("[左手使用]")]   public bool useL;
    [Rename("左手IK")]       public AimIK AimIk_L;
    [Rename("左手瞄準點")]   public Transform AimPos_L;
    [Rename("左手槍口")]     public Transform MuzzlePos_L;
    [Rename("左手開火特效")] public GameObject MuzzleFx_L;
    [Rename("(特效時間)")]   public float MuzzleFxTime_L = 0.1f;
    [Rename("(開火音效)")]   public AudioClip FireSound_L;
    [Rename("瞄準位置調整")] public Vector3 Offset_L = new Vector3(0, 0, 0);
    [Line()]
    [Rename("攜彈量 (由RunAI表決定)")] public int BulletAmount = 7;
    private int MaxBulletAmount; //最大攜彈量 = 一開始的攜彈量



    //相關元件的初始化
    void Start(){
        _animator = this.GetComponent<Animator>();
        _BaseRole = this.GetComponent<BaseRoleControllV2>();

        //設定雙手瞄準點、瞄準姿勢偏差
        if (AimIk_R != null) {
            if (AimPos_R != null) {
                AimIk_R.solver.transform = AimPos_R;
            }
            AimIk_R.solver.IKPositionWeight = 0;
        }
        if (AimIk_L != null) {
            if (AimPos_L != null) {
                AimIk_L.solver.transform = AimPos_L;
            }
            AimIk_L.solver.IKPositionWeight = 0;
        }




    }


    /// <summary>
    /// 取得外部參數後的初始化
    /// </summary>
    public void f_Init() {
        if (!_bInit) {
            MaxBulletAmount = BulletAmount;
            _bInit = true;
        }
    }


    void Update() {

        //除錯用
        DebugShoot();

        //如果在瞄準玩家
        if (_bSee) {
            if (TargetPos != null) {
                if (!OnlyAnimator) {
                    tmpLookAtPos = TargetPos.position;
                    tmpLookAtPos.y = transform.position.y;
                    tmpRotation = Quaternion.LookRotation(tmpLookAtPos - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, tmpRotation, Time.deltaTime * 1.0f);
                    //transform.LookAt(tmpLookAtPos);
                }
            }


            if (AimIk_R != null) {
                MuzzlePos_R.position = AimPos_R.position;
                if (TargetPos != null){
                    AimIk_R.solver.IKPosition = TargetPos.position + Offset_R;
                    MuzzlePos_R.LookAt(TargetPos.position + Offset_R);
                }
            }
            if (AimIk_L != null) {
                MuzzlePos_L.position = AimPos_L.position;
                if (TargetPos != null){
                    AimIk_L.solver.IKPosition = TargetPos.position + Offset_L;
                    MuzzlePos_L.LookAt(TargetPos.position);
                }
            }
        }

    }


    /// <summary>
    /// [动作事件] 触发射擊操作
    /// </summary>
    public void Mv_AttackEvent(int BulletID) {

        //沒子彈時的行為
        if (BulletAmount <= 0) {
            //ReloadShoot();     //透過射線偵測附近碰撞，做出相應的換彈躲藏動作
            GetHideInfo();       //讀取躲藏點資訊，做出相應的換彈躲藏動作，沒躲藏點會執行ReloadShoot()
            ForceStopMuzzleFX(); //強制關閉開火特效
            Mv_ChangeTarget();   //更換攻擊目標
            return;
        }

        //有子彈時的行為：
        AttackShoot(BulletID);
    }



    /// <summary>
    /// [动作事件] 触发換彈操作 (換彈動畫在狀態機上直接接回攻擊動畫)
    /// </summary>
    private void Mv_Reload() {
        _BaseRole.f_AttactVoice();                                         //播放攻擊語音
        //Mv_ChangeTarget();                                                //更換攻擊目標
        Action_Shoot tmpAction = new Action_Shoot();                       //同步射擊動作
        tmpAction.f_SetRoleId(_BaseRole.m_iId);                            //設定開槍的人
        tmpAction.f_SetBulletAmount(MaxBulletAmount);                      //設定彈藥變化量(補滿彈夾)
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction); //執行動作
        _animator.ResetTrigger("Reload_L");                                //重置AnimatorTrigger的資訊
        _animator.ResetTrigger("Reload_R");                                //重置AnimatorTrigger的資訊
    }





    /// <summary>
    /// 攻擊動作
    /// </summary>
    /// <param name="BulletID"> 發射的子彈 </param>
    private void AttackShoot(int BulletID)
    {
        //瞄準敵人
        Mv_IKSee(1);

        //解除無敵
        if (_BaseRole.f_IsInv()){
            Mv_SetInv(0);
        }

        //判斷有沒有瞄準到敵人
        if (useR) {
            if (AimIk_R.solver.IKPositionWeight < 1) {
                _bCanShoot = false;
                return;
            } else {
                _bCanShoot = true;
            }
        }

        if (useL){
            if (AimIk_L.solver.IKPositionWeight < 1) {
                _bCanShoot = false;
                return;
            } else {
                _bCanShoot = true;
            }
        }


        //由主玩家開啟彈藥損耗的同步
        if (StaticValue.m_bIsMaster) {
            Action_Shoot tmpAction = new Action_Shoot();                       //同步射擊動作
            tmpAction.f_SetRoleId(_BaseRole.m_iId);                            //設定開槍的人
            tmpAction.f_SetBulletAmount(-1);                                   //設定彈藥變化量(-1)

            if (MuzzlePos_R != null && useR) {
                tmpAction.f_SetBullet(BulletID, MuzzlePos_R.position, MuzzlePos_R.rotation);
            }
            if (MuzzlePos_L != null && useL) {
                tmpAction.f_SetBullet(BulletID, MuzzlePos_L.position, MuzzlePos_L.rotation);
            }

            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction); //執行動作     
        }

        //子彈各自產生，再由子彈去做扣血的同步
        //BulletDT tBulletDT = (BulletDT)glo_Main.GetInstance().m_SC_Pool.m_BulletSC.f_GetSC(BulletID); //獲取子彈資料(攻擊、速度、存活時間)
        //BaseBullet tBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(tBulletDT);      //產生子彈
        if (MuzzlePos_R != null && useR){                               //如果右手使用中
            //tBullet.transform.position = MuzzlePos_R.position;          //設定子彈位置
            //tBullet.transform.rotation = MuzzlePos_R.rotation;          //設定子彈朝向
            if (MuzzleFx_R != null) {                                   //如果身上有開火特效
                StartCoroutine(f_MuzzleFx(MuzzleFx_R, MuzzleFxTime_R)); //打開開火特效，並於 MuzzleFxTime後關閉
            }
            if (FireSound_R!=null) {
                //GetComponent<AudioSource>().PlayOneShot(FireSound_R,1);
                //AudioManager._ins.PlayAudioClip("Module_Shoot_TwoHand.R", MuzzlePos_R.position, FireSound_R); //開火音效
                AudioManager._ins.PlayOneShot(FireSound_R, 1, this.gameObject.name + " Module_Shoot_TwoHand.R");
            }
        }
        if (MuzzlePos_L != null && useL) {                     //如果左手使用中
            //tBullet.transform.position = MuzzlePos_L.position; //設定子彈位置
            //tBullet.transform.rotation = MuzzlePos_L.rotation; //設定子彈朝向
            if (MuzzleFx_L != null){                           //如果身上有開火特效
                StartCoroutine(f_MuzzleFx(MuzzleFx_L, MuzzleFxTime_L)); //打開開火特效，並於 MuzzleFxTime後關閉
            }
            if (FireSound_L != null){
                //AudioManager._ins.PlayAudioClip("Module_Shoot_TwoHand.L", MuzzlePos_L.position, FireSound_L); //開火音效
                AudioManager._ins.PlayOneShot(FireSound_L, 1, this.gameObject.name + " Module_Shoot_TwoHand.L");
            }
        }
        //tBullet.f_Fired(ccMath.f_CreateKeyId(), BulletID, _BaseRole.f_GetTeamType(), _BaseRole.m_iId); //子彈擊出

    }



    /// <summary>
    /// 沒子彈時的行為
    /// </summary>
    private void ReloadShoot()
    {
        
        //偵測左右牆壁決定躲藏位置，左右無牆壁就原地換子彈
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 1f)) {
            _animator.SetTrigger("Reload_L");
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 1f)){
            _animator.SetTrigger("Reload_R");
        }
        else {
            _animator.Play("Reload");
        }

        //停止瞄準玩家
        Mv_IKSee(0);

        //開啟無敵 (動畫決定)
        //Mv_SetInv(1);
    }


    /// <summary>
    /// 開火特效
    /// </summary>
    /// <param name="delay"> 開火特效持續的時間 </param>
    IEnumerator f_MuzzleFx(GameObject tmpObj, float delay) {
        tmpObj.SetActive(true);
        yield return new WaitForSeconds(delay);
        tmpObj.SetActive(false);
    }


    /// <summary>
    /// 強制關閉開火特效
    /// </summary>
    public void ForceStopMuzzleFX() {
        if (MuzzleFx_R != null) {
            MuzzleFx_R.SetActive(false);
        }
        if (MuzzleFx_L!=null) {
            MuzzleFx_L.SetActive(false);
        }
    }


    /// <summary>
    /// 指定IK瞄準的位置
    /// </summary>
    /// <param name="targetPos"> 指定看向的位置 </param>
    public void IKSee_SetTarget(Transform tPos = null) {
        if (tPos != null) {
            TargetPos = tPos;
            if (AimIk_R != null) {
                //為了能調整瞄準姿勢的誤差，改到 Update()裡去設定 AimIk_R.solver.IKPosition
                //AimIk_R.solver.target = tPos;
            }
            if (AimIk_L != null) {
                //為了能調整瞄準姿勢的誤差，改到 Update()裡去設定 AimIk_L.solver.IKPosition
                //AimIk_L.solver.target = tPos;
            }
        }
    }


    /// <summary>
    /// [动作事件] 開關IK瞄準 (0=關 / 1=開)
    /// </summary>
    /// <param name="tmpValue"> 0=關 / 1=開 </param>
    public void Mv_IKSee(int tmpValue) {
        if (tmpValue == 0){
            _bSee = false; //停止瞄準玩家
        }
        else{
            _bSee = true; //開始瞄準玩家
        }

        if (useR) {
            if (AimIk_R != null) {
                StopCoroutine("FadeSeeTarget_R");
                StartCoroutine(FadeSeeTarget_R(AimIk_R.solver.IKPositionWeight, tmpValue, 1.5f));
            }
            if (AimIk_L != null) {
                StopCoroutine("FadeSeeTarget_L");
                StartCoroutine(FadeSeeTarget_L(AimIk_L.solver.IKPositionWeight, 0, 1.5f));
            }
        }
        if (useL) {
            if (AimIk_R != null){
                StopCoroutine("FadeSeeTarget_R");
                StartCoroutine(FadeSeeTarget_R(AimIk_R.solver.IKPositionWeight, 0, 1.5f));
            }
            if (AimIk_L != null){
                StopCoroutine("FadeSeeTarget_L");
                StartCoroutine(FadeSeeTarget_L(AimIk_L.solver.IKPositionWeight, tmpValue, 1.5f));
            }
        }
        if (!useR && !useL){
            if (AimIk_R != null) {
                StopCoroutine("FadeSeeTarget_R");
                StartCoroutine(FadeSeeTarget_R(AimIk_R.solver.IKPositionWeight, 0, 1.5f));
            }
            if (AimIk_L != null) {
                StopCoroutine("FadeSeeTarget_L");
                StartCoroutine(FadeSeeTarget_L(AimIk_L.solver.IKPositionWeight, 0, 1.5f));
            }
        }

    }


    /// <summary>
    /// [动作事件] 開關無敵 (0=關 / 1=開)
    /// </summary>
    /// <param name="value"> 0=關 / 1=開 </param>
    public void Mv_SetInv(int value){
        if (value == 0) {
            _BaseRole.f_SetInv(false);
        }
        else {
            _BaseRole.f_SetInv(true);
        }
    }




    /// <summary>
    /// 切換目標
    /// </summary>
    public void Mv_ChangeTarget() {
        if (StaticValue.m_bIsMaster) {
            BaseRoleControllV2 newTarget = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemyForRand(_BaseRole, _BaseRole.f_GetAttackSize());
            if (newTarget != null) {
                Action_SetTarget tmpAction = new Action_SetTarget();
                tmpAction.f_Set(_BaseRole.m_iId, newTarget.m_iId);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction); //執行動作  
            }
        }
    }


    /// <summary>
    /// 重置射擊模組 (死亡時執行)
    /// </summary>
    public void ResetShoot() {
        Mv_SetInv(0);
        IKSee_SetTarget(null);
        Mv_IKSee(0);
        ForceStopMuzzleFX(); //強制關閉開火特效
        //_animator.SetBool("CanAttack", false);
        if (AimIk_R != null) {
            AimIk_R.solver.IKPositionWeight = 0;
        }
        if (AimIk_L != null) {
            AimIk_L.solver.IKPositionWeight = 0;
        }
    }


    /// <summary>
    /// [动作事件] 設定使用手
    /// </summary>
    /// <param name="value">  </param>
    public void Mv_SetHand(string value){
        if (value == "L"){
            useL = true;
            useR = false;
            Mv_IKSee(1);
        }
        else if (value == "R") {
            useL = false;
            useR = true;
            Mv_IKSee(1);
        }
        else if (value == "LR") {
            useL = true;
            useR = true;
            Mv_IKSee(1);
        }
        else if (value == "RL") {
            useL = true;
            useR = true;
            Mv_IKSee(1);
        }
    }


    /// <summary>
    /// 處理看向敵人的過程 (左手)
    /// </summary>
    /// <param name="startValue"> 當前IK轉向的權重 </param>
    /// <param name="endValue"  > 1=看/0=不看 </param>
    /// <param name="speed"     > 轉頭的速度  </param>
    IEnumerator FadeSeeTarget_L(float startValue, float endValue, float speed) {
        if (startValue == endValue)  {
            yield break;
        }
        float startTime = Time.time;
        float length = Mathf.Abs(endValue - startValue);
        float frac = 0;
        while (frac < 1.0f) {
            float dist = (Time.time - startTime) * speed;
            frac = dist / length;
            if (AimIk_L != null) {
                AimIk_L.solver.IKPositionWeight = Mathf.Lerp(startValue, endValue, frac);
            }
            yield return null;
        }
    }

    /// <summary>
    /// 處理看向敵人的過程 (右手)
    /// </summary>
    /// <param name="startValue"> 當前IK轉向的權重 </param>
    /// <param name="endValue"  > 1=看/0=不看 </param>
    /// <param name="speed"     > 轉頭的速度  </param>
    IEnumerator FadeSeeTarget_R(float startValue, float endValue, float speed){
        if (startValue == endValue){
            yield break;
        }
        float startTime = Time.time;
        float length = Mathf.Abs(endValue - startValue);
        float frac = 0;
        while (frac < 1.0f) {
            float dist = (Time.time - startTime) * speed;
            frac = dist / length;
            if (AimIk_R != null) {
                AimIk_R.solver.IKPositionWeight = Mathf.Lerp(startValue, endValue, frac);
            }
            yield return null;
        }
    }


    /// <summary>
    /// 除錯和狀況監測用
    /// </summary>
    private void DebugShoot() {
        //牆壁偵測情況
        if (_bDebug_WallCast) {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 1f)) {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 1f, Color.green);
            }
            else if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 1f)){
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 1f, Color.yellow);
            }
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 1f)) {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 1f, Color.green);
            }
            else{
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 1f, Color.yellow);
            }
        }

        //偵測子彈數目
        if (_DebugUI_BulletAmount != null) {
            if (_bDebug_BulletAmount){
                _DebugUI_BulletAmount.text = BulletAmount.ToString();
            }
            else {
                _DebugUI_BulletAmount.text = "";
            }
        }

        //偵測血量
        if (_DebugUI_HP != null) {
            if (_bDebug_HP){
                if (_BaseRole.f_IsInv()) {
                    _DebugUI_HP.text = _BaseRole.f_GetHp().ToString() + " (無敵中)";
                }
                else {
                    _DebugUI_HP.text = _BaseRole.f_GetHp().ToString();
                }
            }
            else {
                _DebugUI_HP.text = "";
            }
        }

    }



    /// <summary>
    /// 以躲藏點來決定動作
    /// </summary>
    private void GetHideInfo(){
        ArcherRoleControl _Solider = _BaseRole.GetComponent<ArcherRoleControl>();
        if (_Solider == null){
            ReloadShoot();
            return;
        }
        if (_Solider.HidePos.Count == 0) {
            ReloadShoot();
            return;
        }
        PointPart tmp = _Solider.HidePos[_Solider.CurHidePos].GetComponent<PointPart>();
        if (tmp == null){
            ReloadShoot();
            return;
        }
        if (tmp.HideType == EM_Hide.LeftHide){
            _animator.Play("Reload_L");
        }
        else if (tmp.HideType == EM_Hide.RightHide){
            _animator.Play("Reload_R");
        }
        else if (tmp.HideType == EM_Hide.StandHide) {
            _animator.Play("Reload");
        }
        else if (tmp.HideType == EM_Hide.SquatHide){
            _animator.Play("SquatHide");
        }

        //停止瞄準玩家
        Mv_IKSee(0);
    }

}

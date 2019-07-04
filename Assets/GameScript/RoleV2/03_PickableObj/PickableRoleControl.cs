using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 拾取物類型
/// </summary>
public enum PickableEM
{
    [Rename("無效果，純觸碰特效 (None)")] None,
    [Rename("補血藥 (Tonic)")] Tonic,
    [Rename("彈藥 (Ammo)")] Ammo,
    [Rename("彈夾 (AmmoClip)")] AmmoClip,
    [Rename("改參數 ")] Parameter,
}


/// <summary>
/// 用什麼拾取
/// </summary>
public enum PickByEM{
    [Rename("玩家右手")] RightHand,
    [Rename("玩家左手")] LeftHand,
    [Rename("玩家右腳")] RightFoot,
    [Rename("玩家左腳")] LeftFoot,
    [Rename("Tracker_1")] Tracker_1,
    [Rename("Tracker_2")] Tracker_2,
}




public class PickableRoleControl : BaseRoleControllV2
{
    [HelpBox(
        "觸發 CD    以物件的 HP決定" +
        "調整的數值 以物件的攻擊力決定\n" + 
        "觸發的範圍 以物件的攻擊範圍來算\n" + 
        "可用的次數 以物件的生命數決定\n" + 
        "要改的參數名稱 由物件的註解決定\n\n" +
        "德軍AI：\n" + 
        "手是14，Tracker1是18，其餘沒完成", 
        HelpBoxType.Info)]
    [Rename("物件類型")] public PickableEM PickableType = PickableEM.None;
    [Rename("如何拾取")] public PickByEM PlayerHandType = PickByEM.RightHand;
    private Transform PickBy_Self;  //用玩家哪個部位去撿東西
    private Transform PickBy_Other; //用玩家哪個部位去撿東西
    [Line()]
    [Rename("撿取者 (透過程式給指定)")]   public BaseRoleControllV2 _Owner;
    [Rename("當下可觸發 (透過程式切換)")] public bool canUSE = true; //可以用來判斷手是否拿開 (true=等著被人撿 / false=正在撿取中)
    private MySelfPlayerControll2 tmpPlayerSelf;
    private OtherPlayerControll2 tmpPlayerOther;
    [Line()]
    [Rename("(檢查用) 調整值")]   public float value;
    [Rename("(檢查用) 觸發範圍")] public float range;
    [Rename("(檢查用) 可用次數")] public int useNumber;
    [Line()]
    [Rename("開啟進度保存 (手拿開後，讀取進度不重置)")]
    public bool bKeepProgress = false;

    [Rename("開啟自動重置 (判斷東西可被多次拿取、要拿第二次時，手是要拿開再觸碰，還是手放著就會一直拿)")]
    public bool bAutoReset = false;

    [Rename("開啟文字倒數 (倒數是否需要顯示數字)")]
    public bool bTextCountDown = false;

    [Line()]
    [Rename("觸發CD時間")]   public float cdTime = 3;
    [Rename("CD跑條(文字)")] public Text cdText;
    [Rename("CD跑條(圖片)")] public Image cdImage;
    [Rename("CD背景(圖片)")] public Image cdImageBackGround;
    private float cdSaveProgress = 0;  //記錄cd讀取進度用
    private float cdImageSaveProgress; //記錄cs跑條的進度用
    [Line()]
    [Rename("可以碰時的文字提示")] public string tipTrue = "碰我";
    [Rename("觸碰中的文字提示")]   public string tipLoading = "觸碰中...";
    [Rename("不能碰時的文字提示")] public string tipFalse = "請重新觸碰";
    [Line()]
    [Rename("觸發成功特效")]
    public GameObject PickableFx;
    [Rename("手拿開的特效")]
    public GameObject HandLeaveFx;
    //[Line()] 
    //[Rename("待機特效 (開/關)")]
    //public GameObject IdleFx;
    //[Rename("觸摸中特效 (開/關)")]
    //public GameObject TouchingFx;

    /// <summary>
    /// 暫存參數
    /// </summary>
    private string tempPara;

    //初始化
    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true) {
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight, bUpdatePos = true);
        value = f_GetAttackPower();
        range = f_GetAttackSize();
        useNumber = f_GetHaveLife();
        cdTime = f_GetBodySize();
        canUSE = true;
        if (cdText != null){
            cdText.text = tipTrue;
        }
        if (cdImage != null){
            cdImage.fillAmount = 0 / cdTime;
        }
        if (cdImageBackGround != null){
            cdImageBackGround.gameObject.SetActive(false);
        }
        cdSaveProgress = cdTime;
        cdImageSaveProgress = 0;
    }


    //跳過需要把模型放置在RoleModel上的步驟 ======================================
    protected override Transform GetRoleModel(){
        return transform;
    }


    /// <summary>
    /// 死亡 (消失時間強制0.3)
    /// </summary>
    public override void f_Die(){
        StopAllCoroutines(); //停止所有 IEnumerator
        if (StaticValue.m_bIsMaster){
            UnSetPos();
            Action_Die tRoleDieAction = new Action_Die();
            tRoleDieAction.f_Die(m_iId, f_GetTeamType(), 0.3f);
            f_AddMyAction(tRoleDieAction);
        }
    }



    #region 讓撿取物能被撿取的地方
    /// <summary>
    /// 撿取效果 (接口)
    /// </summary>
    public void f_PickUp() {
        canUSE = false;
        StopAllCoroutines();
        StartCoroutine(CD());
        StartCoroutine(CDUI());
        if (_Owner.GetComponent<MySelfPlayerControll2>() != null){
            tmpPlayerSelf = _Owner.GetComponent<MySelfPlayerControll2>();
            InitTrueHand();
        }
        if (_Owner.GetComponent<OtherPlayerControll2>() != null){
            tmpPlayerOther = _Owner.GetComponent<OtherPlayerControll2>();
            InitTrueHand();
        }
    }


    /// <summary>
    /// 設定撿取判斷的基準
    /// </summary>
    private void InitTrueHand()  {
        if (PlayerHandType == PickByEM.RightHand) {
            if (tmpPlayerSelf != null) {
                PickBy_Self = tmpPlayerSelf.m_BulletStart.transform;
            }
            if (tmpPlayerOther != null) {
                PickBy_Other = tmpPlayerOther.m_BulletStart.transform;
            }
        }
        else if (PlayerHandType == PickByEM.LeftHand) {
            if (tmpPlayerSelf != null) {
                PickBy_Self = tmpPlayerSelf.m_oLeftHand.transform;
            }
            if (tmpPlayerOther != null) {
                PickBy_Other = tmpPlayerOther.m_oLeftHand.transform;
            }
        }
        else if (PlayerHandType == PickByEM.Tracker_1) {
            if (tmpPlayerSelf != null) {
                PickBy_Self = tmpPlayerSelf.m_oOther1.transform;
            }
            if (tmpPlayerOther != null){
                PickBy_Other = tmpPlayerOther.m_oOther1.transform;
            }
        }
        else if (PlayerHandType == PickByEM.Tracker_2) {
            if (tmpPlayerSelf != null) {
                PickBy_Self = tmpPlayerSelf.m_oOther2.transform;
            }
            if (tmpPlayerOther != null){
                PickBy_Other = tmpPlayerOther.m_oOther2.transform;
            }
        }

    }



    /// <summary>
    /// 持續檢查玩家的手有無離開
    /// </summary>
    private void FixedUpdate(){
        if (!canUSE) {
            if (tmpPlayerSelf != null){
                UI_LookAtPlayer(tmpPlayerSelf.transform.position);
                if (Vector3.Distance(PickBy_Self.position, transform.position) > f_GetAttackSize()) {
                    //f_Reset();
                    if (StaticValue.m_bIsMaster) {
                        Action_GetPickable tmpAction = new Action_GetPickable();
                        tmpAction.f_Reset(this.m_iId);
                        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
                    }
                }
            }
            if (tmpPlayerOther != null) {
                UI_LookAtPlayer(tmpPlayerOther.transform.position);
                if (Vector3.Distance(PickBy_Other.transform.position, transform.position) > f_GetAttackSize()) {
                    //f_Reset();
                    if (StaticValue.m_bIsMaster) {
                        Action_GetPickable tmpAction = new Action_GetPickable();
                        tmpAction.f_Reset(this.m_iId);
                        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
                    }
                }
            }
        }
    }





    /// <summary>
    /// UI 朝向玩家
    /// </summary>
    /// <param name="_LookTarget"> 朝向的位置 </param>
    public void UI_LookAtPlayer(Vector3 _LookTarget) {
        if (cdText != null) {
            cdText.transform.LookAt(_LookTarget);
        }
        if (cdImage != null) {
            cdImage.transform.LookAt(_LookTarget);
        }
        if (cdImageBackGround != null) {
            cdImageBackGround.transform.LookAt(_LookTarget);
        }
    }


    /// <summary>
    /// 重置使用時間
    /// </summary>
    IEnumerator CD(){
        float value = 0;

        //如果有記錄讀取進度，就從記錄的進度開始倒數，否則從頭開始計算
        if (bKeepProgress) {
            value = cdSaveProgress;
        } else {
            value = cdTime;
        }
        

        //倒數文字顯示 數字或觸碰中的提示
        if (cdText != null){
            if (bTextCountDown){
                cdText.text = value.ToString();
            }
            else{
                cdText.text = tipLoading;
            }
        }

        //顯示讀取的跑條
        if (cdImage != null){
            cdImage.fillAmount = value / cdTime;
        }
        if (cdImageBackGround != null){
            cdImageBackGround.gameObject.SetActive(true);
        }


        while (value > 0){

            //如果手拿開就停止執行
            if (canUSE){
                yield break;
            }

            yield return new WaitForSeconds(1);

            //如果手拿開就停止執行
            if (canUSE){
                yield break;
            }

            //變更、記錄進度
            value -= 1;            
            cdSaveProgress = value;

            //更新倒數文字顯示的數值
            if (cdText != null) {
                if (bTextCountDown){
                    cdText.text = value.ToString();
                }
            }
        }

        //觸碰完成，執行以下.....
        PickableEffect();                 //啟動效果
        this.f_LostLife(1);               //失去使用次數(生命)
        useNumber = this.f_GetHaveLife(); //刷新資訊
        cdSaveProgress = cdTime;          //重置進度的紀錄
        cdImageSaveProgress = 0;          //重置跑條的記錄進度

        if (StaticValue.m_bIsMaster){        //以主玩家去判斷
            if (this.f_GetHaveLife() == 0) { //如果使用次數歸零，殺死自己來消除物件
                RolePureHpAction tmpAction = new RolePureHpAction();               //新同步扣血
                tmpAction.f_BeAttack(m_iId, this.f_GetHp());                       //殺死自己（血量歸０）
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction); //執行同步扣血
            }
        }


        if (cdText != null){
            cdText.text = tipFalse;
        }


        if (bAutoReset){ //如果開啟自動重置 (用於東西如果可被多次拿取時，拿完一次後手不用拿開，就會自動拿第二次)
            f_Reset();     //開始重置 (否則是在 FixedUpdate()觸發重置，要先手拿開，才能觸發第二次拿取)
        }

        //隱藏跑條
        else {
            if (cdImage != null) {
                cdImage.fillAmount = 0;
            }
            if (cdImageBackGround != null) {
                cdImageBackGround.gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// 跑條UI
    /// </summary>
    IEnumerator CDUI() {

        //如果沒有設置跑條物件就不執行
        if (cdImage == null){
            yield break;
        }

        //如果沒有紀錄拿取進度，就讓跑條從0開始跑，否則就從記錄的位置開始讀取
        if (!bKeepProgress) {
            cdImage.fillAmount = 0;
        }
        else {
            cdImage.fillAmount = cdImageSaveProgress;
        }


        float speed = 1 / cdTime;
        while (cdImage.fillAmount < 1.0f) {

            //如果中途手拿開，跑條進度歸零(隱藏不顯示)，並停止跑條
            if (canUSE){
                cdImage.fillAmount = 0;
                yield break;            
            }

            //手沒拿開，跑條顯示讀取進度
            cdImage.fillAmount += Time.deltaTime * speed;
            cdImageSaveProgress = cdImage.fillAmount;
            yield return null;
        }

    }


    /// <summary>
    /// 重置
    /// </summary>
    /// <param name="bCanUse"> 能否再次拿取 </param>
    public void f_Reset(bool bCanUse = true) {
        canUSE = true;         //可再次拿取 (true=可以)
        StopCoroutine("CD");   //停止CD
        StopCoroutine("CDUI"); //停止跑條
        _Owner = null;
        tmpPlayerSelf = null;
        tmpPlayerOther = null;
        PickBy_Self = null;
        PickBy_Other = null;

        //文字顯示可以碰、跑條隱藏
        if (cdText != null){
            cdText.text = tipTrue; 
        }
        if (cdImage != null){
            cdImage.fillAmount = 0; 
        }
        if (cdImageBackGround != null){
            cdImageBackGround.gameObject.SetActive(false);
        }

        //手拿開特效
        if (HandLeaveFx != null) {
            Instantiate(HandLeaveFx, transform.position, transform.rotation);
        }

    }
    #endregion



    #region 觸發效果 (在這新增新物品、對應效果)
    /// <summary>
    /// 觸發效果
    /// </summary>
    private void PickableEffect() {
        if (PickableFx != null) {
            Instantiate(PickableFx, transform.position, transform.rotation);
        }

        if (PickableType == PickableEM.Tonic){
            Tonic();
        }
        else if (PickableType == PickableEM.Ammo){
            Ammo();
        }
        else if (PickableType == PickableEM.AmmoClip) {
            AmmoClip();
        }
        else if (PickableType == PickableEM.Parameter){
            SetParameter();
        }
    }



    /// <summary>
    /// 補血效果
    /// </summary>
    public void Tonic(){
        if (StaticValue.m_bIsMaster) {
            RolePureHpAction tmpAction = new RolePureHpAction();
            tmpAction.f_Hp(_Owner.m_iId, this.f_GetAttackPower());
            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
        }
    }


    /// <summary>
    /// 彈藥包
    /// </summary>
    public void Ammo(){
        if (StaticValue.m_bIsMaster) {
            Action_PushBullet tmpAction = new Action_PushBullet();
            tmpAction.f_Set(_Owner.m_iId, this.f_GetAttackPower());
            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
        }
    }


    /// <summary>
    /// 彈夾
    /// </summary>
    public void AmmoClip(){
        if (StaticValue.m_bIsMaster) {
            Action_AddGunClip tmpAction = new Action_AddGunClip();
            tmpAction.f_Set(_Owner.m_iId, this.f_GetAttackPower());
            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
        }
    }


    /// <summary>
    /// 改參數
    /// </summary>
    public void SetParameter() {
        BattleMain.GetInstance().f_SetParamentData(this.f_GetReadme(), this.f_GetAttackPower().ToString());
    }

    #endregion






}


using ccU3DEngine;
using System.Collections.Generic;
using UnityEngine;

public class ArcherRoleControl : BaseRoleControllV2
{
    [Header("---------遠程初始----------------------")]
    [Rename("躲藏點是否初始化了")] public bool ArcherInit;
    [Rename("頭部是否被擊中")]     public bool hitHead;
    private bool DieFxCreate = false;

    [HelpBox("Mv_ChangePos() 換位置", HelpBoxType.Info)]

    [Header("---------死亡相關----------------------")]
    [HelpBox("是否使用布娃娃的開關在布娃娃程式上", HelpBoxType.Info, height = 2)]
    [Rename("死亡時間 (多久後消失)")] public float DieTime = 3;
    [Rename("死亡特效 (普通)")]       public GameObject DieFx;
    [Rename("死亡特效 (暴頭)")]       public GameObject DieFxHead;

    [Header("---------常用音效----------------------")]
    [Rename("待機音效")] public AudioClip Clip_Idle;
    [Rename("追擊音效")] public AudioClip Clip_Chase;
    [Rename("攻擊音效")] public AudioClip Clip_Attack;
    [Rename("死亡音效")] public AudioClip Clip_Die;
    private AudioSource audioOne;  //播放聲音用

    [Header("---------自爆----------------------")]
    [Rename("開啟自爆")] public bool useSelfDestroy = false;
    [Rename("自爆範圍")] public float SelfDestroyRange;
    [Rename("自爆傷害")] public int SelfDestroyAttack;
    private List<BaseRoleControllV2> NearRole = null; //被自爆傷到的人


    [Header("---------躲避点----------------------")]
    public List<Transform> HidePos; //自己擁有的躲藏點
    [Rename("當下躲藏點")]     public int CurHidePos;
    [Rename("躲藏點偵測範圍")] public float HideDistance = 10f;

    [Header("---------被攻擊的忍耐次數--------------")]
    [Rename("當下忍耐次數")] public int CurPatienceCount;
    [Rename("最大忍耐次數")] public int MaxPatienceCount;

    [Header("---------攻擊語音----------------------")]
    public AudioClip[] AttactVoice;

    //接收動畫事件用
    private ccCallback _CallBack_RecvAnimatorEvent = null;


    //初始化
    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true) {
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight = 1, bUpdatePos = true);

        // 遠程初始
        ArcherInit = false;
        CurPatienceCount = MaxPatienceCount;

        // 躲藏點
        HidePos.Clear();

        for (int i = 0; i < BattleMain.GetInstance().HidePos.Length; i++) {
            if (BattleMain.GetInstance().HidePos[i] != null) {
                if (Vector3.Distance(transform.position, BattleMain.GetInstance().HidePos[i].position) <= HideDistance) {
                    HidePos.Add(BattleMain.GetInstance().HidePos[i]);
                }
            }
        }

        if (HidePos.Count == 0) {
            //Debug.LogError(this.m_iId + "未找到任何躲藏點請重新設置");
            //Debug.LogWarning(m_iId + "距離 " + HideDistance + " 內，無法在" + BattleMain.GetInstance().HidePos.Length  + " 中找不到躲藏點!");
            ArcherInit = true;
        }

        //if (this.GetComponent<Module_Shoot_TwoHand>() != null) {
        //    this.GetComponent<Module_Shoot_TwoHand>().ForceStopMuzzleFX();
        //}

        DieFxCreate = false;
        audioOne = this.GetComponent<AudioSource>();
    }


    //跳過需要把模型放置在RoleModel上的步驟
    protected override Transform GetRoleModel(){
        return transform;
    }



    public override void f_Die(){

        if (hitHead){
            if (DieFxHead != null && DieFxCreate == false) {
                //Instantiate(DieFxHead, transform.position, transform.rotation);
                BattleMain.GetInstance().m_ParticlePool.Spawn(DieFxHead, transform.position, transform.rotation);
                hitHead = false;
            }
        }
        else {
            if (DieFx != null && DieFxCreate == false) {
                //Instantiate(DieFx, transform.position, transform.rotation);
                BattleMain.GetInstance().m_ParticlePool.Spawn(DieFx, transform.position, transform.rotation);
            }
        }
        DieFxCreate = true;
        if (StaticValue.m_bIsMaster) {
            if (useSelfDestroy) {
                SelfDestroy();
            }
            UnSetPos();
            Action_Die tRoleDieAction = new Action_Die();
            tRoleDieAction.f_Die(m_iId, f_GetTeamType(), DieTime);
            f_AddMyAction(tRoleDieAction);
        }


        if (this.GetComponent<Module_Shoot_TwoHand>() != null) {
            this.GetComponent<Module_Shoot_TwoHand>().ForceStopMuzzleFX();
        }
    }







    public override void f_BeAttack(int iHP, int iRoleId, GameEM.EM_BodyPart tBodyPart)
    {
        CurPatienceCount--;
        base.f_BeAttack(iHP, iRoleId, tBodyPart);
    }


    /// <summary>
    /// 播放聲音
    /// </summary>
    /// <param name="ClipName"> 聲音名稱 </param>
    public void SoundOne(string ClipName) {
        if (ClipName == "Idle"){
            //audioOne.PlayOneShot(Clip_Idle);
        }
        else if (ClipName == "Chase") {
            //audioOne.PlayOneShot(Clip_Chase);
        }
        else if (ClipName == "Attack"){
            //audioOne.PlayOneShot(Clip_Attack);
        }
        else if (ClipName == "Die"){
            audioOne.PlayOneShot(Clip_Die);
        }
    }



    public override void f_AttactVoice(){
        if (AttactVoice != null && AttactVoice.Length > 0) {
            AudioManager._ins.PlayOneShot(AttactVoice[Random.Range(0, AttactVoice.Length)], 1, this.gameObject.name + " (ArcherRoleControl.cs)");
            //AudioManager._ins.PlayAudioClip("ArcherRoleControl_m_iId:" + m_iId, transform.position, AttactVoice[Random.Range(0, AttactVoice.Length)]);
        }
    }


    /// <summary>
    /// 自爆
    /// </summary>
    private void SelfDestroy(){
        if (!StaticValue.m_bIsMaster) {
            return;
        }
        NearRole = BattleMain.GetInstance().m_BattleRolePool.f_FindTeamTargetAll(this, this.f_GetTeamType(), SelfDestroyRange);
        if (NearRole.Count != 0) { //如果指定的隊伍有玩家
            for (int i = 0; i < NearRole.Count; i++) {
                if (NearRole[i].f_GetRoleType() == GameEM.emRoleType.Pickable) {
                    continue;
                }
                RolePureHpAction tmpAction = new RolePureHpAction();
                tmpAction.f_BeAttack(NearRole[i].m_iId, SelfDestroyAttack);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
            }
        }
    }



    /// <summary>
    /// 切換點
    /// </summary>
    public void Mv_ChangePos(){
        if (!StaticValue.m_bIsMaster){
            return;
        }
        int rangeRadomNum = 0;
        if (HidePos.Count > 1) {
            rangeRadomNum = Random.Range(0, HidePos.Count);
            if (CurHidePos != rangeRadomNum) {
                RoleChangePointAction tmpAction = new RoleChangePointAction();
                tmpAction.f_ChangePoint(m_iId, "AI_ChangePoint", rangeRadomNum);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
            }
        }
    }


    /// <summary>
    /// (動畫事件) 開槍
    /// </summary>
    //void zShoot() {
    //    GameObject oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(); //產生子彈
    //    oBullet.transform.position = MuzzlePos.position;                              //設定子彈位置
    //    oBullet.transform.rotation = MuzzlePos.rotation;                              //設定子彈朝向
    //    BaseBullet tBaseBullet = oBullet.GetComponent<BaseBullet>();                    //取得子彈元件
    //    tBaseBullet.f_Fired((GameEM.TeamType)this.f_GetTeamType(), BulletSpeed, BulletLife, this.m_iId, this.f_GetAttackPower());
    //}


    /// <summary>
    /// 播放完換子彈動作，完成換彈
    /// </summary>
    //private void CallBack_Reload(object obj){
    //    BulletAmount = 7;
    //   GetComponent<Animator>().Play("Attack");
    //}


    /// <summary>
    /// 顯示躲避點偵測範圍
    /// </summary>
    //private void OnDrawGizmos() {
    //    if (!_bDebugHide){
    //        return;
    //    }
    //    #if UNITY_EDITOR
    //    Handles.color = tmpColor;
    //    Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, HideDistance);
    //    Gizmos.color = tmpColor;
    //    Gizmos.DrawSphere(transform.position, HideDistance);
    //    #endif
    //}
}

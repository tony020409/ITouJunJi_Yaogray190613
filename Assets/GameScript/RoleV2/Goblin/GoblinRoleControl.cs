
using ccU3DEngine;
using UnityEngine;

public class GoblinRoleControl : BaseRoleControllV2
{

    [Header("---------遠程攻擊----------------------")]
    [Rename("子彈發射口")]       public Transform MuzzlePos;
    //[Rename("射速")]             public float BulletSpeed;
    //[Rename("子彈存活時間")]     public float BulletLife;
    //[Rename("攜彈量")]           public float BulletAmount;
    //[Rename("換子彈的動畫時間")] public float ReloadTime;
    //[HelpBox("動畫調用 zShoot() 發射子彈", HelpBoxType.Info, height = 2)]

    [Header("---------死亡相關----------------------")]
    [HelpBox("是否使用布娃娃的開關在布娃娃程式上",HelpBoxType.Info, height = 2)]

    [Header("---------常用音效----------------------")]
    [Rename("待機音效")] public AudioClip Clip_Idle;
    [Rename("追擊音效")] public AudioClip Clip_Chase;
    [Rename("攻擊音效")] public AudioClip Clip_Attack;
    [Rename("死亡音效")] public AudioClip Clip_Die;
    private AudioSource audioOne;  //播放聲音用

    public Transform StonePos;
    public MeshRenderer StoneRenderer;


    //接收動畫事件用
    private ccCallback _CallBack_RecvAnimatorEvent = null;


    //初始化
    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true){
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight, bUpdatePos = true);

    }


    //跳過需要把模型放置在RoleModel上的步驟 ======================================
    protected override Transform GetRoleModel() {
        return transform;
    }


    /// <summary>
    /// 播放聲音
    /// </summary>
    /// <param name="ClipName"> 聲音名稱 </param>
    public void SoundOne(string ClipName) {
        if (ClipName == "Idle") {
            //audioOne.PlayOneShot(Clip_Idle);
        }
        else if (ClipName == "Chase") {
            //audioOne.PlayOneShot(Clip_Chase);
        }
        else if (ClipName == "Attack") {
            //audioOne.PlayOneShot(Clip_Attack);
        }
        else if (ClipName == "Die") {
            //audioOne.PlayOneShot(Clip_Die);
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

    //public override void f_CallBack_Throw (object obj)
    //{
    //    base.f_CallBack_Throw(obj);
    //
    //    StonePos.LookAt(BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(this, this.f_GetViewSize()).transform.position);
    //    
    //    int iBulletId = 4;
    //    RoleThrowAttackAction tRoleThrowAttackAction = new RoleThrowAttackAction();
    //    tRoleThrowAttackAction.f_Attack(
    //        ccMath.f_CreateKeyId(), 
    //        m_iId, iBulletId, 
    //        GameEM.TeamType.B,
    //        StonePos.transform.position, 
    //        StonePos.transform.rotation
    //        );
    //    f_AddMyAction(tRoleThrowAttackAction);
    //}
}

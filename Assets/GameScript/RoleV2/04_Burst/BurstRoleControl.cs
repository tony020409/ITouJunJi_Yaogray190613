using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurstRoleControl : BaseRoleControllV2
{
    [HelpBox("自爆物件-死亡時給周邊敵人傷害\n" +
             "-------------------------------------\n" +
             "自爆範圍：依照角色表\n" +
             "自爆傷害；依照角色表  " , HelpBoxType.Info)]

    [Rename("開啟自爆傷害")]         public bool useSelfDestroy = true;
    [Rename("自爆後本體多久後消失")] public float DieTime = 0.5f;
    [Rename("自爆音效")]             public AudioClip  DiesSound;
    [Rename("自爆特效")]             public GameObject DieFx;
    private bool DieFxCreate = false;
    private AudioSource audioOne;  //播放聲音用



    //初始化
    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true){
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight = 1, bUpdatePos = true);
        audioOne = this.GetComponent<AudioSource>();
        if (audioOne == null && DiesSound!=null) {
            this.gameObject.AddComponent<AudioSource>();
            audioOne = this.GetComponent<AudioSource>();
        }
        DieFxCreate = false;
    }


    /// <summary>
    /// 跳過需要把模型放置在RoleModel上的步驟
    /// </summary>
    protected override Transform GetRoleModel() {
        return transform;
    }


    /// <summary>
    /// 受到傷害時
    /// </summary>
    public override void f_BeAttack(int iHP, int iRoleId, GameEM.EM_BodyPart tBodyPart)
    {
        if (f_IsDie())
        {
            return;
        }
        else if (f_IsInv())
        {
            return;
        }
        LostHP(iHP);

        //处理死亡
        if (f_GetHp() <= 0)
        {
            if (StaticValue.m_bIsMaster)
            {
                float delay = UnityEngine.Random.Range(0.1f, 0.5f);                          //隨機延後爆炸時間
                Action_DelayDie tmpAction = new Action_DelayDie();
                tmpAction.f_DelayDie(m_iId, f_GetTeamType(), delay);
                f_AddMyAction(tmpAction);
                //ccTimeEvent.GetInstance().f_RegEvent(delay, false, null, CallBack_DelayDie); //執行延後爆炸
            }
            DispScore(iRoleId, m_iId, tBodyPart, true);
        }
        else
        {
            DispScore(iRoleId, m_iId, tBodyPart, false);
        }
    }

    /// <summary>
    /// 同步執行f_Die()
    /// </summary>
    private void CallBack_DelayDie(object Obj){
        Action_DelayDie tmpAction = new Action_DelayDie();
        tmpAction.f_DelayDie(m_iId, f_GetTeamType());
        f_AddMyAction(tmpAction);
    }


    /// <summary>
    /// 死亡行為：產生爆炸特效、給周圍傷害、自身死亡
    /// </summary>
    public override void f_Die() {
        if (DieFx != null && DieFxCreate == false){
            //Instantiate(DieFx, transform.position, transform.rotation);
            BattleMain.GetInstance().m_ParticlePool.Spawn(DieFx, transform.position, transform.rotation);
        }
        if (DiesSound != null && DieFxCreate == false) {
            audioOne.PlayOneShot(DiesSound);
        }
        DieFxCreate = true;
        if (StaticValue.m_bIsMaster){
            if (useSelfDestroy){
                SelfDestroy();
            }
            UnSetPos();
            Action_Die tRoleDieAction = new Action_Die();
            tRoleDieAction.f_Die(m_iId, f_GetTeamType(), DieTime);
            f_AddMyAction(tRoleDieAction);
        }
    }


    /// <summary>
    /// 自爆傷害
    /// </summary>
    private void SelfDestroy(){
        if (!StaticValue.m_bIsMaster) {
            return;
        }
        List<BaseRoleControllV2> NearRole = BattleMain.GetInstance().m_BattleRolePool.f_FindTeamTargetAll(this, this.f_GetTeamType(), this.f_GetAttackSize());
        if (NearRole.Count != 0) { //如果指定的隊伍有玩家
            NearRole = NearRole.OrderBy( x => Vector2.Distance(this.transform.position, x.transform.position)).ToList(); //排序
            for (int i = 0; i < NearRole.Count; i++) {
                RolePureHpAction tmpAction = new RolePureHpAction();
                tmpAction.f_BeAttack(NearRole[i].m_iId, this.f_GetAttackPower());
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
            }
        }
    }



}

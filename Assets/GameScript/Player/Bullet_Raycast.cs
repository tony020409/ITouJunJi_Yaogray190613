using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Bullet_Raycast_HitEffect {
    [Rename("擊中怪物")] public GameObject HitMonster;
    [Rename("擊中頭部")] public GameObject HitMonsterHead;
    [Rename("擊中木頭")] public GameObject HitWood;
    [Rename("擊中石頭")] public GameObject HitRock;
    [Rename("擊中金屬")] public GameObject HitMetal;
}


public class Bullet_Raycast : BaseBullet
{
    //判斷是否完成射線初始化
    private bool _bInitRay = false;
    [Rename("開啟小吳子彈間碰撞")] public bool useBulletCollider;
    [Rename("射線偵測的Layer")]    public LayerMask EnemyLayer;
    [Rename("射線調整值")]         public float rayOffset = 0.0f;
    [Rename("射線當前的長度(由子彈表射速與存活時間控制)")]  public float rayLength = 0.1f;
    [Line()]
    [Rename("啟用擊中特效")]                                            public bool useDieEffect = true;
    [Rename("自爆特效")]                                                public GameObject DieEffectSelf;
    [Rename("擊中效果 (資源請放在 Resource/Model/HitEffect 資料夾下)")] public Bullet_Raycast_HitEffect DieEffect;
    [Line()]
    [Rename("啟用擊中音效")] public bool useReboundSounds = true;
    [Rename("死亡音效")]     public AudioClip[] ReboundSounds;

    private Vector3 StartPos;               //射線起點(槍口位置)
    private float tmpRayLength = 0;         //記錄射線的初始長度 (還原用)
    private bool haveTrailRenderer = false; //是否掛有TrailRenderer組件
    private TrailRenderer _TrailRenderer;   //TrailRenderer組件
    private string ResourcePath;            //特效資源路徑
    private bool onlyCustomFX = false;      //只使用特殊特效
    //private SpawnPool _EffectPool = PoolManager.Pools["Particle"];


    //開火時記錄或還原射線長度
    protected override void f_TypeFired() {
        base.f_TypeFired();
        if (!_bInitRay) {
            tmpRayLength = rayLength;
            if (GetComponent<TrailRenderer>() != null) {
                haveTrailRenderer = true;
                _TrailRenderer = GetComponent<TrailRenderer>();
            }
            else if (GetComponentInChildren<TrailRenderer>() != null) {
                haveTrailRenderer = true;
                _TrailRenderer = GetComponentInChildren<TrailRenderer>();
            }
            _bInitRay = true;
        }
        //rayLength = tmpRayLength;
        rayLength = 0;
        if (haveTrailRenderer) {
            _TrailRenderer.enabled = true;
        }
        StartPos = transform.position;
    }


    //覆寫子彈間的碰撞，讓它永遠不檢測
    protected override bool f_CheckIsCollide(){
        if (useBulletCollider) {
            base.f_CheckIsCollide();
        }
        return false;
    }

    protected override bool f_CheckIsCollideRole() {

        //子彈壽命歸零時，銷毀子彈
        if (_BulletDT.fBulletLife < 0){
            rayLength = 0;
            Die();
            return false;
        }

        //否則向前發出一條射線
        else
        {
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            rayLength += _BulletDT.fSpeed * Time.deltaTime;

            //如果射線擊中敵人
            if (Physics.Raycast(StartPos, fwd, out hit, rayLength + rayOffset, EnemyLayer)) {

                //忽視死掉的玩家
                if (hit.collider.GetComponent<AttackPart>() != null) {
                    AttackPart ap = hit.collider.GetComponent<AttackPart>();
                    if (ap._BaseRoleControl != null) {
                        if (ap._BaseRoleControl.f_GetRoleType() == GameEM.emRoleType.Player) {
                            if (ap._BaseRoleControl.f_IsDie()) {
                                return false;
                            }
                        }
                    }
                }

                //射線停止
                rayLength = 0;
                DebugBullet(Color.green);

                //如果敵人身上有攻擊判定的碰撞體
                if (hit.collider.GetComponent<AttackPart>() != null) {
                    AttackPart ap = hit.collider.GetComponent<AttackPart>(); 
                    if (ap._BaseRoleControl != null) {
                        HitRole(ap);
                        //if (ap.EW == weakness.head) {
                        //    CustomHitEffect(hit, "Head");
                        //}
                    }

                    if (ap._BaseBullet != null) {
                        HitBullet(ap);
                    }
                }

                //如果偵測到的碰撞體沒有攻擊判定
                else {
                    Die();
                }

                //產生擊中特效
                if (!onlyCustomFX) {
                    FX(hit);
                }
                Die();
                return true;
            }

            //沒射到敵人的情況
            else {
                DebugBullet(Color.white);
            }
        }
        return false;
    }

    /// <summary>
    /// 子彈射擊情況測試用
    /// </summary>
    /// <param name="hitValue"></param>
    private void DebugBullet(Color hitValue) {
        Debug.DrawRay(StartPos, transform.TransformDirection(Vector3.forward) * rayLength, hitValue);
    }

    protected override void Die() {
        //GameObject oBullet = null;
        //oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateResource2(DieEffectPath);                   //產生資源
        //if (oBullet != null) {                                                                                 //如果資源存在
        //    oBullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);     //設定資源位置
        //    oBullet.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w); //設定資源朝向
        //}
        onlyCustomFX = false;
        if (haveTrailRenderer) {
            _TrailRenderer.enabled = false;
        }
        if (DieEffectSelf != null){
            //Instantiate(DieEffectSelf, transform.position, transform.rotation); //在擊中位置產生自帶的特殊銷毀特效 (e.g. 飛彈爆炸)
            BattleMain.GetInstance().m_ParticlePool.Spawn(DieEffectSelf, transform.position, transform.rotation);
        }
        base.Die();
    }




    /// <summary>
    /// 產生擊中特效+音效
    /// </summary>
    /// <param name="hit"         > 擊中點資訊 </param>
    /// <param name="hcustomCheck"> 非 Layer層的特殊判斷 </param>
    private void FX(RaycastHit hit) {

        //擊中特效
        if (useDieEffect) {
            GameObject oBullet = null;
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Monster")) {
                if (DieEffect.HitMonster != null) {
                    //ResourcePath = "Model/HitEffect/" + DieEffect.HitMonster.name;
                    AttackPart ap = hit.collider.GetComponent<AttackPart>();
                    if (ap != null && ap.EW == GameEM.EM_BodyPart.Body) {
                        //oBullet = Instantiate(DieEffect.HitMonster, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); //在擊中位置產生擊中特效
                        CreateEffect(DieEffect.HitMonster, hit);
                    }
                    else if (ap != null && ap.EW == GameEM.EM_BodyPart.Head) {
                        if (DieEffect.HitMonsterHead != null) {
                            //Instantiate(DieEffect.HitMonsterHead, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                            CreateEffect(DieEffect.HitMonsterHead, hit);
                        }
                    }
                    else if (ap == null) {
                        CreateEffect(DieEffect.HitMonster, hit);
                    }
                }
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wood")) {
                if (DieEffect.HitWood != null) {
                    //ResourcePath = "Model/HitEffect/" + DieEffect.HitWood.name;
                    //oBullet = Instantiate(DieEffect.HitWood, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); //在擊中位置產生擊中特效
                    CreateEffect(DieEffect.HitWood, hit);
                }
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Rock")) {
                if (DieEffect.HitRock != null)  {
                    //ResourcePath = "Model/HitEffect/" + DieEffect.HitRock.name;
                    //oBullet = Instantiate(DieEffect.HitRock, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); //在擊中位置產生擊中特效
                    CreateEffect(DieEffect.HitRock, hit);
                }
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Metal")) {
                if (DieEffect.HitMetal != null) {
                    //ResourcePath = "Model/HitEffect/" + DieEffect.HitMetal.name;
                    //oBullet = Instantiate(DieEffect.HitMetal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); //在擊中位置產生擊中特效
                    CreateEffect(DieEffect.HitMetal, hit);
                }
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Default")) {
                //ResourcePath = "Model/HitEffect/" + "Null";
            }
            //oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateResource(ResourcePath);       //產生資源
            //if (oBullet != null) {                                                                   //如果資源存在
            //    oBullet.transform.position = hit.point;                                              //設定資源位置
            //    oBullet.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal); //計算特效朝向
            //}
        }

        //擊中音效
        if (useReboundSounds) {
            if (ReboundSounds.Length == 0) {
                return;
            }
            int rangeRadomNum = Random.Range(0, ReboundSounds.Length);
            AudioManager._ins.PlayOneShot(ReboundSounds[rangeRadomNum], 1, this.gameObject.name + " (Bullet_Raycast.cs)");
            //AudioManager._ins.PlayAudioClip("Bullet_Raycast", transform.position, ReboundSounds[rangeRadomNum]);
        }

    }



    /// <summary>
    /// 生物件
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="tmp"></param>
    private void CreateEffect( GameObject tmp, RaycastHit hit) {
        //if (_EffectPool != null && _EffectPool.Contains(tmp.transform)) {
        //    _EffectPool.Spawn(tmp, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
        //} else {
        //    Instantiate(tmp, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
        //}
        BattleMain.GetInstance().m_ParticlePool.Spawn(tmp, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
    }



    /// <summary>
    /// 特殊的擊中特效
    /// </summary>
    /// <param name="hit"        > 擊中點判斷   </param>
    /// <param name="customCheck"> 特殊判斷字符 </param>
    private void CustomHitEffect(RaycastHit hit, string customCheck) {
        if (customCheck == "Head") {
            //ResourcePath = "Model/HitEffect/" + DieEffect.HitMonsterHead.name;
            onlyCustomFX = true;
            if (DieEffect.HitMonsterHead != null) {
                CreateEffect(DieEffect.HitMonsterHead, hit);
            }
        }
        //GameObject oBullet = null;
        //oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateResource(ResourcePath);       //產生資源
        //if (oBullet != null){                                                                    //如果資源存在
        //    oBullet.transform.position = hit.point;                                              //設定資源位置
        //    oBullet.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal); //計算特效朝向
        //}
    }



    /// <summary>
    /// 擊中角色
    /// </summary>
    /// <param name="ap"> 擊中點資訊 </param>
    private bool HitRole(AttackPart ap) {
        BaseRoleControllV2 tmpEnemy = BattleMain.GetInstance().f_GetRoleControl2(ap._BaseRoleControl.m_iId); //被攻擊者

        //如果射中的敵人不存在，不給予傷害
        if (tmpEnemy == null) {
            return false;
        }

        //射中自己人，不給予傷害
        if (!GloData.glo_bCanShootMySelf) {
            if (tmpEnemy.f_GetTeamType() == _emTeamType) {
                Die();
                return false;
            }
        }


        //射中敵人身體，普通扣血
        if (ap.EW == GameEM.EM_BodyPart.Body) {
            if (StaticValue.m_bIsMaster) {                                    //由主玩家場景的情況發動同步扣血
                RoleHpAction tRoleHpAction = new RoleHpAction();
                tRoleHpAction.f_Hp(_iPlayerId, tmpEnemy.m_iId, _BulletDT.iPower, Vector3.zero, GameEM.EM_BodyPart.Body);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleHpAction); //同步攻擊執行
            }
            Die();
            return true;
        }


        //射中敵人頭部，一擊斃殺
        if (ap.EW == GameEM.EM_BodyPart.Head) {
            if (StaticValue.m_bIsMaster){  //由主玩家場景的情況發動同步扣血
                RoleHpAction tRoleHpAction = new RoleHpAction();
                tRoleHpAction.f_Hp(_iPlayerId, tmpEnemy.m_iId, tmpEnemy.f_GetHp(), Vector3.zero, GameEM.EM_BodyPart.Head);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleHpAction);
            }
            if (tmpEnemy.GetComponent<ArcherRoleControl>() != null){
                tmpEnemy.GetComponent<ArcherRoleControl>().hitHead = true;
            }
            Die();
            return true;
        }


        return false;
    }


    /// <summary>
    ///  擊中子彈
    /// </summary>
    /// <param name="ap">  擊中點資訊 </param>
    private bool HitBullet(AttackPart ap) {
        if (StaticValue.m_bIsMaster) {                                           //由主玩家場景的情況發動同步扣血
            //RoleBeAttackAction tmpAction = new RoleBeAttackAction();             //新攻擊指令
            //tmpAction.f_NetBeAttack(ap._BaseBullet.f_GetId(), _BulletDT.iPower); //同步攻擊設定
            RoleHpAction tRoleHpAction = new RoleHpAction();
            tRoleHpAction.f_Hp(_iPlayerId, ap._BaseBullet.f_GetId(), _BulletDT.iPower, Vector3.zero);
            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleHpAction);   //同步攻擊執行 (角色發動)
        }
        Die();
        return true;
    }

}


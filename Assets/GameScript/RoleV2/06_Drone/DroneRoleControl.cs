using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 無人機怪物
/// </summary>
public class DroneRoleControl : BaseRoleControllV2 {

    [Rename("攻擊狀態開關")]  public bool _CanAtack = false;
    [Rename("攻擊目標")] public Transform target = null;
    [Rename("武器清單")] public GameObject[] direction;
    

    [Header("---------被攻擊的忍耐次數--------------")]
    [Rename("當下忍耐次數")] public int CurPatienceCount = 0;
    [Rename("最大忍耐次數")] public int MaxPatienceCount = 20;

    [Header("---------死亡相關----------------------")]
    [Rename("死亡特效")] public GameObject DieFx;

    [Header("---------攻擊相關----------------------")]
    [Rename("當前彈藥量")] public int BulletAmount = 0;



    //動畫控制器
    private Animator _animator;


    /// <summary>
    /// (覆寫) 初始化
    /// </summary>
    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true) {
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight = 1, bUpdatePos = true);

        //取得動畫控制器
        _animator = this.GetComponent<Animator>();

        //重置受到攻擊的忍耐次數
        CurPatienceCount = MaxPatienceCount;
    }

    /// <summary>
    /// (覆寫) 死亡
    /// </summary>
    public override void f_Die() {
        base.f_Die();

        //產生死亡特效
        if (DieFx != null)  {
            Instantiate(DieFx, transform.position, transform.rotation);
        }

    }

    /// <summary>
    /// (覆寫) 受到攻擊
    /// </summary>
    public override void f_BeAttack(int iHP, int iRoleId, GameEM.EM_BodyPart tBodyPart) {
        base.f_BeAttack(iHP, iRoleId, tBodyPart);

        //忍耐次數-1
        CurPatienceCount -= 1;

        //若忍耐次數耗盡，播放特殊動畫，然後重置忍耐次數
        if (CurPatienceCount == 0) {
            _animator.Play("HeavyHurt");
            CurPatienceCount = MaxPatienceCount;
        }

    }


}

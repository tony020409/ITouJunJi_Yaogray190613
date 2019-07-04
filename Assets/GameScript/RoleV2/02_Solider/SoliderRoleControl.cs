
using ccU3DEngine;
using System.Collections.Generic;
using UnityEngine;


public class SoliderRoleControl : BaseRoleControllV2 {

    [Header("---------遠程攻擊----------------------")]
    [HelpBox("掛上 Module_Shoot.cs", HelpBoxType.Info, height = 2)]

    [Header("---------死亡相關----------------------")]
    [HelpBox("是否使用布娃娃的開關在布娃娃程式上", HelpBoxType.Info, height = 2)]

    [Header("---------常用音效----------------------")]
    [Rename("待機音效")]
    public AudioClip Clip_Idle;
    [Rename("追擊音效")] public AudioClip Clip_Chase;
    [Rename("攻擊音效")] public AudioClip Clip_Attack;
    [Rename("死亡音效")] public AudioClip Clip_Die;
    private AudioSource audioOne;  //播放聲音用

    [Header("---------躲避点----------------------")]
    public List<Transform> HidePos;
    [Rename("當下躲藏點")] public int CurHidePos;

    //接收動畫事件用
    private ccCallback _CallBack_RecvAnimatorEvent = null;


    //初始化
    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true) {
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight, bUpdatePos = true);
        audioOne = this.GetComponent<AudioSource>();
        // 最近躲藏點序號
        int HideIndex = 0;
        // 最近躲藏點距離
        float HideDistance = 10f;

        // 躲藏點
        HidePos.Clear();

        for (int i = 0; i < BattleMain.GetInstance().HidePos.Length; i++) {
            if (Vector3.Distance(transform.position, BattleMain.GetInstance().HidePos[i].position) <= 10) {
                HidePos.Add(BattleMain.GetInstance().HidePos[i]);
            }
        }

        for (int i = 0; i < HidePos.Count; i++) {
            if (Vector3.Distance(transform.position, HidePos[i].position) < HideDistance) {
                HideIndex = i;
                HideDistance = Vector3.Distance(transform.position, HidePos[i].position);
            }
        }

        CurHidePos = HideIndex;
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
            audioOne.PlayOneShot(Clip_Die);
        }
    }
}

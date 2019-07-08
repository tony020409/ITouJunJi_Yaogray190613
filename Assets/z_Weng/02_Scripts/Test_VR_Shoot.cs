using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_VR_Shoot : MonoBehaviour {

    [Header("Raycast偵測的Layer")]
    public LayerMask teleportMask;     //允許瞄準的目標Layer層

    [Header("槍口")]
    public Transform GunStartPosition; //槍口

    [Header("射擊頻率 (越小越快)")]
    public float _fShootRate = 0.1f; //射擊頻率
    private float _fShootTime;       //計算頻率用

    [Header("各式特效、音效")]
    public GameObject[] FX;  //擊中特效
    public AudioClip[] clip; //擊中音效
    private RaycastHit hit;  //Raycast擊中的點

    //[HideInInspector]
    [Header("當前能否射擊")]
    public bool canShoot = true;

    [Header("是否開啟同步")]
    public bool useSync = true;

    // SteamVR控制器宣告
    //private SteamVR_TrackedObject trackedObj; 
    //private SteamVR_Controller.Device Controller{
    //    get { return SteamVR_Controller.Input((int)trackedObj.index); }
    //}
    //void Awake(){
    //    trackedObj = GetComponent<SteamVR_TrackedObject>();
    //}


    //玩家宣告
    BaseRoleControllV2 tmpPlayer;
    void Start(){
        tmpPlayer = this.GetComponentInParent<BaseRoleControllV2>();
    }


    void Update(){
        //按下開槍鍵 (canShoot 是由 MySelfPlayerControll2.cs、RifleState.cs去控制)
        //if (Controller.GetHairTrigger()){
            if (canShoot){
               // _MySelfPlayerControll2.OnRightTriggerPressed();
               //Shoot(); //檢查 Raycast


            }
            else{
               // _MySelfPlayerControll2.OnRightTriggerReleased();
                //播放無彈夾音效
            } 
        //}




    }


    //public void Shoot(){
    //    if (Physics.Raycast(trackedObj.transform.position, GunStartPosition.forward, out hit, 100, teleportMask)){
    //        if (_fShootTime <= 0){
    //            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal); //計算特效朝向
    //
    //            //---------------------------------------------------------------------------------------------------------------
    //            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Monster")){ //擊中怪物
    //                CreateObject( 0, hit.point, rot, useSync);                           //在擊中位置產生擊中特效[0]
    //
    //                BaseRoleControllV2 tmpRole = hit.transform.gameObject.GetComponentInParent<BaseRoleControllV2>();
    //                if (tmpRole != null && tmpRole.f_GetHp() < 20){ //20滴血(最後一擊產生另外一種血)
    //                    CreateObject( 1, hit.point, rot, useSync);  //在擊中位置產生擊中特效[1]
    //                }
    //            }
    //
    //            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain")){ //擊中地形
    //                CreateObject( 2, hit.point, rot, useSync);                                //在擊中位置產生擊中特效[2]
    //            }
    //            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wood")){ //擊中木頭 
    //                CreateObject( 3, hit.point, rot, useSync);                              //在擊中位置產生擊中特效[3]
    //            }
    //            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Metal")){ //擊中金屬
    //                CreateObject( 4, hit.point, rot, useSync);                              //在擊中位置產生擊中特效[4]
    //            }
    //            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Rock")) { //擊中石頭
    //                CreateObject(2, hit.point, rot, useSync);                               //在擊中位置產生擊中特效[5]
    //            }
    //            //---------------------------------------------------------------------------------------------------------------
    //            _fShootTime = _fShootRate;
    //        }
    //        _fShootTime -= Time.deltaTime;
    //    }
    //}


    /// <summary>
    /// 產生物件
    /// </summary>
    /// <param name="fxNumber" > 特效編號 </param>
    /// <param name="createPos"> 產生位置 </param>
    /// <param name="createRot"> 產生朝向 </param>
    /// <param name="useSync"  > 是否同步 </param>
    public void CreateObject(int fxNumber, Vector3 createPos, Quaternion createRot, bool useSync) {

        //如果不同步
        if (!useSync){
            if (FX[fxNumber] != null) {
                GameObject childGameObject = Instantiate(FX[fxNumber], createPos, createRot); //在擊中位置產生擊中特效
            }
            else {
                //沒有掛擊中特效
            }
        }

        //如果同步
        else {
            //遊戲開始後才同步特效
            if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming){
                CreateHitEffectAction tmpAction = new CreateHitEffectAction();
                tmpAction.f_SetCreate(fxNumber, createPos, createRot);
                tmpPlayer.f_AddMyAction(tmpAction);
            }
            //否則仍採用本地
            else {
                GameObject childGameObject = Instantiate(FX[fxNumber], createPos, createRot); //在擊中位置產生擊中特效
            }

        }

    }

 
}

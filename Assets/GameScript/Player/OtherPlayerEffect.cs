using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerEffect : MonoBehaviour {

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
    public bool useSync = false;

    private OtherPlayerControll2 _OtherPlayerControl2;


    void Start(){
        _OtherPlayerControl2 = this.GetComponent<OtherPlayerControll2>();
    }

    void Update(){
        if (canShoot){
            if (useSync) {
                Shoot(); //檢查 Raycast
            }
        }
        else{
            //播放無彈夾音效
        }
    }


    public void Shoot()
    {
        if (Physics.Raycast(GunStartPosition.position, GunStartPosition.forward, out hit, 100, teleportMask))
        {
            if (_fShootTime < 0)
            {
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal); //計算特效朝向

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Monster")){ //擊中怪物
                    GameObject childGameObject1 = Instantiate(FX[0], hit.point, rot);    //在擊中位置產生擊中特效

                    BaseRoleControllV2 tmpRole = hit.transform.gameObject.GetComponentInParent<BaseRoleControllV2>();
                    if (tmpRole != null && tmpRole.f_GetHp() < 20){                       //20滴血(最後一擊產生另外一種血)
                        GameObject childGameObject2 = Instantiate(FX[1], hit.point, rot); //在擊中位置產生擊中特效
                    }
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain")){ //擊中地形
                    GameObject childGameObject2 = Instantiate(FX[2], hit.point, rot);         //在擊中位置產生擊中特效
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wood")){  //擊中木頭
                    GameObject childGameObject2 = Instantiate(FX[2], hit.point, rot);       //在擊中位置產生擊中特效
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Metal")){ //擊中金屬
                    GameObject childGameObject2 = Instantiate(FX[3], hit.point, rot);       //在擊中位置產生擊中特效
                }

                _fShootTime = _fShootRate;
            }
            _fShootTime -= Time.deltaTime;
        }
    }
}

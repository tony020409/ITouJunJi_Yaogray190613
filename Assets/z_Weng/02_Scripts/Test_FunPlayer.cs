using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FunPlayer : MonoBehaviour
{


    //參數區 ==========================================================================================
    #region 移動參數 ---------------------------------------
    [Header("移動、轉身速度 ---------------------")]
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
    #endregion

    #region 子彈參數 ---------------------------------------
    [Header("子彈發射點--------------------------")]
    public Transform firepoint; //開火點

    [Header("後座力大小 (發射點位置偏差量)")]
    public float Recoil = 0.2f;

    [Header("子彈物件、發射間隔時間、子彈射出去的速度")]
    public Rigidbody bullet;         //子彈物件(Prefabs)
    public float fireRate = 0.5f;    //發射間隔時間(多久射一發)
    private float timer = 0;         //子彈發射緩衝(計時用)
    public float fireSpeed = 400.0f; //射出去的速度(Rigibody)

    [Header("開火特效、音效")]
    public Transform pointFX; //槍口火焰
    public AudioClip FXSound; //槍聲
    private bool isPlay = false;

    [Header("子彈忽略碰撞的物件")]
    public GameObject[] ignoreCollider;
    #endregion

    #region 鍵盤訊息 ---------------------------------------
    //射擊
    private KeyCode Key_Shoot = KeyCode.Keypad0;

    //前後左右 移動
    private KeyCode Key_Move = KeyCode.UpArrow;
    private KeyCode Key_Back = KeyCode.DownArrow;
    private KeyCode Key_L = KeyCode.LeftArrow;
    private KeyCode Key_R = KeyCode.RightArrow;

    //上下 移動
    private KeyCode Key_Up = KeyCode.R;
    private KeyCode Key_Down = KeyCode.F;

    //"左右 平移
    private KeyCode Key_QR = KeyCode.Q;
    private KeyCode Key_QL = KeyCode.E;
    #endregion

    #region 動畫參數 ---------------------------------------
    private int Hand_Action = 0;     //紀錄正在做的動作
    private bool isShooting = false; //是否正在射擊，如果是true，強制設定手部動作為射擊
    #endregion

    #region 聲音參數 ---------------------------------------
    [Header("插件的開槍聲------------------------")]
    public bool play1;
    public bool play2;
    public AimSound.GunSoundSource Ani_Attack1;
    public AimSound.GunSoundSource Ani_Attack2;
    #endregion


    // Start ===========================================================================================
    void Start () {
    }

    // Update ==========================================================================================
    void Update() {

        #region 01. 射擊控制 ==================================================================
        timer += Time.deltaTime;
        if (Input.GetKey(Key_Shoot) &&  timer > fireRate)
        {
            //生成與發射子彈
            timer = 0;                                                                                          //發射頻率設定
            Rigidbody clone;                                                                                    //預製子彈//後座力效果用
            clone = (Rigidbody)Instantiate(bullet, firepoint.position, firepoint.rotation);                     //生成子彈
            clone.transform.position = (firepoint.position + firepoint.forward * 0.0f + firepoint.up * 0.0f);   //子彈產生的位置，數值可隨意調整直到位置OK
            clone.velocity = clone.transform.forward * fireSpeed * -1 + transform.up * Random.Range(-Recoil, Recoil) + transform.right * -1 * Random.Range(-Recoil, Recoil);  //子彈射出的力道與方向 ※ps.「transform.forward*-1」表示transform.backward，因為無法直接打transform.backward，故*-1

            //子彈忽略與指定物件的碰撞
            //Physics.IgnoreCollision(clone.transform.GetComponent<Collider>(), ignoreCollider[0].GetComponent<Collider>());

            //開槍音效
            if (!isPlay){
                //AudioSource.PlayClipAtPoint(FXSound, firepoint.transform.position);
            }

            //開火特效
            Transform t = Instantiate(pointFX) as Transform; // 生成開火特效 並獲得其transform (instantiate prefab and get its transform)
            t.parent = firepoint.transform;                  // 讓開火特效位置固定在發射口     (group the instance under the spawner)
            t.localPosition = Vector3.zero;                  // 使開火特效在spawner的確切位置  (make it at the exact position of the spawner)
            t.localRotation = Quaternion.identity;           // 使開火特效在spawner的確切角度  (same for rotation)

            //5秒後消除子彈
            Destroy(clone.gameObject, 5);
        }
        #endregion


        #region 02. 移動控制 ==================================================================
        // 前進
        if (Input.GetKey(Key_Move))
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
           
        // 後退
        if (Input.GetKey(Key_Back))
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // 左轉
        if (Input.GetKey(Key_L))
            transform.Rotate(-Vector3.up, turnSpeed * Time.deltaTime);

        // 右轉
        if (Input.GetKey(Key_R))
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        // 左平移
        if (Input.GetKey(Key_QL))
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 右平移
        if (Input.GetKey(Key_QR))
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // 上升
        if (Input.GetKey(Key_Up))
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 下降
        if (Input.GetKey(Key_Down))
            transform.Translate(-Vector3.up * moveSpeed * Time.deltaTime);
        #endregion


        #region 03. 動畫控制、開槍聲 ==========================================================
        //手部動畫控制
        if (Input.GetKeyDown(Key_Shoot)){
            isShooting = true;                                            //射擊中
            if(play1) Ani_Attack1.Play();                                 //播放開槍聲1
            if(play2) Ani_Attack2.Play();                                 //播放開槍聲2
            this.GetComponent<Animator>().CrossFade("Hand_Shoot", 0.25f); //手部動畫
        } 
        if (Input.GetKeyUp(Key_Shoot)){
            isShooting = false;                                                                     //停止射擊
            if (play1) Ani_Attack1.Stop();                                                          //停止開槍聲1
            if (play2) Ani_Attack2.Stop();                                                          //停止開槍聲2
            if      (Hand_Action == 0) this.GetComponent<Animator>().CrossFade("Hand_Idle", 0.25f); //待機動畫 = 0
            else if (Hand_Action == 1) this.GetComponent<Animator>().CrossFade("Hand_Move", 0.25f); //前進動畫 = 1
            else if (Hand_Action == 2) this.GetComponent<Animator>().CrossFade("Hand_Back", 0.25f); //後退動畫 = 2
            else if (Hand_Action == 3) this.GetComponent<Animator>().CrossFade("Hand_L", 0.25f);    //左轉動畫 = 3
            else if (Hand_Action == 4) this.GetComponent<Animator>().CrossFade("Hand_R", 0.25f);    //右轉動畫 = 4
        } 

        // 前進----------------------------------------------------------------------
        if (Input.GetKeyDown(Key_Move)){
            Hand_Action = 1;                                                                 //手部當前動作紀錄
            if (!isShooting){ this.GetComponent<Animator>().CrossFade("Hand_Move", 0.25f); } //手部動畫
            this.GetComponent<Animator>().CrossFade("Body_Move", 0.25f);                     //身體動畫
        }
        else if (Input.GetKeyUp(Key_Move)){
            Hand_Action = 0;                                                                 //手部當前動作紀錄
            if (!isShooting){ this.GetComponent<Animator>().CrossFade("Hand_Idle", 0.25f); } //手部動畫
            this.GetComponent<Animator>().CrossFade("Idle", 0.25f);                          //身體動畫
        }

        // 後退-----------------------------------------------------------------------
        if (Input.GetKeyDown(Key_Back)){
            Hand_Action = 2;                                                                 //手部當前動作紀錄
            if (!isShooting){ this.GetComponent<Animator>().CrossFade("Hand_Back", 0.25f); } //手部動畫
            this.GetComponent<Animator>().CrossFade("Body_Back", 0.25f);                     //身體動畫
        }
        else if (Input.GetKeyUp(Key_Back)){
            Hand_Action = 0;                                                                 //手部當前動作紀錄
            if (!isShooting){ this.GetComponent<Animator>().CrossFade("Hand_Idle", 0.25f); } //手部動畫
            this.GetComponent<Animator>().CrossFade("Idle", 0.25f);                          //身體動畫
        }
        #endregion
    }


}

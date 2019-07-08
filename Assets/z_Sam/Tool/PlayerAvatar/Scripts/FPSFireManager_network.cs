using UnityEngine;
using System.Collections;
//using DouduckGame;
using System.Collections.Generic;

public class FPSFireManager_network : MonoBehaviour
{
    //public ImpactInfo[] ImpactElemets = new ImpactInfo[0];
    public float BulletDistance = 100;
    public int MaxBullet = 70;
    public GameObject ImpactEffect;
    public GameObject M4A1_Sopmod;
    public GameObject Shell_Copper;
    public Transform BulletStart;
    //public GameObject BulletStart2;
    //public PlayerControl playerControl;
    //public FireShellController fshellController;
    public float ReloadAngle = 30;

    //private Animator M4A1_Anim;
    //private Animator M4A1_Shell;
    //private Animator M4A1_Magazine;
    private int _nowBullet;
    private bool isReload = false;
    private bool isReloading = false;

    //private BloodEffectID bloodID;

    //SoundManager sm;
    float ShootInterval = 0.1f;
    float noBulleteSoundLength;
    bool isNoBulletSoundPlayEnd;
    public GameObject bullet;
    void Start()
    {
        // M4A1_Anim = M4A1_Sopmod.GetComponent<Animator>();
        //M4A1_Shell = Shell_Copper.GetComponent<Animator>();
        //M4A1_Magazine = M4A1_Sopmod.transform.FindChild("M4A1_Sopmod_Magazine").GetComponent<Animator>();
        //sm = DouduckGameCore.GetSystem<SoundManager>();
        //AudioClip ac = sm.GetAudioClip(GameEM.SoundList.玩家_沒子彈);
        //noBulleteSoundLength = ac.length;
        init();

    }

    void Update()
    {
        #region 有手把的話  手把向下50度  目前子彈如果不大於最大子彈數量 及目前子彈 為0 則補充子彈
        if (transform.eulerAngles.x > ReloadAngle && transform.eulerAngles.x < ReloadAngle + 20 && _nowBullet != MaxBullet && _nowBullet == 0)
        {
            GunReload();
         
        }
        #endregion
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Shoot();
        //    InvokeRepeating("Shoot", 0, ShootInterval);
        //}
        //if (Input.GetMouseButtonDown(1))
        //{

        //    GunReload();
        //    playerFeatures.Bulletjudgment(_nowBullet);
        //}

    }


    public void init()
    {
        isReload = false;
        isReloading = false;
        isNoBulletSoundPlayEnd = true;
        _nowBullet = MaxBullet;
        //fshellController.init();
        CancelInvoke("Shoot");
    
    }

    public void onTriggerPressed()
    {
        InvokeRepeating("Shoot", 0, ShootInterval);

     

    }

    public void onTriggerReleased()
    {

        CancelInvoke("Shoot");

  

    }

    //public void onShellTriggerPressed()
    //{
    //    fshellController.AimShellTarget();
    //}

    //public void onShellTriggerReleased()
    //{
    //    fshellController.FireShell();
    //}

    public void onReloadPressed()
    {
        GunReload();
      
    }

    void Shoot()
    {


        //if (playerControl.IsPlayerAlive())
        //{

        if (!isReload)
        {
            //        bloodID = (BloodEffectID)Random.Range(0, 2);
            RpcShoot();
        }


        //if (photonView.isMine)
        //{
        CheckBullet();
        //}
        //}
    }

    void GunReload()
    {
        //if (playerControl.IsPlayerAlive() && !isReloading)
        //{
        //    sm.PlaySound(GameEM.SoundList.玩家_換子彈);

        isReload = true;
        isReloading = true;
        _nowBullet = MaxBullet;
        Invoke("reloading", 0.4f);
      
        //M4A1_Anim.Play("M4A1_Magazine");
        //}
    }
    void RpcShoot()
    {
        // print(num);
        //sm.PlaySound(GameEM.SoundList.玩家_開槍);
        // M4A1_Anim.Play("M4A1_Anim");
        //M4A1_Shell.Play("M4A1_Shell");
        ImpactEffect.SetActive(false);
        ImpactEffect.SetActive(true);
        Instantiate(bullet, BulletStart.position, BulletStart.rotation);// /*= SmartPool.Spawn("bullet")*/= JObjectPool._InstanceJObjectPool.GetGameObject("bullet", BulletStart.position);
      

        //    Ray myRay = new Ray(BulletStart.position,  Vector3.left); //射線方向
        //  Debug.DrawRay(BulletStart.transform.position, myRay., Color.magenta);

        //b1.playerFeatures.playerStruct.PlayerNumber = playerFeatures.playerStruct.PlayerNumber;

        if (!bullet) return;
        fire(bullet/*, bloodID*/, BulletStart);//
       

    }

    public void fire(GameObject bullet/*, BloodEffectID bloodID*/, Transform bulletStart)
    {
        //bulletController bb = bullet.GetComponent<bulletController>();
        // Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //  bulletController bb = bullet.GetComponent<bulletController>();


        bullet.transform.position = bulletStart.position;
        bullet.transform.rotation = bulletStart.rotation;

        // bb.bulletInit();
        //  bb.DestroyBullet();
        //bb.bloodEffectID = bloodID;
        //rb.AddForce(bulletStart.up * 1500);//子彈移動
     
    }

    void CheckBullet()
    {
        if (!isReload && _nowBullet > 0)
        {


            //扣子彈
            _nowBullet = _nowBullet - 1;
            if (_nowBullet == 0)
            {
                isReload = true;
            }
        }
        else if (!isReloading)
        {
            //沒子彈
            if (isNoBulletSoundPlayEnd)
            {
                //M4A1_Magazine.Play("Hint", 0);
                //sm.PlaySound(GameEM.SoundList.玩家_沒子彈);
                isNoBulletSoundPlayEnd = false;
                Invoke("noBulletSoundDelay", noBulleteSoundLength);
            }
        }
    }

    void noBulletSoundDelay()
    {
        isNoBulletSoundPlayEnd = true;
    }

    void reloading()
    {
        //M4A1_Magazine.Play("Idle");
        isReload = false;
        isReloading = false;
    }
    /*
    [System.Serializable]
    public class ImpactInfo
    {
        public MaterialType.MaterialTypeEnum MaterialType;
        public GameObject ImpactEffect;
    }

    GameObject GetImpactEffect(GameObject impactedGameObject)
    {
        var materialType = impactedGameObject.GetComponent<MaterialType>();
        if (materialType == null)
            return null;
        foreach (var impactInfo in ImpactElemets)
        {
            if (impactInfo.MaterialType == materialType.TypeOfMaterial)
                return impactInfo.ImpactEffect;
        }
        return null;
    }
    */
}

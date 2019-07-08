using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrumsExplosionBB : MonoBehaviour
{
    public GameObject Bombs;
    public GameObject OilObj;
    public GameObject BombObj_Box;
    public GameObject OilBombsParticle;
    public GameObject YellowLight;
    public HPController hpcontroller;
    public LayerMask Layers;
    public position bbPosition;


    [Header("爆炸半徑")]
    public float radius = 5.0F;
    [Header("爆炸威力")]
    public float power = 10.0F;
    [Header("傷害暴龍血量")]
    public int TRexHpBombs = 50;
    //public string explosionPosName;
    public ForceMode ForceMode;
    [Header("下一批箱子生成時間(秒)")]
    public float f_Time = 5.0f;

    [Header("箱子出生位置細調")]
    public float boxPosX = 1f;
    public float boxPosZ = -1f;


    private bool initing = false;
    private bool ShowGizmos;
    private Collider[] colliders;
    private int BoxesCount = 8;
    public int BoxesCountMax = 8;
    private int newBox = 0;

    private GameObject[] Boxes;
    private ParticleSystem BombBlast;
    private Animator OilBombsAnim;
    //private ComPortController comPortController;
    //SoundManager m_SoundManager;

    void Start()
    {
        hpcontroller.OnDie += onDie;
        hpcontroller.OnHit += onHit;
        OilBombsParticle.SetActive(false);
        BombBlast = OilBombsParticle.GetComponent<ParticleSystem>();
        OilBombsAnim = OilObj.GetComponent<Animator>();
        Boxes = new GameObject[10];
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //油桶扣血
            hpcontroller.HP-=1;
        }

    }
    void OnDrawGizmos()
    {
        if (Application.isPlaying && ShowGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    /*油桶沒血後*/
    void onDie(object sender)
    {
        RPC_SetOffBomb();
    }

    /*每攻擊一次油桶*/
    void onHit(object sender)
    {
        OilBombsAnim.Play("ShootYellow");
    }

    /*引爆油桶使周圍物爆炸*/

    void SetOffBomb()
    {

    }

    void RPC_SetOffBomb()
    {
        initing = false;

        Vector3 explosionPos = transform.position;
        Rigidbody rb = GetComponent<Rigidbody>();
        OilBombsParticle.SetActive(true);
        BombBlast.Play();
        //產生爆炸範圍
        colliders = Physics.OverlapSphere(explosionPos, radius, Layers);
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.name.Contains("BombObj") || hit.gameObject.name.Contains("Bomb_Obj"))
            {
                if (hit.GetComponent<Rigidbody>() != null)
                {

                    hit.GetComponent<Rigidbody>().AddExplosionForce(power, explosionPos, radius, 3.0f);
                    if (hit.gameObject.GetComponent<DestroyBoxes>() != null)
                    {
                        hit.gameObject.GetComponent<DestroyBoxes>().Destroyy();
                        BoxesCount = 0; //產生箱子初始化//

                    }
                }
            }

            else
            {
                HPController _HPController = hit.gameObject.GetComponentInParent<HPController>();
            
                if (_HPController != null)
                {
                   
                    Vector3 ExploVector = hit.gameObject.transform.position - explosionPos;
                    _HPController.HP = 0;
               
                }
            }

        }
    
        Invoke("hideOilObj", 2f);
    }



    /*射擊油桶扣油桶血*/
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name.Contains("bullet"))
        {
            hpcontroller.HP -= 1;
        }
    }



    /*爆炸後隱藏油桶*/
    void hideOilObj()
    {
        if (initing == true)
        {

        }
        else
        {
            OilObj.SetActive(false);
            initing = true;
        }

    }


    /*初始化*/
    void Restart(object data)
    {
        CreateNewBoxes(gameObject);
    }



    void CreateNewBoxes(object date)
    {
        initing = true;

        transform.localPosition = new Vector3(0f, 0.1f, 0f);
        transform.localRotation = Quaternion.Euler(0, 1, 0);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        OilObj.SetActive(true);

        hpcontroller.HPControllerInit();
        hpcontroller.RemoveAllEvents();
        hpcontroller.OnDie += onDie;
        hpcontroller.OnHit += onHit;

        GameObject BombObj_Box = Resources.Load<GameObject>("BombObj_Box");

        for (int i = 0; i < Boxes.Length; i++)
        {
            if (BoxesCount < BoxesCountMax)
            {
                float pX = transform.position.x + Random.Range(-1.0f, 1.0f) + i + boxPosX;
                float pZ = transform.position.z + Random.Range(-1.0f, 1.0f) - i + boxPosZ;
                Vector3 P = new Vector3(pX, transform.position.y, pZ);
                float rY = transform.localRotation.y + Random.Range(-1.0f, 1.0f) + i;
             
                Quaternion R = Quaternion.Euler(transform.localRotation.x - 90, rY, transform.localRotation.z);
                Boxes[i] = BombObj_Box;
                BoxesCount = BoxesCount + 1;
            }

        }


    }

}

public enum position
{
    left, right
}





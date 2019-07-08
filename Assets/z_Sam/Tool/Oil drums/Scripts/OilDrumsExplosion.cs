using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrumsExplosion : MonoBehaviour
{
    public GameObject Bombs;
    public GameObject OilObj;
    public GameObject BombObj_Box;
    public GameObject OilBombsParticle;
    public GameObject YellowLight;
    public HPController hpcontroller;
    public LayerMask Layers;

    public float radius = 5.0F;
    public float power = 10.0F;
    public string explosionPosName;
    public ForceMode ForceMode;
    


    private Collider[] colliders;
    private ParticleSystem BombBlast;
    private Animator OilBombsAnim;


   void Start()
    {
        hpcontroller.OnDie += onDie;
        hpcontroller.OnHit += onHit;
        OilBombsParticle.SetActive(false);
        BombBlast = OilBombsParticle.GetComponent<ParticleSystem>();
        OilBombsAnim = GetComponent<Animator>();
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
        print("油桶剩下" + hpcontroller.HP + "次引爆");
    }

    /*引爆油桶使周圍物爆炸*/
    void RPC_SetOffBomb()
    {
        Vector3 explosionPos = transform.position;
        Rigidbody rb = GetComponent<Rigidbody>();
        OilBombsParticle.SetActive(true);
        BombBlast.Play();

        colliders = Physics.OverlapSphere(explosionPos, radius, Layers);

        foreach (Collider hit in colliders)
        {
            HPController _HPController = hit.gameObject.GetComponentInParent<HPController>();

            if (hit.gameObject.name.Contains("BombObj"))
            {
                if (hit.GetComponent<Rigidbody>() != null)
                {
                    hit.GetComponent<Rigidbody>().AddExplosionForce(power, explosionPos, radius, 3.0f);
                    if(hit.gameObject.GetComponent<DestroyBoxes>() != null)
                    {
                        //hit.gameObject.GetComponent<DestroyBoxes>().LetBoxesDone = true;
                        //箱子消失//
                    }
                }
                    
            }

            else
            {
                if (_HPController != null)
                {
                        Vector3 ExploVector = hit.gameObject.transform.position - explosionPos;
                        _HPController.HP = 0;
                }
            }
           
        }

        Invoke("hideOilObj" , 2f);

       

    }

    /*射擊油桶扣油桶血*/
    void OnTriggerEnter(Collider other)
    {
            if (other.transform.gameObject.name.Contains("bullet"))
            {
                hpcontroller.HP -=1;

            }
    }
    /*爆炸後隱藏油桶*/
    void hideOilObj()
    {
        OilObj.SetActive(false);


    }
    

    /*初始化*/
    void Restart(object data)
    {
        OilObj.GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;
        OilObj.transform.localPosition = Vector3.zero;
        OilObj.transform.localRotation = Quaternion.Euler(270, 0, 0);
        OilObj.SetActive(true);
        OilObj.GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().enabled = true;
        CreateNewBoxes();
    }

    void CreateNewBoxes()
    {
        float X = OilObj.transform.position.x + Random.Range(-5.0f, 5.0f);
        float Z = OilObj.transform.position.z + Random.Range(-5.0f, 5.0f);
        Vector3 A = new Vector3(X, Terrain.activeTerrain.SampleHeight(new Vector3(X, OilObj.transform.position.y, Z)) + 2F, Z);
        GameObject BombObj_Box = Resources.Load<GameObject>("BombObj_Box");
        Instantiate(BombObj_Box, A, OilObj.transform.rotation);
    }

}




 


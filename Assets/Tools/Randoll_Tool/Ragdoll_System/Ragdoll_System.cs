using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll_System : MonoBehaviour
{
    [Header("Ragdoll管理器")]
    public Ragdoll_Data ragdoll_Data;
    [Header("動畫")]
    public Animator an;
    Rigidbody[] rigidBodies;
    bool UseRagdoll;
    Vector3 impact;
    float random_X;
    float random_Y;
    float random_Z;
    float power;

    public bool isDie = false;

    void Start()
    {
        //初始化
        Randle_Data_Init();
        //把受傷程式放入各個部位
        //foreach (Rigidbody body in rigidBodies)
        //{
        //    BodyPartHurt rps = body.gameObject.AddComponent<BodyPartHurt>();
        //    rps.ragdoll_System = this;
        //}
    }
    void Randle_Data_Init()
    {
        //rigbody
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        setKinematic(true);
        //動畫控制
        an = GetComponent<Animator>();
        an.enabled = true;
    }
    //設置全身Kinematic
    public void setKinematic(bool newValue)
    {
        Component[] components = GetComponentsInChildren(typeof(Rigidbody));

        foreach (Component c in components)
        {
            (c as Rigidbody).isKinematic = newValue;
        }
    }
    //擊中後觸發功能
    public void Hit(GameObject bodypart, Rigidbody rig)
    {
        if (UseRagdoll == false && isDie == true)
        {
            an.enabled = false;
            setKinematic(false);
            //Random_Power();
            //力道Vector3
            //impact = bodypart.transform.TransformDirection(power+random_X,power+random_Y,power+random_Z);
            //impact = bodypart.transform.TransformDirection(-transform.forward * 50);
            //無質量狀態
            rig.AddForce(impact, ForceMode.VelocityChange);
            UseRagdoll = true;
        }
    }
    //隨機力道xyz軸
    void Random_Power()
    {
        power = ragdoll_Data.TransformDirection;
        //Vector3力道(想隨機力道可改變min max)
        random_X = Random.Range(ragdoll_Data.minTransformDirection, ragdoll_Data.maxTransformDirection);
        random_Y = Random.Range(ragdoll_Data.minTransformDirection, ragdoll_Data.maxTransformDirection);
        random_Z = Random.Range(ragdoll_Data.minTransformDirection, ragdoll_Data.maxTransformDirection);
    }
}

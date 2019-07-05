using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
public class BodyPartHurt : MonoBehaviour
{
    public Ragdoll_System ragdoll_System;
    public Ragdoll_Data ragdoll_Data;
    CharacterJoint Joint;
    Rigidbody rig;
    void Start()
    {
        Init();
    }
    void Init()
    {
        //取得Ragdle資料
        ragdoll_Data = ragdoll_System.ragdoll_Data;
        //取得Rigbody
        Rigbody_Get();
        //rigbody是否要mass與drap一致
        IsIsRigbody_the_Same();
        //Hips是涵蓋全身，並不會有Joint
        if (gameObject.name != "Hips")
        {
            Joint_Get();
        }
    }
    void Joint_Get()
    {
        CharacterJoint characterJoint = GetComponent<CharacterJoint>();
        if (characterJoint != null)
        {
            Joint = GetComponent<CharacterJoint>();
        }
        else
        {
            Debug.Log(gameObject.name + ".CharacterJoint is null");
        }
        Joint.enableProjection = ragdoll_Data.Joint_enable;
        Joint.projectionDistance = ragdoll_Data.Joint_projection_Distance;
        Joint.projectionAngle = ragdoll_Data.Joint_projection_Angle;
    }
    void Rigbody_Get()
    {
        Rigidbody rigidbody1 = GetComponent<Rigidbody>();
        if (rigidbody1 != null)
        {
            rig = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.Log(gameObject.name + ".Rigidbody is null");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bullet")
        {
            Hit(gameObject, rig);

        }
    }
    void Hit(GameObject bodypart, Rigidbody rig)
    {
        ragdoll_System.Hit(bodypart, rig);
    }

    //質量跟阻力是否一致
    void IsIsRigbody_the_Same()
    {
        if (ragdoll_Data.IsRigbody_the_Same)
        {
            rig.mass = ragdoll_Data.Mass;
            rig.drag = ragdoll_Data.Drag;
        }
    }
}

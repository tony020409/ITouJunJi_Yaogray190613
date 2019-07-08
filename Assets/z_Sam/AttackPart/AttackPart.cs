using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

//public enum weakness
//{
//    body,
//    head,
//    Throw,
//}

public class AttackPart : MonoBehaviour
{

    GameEM.EM_BodyPart status;
    public BaseRoleControllV2 _BaseRoleControl;
    public BaseBullet _BaseBullet;

    [Rename("類型")]
    public GameEM.EM_BodyPart EW;


    void Start()
    {
        _BaseBullet = this.GetComponentInParent<BaseBullet>();
        _BaseRoleControl = this.GetComponentInParent<BaseRoleControllV2>();
    }


}

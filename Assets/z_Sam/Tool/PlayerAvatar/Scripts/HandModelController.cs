using UnityEngine;
using System.Collections;

public class HandModelController : MonoBehaviour
{

    public GameObject pointerFinger;
    public GameObject gripFinger;

    [HideInInspector]
    public bool isTriggerPressed = false;


    //void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //        stream.SendNext(isTriggerPressed);
    //    else
    //        isTriggerPressed = (bool)stream.ReceiveNext();
    //}



    // Update is called once per frame
    void Update()
    {

        if (isTriggerPressed)
        {
            pointerFinger.transform.localRotation = new Quaternion(90, 0, 0, 90);
            gripFinger.transform.localRotation = new Quaternion(90, 0, 0, 90);
        }
        else
        {
            pointerFinger.transform.localRotation = new Quaternion(0, 0, 0, 0);
            gripFinger.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }

    }
}

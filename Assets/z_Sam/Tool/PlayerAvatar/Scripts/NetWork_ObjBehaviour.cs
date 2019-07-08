//using UnityEngine;
//using VRTK;

///// <summary>
///// Sets isKinematic to true, GameObject is owned by a another player (PhotonView.isMine == false).
///// For Rigidbody and Rigidbody2D.
///// </summary>
////[RequireComponent(typeof(PhotonView))]
////[RequireComponent(typeof(PhotonTransformView))]
//public class NetWork_ObjBehaviour : MonoBehaviour
//{
//    public bool origiskenecmic;

//    public void Awake()
//    {
//        origiskenecmic = GetComponent<Rigidbody>().isKinematic;
//        PhysicsSetting();
//    }

//    void Start()
//    {
//        VRTK_InteractableObject _InteractableObject = GetComponent<VRTK_InteractableObject>();
//        if (_InteractableObject == null)
//            MessageBox.DEBUG("[ " + gameObject.name + " ] Don't Have VRTK_InteractableObject ");
//        else
//            _InteractableObject.InteractableObjectGrabbed += OnGrabbed;
//    }


//    void FixedUpdate()
//    {

//        //if (!photonView.isMine)
//        //{
//            if (this.GetComponent<Rigidbody>() != null)
//            {
//                if (this.GetComponent<Rigidbody>().isKinematic == false)
//                {
//                    this.GetComponent<Rigidbody>().isKinematic = true;
//                }
//            }
//      //  }
//        else
//        {
//            if (this.GetComponent<Rigidbody>() != origiskenecmic)
//            {
//                this.GetComponent<Rigidbody>().isKinematic = origiskenecmic;
//            }
//        }

//    }


//    public void PhysicsSetting()
//    {

//        //ScenceObject's default owner is MasterClient 
//        //if (PhotonNetwork.isMasterClient && photonView.ownerId == 0)
//        //{
//        //    return;
//        //}

//        //if (!photonView.isMine)
//        //{
//            Rigidbody attachedRigidbody = GetComponent<Rigidbody>();
//            if (attachedRigidbody != null)
//            {
//                attachedRigidbody.isKinematic = true;
//            }
//            else
//            {
//                Rigidbody2D attachedRigidbody2d = GetComponent<Rigidbody2D>();
//                if (attachedRigidbody2d != null)
//                {
//                    attachedRigidbody2d.isKinematic = true;
//                }
//            }
//        //}
//    }

//    private void OnGrabbed(object sender, InteractableObjectEventArgs e)
//    {
//        GetOwnerShip();
//    }

//    public void GetOwnerShip()
//    {
//        //if (this.photonView.ownerId == PhotonNetwork.player.ID)
//        //{
//        //    Debug.Log("Not requesting ownership. Already mine.");
//        //    return;
//        //}

//        //this.photonView.RequestOwnership();

//    }


//    //private void OnCollisionEnter(Collision collision)
//    //{
//    //    if (!photonView.isMine)
//    //    {
//    //        if (collision.gameObject.GetComponent<VRTK_InteractableObject>() != null && GetComponent<VRTK_InteractableObject>() != null)
//    //        {
//    //            if (!GetComponent<VRTK_InteractableObject>().IsGrabbed() && collision.gameObject.GetComponent<VRTK_InteractableObject>().IsGrabbed())
//    //            {
//    //                this.GetComponent<NetWork_ObjBehaviour>().GetOwnerShip();
//    //            }
//    //        }
//    //    }
//    //}
//}
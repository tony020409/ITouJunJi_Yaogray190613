//using UnityEngine;
//using System.Collections;
//using VRTK;

//public class ViveManager : MonoBehaviour {


//    public GameObject Head;
//    public GameObject RightHand;
//    public GameObject LeftHand;
//    public VRTK_ControllerEvents RVRTK_ControllerEvents;
//    public VRTK_ControllerEvents LVRTK_ControllerEvents;

//    static ViveManager _inst;
//    static public ViveManager inst
//    {
//        get
//        {
//            if (_inst == null)
//            {
//                _inst = (ViveManager)FindObjectOfType(typeof(ViveManager));
//                if (_inst == null)
//                {
//                    Debug.LogError("No GameMain object exists");
//                    return null;
//                }
//            }
//            return _inst;
//        }
//    }

//    void Awake()
//    {
//        Cursor.visible = false;
//        Cursor.lockState = CursorLockMode.Locked;

//    }
//    // Use this for initialization
//    void Start () {
       
//    }
	
//	// Update is called once per frame
//	void Update () {
	
//	}
//}

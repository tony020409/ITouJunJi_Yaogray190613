using UnityEngine;
using System.Collections;
using System;

public class CameraManager : MonoBehaviour
{

    // Use this for initialization
    public GameObject FPSCamera;
    public GameObject TPCamera;
    static CameraManager _inst;
    public Transform target;
    public bool display;
    public int Playernumber;
    // GameObject[] playerList;
   // public GameProcess gameProcess;
    public GameObject LogoUI;
    static public CameraManager inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = (CameraManager)FindObjectOfType(typeof(CameraManager));
                if (_inst == null)
                {
                    Debug.LogError("No GameMain object exists");
                    return null;
                }
            }
            return _inst;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Playernumber += 1;
            //if (Playernumber > gameProcess.PlayermobileArray.Length - 1)
            //{
            //    Playernumber = 0;
            //}
            //if (gameProcess.PlayermobileArray[Playernumber].playerFeatures != null)
            //{
            //    target = gameProcess.PlayermobileArray[Playernumber].playerFeatures.gameObject.GetComponent<AvatarController>().Avatar_Head.gameObject.transform;
            //}
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FPSCamera.SetActive(!FPSCamera.activeInHierarchy);
            TPCamera.SetActive(!TPCamera.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            display = !display;
            LogoUI.gameObject.SetActive(display);
        }
    }

    void OnGUI()
    {
        if (display)
        {

            GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");

            //if (GUI.Button(new Rect(0, 0, 100, 50), "None"))
            //{
            //    FPSCamera.SetActive(false);
            //    TPCamera.SetActive(false);
            //}

            if (GUI.Button(new Rect(0, 0, 400, 50), "第一人稱視角與第三人稱視角 (鍵盤數字2)"))
            {
                FPSCamera.SetActive(!FPSCamera.activeInHierarchy);
                TPCamera.SetActive(!TPCamera.activeInHierarchy);
            }

            //if (Playernumber >= 0 && gameProcess.PlayermobileArray[Playernumber].playerFeatures == null)
            //{
            //    GUI.Label(new Rect(0, 50, 200, 80), "目前觀看玩家" + (Playernumber + 1) + "(玩家未進入遊戲)");
            //}
            //else
            //{
            //    GUI.Label(new Rect(0, 50, 200, 80), "目前觀看玩家" + (Playernumber + 1));
            //}

            //if (GUI.Button(new Rect(100, 0, 100, 50), "第三人稱視角"))
            //{
            //    FPSCamera.SetActive(false);
            //    TPCamera.SetActive(true);
            //}

            if (playerList.Length > 0)
            {
                int k = 80;
                //foreach (var item in playerList)
                //{
                //    if (GUI.Button(new Rect(0, i, 200, 50), "Player" + item.GetPhotonView().owner.ID.ToString() + "\n" + "玩家血量:"))//+ item.GetPhotonView().gameObject.GetComponent<PlayerFeatures>().playerStruct.HP)
                //    {
                //        target = item.transform.Find("Head");
                //    }
                //    i = i + 50;
                //}
                //for (int i = 0; i < gameProcess.PlayermobileArray.Length; i++)
                //{
                //    if (gameProcess.PlayermobileArray[i].playerFeatures != null)
                //    {
                //        if (GUI.Button(new Rect(0, k, 200, 80), "玩家" + (gameProcess.PlayermobileArray[i].playerFeatures.playerStruct.PlayerNumber + 1) + "\n" + "   血量:" + (int)gameProcess.PlayermobileArray[i].playerFeatures.playerStruct.HP + "\n分數:" + GameProcess.PlayerFraction[i] + "\n(鍵盤數字1切換玩家)"))//+ item.GetPhotonView().gameObject.GetComponent<PlayerFeatures>().playerStruct.HP)
                //        {
                //            target = gameProcess.PlayermobileArray[i].playerFeatures.gameObject.GetComponent<AvatarController>().Avatar_Head.gameObject.transform;
                //        }
                //        k = k + 80;
                //    }
                //}


            }

        }
    }
}

//using GameSysc;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;



///// <summary>
///// 這個 Player應該是框架早期的程式，功能不明，
///// 看起來是同步鍵盤輸入訊息用的。
///// </summary>
//public class Player : MonoBehaviour//, IHasGameFrame
//{

//    // 這兩個參數是妖鬼 Player.cs 的，用到的地方只有 Ontology.cs
//    // Ontology.cs 作用不明，預防萬一還是留著沒刪除，
//    // 不過 Ontology.cs 關聯到的 Missile.cs 有用到 Photon，
//    // 框架沒用到 Photon，所以 Ontology.cs、Missile.cs 的內容是全部註解的狀態，
//    // 這兩個參數其實沒啥用處。
//    public GameObject head;
//    public GameObject lantern;


//    private static Player _Instance = null;
//    public static Player GetInstance()
//    {
//        return _Instance;
//    }

//    //KeyCode _KeyCode = KeyCode.None;

//    public bool Finished
//    {
//        get
//        {
//            return false;
//        }
//    }

//    float fSpeed = 0.8f;

//    // Use this for initialization
//    void Start()
//    {
//        _Instance = this;
//        //glo_Main.GetInstance().m_GameSyscManager.f_AddSyscObject(this);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            SaveKeyDownAction(KeyCode.A);
//        }
//        else if (Input.GetKeyDown(KeyCode.W))
//        {
//            SaveKeyDownAction(KeyCode.W);
//        }
//        else if (Input.GetKeyDown(KeyCode.S))
//        {
//            SaveKeyDownAction(KeyCode.S);
//        }
//        else if (Input.GetKeyDown(KeyCode.D))
//        {
//            SaveKeyDownAction(KeyCode.D);
//        }
//        else if (Input.GetKeyDown(KeyCode.Q))
//        {
//            SaveKeyDownAction(KeyCode.Q);
//        }
//        else if (Input.GetKeyDown(KeyCode.E))
//        {
//            SaveKeyDownAction(KeyCode.E);
//        }
//        else if (Input.GetKeyUp(KeyCode.A))
//        {
//            SaveKeyUpAction(KeyCode.A);
//        }
//        else if (Input.GetKeyUp(KeyCode.W))
//        {
//            SaveKeyUpAction(KeyCode.W);
//        }
//        else if (Input.GetKeyUp(KeyCode.S))
//        {
//            SaveKeyUpAction(KeyCode.S);
//        }
//        else if (Input.GetKeyUp(KeyCode.D))
//        {
//            SaveKeyUpAction(KeyCode.D);
//        }
//        else if (Input.GetKeyUp(KeyCode.Q))
//        {
//            SaveKeyUpAction(KeyCode.Q);
//        }
//        else if (Input.GetKeyUp(KeyCode.E))
//        {
//            SaveKeyUpAction(KeyCode.E);
//        }
//        else if (Input.GetKeyUp(KeyCode.R))
//        {
//            SaveKeyUpAction(KeyCode.R);
//        }

//    }

//    private void SaveKeyDownAction(KeyCode tKeyCode)
//    {
//        PlayerAction tPlayerAction = new PlayerAction();
//        tPlayerAction.f_KeyDown((int)tKeyCode);
//        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tPlayerAction);
//    }

//    private void SaveKeyUpAction(KeyCode tKeyCode)
//    {
//        PlayerAction tPlayerAction = new PlayerAction();
//        tPlayerAction.f_KeyUp((int)tKeyCode);
//        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tPlayerAction);
//    }

//    class KeyData
//    {
//        public KeyCode KeyCode;
//        public int iKeyDown;
//    }
//    Dictionary<int, KeyData> _aKeyData = new Dictionary<int, KeyData>();


//    public void f_SaveKeyDown(int tKeyCode)
//    {
//        KeyData tKeyData = null;
//        if (!_aKeyData.TryGetValue(tKeyCode, out tKeyData))
//        {
//            tKeyData = new KeyData();
//            tKeyData.KeyCode = (KeyCode)tKeyCode;
//            _aKeyData.Add(tKeyCode, tKeyData);
//        }
//        tKeyData.iKeyDown = 1;
//    }
    
//    public void f_SaveKeyUp(int tKeyCode)
//    {
//        if ((KeyCode)tKeyCode == KeyCode.R)
//        {
//            transform.position = Vector3.zero;
//        }
//        KeyData tKeyData = null;
//        if (!_aKeyData.TryGetValue(tKeyCode, out tKeyData))
//        {
//            tKeyData = new KeyData();
//            tKeyData.KeyCode = (KeyCode)tKeyCode;
//            _aKeyData.Add(tKeyCode, tKeyData);
//        }
//        tKeyData.iKeyDown = 0;
//    }


//    /// <summary>
//    /// 找不到參考的部分
//    /// </summary>
//    /// <param name="gameFramesPerSecond"></param>
//    //public void GameFrameTurn(int gameFramesPerSecond)
//    //{
//    //    foreach(KeyValuePair<int, KeyData> tItem in _aKeyData)
//    //    {
//    //        if (tItem.Value.iKeyDown == 1)
//    //        {
//    //            Vector3 Pos = transform.position;
//    //            if (tItem.Value.KeyCode == KeyCode.W)
//    //            {
//    //                Pos.y = Pos.y + fSpeed;
//    //            }
//    //            else if (tItem.Value.KeyCode == KeyCode.S)
//    //            {
//    //                Pos.y = Pos.y - fSpeed;
//    //            }
//    //            else if (tItem.Value.KeyCode == KeyCode.D)
//    //            {
//    //                Pos.x = Pos.x + fSpeed;
//    //            }
//    //            else if (tItem.Value.KeyCode == KeyCode.A)
//    //            {
//    //                Pos.x = Pos.x - fSpeed;
//    //            }
//    //            else if (tItem.Value.KeyCode == KeyCode.Q)
//    //            {
//    //                Pos.z = Pos.z + fSpeed;
//    //            }
//    //            else if (tItem.Value.KeyCode == KeyCode.E)
//    //            {
//    //                Pos.z = Pos.z - fSpeed;
//    //            }
//    //            transform.position = Pos;
//    //        }
//    //    }
//    //    
//    //}


//}

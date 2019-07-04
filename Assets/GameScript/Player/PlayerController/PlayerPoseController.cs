using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// <summary>
/// 店家客製化修改 玩家Tracker與槍枝 位置、朝向用
/// </summary>
public class PlayerPoseController : MonoBehaviour {

    [Header("【UI 介面】")]
    [Space(8)]
    public GameObject ActiveRoot;
    public Button Btn_CloseUI;
    [Space(8)]
    public Dropdown m_Dropdown;
    public Text Info_Chose;
    [Space(8)]
    public InputField PosX;
    public InputField PosY;
    public InputField PosZ;
    public Button Btn_SeePos;
    public Button Btn_SavePos;
    public Button Btn_ClearPos;
    public Text Info_SavePos;
    [Space(8)]
    public InputField RotX;
    public InputField RotY;
    public InputField RotZ;
    public Button Btn_SeeRot;
    public Button Btn_SaveRot;
    public Button Btn_ClearRot;
    public Text Info_SaveRot;
    [Space(8)]
    public Button Btn_DefultPos;
    public Button Btn_DefultRot;
    public Button Btn_DefultPos_newSteam;
    public Button Btn_DefultRot_newSteam;


    private string folderPath;      //文件存放的資料夾路徑(在 Start()裡指定)

    private Transform currentObj;   //當前修改的物件
    private PoseData poseData;      //當前修改的文件資料
    private string currentFileName; //當前修改的文件名稱

    private PoseData_Defult poseData_Defult;                   //預設的文件資料
    private string FileName_Defult_1 = "DefultPoseJson_1.txt"; //預設文件-1 的名稱 (舊版 Steam的坐標軸)
    private string FileName_Defult_2 = "DefultPoseJson_2.txt"; //預設文件-2 的名稱 (新版 Steam的坐標軸)


    [Line()]
    [HelpBox("文件路徑在 StreamingAssets/Pose 裡", HelpBoxType.Info)]
    [Rename("槍 文件名稱")] public string FileName_Gun = "GunJson.txt";
    [Rename("槍 物件")]     public Transform Obj_Gun;

    [Space(8)]
    [Rename("Tracker 1 文件名稱")] public string FileName_Tracker_1 = "TrackerJson_1.txt";
    [Rename("Tracker 1 物件")]     public Transform Obj_Tracker_1;

    [Space(8)]
    [Rename("Tracker 2 文件名稱")] public string FileName_Tracker_2 = "TrackerJson_2.txt";
    [Rename("Tracker 2 物件")]     public Transform Obj_Tracker_2;



    // Use this for initialization
    void Start() {

        //關閉介面
        ActiveRoot.SetActive(false);

        //設定路徑資料夾
        folderPath = Application.streamingAssetsPath + "/Pose";

        //套用儲存的設定
        InitPlayerPose();

        //註冊按鈕事件
        RegButtonEvent();

        //設定一開始預設設定的東西
        currentObj = Obj_Gun;
        currentFileName = FileName_Gun;
        Info_Chose.text = "當前 [ Gun ] 儲存的資訊";
        Read(folderPath + "/" + currentFileName);
        f_RefreshInfo();
    }


    // 右Shift + C 開關修改介面
    void Update() {
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.C)) {
            ActiveRoot.SetActive(!ActiveRoot.activeInHierarchy);
        }
    }


    /// <summary>
    /// 設定玩家動作
    /// </summary>
    private void InitPlayerPose() {
        SetPlayerPose(Obj_Gun, FileName_Gun);
        SetPlayerPose(Obj_Tracker_1, FileName_Tracker_1);
        SetPlayerPose(Obj_Tracker_2, FileName_Tracker_2);
    }



    /// <summary>
    /// 讀取文件並修改物件的位置、角度
    /// </summary>
    /// <param name="targetObj"> 要修改的物件 </param>
    /// <param name="FileName" > 對照的資料文件 </param>
    private void SetPlayerPose(Transform targetObj, string FileName) {
        Read(folderPath + "/" + FileName);
        if (poseData.m_Enabled) {
            targetObj.localPosition = poseData.m_Position;
            //targetObj.localEulerAngles = poseData.m_Rotation;
            targetObj.localRotation = Quaternion.Euler(poseData.m_Rotation.x, poseData.m_Rotation.y, poseData.m_Rotation.z);
        }
    }


    #region 介面 UI事件
    /// <summary>
    /// 註冊按鈕事件
    /// </summary>
    private void RegButtonEvent() {

        //註冊 Dropdown變更時執行的動作
        if (m_Dropdown != null) {
            m_Dropdown.onValueChanged.AddListener(delegate {
                f_ChangeTargetObject(m_Dropdown);
            });
        } else {
            Debug.LogWarning("【警告】「下拉清單」的按鈕未設置，功能失效！");
        }

        //註冊按「看位置結果」的按鈕事件
        if (Btn_SeePos != null) {
            Btn_SeePos.onClick.AddListener(delegate {
                f_SeePos();
            });
        } else {
            Debug.LogWarning("【警告】「看位置結果」的按鈕未設置，功能失效！");
        }

        //註冊「看位朝向結果」的按鈕事件
        if (Btn_SeeRot != null) {
            Btn_SeeRot.onClick.AddListener(delegate {
                f_SeeRot();
            });
        } else {
            Debug.LogWarning("【警告】「看位朝向結果」的按鈕未設置，功能失效！");
        }

        //註冊按「儲存位置」的按鈕事件
        if (Btn_SavePos != null) {
            Btn_SavePos.onClick.AddListener(delegate {
                f_SavePos();
            });
        } else {
            Debug.LogWarning("【警告】「儲存位置」的按鈕未設置，功能失效！");
        }

        //註冊「儲存朝向」的按鈕事件
        if (Btn_SaveRot != null) {
            Btn_SaveRot.onClick.AddListener(delegate {
                f_SaveRot();
            });
        } else {
            Debug.LogWarning("【警告】「儲存朝向」的按鈕未設置，功能失效！");
        }

        //註冊「顯示預設位置的數值_1」
        if (Btn_DefultPos != null) {
            Btn_DefultPos.onClick.AddListener(delegate {
                f_SeeDefult_Position(1);
            });
        } else {
            Debug.LogWarning("【警告】「顯示預設位置的數值_1」的按鈕未設置，功能失效！");
        }

        //註冊「顯示預設位置的數值_2」
        if (Btn_DefultRot != null) {
            Btn_DefultRot.onClick.AddListener(delegate {
                f_SeeDefult_Rotation(1);
            });
        } else {
            Debug.LogWarning("【警告】「顯示預設位置的數值_2」的按鈕未設置，功能失效！");
        }

        //註冊「顯示預設朝向的數值_1」
        if (Btn_DefultPos_newSteam != null) {
            Btn_DefultPos_newSteam.onClick.AddListener(delegate {
                f_SeeDefult_Position(2);
            });
        } else {
            Debug.LogWarning("【警告】「顯示預設朝向的數值_1」的按鈕未設置，功能失效！");
        }

        //註冊「顯示預設朝向的數值_2」(針對 SteamVR 新版)
        if (Btn_DefultRot_newSteam != null) {
            Btn_DefultRot_newSteam.onClick.AddListener(delegate {
                f_SeeDefult_Rotation(2);
            });
        } else {
            Debug.LogWarning("【警告】「顯示預設朝向的數值_2」的按鈕未設置，功能失效！");
        }

        //註冊「清空位置輸入」
        if (Btn_ClearPos != null) {
            Btn_ClearPos.onClick.AddListener(delegate {
                ClearInput(PosX, PosY, PosZ);
            });
        } else {
            Debug.LogWarning("【警告】「清空位置輸入」的按鈕未設置，功能失效！");
        }

        //註冊「清空朝向輸入」
        if (Btn_ClearRot != null) {
            Btn_ClearRot.onClick.AddListener(delegate {
                ClearInput(RotX, RotY, RotZ);
            });
        } else {
            Debug.LogWarning("【警告】「清空朝向輸入」的按鈕未設置，功能失效！");
        }

        //註冊「關閉視窗」的按扭
        if (Btn_CloseUI != null) {
            Btn_CloseUI.onClick.AddListener(delegate  {
                CloseUI();
            });
        } else {
            Debug.LogWarning("【警告】「關閉視窗」的按扭未設置，功能失效！");
        }

    }



    /// <summary>
    /// [下拉選單變更時執行] 設定要修改的物件、讀取相關的文件
    /// </summary>
    public void f_ChangeTargetObject(Dropdown change) {

        if (currentObj == null) {
            Debug.LogWarning("【警告】物件: " + currentObj + "未設置，故無法對其位置和朝向進行修改！" );
            return;
        }

        if (change.options[change.value].text == "Gun") {
            currentObj = Obj_Gun;
            currentFileName = FileName_Gun;
            Info_Chose.text = "當前 [ Gun ] 儲存的資訊";
            
        }

        else if (change.options[change.value].text == "Tracker_1") {
            currentObj = Obj_Tracker_1;
            currentFileName = FileName_Tracker_1;
            Info_Chose.text = "當前 [ Tracker_1 ] 儲存的資訊";
        }

        else if (change.options[change.value].text == "Tracker_2") {
            currentObj = Obj_Tracker_2;
            currentFileName = FileName_Tracker_2;
            Info_Chose.text = "當前 [ Tracker_2 ] 儲存的資訊";
        }

        //檢查文件是否存在
        CheckFilePath();

        //讀取文件
        Read(folderPath + "/" + currentFileName);

        //顯示儲存的資訊
        f_RefreshInfo();
    }


    /// <summary>
    /// 刷新當前文件顯示的資訊
    /// </summary>
    public void f_RefreshInfo() {
        Info_SavePos.text = "位置= ( " + poseData.m_Position.x + ", " + poseData.m_Position.y + ", " + poseData.m_Position.z + " )";
        Info_SaveRot.text = "朝向= ( " + poseData.m_Rotation.x + ", " + poseData.m_Rotation.y + ", " + poseData.m_Rotation.z + " )";
    }

    /// <summary>
    /// [Button事件] 觀看修改後的位置
    /// </summary>
    public void f_SeePos()  {
        if (currentObj == null) {
            Debug.LogWarning("【警告】當前設置物件沒有設置，故功能失效！");
            return;
        }
        currentObj.localPosition = new Vector3(ParseInput(PosX.text), ParseInput(PosY.text), ParseInput(PosZ.text));
    }

    /// <summary>
    /// [Button事件] 觀看修改後的朝向
    /// </summary>
    public void f_SeeRot() {
        if (currentObj == null) {
            Debug.LogWarning("【警告】當前設置物件沒有設置，故功能失效！");
            return;
        }
        currentObj.localRotation = Quaternion.Euler(ParseInput(RotX.text), ParseInput(RotY.text), ParseInput(RotZ.text));
        //currentObj.localEulerAngles = new Vector3(ParseInput(RotX.text), ParseInput(RotY.text), ParseInput(RotZ.text));
    }

    /// <summary>
    /// [Button事件] 儲存輸入的位置
    /// </summary>
    public void f_SavePos() {
        poseData.m_Position = new Vector3(ParseInput(PosX.text), ParseInput(PosY.text), ParseInput(PosZ.text));
        Save(poseData);
        f_RefreshInfo();
    }

    /// <summary>
    /// [Button事件] 儲存輸入的朝向
    /// </summary>
    public void f_SaveRot() {
        poseData.m_Rotation = new Vector3(ParseInput(RotX.text), ParseInput(RotY.text), ParseInput(RotZ.text)); ; ;
        Save(poseData);
        f_RefreshInfo();
    }


    /// <summary>
    /// 文字轉 Flaot
    /// </summary>
    /// <param name="tmp"></param>
    public float ParseInput(string tmp) {
        if (tmp == "") {
            tmp = "0";
            return 0;
        } else {
            return float.Parse(tmp);
        }
    }


    /// <summary>
    /// [Button事件] 清空輸入內容
    /// </summary>
    public void ClearInput(InputField x, InputField y, InputField z) {
        x.text = "";
        y.text = "";
        z.text = "";
    }



    /// <summary>
    /// [Button事件] 關閉介面
    /// </summary>
    private void CloseUI() {
        ActiveRoot.SetActive(false);
    }



    /// <summary>
    /// [Button事件]顯示預設的數值 (位置)
    /// </summary>
    /// <param name="type"> 1對應舊版Steam，2對應新版的Steam</param>
    public void f_SeeDefult_Position(int type = 1) {
        if (type == 1) {
            ReadDefult(folderPath + "/" + FileName_Defult_1);
        }
        else if (type == 2) {
            ReadDefult(folderPath + "/" + FileName_Defult_2);
        }
        else {
            Debug.LogWarning("超出 Pose預設位置範圍!");
            return;
        }

        if (currentObj == Obj_Gun) {
            PosX.text = poseData_Defult.m_Defult_Position_Gun.x.ToString();
            PosY.text = poseData_Defult.m_Defult_Position_Gun.y.ToString();
            PosZ.text = poseData_Defult.m_Defult_Position_Gun.z.ToString();
        }

        else if (currentObj == Obj_Tracker_1) {
            PosX.text = poseData_Defult.m_Defult_Position_Tracker_1.x.ToString();
            PosY.text = poseData_Defult.m_Defult_Position_Tracker_1.y.ToString();
            PosZ.text = poseData_Defult.m_Defult_Position_Tracker_1.z.ToString();
        }

        else if (currentObj == Obj_Tracker_2) {
            PosX.text = poseData_Defult.m_Defult_Position_Tracker_2.x.ToString();
            PosY.text = poseData_Defult.m_Defult_Position_Tracker_2.y.ToString();
            PosZ.text = poseData_Defult.m_Defult_Position_Tracker_2.z.ToString();
        }
    }


    /// <summary>
    /// [Button事件] 顯示預設的數值(朝向)
    /// </summary>
    /// <param name="type"> 1對應舊版Steam，2對應新版的Steam</param>
    public void f_SeeDefult_Rotation(int type = 1) {
        if (type == 1) {
            ReadDefult(folderPath + "/" + FileName_Defult_1);
        }
        else if (type == 2) {
            ReadDefult(folderPath + "/" + FileName_Defult_2);
        }
        else {
            Debug.LogWarning("超出 Pose預設朝向範圍!");
            return;
        }

        if (currentObj == Obj_Gun)  {
            RotX.text = poseData_Defult.m_Defult_Rotation_Gun.x.ToString();
            RotY.text = poseData_Defult.m_Defult_Rotation_Gun.y.ToString();
            RotZ.text = poseData_Defult.m_Defult_Rotation_Gun.z.ToString();
        }

        else if (currentObj == Obj_Tracker_1) {
            RotX.text = poseData_Defult.m_Defult_Rotation_Tracker_1.x.ToString();
            RotY.text = poseData_Defult.m_Defult_Rotation_Tracker_1.y.ToString();
            RotZ.text = poseData_Defult.m_Defult_Rotation_Tracker_1.z.ToString();
        }

        else if (currentObj == Obj_Tracker_2) {
            RotX.text = poseData_Defult.m_Defult_Rotation_Tracker_2.x.ToString();
            RotY.text = poseData_Defult.m_Defult_Rotation_Tracker_2.y.ToString();
            RotZ.text = poseData_Defult.m_Defult_Rotation_Tracker_2.z.ToString();
        }
    }
    #endregion



    #region 文件存檔與讀取
    /// <summary>
    /// 檢查路徑和文件是否存在,如果不存在就建立一個
    /// </summary>
    public void CheckFilePath() {
        
        //如果檔案的位置資料夾不存在，就在指定的路徑中創建資料夾。
        if (Directory.Exists(folderPath) == false) {
            DirectoryInfo di = Directory.CreateDirectory(folderPath);
        }

        //如果檔案不存在，就給予出值，然後創建檔案
        if (File.Exists(folderPath + "/" + currentFileName) == false){
            PoseData tmp = new PoseData();                //給予文件初始值
            tmp.m_Enabled = true;                         //給予文件初始值 (true)
            tmp.m_Position = currentObj.localPosition;    //給予文件初始值 (位置)
            tmp.m_Rotation = currentObj.localEulerAngles; //給予文件初始值 (朝向)
            Save(tmp);                                    //存檔
        }
    }

    /// <summary>
    /// 存檔
    /// </summary>
    public void Save(PoseData tmp) {
        string saveString = JsonUtility.ToJson(tmp);                            //將資料格式轉換成json格式的字串
        StreamWriter fr = new StreamWriter(folderPath + "/" + currentFileName); //將字串存到streamingAssets資料夾
        fr.Write(saveString);                                                   //寫檔
        fr.Close();                                                             //關閉寫檔(？)
    }


    /// <summary>
    /// 讀取指定文件
    /// </summary>
    public void Read(string path) {
        StreamReader file = new StreamReader(path, Encoding.Default); //讀文件
        string loadJson = file.ReadToEnd();                           //讀內文
        file.Close();                                                 //關閉讀檔(？)
        poseData = JsonUtility.FromJson<PoseData>(loadJson);          //讀檔
    }


    /// <summary>
    /// 存檔 (預設值)
    /// 產生預設文件用的，有做修改時到 Update弄個鍵盤事件產生文件，產生完再註解掉，手動修改文件數值
    /// </summary>
    public void SaveDefult() {
        PoseData_Defult tmp = new PoseData_Defult();  //給予文件初始值
        tmp.m_Defult_Position_Gun       = new Vector3( 0, 0.05f, -0.1f);
        tmp.m_Defult_Rotation_Gun       = new Vector3( -85, 180, 0);
        tmp.m_Defult_Position_Tracker_1 = new Vector3( 0, 0, -0.05f);
        tmp.m_Defult_Rotation_Tracker_1 = new Vector3( 0, 0, 0);
        tmp.m_Defult_Position_Tracker_2 = new Vector3( 0, 0, 0);
        tmp.m_Defult_Rotation_Tracker_2 = new Vector3( 0, 0, 0);
        string saveString = JsonUtility.ToJson(tmp);                               //將資料格式轉換成json格式的字串
        StreamWriter fr = new StreamWriter(folderPath + "/" + FileName_Defult_1);  //將字串存到streamingAssets資料夾
        fr.Write(saveString);                                                      //寫檔
        fr.Close();                                                                //關閉寫檔(？)
        StreamWriter fr2 = new StreamWriter(folderPath + "/" + FileName_Defult_2); //將字串存到streamingAssets資料夾
        fr2.Write(saveString);                                                     //寫檔
        fr2.Close();                                                               //關閉寫檔(？)
    }


    /// <summary>
    /// 讀取指定文件 (預設值)
    /// </summary>
    public void ReadDefult(string path){
        StreamReader file = new StreamReader(path, Encoding.Default);      //讀文件
        string loadJson = file.ReadToEnd();                                //讀內文
        file.Close();                                                      //關閉讀檔(？)
        poseData_Defult = JsonUtility.FromJson<PoseData_Defult>(loadJson); //讀檔
    }
    #endregion


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using System.Text.RegularExpressions;
using DG.Tweening;


/// <summary>
/// 手把移動
/// </summary>
public class Test_VR_Move_ViveInput : MonoBehaviour {

    /// <summary> 是否啟用圓盤移動 </summary>  
    [Rename("是否啟用圓盤移動")] public bool use_TestMove = false;

    public enum EM_VR_Move {
        Face = 1,      //移動方式1 - 朝攝影機看的方向移動
        Norma_l = 2,   //移動方式2 - 普通的前後左右
        Norma_2 = 3,   //移動方式3 - 普通的前後左右
        SKStudios = 4, //移動方式4 - 使用 SKStudios插件的移動 (傳送門)
        Soul = 5,      //移動方式5 - 產生一個靈魂物件，控制那物件的位置，然後瞬間移動過去 (還沒弄)
        Teleport = 6,  //移動方式6 - 產生一雷射光，瞬間移動到光點位置 (還沒弄)
        PC = 7,        //移動方式7 - 在家沒 VR時，測試用的
    }

    /// <summary> 移動模式 </summary>
    [Rename("移動模式")] public EM_VR_Move mState = EM_VR_Move.SKStudios;

    [Line()]

    /// <summary> 玩家物件(Transform) </summary>  
    [Rename("放入玩家節點 (Player[CameraRig])")] public Transform player;

    /// <summary> 頭盔朝向 </summary> 
    [Rename("放入玩家攝影機 (Camera(eye))")] public Transform dic;

    [Line()]

    /// <summary> 移動速度 </summary> 
    [Rename("移動速度")] public float moveSpeed = 1.0f;

    /// <summary> 轉向速度 </summary> 
    [Rename("轉向速度")] public float turnSpeed = 10.0f;

    [Line()]

    /// <summary> PC模式下槍的位置 </summary> 
    [Rename("槍")] public Transform m_Gun;

    /// <summary> PC模式下槍的位置 </summary> 
    [Rename("PC模式下槍的位置")] public Transform PC_GunPos;

    /// <summary>
    /// 滑鼠視角用
    /// </summary>
    private Vector2 rotation = new Vector2(0, 0);


    void Awake() {

        //預設不啟用手把移動
        use_TestMove = false;

        #if UNITY_EDITOR
        //不過在編輯器測試模式下，預設是打開的
        use_TestMove = true;
        #endif

    }



    void Start () {
        //外讀讀取一個叫 Config_LogOutput.txt的文件決定要不要保留輸出日誌
        string ConfigDebugFilePath = Application.streamingAssetsPath + "/" + "Config_LogOutput.txt";
        if (CheckOutputDebug(ConfigDebugFilePath, "Pad_Move", "0")) {
            use_TestMove = false;
        }
        else if (CheckOutputDebug(ConfigDebugFilePath, "Pad_Move", "1")) {
            Set_MoveType(1);
        }
        else if (CheckOutputDebug(ConfigDebugFilePath, "Pad_Move", "4")) {
            Set_MoveType(4);
        }
        else if (CheckOutputDebug(ConfigDebugFilePath, "Pad_Move", "7")) {
            Set_MoveType(7);
            if (PC_GunPos != null) {
                if (m_Gun != null) {
                    m_Gun.parent = PC_GunPos;
                    m_Gun.localPosition = Vector3.zero;
                    m_Gun.localEulerAngles = Vector3.zero;
                }
            }
        }
        else {
            use_TestMove = false;
        }
        
    }


    /// <summary>
    /// 設置移動方式
    /// </summary>
    /// <param name="tmp"> 方式 id </param>
    void Set_MoveType(int tmp) {
        use_TestMove = true;
        mState = (EM_VR_Move)tmp;
    }

      
    /// <summary>
    /// 透過外部文檔確認是否開啟圓盤移動和要移動的模式
    /// </summary>
    /// <param name="filePath" > 檔案路徑(含檔名與.txt) </param>
    /// <param name="tmp"      > 參考字 </param>
    /// <param name="trueValue"> 決定項 </param>
    bool CheckOutputDebug(string filePath, string tmp, string trueValue, bool defaultValue = false) {
        if (System.IO.File.Exists(filePath)) {
            string[] strs = System.IO.File.ReadAllLines(filePath);
            for (int i = 0; i < strs.Length; i++) {
                if (strs[i].Contains(tmp)) {
                    if (f_GetSplitTextIndex(strs[i], "：", 1) == trueValue) {
                        return true;
                    }
                    else {
                        continue;
                    }
                }
            }
        }
        //找不到文檔預設關閉不輸出
        return defaultValue;
    }


        
    /// <summary>
    /// 取得某段文字以特殊字元拆分後 第 Index項的內容
    /// </summary>
    /// <param name="tmp"> 要分析的內容(string) </param>
    /// <param name="value" > 特殊字元 </param>
    /// <param name="index" > 拆分後的第幾項 </param>
    public string f_GetSplitTextIndex(string tmp, string value, int index) {
        string[] tmpContent = Regex.Split(tmp, value, RegexOptions.IgnoreCase); //根據換行符劃分出多個行文本
        if (tmpContent.Length == 1) {
            return "-99";
        }
        return tmpContent[index];  //回傳解析結果
    }



	

	void Update () {
        // 右Alt+橫排數字9 啟用或關閉圓盤移動
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.Alpha9)){
            if (use_TestMove) {
                use_TestMove = false;
            } else {
                use_TestMove = true; ;
            }
        }


        if (!use_TestMove) {
            return;
        }

        if (mState == EM_VR_Move.PC) {
            PC_Move();
        }
        else {
            // +- 上升下降
            if (Input.GetKey(KeyCode.KeypadPlus)) {
                player.transform.position += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.KeypadMinus)) {
                player.transform.position -= new Vector3(0, moveSpeed, 0) * Time.deltaTime;
            }
        }



    }



    void FixedUpdate() {

        if (!use_TestMove) {
            return;
        }


        if (mState == EM_VR_Move.PC) {
            return;
        }

        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Pad)) {
            Vector2 cc = ViveInput.GetPadAxis(HandRole.RightHand);
            float angle = VectorAngle(new Vector2(1, 0), cc);
            switch (mState) {
                case EM_VR_Move.Face:
                    EM_Move_Face(angle);
                    break;
                case EM_VR_Move.Norma_l:
                    EM_Move_Normal(angle);
                    break;
                case EM_VR_Move.Norma_2:
                    EM_Move_Norma2(angle);
                    break;
                case EM_VR_Move.SKStudios:
                    SKStudiosMove(angle);
                    break;
                case EM_VR_Move.Soul:

                    break;
                case EM_VR_Move.Teleport:

                    break;
                case EM_VR_Move.PC:

                    break;
                default:
                    Debug.LogError("Type Error");
                    break;
            }
        }
    }



    /// <summary>
    /// 根據在圓盤才按下的位置，返回一個角度值
    /// </summary>
    float VectorAngle(Vector2 from, Vector2 to)  {
        float angle;
        Vector3 cross = Vector3.Cross(from, to);
        angle = Vector2.Angle(from, to);
        return cross.z > 0 ? -angle : angle;   //如果 cross.z > 0，傳回 -angle，否則傳 angle
    }



    /// <summary>
    /// 朝攝影機看的方向移動
    /// </summary>
    /// <param name="tmp"> 手把圓盤按下的位置結果 </param>
    void EM_Move_Face(float tmp) {
        //下  
        if (tmp > 45 && tmp < 135) {
            player.Translate(-dic.forward * Time.deltaTime * moveSpeed);
        }

        //上    
        else if (tmp < -45 && tmp > -135) {
            player.Translate(dic.forward * Time.deltaTime * moveSpeed);
        }

        //左    
        else if ((tmp < 180 && tmp > 135) || (tmp < -135 && tmp > -180)) {
            player.Translate(-dic.right * Time.deltaTime * moveSpeed);
        }

        //右    
        else if ((tmp > 0 && tmp < 45) || (tmp > -45 && tmp < 0)) {
            player.Translate(dic.right * Time.deltaTime * moveSpeed);
        }

        Vector3 newPos = player.transform.position;
        newPos.y = 0.1f;
        player.transform.position = newPos;
    }


    /// <summary>
    /// 普通移動1
    /// </summary>
    /// <param name="tmp"> 手把圓盤按下的位置結果 </param>
    void EM_Move_Normal(float tmp) {
        //下  
        if (tmp > 45 && tmp < 135) {
            player.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
        }

        //上    
        else if (tmp < -45 && tmp > -135){
            player.Translate(player.transform.forward * moveSpeed * Time.deltaTime);
        }

        //左    
        else if ((tmp < 180 && tmp > 135) || (tmp < -135 && tmp > -180)){
            player.Rotate(-Vector3.up, turnSpeed * Time.deltaTime); //旋轉位置可能會因為 VR位置定位的關係而偏差
        }

        //右    
        else if ((tmp > 0 && tmp < 45) || (tmp > -45 && tmp < 0)) {
            player.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }
    }


    /// <summary>
    /// 普通移動1
    /// </summary>
    /// <param name="tmp"> 手把圓盤按下的位置結果 </param>
    void EM_Move_Norma2(float tmp) {
        //下  
        if (tmp > 45 && tmp < 135)  {
            player.Translate(dic.transform.forward * moveSpeed * Time.deltaTime * -1);
        }

        //上    
        else if (tmp < -45 && tmp > -135) {
            player.Translate(dic.transform.forward * moveSpeed * Time.deltaTime);
        }

        //左    
        else if ((tmp < 180 && tmp > 135) || (tmp < -135 && tmp > -180)) {
            player.Rotate(-Vector3.up, turnSpeed * Time.deltaTime); //旋轉位置可能會因為 VR位置定位的關係而偏差
        }

        //右    
        else if ((tmp > 0 && tmp < 45) || (tmp > -45 && tmp < 0)){
            player.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }
    }




    /// <summary>
    /// 配合傳送門插件的移動方式
    /// </summary>
    /// <param name="tmp"> 手把圓盤按下的位置結果 </param>
    private void SKStudiosMove(float tmp) {
        Vector3 dirVector = dic.transform.forward;
        dirVector.y = 0;
        dirVector = dirVector.normalized * moveSpeed;

        //下  
        if (tmp > 45 && tmp < 135)  {
            player.transform.position += (Quaternion.AngleAxis(180, Vector3.up) * dirVector) * Time.deltaTime;
        }

        //上    
        else if (tmp < -45 && tmp > -135){
            player.transform.position += dirVector * Time.deltaTime;
        }

        //左    
        else if ((tmp < 180 && tmp > 135) || (tmp < -135 && tmp > -180)) {
            player.transform.position += (Quaternion.AngleAxis(-90, Vector3.up) * dirVector) * Time.deltaTime;
        }

        //右    
        else if ((tmp > 0 && tmp < 45) || (tmp > -45 && tmp < 0)){
            player.transform.position += (Quaternion.AngleAxis(90, Vector3.up) * dirVector) * Time.deltaTime;
        }

    }



     /// <summary>
    /// 門 VR環境時用
    /// </summary>
    private void PC_Move() {
        Vector3 dirVector = dic.transform.forward;
        dirVector.y = 0;
        dirVector = dirVector.normalized * moveSpeed;

        // WASD 前進
        if (Input.GetKey(KeyCode.W)){
            player.transform.position += dirVector * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            player.transform.position += (Quaternion.AngleAxis(-90, Vector3.up) * dirVector) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            player.transform.position += (Quaternion.AngleAxis(180, Vector3.up) * dirVector) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))  {
            player.transform.position += (Quaternion.AngleAxis(90, Vector3.up) * dirVector) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D)) {
            player.transform.position += (Quaternion.AngleAxis(90, Vector3.up) * dirVector) * Time.deltaTime;
        }

        // RF 上升下降
        if (Input.GetKey(KeyCode.R)){
            player.transform.position += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.F)){
            player.transform.position -= new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        }


        rotation.y +=  Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        dic.transform.eulerAngles = (Vector2)rotation * turnSpeed;
    }



}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Valve.VR;

///// <summary>
///// 針對 SteamVR 2.0 版本前的圓盤移動方式
///// </summary>
////public class Test_VR_Move : MonoBehaviour
//{
//
//    //模式宣告
//    public enum EM_Move{
//        Face,    //移動方式1 - 朝攝影機看的方向移動
//        Norma_l, //移動方式2 - 普通的前後左右
//        Norma_2, //移動方式3 - 普通的前後左右
//        SKStudios, //移動方式4 - 使用 SKStudios插件的移動 (傳送門)
//        Soul,      //移動方式5 - 產生一個靈魂物件，控制那物件的位置，然後瞬間移動過去 (還沒弄)
//        Teleport   //移動方式6 - 產生一雷射光，瞬間移動到光點位置 (還沒弄)
//    }public EM_Move mState; //獲取模式用
//
//
//    [Header("是否使用手把按鈕移動")]
//    public bool use_TestMove = false;
//
//    /// <summary> 玩家物件(Transform) </summary>  
//    [Header("放入玩家")]
//    public Transform player;
//
//    /// <summary> 頭盔朝向 </summary> 
//    [Header("放入頭盔(eye)")]
//    public Transform dic;
//
//    /// <summary> 移動速度 </summary>
//    [Header("移動速度、轉向速度")]
//    public float moveSpeed = 1.0f ;
//    public float turnSpeed = 10.0f;
//
//
//    /// <summary> 手柄控制器 </summary>  
//    SteamVR_TrackedObject tracked;
//
//
//    ///<summary> SteamVR 裝置(?) </summary>
//    SteamVR_Controller.Device deviceright;
//
//
//
//    void Awake(){
//        //获取手柄控制
//        tracked = GetComponent<SteamVR_TrackedObject>();
//
//        //預設關閉手把移動
//        use_TestMove = false;
//    }
//
//    // Use this for initialization  
//    void Start(){}
//
//    void Update(){
//        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.Alpha9)) {
//            if (use_TestMove){
//                use_TestMove = false;
//            } else {
//                use_TestMove = true; ;
//            }
//        }
//    }
//
//
//    // Update is called once per frame  
//    void FixedUpdate(){
//
//
//
//        if (!use_TestMove) {
//            return;
//        }
//
//        //如果這行出錯可能是程式碼放錯位置，這個程式碼必須要放在右手控制器身上
//        //deviceright = SteamVR_Controller.Input((int)tracked.index);
//
//        switch (mState)
//        {
//            case EM_Move.Face:
//                EM_Move_Face();
//                break;
//            case EM_Move.Norma_l:
//                EM_Move_Normal();
//                break;
//            case EM_Move.Norma_2:
//                EM_Move_Norma2(); 
//                break;
//            case EM_Move.SKStudios:
//                SKStudiosMove();
//                break;
//            case EM_Move.Soul:
//                
//                break;
//            case EM_Move.Teleport:
//
//                break;
//            default:
//                Debug.LogError("Type Error");
//                break;
//        }
//    }
//
//    /// <summary> 根據在圓盤才按下的位置，返回一個角度值 </summary>  
//    float VectorAngle(Vector2 from, Vector2 to) {
//        float angle;
//        Vector3 cross = Vector3.Cross(from, to);
//        angle = Vector2.Angle(from, to);
//        return cross.z > 0 ? -angle : angle;   //如果 cross.z > 0，傳回 -angle，否則傳 angle
//    }
//
//
//    /// <summary> 朝攝影機看的方向移動 </summary> ====================================================
//    void EM_Move_Face(){
//        //按下圆盘键  
//        if (deviceright.GetPress(SteamVR_Controller.ButtonMask.Touchpad)){
//
//            Vector2 cc = deviceright.GetAxis();
//            float angle = VectorAngle(new Vector2(1, 0), cc);
//
//            //下  
//            if (angle > 45 && angle < 135){
//                player.Translate(-dic.forward * Time.deltaTime * moveSpeed);
//            }
//
//            //上    
//            else if (angle < -45 && angle > -135){
//                player.Translate(dic.forward * Time.deltaTime * moveSpeed);
//            }
//
//            //左    
//            else if ((angle < 180 && angle > 135) || (angle < -135 && angle > -180)){
//                player.Translate(-dic.right * Time.deltaTime * moveSpeed);
//            }
//
//            //右    
//            else if ((angle > 0 && angle < 45) || (angle > -45 && angle < 0)){
//                player.Translate(dic.right * Time.deltaTime * moveSpeed);
//            }
//
//            Vector3 newPos = player.transform.position;
//            newPos.y = 0.1f;
//            player.transform.position = newPos;
//        }
//    }
//
//
//    /// <summary> 普通1 </summary> ===================================================================
//    void EM_Move_Normal(){
//        //按下圆盘键  
//        if (deviceright.GetPress(SteamVR_Controller.ButtonMask.Touchpad)){
//
//            Vector2 cc = deviceright.GetAxis();
//            float angle = VectorAngle(new Vector2(1, 0), cc);
//
//            //下  
//            if (angle > 45 && angle < 135){
//                player.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
//            }
//
//            //上    
//            else if (angle < -45 && angle > -135){
//                player.Translate(player.transform.forward * moveSpeed * Time.deltaTime);
//            }
//
//            //左    
//            else if ((angle < 180 && angle > 135) || (angle < -135 && angle > -180)){
//                player.Rotate(-Vector3.up, turnSpeed * Time.deltaTime); //旋轉位置可能會因為 VR位置定位的關係而偏差
//            }
//
//            //右    
//            else if ((angle > 0 && angle < 45) || (angle > -45 && angle < 0)){
//                player.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
//            }
//        }
//    }
//
//
//    /// <summary> 普通2 </summary> ===================================================================
//    void EM_Move_Norma2()
//    {
//        //按下圆盘键  
//        if (deviceright.GetPress(SteamVR_Controller.ButtonMask.Touchpad))  {
//
//            Vector2 cc = deviceright.GetAxis();
//            float angle = VectorAngle(new Vector2(1, 0), cc);
//
//            //下  
//            if (angle > 45 && angle < 135){
//                player.Translate(dic.transform.forward * moveSpeed * Time.deltaTime * -1);
//            }
//
//            //上    
//            else if (angle < -45 && angle > -135){
//                player.Translate(dic.transform.forward * moveSpeed * Time.deltaTime);
//            }
//
//            //左    
//            else if ((angle < 180 && angle > 135) || (angle < -135 && angle > -180)){
//                player.Rotate(-Vector3.up, turnSpeed * Time.deltaTime); //旋轉位置可能會因為 VR位置定位的關係而偏差
//            }
//
//            //右    
//            else if ((angle > 0 && angle < 45) || (angle > -45 && angle < 0)){
//                player.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
//            }
//        }
//    }
//
//
//
//
//    private void SKStudiosMove() {
//        Vector3 dirVector = dic.transform.forward;
//        dirVector.y = 0;
//        dirVector = dirVector.normalized * moveSpeed;
//
//        //按下圆盘键  
//        if (deviceright.GetPress(SteamVR_Controller.ButtonMask.Touchpad))  {
//
//            Vector2 cc = deviceright.GetAxis();
//            float angle = VectorAngle(new Vector2(1, 0), cc);
//
//            //下  
//            if (angle > 45 && angle < 135)  {
//                player.transform.position += (Quaternion.AngleAxis(180, Vector3.up) * dirVector) * Time.deltaTime;
//            }
//
//            //上    
//            else if (angle < -45 && angle > -135) {
//                player.transform.position += dirVector * Time.deltaTime;
//            }
//
//            //左    
//            else if ((angle < 180 && angle > 135) || (angle < -135 && angle > -180))   {
//                transform.position += (Quaternion.AngleAxis(-90, Vector3.up) * dirVector) * Time.deltaTime;
//            }
//
//            //右    
//            else if ((angle > 0 && angle < 45) || (angle > -45 && angle < 0))  {
//                transform.position += (Quaternion.AngleAxis(90, Vector3.up) * dirVector) * Time.deltaTime;
//            }
//        }
//    }
//
//
//
//
//
//
//    /// <summary> 靈魂移動 </summary> ===================================================================
//    void EM_Move_Soul()
//    {
//
//        //按下圆盘键  
//        if (deviceright.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
//        {
//          
//        }
//
//
//        //按下圆盘键  
//        if (deviceright.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
//        {
//            Vector2 cc = deviceright.GetAxis();
//            float angle = VectorAngle(new Vector2(1, 0), cc);
//
//            //下  
//            if (angle > 45 && angle < 135){
//                //
//            }
//
//            //上    
//            else if (angle < -45 && angle > -135){
//
//            }
//
//            //左    
//            else if ((angle < 180 && angle > 135) || (angle < -135 && angle > -180)){
//
//            }
//
//            //右    
//            else if ((angle > 0 && angle < 45) || (angle > -45 && angle < 0)){
//
//            }
//        }
//    }
//
//
//}  
using UnityEngine;
using System.Collections;

public class WheelRotate : MonoBehaviour
{

    [Header("要轉動的物體")]
    public Transform[] Wheel;

    [Header("轉動速度")]
    public  float curSpeed = 0;       //當前的速度
    public  float maxSpeed = 500;     //轉動的速度

    [Header("轉動軸向")]
    public bool isX;
    public bool isY;
    public bool isZ;

    [Header("是否限制轉動的時間、轉多久(秒)")]
    public  bool  useTime = false; //是否使用時間限制
    public  float rTime = 10;      //多久後
    private float tTime = 0;       //開始減速的時間

    //是否能轉動
    private bool canRotate = true; //輪子可以轉動

    //加速或減速控制
    private float rSpeed;           //加減速的速率
    private bool speedUp   = false; //輪子開始加速
    private bool speedDown = false; //輪子開始減速


    // 初始設定 ================================================================
    void Start (){
        speedUp = true;                      //開始加速

        rSpeed = maxSpeed / 2;               //減速的速度=速度的二分之一

        tTime = rTime - (curSpeed / rSpeed); //開始減速的時間 = 停止轉動的時間 - 減速需要的時間 (=距離/速率)(=速度總長/減速的速度) (速度總長=當前速度-最低速)
        if (useTime)                         //如果有限制轉動的時間
        { Invoke("StopRotate", tTime); }     //則時間差不多了便開始減速，並在指定時間停止轉動
    }

    // 使輪子持續轉動 ==========================================================
    void Update(){

        //如果可以轉動
        if (canRotate){

            // 轉動---------------------------------------------------------------------------------------
            if (Wheel.Length>0){                                                                                 //如果有指定轉動物件
                for (int i = 0; i < Wheel.Length; i++){                                                          //每個輪子轉動
                    if (isX && Wheel[i] != null) { Wheel[i].transform.Rotate(curSpeed * Time.deltaTime, 0, 0); } //則輪子持續轉動
                    if (isY && Wheel[i] != null) { Wheel[i].transform.Rotate(0, curSpeed * Time.deltaTime, 0); } //則輪子持續轉動
                    if (isZ && Wheel[i] != null) { Wheel[i].transform.Rotate(0, 0, curSpeed * Time.deltaTime); } //則輪子持續轉動
                }
            }else{                                                              //否則
                if (isX) { transform.Rotate(curSpeed * Time.deltaTime, 0, 0); } //自身持續轉動
                if (isY) { transform.Rotate(0, curSpeed * Time.deltaTime, 0); } //自身持續轉動
                if (isZ) { transform.Rotate(0, 0, curSpeed * Time.deltaTime); } //自身持續轉動
            }      
             
            // 加速----------------------------------------------------------------------------------------
            if (speedUp){  //如果開始加速
                SpeedUp(); //加速
            }

            // 減速----------------------------------------------------------------------------------------
            if (speedDown){  //如果開始減速
                SpeedDown(); //減速
            }
        }


        // 測試按鍵----------------------------------------------------
        //if (Input.GetKeyDown(KeyCode.R))
        //{ StartRotate(); }
        //
        //if (Input.GetKeyDown(KeyCode.F))
        //{ StopRotate();  }

    }



    #region 動畫事件接口 =======================================================
    // 開始轉動 ================================================================
    void t_Start(float tmpTime = 0.0f){  // tmpTime = 多久後達到指定速度
        //float gap = maxSpeed - curSpeed;
        //tTime = tmpTime - (gap / rSpeed);
        //Invoke("StartRotate", tTime);
        StartRotate();
    }

    //停止轉動 =================================================================
    void t_Stop(float tmpTime = 0.0f){ // tmpTime = 多久後完全停止
        StopRotate();
    }
    #endregion


    #region 加減速控制 =========================================================
    // 開始轉動 ================================================================
    void StartRotate(){
        speedDown = false; //停止減速
        speedUp   = true;  //開始加速
        canRotate = true;  //輪子開始轉動
    }

    //停止轉動 =================================================================
    void StopRotate(){
        speedUp = false;  //停止加速
        speedDown = true; //開始減速
    }
    #endregion


    #region 速度控制區 =========================================================
    // 加速 ====================================================================
    void SpeedUp(){
        if (curSpeed < maxSpeed){                //如果速度小於指定速度
            curSpeed += rSpeed * Time.deltaTime; //就加速
        }
        else{                    //當加速完畢
            curSpeed = maxSpeed; //速度固定為最大速
            speedUp  = false;    //停止加速
        }
    }


    // 減速 ====================================================================
    void SpeedDown(){
        if (curSpeed > 0){                       //如果速度大於0
            curSpeed -= rSpeed * Time.deltaTime; //就減速
        }
        else{                  //當減速完畢
            curSpeed = 0;      //速度固定0 (因為速度小於0會向後轉)
            speedDown = false; //停止減速
            canRotate = false; //輪子停止轉動
        }
    }
    #endregion


}

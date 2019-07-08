using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test_Bullet_Collision : MonoBehaviour {

    [Header("座標物體1、其半徑")]
    public Transform Posa;
    public float PosSize1;

    [Header("座標物體2、其半徑")]
    public Transform Posb;
    public float PosSize2;


    // Use this for initialization
    void Start () {
        Get_CrossPosition();
    }

    // Update is called once per frame
    void Update () {
        if (Posa!=null && Posb!=null)
        { Check_Cross(); }
    }


    /// <summary> 檢查兩球體是否相交 </summary>
    void Check_Cross(){
        float f = Vector3.Distance(Posa.position, Posb.position); // f = 兩者之間的距離
        float d = f - PosSize1;                                   // d = 兩者間的距離 - 其中一個球體的半徑
        if (d <= PosSize2){                                       // 如果 d < 另一個球體的半徑
            Debug.LogWarning("兩者相交了！");                     // Debug.Log
        }
    }

    /// <summary> 取得交點座標 </summary>
    void Get_CrossPosition(){
        Vector3 dv = Posa.position - Posb.position;            // A位置 - B位置 = B到A的方向向量(ex: dv=(10,15,20)，B的XYZ各要移動 10、15、20才能到A點)
        float distance = dv.magnitude;                         // 取得A、B之間的距離長度
        dv *= (distance == 0.0f ? 0.0f : PosSize2 / distance); // dv 乘上 [如果距離為0，距離=0，如果不等於0，距離等於 (速度or要移動量)/距離] ※註1
        Vector3 croPos = Posb.position + dv;                   // 求兩球體交點座標
        Debug.LogWarning("交點座標 = " + croPos);              // Debug.Log
    }

}




/* (註1)
// 參考自 https://goo.gl/C4WHvu
// dv 是 directionVector 的縮寫
// dv 乘上 (如果距離為0，距離=0，如果不等於0，距離等於 速度(半徑)/距離)
// dv *=   (distance == 0.0f ? 0.0f : PosSize2 / distance);
// ↓
// ↓可以等於下面寫法
// ↓

    if ( distance == 0.0f ) {     //如果兩者距離=0
        dv *= 0.0f;               //表示不用移動了,已經在目標點上了，故=0
    }
    else{                         //如果不等於0
        dv *= (speed / distance); //dv 是B移動到A的話 B的XYZ各要移動多少形成的座標，故 距離分之速度 表示一次移動的量
    }

    Posb += dv;                   //執行移動

*/

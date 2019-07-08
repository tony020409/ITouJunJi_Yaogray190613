using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAddForce : MonoBehaviour {

    [Header("多久後噴飛")]
    public float ptime;

    [Header("放入要噴飛的東西")]
    public Rigidbody[] Fracture;

    [Header("噴飛的力道")]
    public float minX;
    public float maxX;

    [Space(10)]
    public float minY;
    public float maxY;

    [Space(10)]
    public float minZ;
    public float maxZ;



    // Use this for initialization
    void Start () {
        Invoke("penfa",ptime);

    }
	
	// Update is called once per frame
	void Update () {


        //物體碎片選轉
        //for (int i = 0; i < Fracture.Length; i++){
        //    Fracture[i].AddTorque( Random.Range(500, 1000), Random.Range(500, 1000), Random.Range(500, 1000), ForceMode.VelocityChange); //增加力矩讓轉動自然
        //}
    }


    void penfa()
    {
        for (int i = 0; i < Fracture.Length; i++){ //物體碎片噴發
            Fracture[i].isKinematic = false;                                                                  //isKinematic取消
            //Fracture[i].AddForce(0, Random.Range(40, 60), Random.Range(50, 70));                            //給予Y軸Z軸(斜前方)隨機力道
            //Fracture[i].AddTorque(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)); //增加力矩讓轉動自然
            Fracture[i].AddForce (0                      , Random.Range(160, 200) , Random.Range(160, 200));  //給予Y軸Z軸(斜前方)隨機力道
            //Fracture[i].AddForce(Vector3.up * 10, );
           // Fracture[i].AddTorque(Random.Range(500, 1000), Random.Range(500, 1000), Random.Range(500, 1000), ForceMode.VelocityChange);        //給予力矩讓轉動自然
        }
    }



}

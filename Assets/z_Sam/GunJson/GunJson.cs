using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;



public class GunJson : MonoBehaviour
{
    public GunData gunData;

    [Rename("槍枝節點")] public GameObject Gun;
    public GameObject ControllerModelLeft, ControllerModelRight;
    [Rename("遊戲畫面顯示資訊用的文字")] public Text SupplementRotationtext;


    // Use this for initialization
    void Start()
    {
        //Write(JsonUtility.ToJson(gunData));
        Read(Application.streamingAssetsPath + "/GunJson.txt");
        if (gunData.change)
        {
            Adaptation();
        }
        else
        {
            gunData.SupplementRotation = 330f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (gunData.Adaptation) {
            if (SupplementRotationtext != null) {
                SupplementRotationtext.gameObject.SetActive(true);
                SupplementRotationtext.text = Gun.transform.eulerAngles.ToString();
            }

            if (Input.GetKeyDown(KeyCode.A)) {
                Read(Application.streamingAssetsPath + "/GunJson.txt");
                Adaptation();
            }
        }
        else {
            if (SupplementRotationtext != null) {
                SupplementRotationtext.gameObject.SetActive(false);
            }   
        }
    }


    public void Adaptation() {
        Gun.gameObject.transform.localPosition = gunData.GunPosition;
        Gun.gameObject.transform.localEulerAngles = gunData.GunRotation;
        if (ControllerModelLeft != null) {
            ControllerModelLeft.gameObject.transform.localScale = gunData.HandleModelSize;
        }
        if (ControllerModelLeft != null) {
            ControllerModelRight.gameObject.transform.localScale = gunData.HandleModelSize;
        }
    }


    public void Write(string s) {
        FileStream fs = new FileStream(Application.streamingAssetsPath + "/GunJson.txt", FileMode.Create);
        byte[] data = System.Text.Encoding.Default.GetBytes(s); //獲得位元組陣列
        fs.Write(data, 0, data.Length);                         //開始寫入
        fs.Flush();                                             //清空緩衝區
        fs.Close();                                             //關閉流
    }


    public void Read(string path) {
        StreamReader sr = new StreamReader(path, Encoding.Default);
        String line;
        while ((line = sr.ReadLine()) != null) {
            //print(line.ToString());
            gunData = JsonUtility.FromJson<GunData>(line.ToString());
        }
        sr.Close();
    }


    public IEnumerator ooo(float f){
        yield return new WaitForSeconds(f);
        Gun.gameObject.transform.position = gunData.GunPosition;
    }

}

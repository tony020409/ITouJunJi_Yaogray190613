using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

//using System.IO.Ports;
using UnityEngine;
using System;


public class BlackTeaBuzzFloor : MonoBehaviour
{
    public bool init = true;
    string Message = "";
    bool order1, order2, order3, order4;

    //public SerialPort Port;
    int PortIndex = 1;
   
    void Start ()
    {

    }

    void OnApplicationQuit()
    {
        
    }

    void Update ()
    {
        if (init) PortSet();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            StartCoroutine(s1Button(3f));
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            StartCoroutine(s2Button(1.5f));
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            StartCoroutine(s3Button(1f));
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            StartCoroutine(s4Button(0.1f, 100f));
        }
#endif
    }

    private void OnGUI()
    {
        try
        {
            GUI.Label(new Rect(5, Screen.height - 60, 1000, 20), Message);
        }
        catch (Exception ex)
        {
            Message = ex.Message;
        }
    }

    void PortSet()
    {
        
    }

    public void OpenPort()
    {
       
    }

    public void SetComport(int comportswitchdata)
    {
       
    }

    public void f_setVol(float mp3_set_volume_data)
    {
        int normalizeVol = (int)(mp3_set_volume_data / 100 * 30);
        //print(normalizeVol);
        WritePORT("20" + normalizeVol.ToString());
        WritePORT("20" + normalizeVol.ToString());
    }

    public void Play(String mp3_play_data)//1-5首
    {
        WritePORT("0000");
        WritePORT("1" + mp3_play_data);
        WritePORT("1" + mp3_play_data);
    }

    public void Stop()
    {
        WritePORT("0000");
        WritePORT("0000");
        WritePORT("0000");
    }

    public void WritePORT(string Writedata)
    {
       
    }

    //暴龍吼叫
    public IEnumerator s1Button(float WaitTime)
    {
        f_setVol(100f);
        //Debug.Log("s1Button");
        Play("01");
        order1 = true;

        yield return new WaitForSeconds(WaitTime);

        order1 = false;
    }
    //油桶爆炸
    public IEnumerator s2Button(float WaitTime)
    {
        if (order1 != true)
        {
            f_setVol(100f);
            //Debug.Log("s2Button");
            Play("02");
            order2 = true;

            yield return new WaitForSeconds(WaitTime);

            order2 = false;
        }
    }
    public void do_s3Button(float WaitTime)
    {
        StartCoroutine(s3Button(WaitTime));
    }


    //榴彈槍爆炸
    public IEnumerator s3Button(float WaitTime)
    {
        //Debug.Log("s3ButtonPre");
        if (order1 != true && order2 != true)
        {
            f_setVol(75f);
            //Debug.Log("s3ButtonLate");
            Play("03");
            order3 = true;

            yield return new WaitForSeconds(WaitTime);

            order3 = false;
            //Debug.Log("order3 = false");
        }
    }
    //暴龍踏步
    public IEnumerator s4Button(float WaitTime, float Vol)
    {
        //Debug.Log("s4ButtonPre");
        if (order1 != true && order2 != true && order3 != true && order4 != true)
        {
            f_setVol(Vol);
            //Debug.Log("s4ButtonLate");
            Play("04");
            order4 = true;

            yield return new WaitForSeconds(WaitTime);

            order4 = false;
        }
        //else
        //{
        //    Debug.Log(order1.ToString() + ", " + order2.ToString() + ", " + order3.ToString());
        //}
    }
    public IEnumerator s9Button(float WaitTime)
    {
        f_setVol(100f);
        //Debug.Log("s1Button");
        Play("09");
        order1 = true;

        yield return new WaitForSeconds(WaitTime);

        order1 = false;
    }
    public IEnumerator s10Button(float WaitTime)
    {
        f_setVol(100f);
        //Debug.Log("s1Button");
        Play("10");
        order1 = true;

        yield return new WaitForSeconds(WaitTime);

        order1 = false;
    }

    public IEnumerator s99Button(float WaitTime)
    {
        f_setVol(100f);
        //Debug.Log("s1Button");
        Play("999");
        order1 = true;

        yield return new WaitForSeconds(WaitTime);

        order1 = false;
    }
    public IEnumerator s12Button(float WaitTime)
    {
        f_setVol(100f);
        //Debug.Log("s1Button");
        Play("12");
        order1 = true;

        yield return new WaitForSeconds(WaitTime);

        order1 = false;
    }
    public IEnumerator s13Button(float WaitTime)
    {
        f_setVol(100f);
        //Debug.Log("s1Button");
        Play("13");
        order1 = true;

        yield return new WaitForSeconds(WaitTime);

        order1 = false;
    }
    public IEnumerator s14Button(float WaitTime)
    {
        f_setVol(100f);
        //Debug.Log("s1Button");
        Play("14");
        order1 = true;

        yield return new WaitForSeconds(WaitTime);

        order1 = false;
    }


    public IEnumerator Playshock(string AudioName, float WaitTime)
    {
        f_setVol(100f);
        //Debug.Log("s1Button");
        Play(AudioName);
        order1 = true;

        yield return new WaitForSeconds(WaitTime);

        order1 = false;

    }

}

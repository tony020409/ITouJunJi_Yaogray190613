using UnityEngine;
using System;
using System.Collections;
//using System.IO.Ports;
using System.Threading;
using VivelandEffect;

public class HTCBuzzFloor : MonoBehaviour
{
    private BoomEffect be;
    string Message = "";
    bool order1 = false, order2 = false, order3 = false, order4 = false;
    // Use this for initialization
    void Start()
    {
        // test
        be = new BoomEffect();
        if (be.init())
        {
            //Debug.Log("BoomEffect init on success!");
            Message = "BoomEffect init on success!";
        }
        else
        {
            //Debug.Log("BoomEffect init failed!");
            Message = "BoomEffect init failed!";
        }
            
        //print(be.getInfo());
        //print(be.getPorts());
        be.setVol(100);
    }

    // Update is called once per frame
    void Update()
    {
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

    public void f_setVol(float V)
    {
        be.setVol(Convert.ToInt32(V));
        //print(Convert.ToInt32(V));
    }

    public IEnumerator offButton(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        //Debug.Log("offButton");
        be.stop();
    }

    //暴龍吼叫
    public IEnumerator s1Button(float WaitTime)
    {
        f_setVol(100f);
        //Debug.Log("s1Button");
        be.play(1);
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
            be.play(2);
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
            be.play(3);
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
            be.play(4);
            order4 = true;

            yield return new WaitForSeconds(WaitTime);

            order4 = false;
        }
        //else
        //{
        //    Debug.Log(order1.ToString() + ", " + order2.ToString() + ", " + order3.ToString());
        //}
    }
}

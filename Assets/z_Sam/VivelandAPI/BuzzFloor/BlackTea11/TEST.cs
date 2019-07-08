using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using System.IO;
using UnityEngine;
using System;

public class TEST : MonoBehaviour
{
    public BlackTeaBuzzFloor m_BlackTeaBuzzFloor;

    public bool mp3_playset = true;
    public bool set_volumeset = false;
    public bool stopset = false;
    public string playdata1 = "01";
    public float volumedata = 10;
    public string stopdata = "0";

    void Update()
    {
        try
        {
           
        }

        catch (Exception e){}

        try
        {
        }

        catch (Exception e){}

        if (stopset)
        {
            m_BlackTeaBuzzFloor.Stop();
            stopset = false;
        }
    }
}
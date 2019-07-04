using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITools
{


    public static void f_SetText(GameObject Obj, string strText)
    {
        if (Obj == null)
        {
            return;
        }
        Text tText = Obj.GetComponent<Text>();
        if (tText == null)
        {
            tText = Obj.GetComponentInChildren<Text>();
        }
        if (tText != null)
        {
            tText.text = strText;
        }
    }

    



}
using System;
using UnityEngine;


//修改自下方網址六樓回文：
//https://forum.unity.com/threads/helpattribute-allows-you-to-use-helpbox-in-the-unity-inspector-window.462768/


public enum HelpBoxType {
    None, Info, Warning, Error
}


[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class HelpBoxAttribute : PropertyAttribute
{

    public string text;
    public HelpBoxType BoxType;
    public float height;

    public HelpBoxAttribute( string text, HelpBoxType BoxType = HelpBoxType.None, float height = 8){
        this.text = text;
        this.BoxType = BoxType;
        this.height = height;
    }


}
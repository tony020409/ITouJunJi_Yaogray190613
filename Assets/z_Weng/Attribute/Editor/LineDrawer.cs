

using System;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(LineAttribute))]
public class LineDrawer : DecoratorDrawer
{

    //上一個參數 與 下一個參數間的距離
    public override float GetHeight()
    {
        LineAttribute tmp = (LineAttribute)attribute;
        return tmp.height + 8;
    }

    //
    public override void OnGUI(Rect position)
    {

        //獲取Attribute
        LineAttribute tmp = (LineAttribute)attribute;

        //分割線粗細和位置
        position.height = 3;                  //分割線粗細
        position.y = position.y + tmp.height; //分割線位置

        //顯示分割線
        GUI.Box(position, "");
    }
}
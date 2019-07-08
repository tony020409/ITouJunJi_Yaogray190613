
using UnityEngine;
using System;


[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class LineAttribute : PropertyAttribute {

    //與上一個參數的距離
    public float height;

    public LineAttribute(float height = 6){
        this.height = height;
    }

}



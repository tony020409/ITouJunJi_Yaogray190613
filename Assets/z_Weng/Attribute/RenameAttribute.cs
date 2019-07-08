using System;
using UnityEngine;


//參考自 http://gad.qq.com/article/detail/43804
//然後補上漏掉 public override void OnGUI(...) 的部分


[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class RenameAttribute : PropertyAttribute
{
    //自定義的名稱
    public string name;
    public int enumType;
    public string[] ArrayName;
    public string ArrayDefaultName;


    /// <summary>
    /// 對應大部分情形, 使用如右： [Rename("名稱")]
    /// </summary>
    /// <param name="enumType"> 0=只能選一個Enum，1=能選多個Enum，Unity預設只能選一個，這算附加功能 </param>
    public RenameAttribute(string name, int enumType = 0){
        this.name = name;
        this.enumType = enumType;
    }


    /// <summary>
    /// 對應 Array類型, 使用如下：
    /// [Rename(new string[] {"小吳","小陳","小王"}, DefaultName : "其它同學" )] 
    /// 陣列參數名稱會為 [0]=小吳 [1]=小陳 [2]=小王 [3]=其它同學[3] [4]=其他同學[4]
    /// </summary>
    /// <param name="name"       > 前幾項的名稱，如右: new string[] {"","",""} </param>
    /// <param name="DefaultName"> 後幾項的預設名稱 </param>
    public RenameAttribute(string[] name, string DefaultName = null){
        this.ArrayName = name;
        this.ArrayDefaultName = DefaultName;
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html


/// <summary>
/// Public 參數僅顯示，不能修改
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute {

    /// <summary>
    /// 新名字
    /// </summary>
    public string new_Name;


    /// <summary>
    /// 陣列類型的前幾個名稱
    /// </summary>
    public string[] ArrayName;

    /// <summary>
    /// 陣列類型的預設名稱 (僅在有預設前幾個名稱的情況下)
    /// </summary>
    public string ArrayDefaultName;

    /// <summary>
    /// 陣列類型的預設名稱是否顯示編號
    /// </summary>
    public bool ShowArrayIndex;



    public ReadOnlyAttribute(string m_new_Name = "No_New_Name_99") {
        this.new_Name = m_new_Name;
    }


}
using UnityEngine;
using System.Collections;


public class ArraysExample : MonoBehaviour {

    [System.Serializable]
    public class MyClass
    {
        [Rename("註解")] public string aString;
        [Rename("整數")] public int aInt;
    }

    [Array]
    public MyClass[] theArray;

    [Array]
    public int[] theIntArray;

    [Line()]
    [Rename(new string[] {"待機音效", "攻擊音效", "死亡音效" } )]
    public MyClass[] theArray2;

    [Rename(new string[] { "測試A", "測試B", "測試C" })]
    public int[] theIntArray2;

    [Line()]
    [Rename("Class")]
    public MyClass[] theArray3;

    [Rename("測試")]
    public int[] theIntArray3;


    [Line()]
    [Rename("測試Class非陣列")]
    public MyClass theArrayo;

    [Rename("測試整數非陣列")]
    public int theIntArrayo;


    [Line()]
    [Header("Class參數第一個為string的自帶功能")]
    //Class參數第一個為string的自帶功能
    public MyClass[] theArray5;
}

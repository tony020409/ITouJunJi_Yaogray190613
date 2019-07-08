
using UnityEngine;
#if UNITY_EDITOR
using System;
using UnityEditor;
#endif

//參考自後方網址並進行了以下修改 https://segmentfault.com/a/1190000013174368
//1. 添加分隔線。
//2. 添加參數：與上一個參數的距離。
//3. 添加參數：字體樣式。
//已知問題：分割線在文字後在 title有英文的情況下，無法完美運作。

/*
/// <summary>
/// 標題屬性(Attribute)
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class TitleAttribute : PropertyAttribute
{

    public string title;     //標題名稱
    public bool showLine;    //是否開啟分隔線
    public float height;     //與上一個參數的距離 (預設8)
    public bool Bold;        //是否開啟粗體
    public bool Italic;      //是否開啟斜體
    public Color tColor;      //標題顏色



    public enum ColorType {
        Black,
        Yellow,
    }

    /// <summary>
    /// 在屬性上方添加一个標題
    /// </summary>
    /// <param name="title"   > 標題名稱 </param>
    /// <param name="showLine"> 是否顯示分隔線 </param>
    /// <param name="AreaSize"> title 所包含的範圍 </param>
    /// <param name="height"  > 與上一個參數的距離 </param>
    /// <param name="Bold"    > 是否開啟粗體 </param>
    /// <param name="Italic"  > 是否開啟斜體 </param>
    public TitleAttribute(
        string title,
        bool showLine = false,
        float height = 8, 
        bool Bold = false, 
        bool Italic = false)
    {
        this.title = title;
        this.showLine = showLine;
        this.height = height;
        this.Bold = Bold;
        this.Italic = Italic;
        //this.tColor = Color;
    }

}
*/

/*
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TitleAttribute))]
public class TitleAttributeDrawer : DecoratorDrawer
{
    // 文本樣式
    private GUIStyle style = new GUIStyle();

    //用來計算文字長度 (為了讓線跑到文字後用)
    private int fontLength; 

    public override float GetHeight(){
        TitleAttribute tmp = (TitleAttribute)attribute;
        return tmp.height +16.0f;
    }


    public override void OnGUI(Rect position) {
        //獲取Attribute
        TitleAttribute tmp = (TitleAttribute)attribute;

        //文字顏色
        //style.normal.textColor = tmp.tColor;


        //文字形式
        if (tmp.Bold && tmp.Italic) {      //粗體+斜體
            style.fontStyle = FontStyle.BoldAndItalic;
        }
        else if (!tmp.Bold && tmp.Italic) { //普通斜體
            style.fontStyle = FontStyle.Italic;
        }
        else if (tmp.Bold) {                //粗體(預設)
            style.fontStyle = FontStyle.Bold;
        }
        else {                              //無效果
            style.fontStyle = FontStyle.Normal;
        }
        

        // 文字位置與顯示
        position.y = position.y + tmp.height;        //文字的位置
        position = EditorGUI.IndentedRect(position); 
        GUI.Label(position, tmp.title, style);       //顯示文字


        //分割線位置與顯示
        if (tmp.showLine) {
            position.height = 3;                                                                             //分割線粗細
            position.y = position.y + 8f;                                                                    //分割線位置
            fontLength = System.Text.Encoding.Default.GetByteCount(tmp.title);                               //適應中英混雜的情況(?)
            if (fontLength % 2 == 0) {                                                                       //全中文的情形
                position.x = position.x + GUI.skin.label.CalcSize(new GUIContent(tmp.title)).x + 2;          //分割線在文字後
            } else {                                                                                         //中英混雜的情形
                position.x = position.x + GUI.skin.label.CalcSize(new GUIContent(tmp.title)).x + fontLength; //分割線在文字後
            }
            GUI.Box(position, "");                                                                           //顯示分割線
        }
       
    }
    



}
#endif
*/



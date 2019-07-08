
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

//基礎參考自 http://gad.qq.com/article/detail/43804
//Enum參考自 https://segmentfault.com/a/1190000013174368
//Array參考自 https://forum.unity.com/threads/generic-editor-array-propertyattribute-tools.240895/
//Array參考2  https://answers.unity.com/questions/629095/can-i-change-inspector-element-0-to-something-else.html (沒用到，但感覺可以試試)

[CustomPropertyDrawer(typeof(RenameAttribute))] //用到RenameAttribute的地方都會被重繪製  
public class RenameDrawer : PropertyDrawer      //相對於Editor類可以修改MonoBehaviour的外觀，我們可以簡單的理解PropertyDrawer為修改 struct/ class的外觀的Editor類
{
    //自適應高度 (e.g. Vector3的參數在 Inspector視窗不寬時，參數會跑到下面一行)
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        RenameAttribute rename = (RenameAttribute)attribute; //獲取套用 [Rename] 的物件
      
        if (property.propertyType == SerializedPropertyType.Enum) {
            label.text = rename.name;                             //替換屬性名稱
            DrawEnum(position, property, label, rename.enumType); //重繪GUI (Enum類型)
        }
        else if (property.propertyPath.LastIndexOf(".Array") > 0) {
            DrawArray(position, property, label); //重繪GUI (Array類型)
        }
        else {
            label.text = rename.name;                                 //替換屬性名稱
            EditorGUI.PropertyField(position, property, label, true); //重繪GUI (通用)
        }
    } 





    /// <summary>
    /// 繪製枚舉類型
    /// </summary>
    private void DrawEnum(Rect position, SerializedProperty property, GUIContent label, int enumType)  {
        //偵測當前選擇的Enum
        EditorGUI.BeginChangeCheck();

        //獲取枚舉相關屬性
        Type type = fieldInfo.FieldType;
        string[] enumNames = property.enumNames;
        string[] values = new string[enumNames.Length];

        // 獲取枚舉相對應名稱
        for (int i = 0; i < enumNames.Length; i++) {
            FieldInfo info = type.GetField(enumNames[i]);
            RenameAttribute[] atts = (RenameAttribute[])info.GetCustomAttributes(typeof(RenameAttribute), false);
            if (atts.Length == 0) {
                values[i] = enumNames[i];
            } else {
                values[i] = atts[0].name;
            }
        }

        //重繪選單型GUI (單選 Enum)
        if (enumType == 0) {
            //單選 (最初看到的寫法)
            int index = EditorGUI.Popup(position, label.text, property.enumValueIndex, values, EditorStyles.popup); //原本的
            if (EditorGUI.EndChangeCheck() && index != -1) {
                property.enumValueIndex = index; //原本的
            }
            //單選 (參考多選的寫法)
            //int index = EditorGUI.Popup(position, label.text, property.intValue, values, EditorStyles.popup);     
            //if (EditorGUI.EndChangeCheck() && index != -1){
            //    property.intValue = index;
            //}
        }

        //重繪選單型GUI (多選 Enum)
        else if (enumType == 1) {
            int index = EditorGUI.MaskField(position, label.text, property.intValue, values);
            if (EditorGUI.EndChangeCheck()) {
                property.intValue = index;
            }
        }

    }



    /// <summary>
    /// 繪製Array類型
    /// </summary>
    private void DrawArray(Rect position, SerializedProperty property, GUIContent label) {
        RenameAttribute rename = (RenameAttribute)attribute; //獲取套用 [Rename] 的物件
        try {
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            if (rename.ArrayName != null){
                label.text = rename.ArrayName[pos];
            } else {
                if (rename.ArrayDefaultName != null) {
                    label.text = rename.ArrayDefaultName + " [" + pos.ToString() + "]";
                } else {
                    label.text = rename.name + " [" + pos.ToString() + "]";
                }
            }
            EditorGUI.PropertyField(position, property, label, true);
        }
        catch{
            if (rename.ArrayDefaultName != null) {
                label.text = rename.ArrayDefaultName;
                EditorGUI.PropertyField(position, property, label, true);
            }
            else {
                EditorGUI.PropertyField(position, property, label, true);
            }
           
        }
    }


}





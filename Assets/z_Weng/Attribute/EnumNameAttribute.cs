using UnityEngine;
#if UNITY_EDITOR
using System;
using UnityEditor;
using System.Reflection;
using System.Text.RegularExpressions;
#endif


//複製於 https://segmentfault.com/a/1190000013174368


/// <summary>
/// 設置枚舉名稱
/// </summary>
#if UNITY_EDITOR
[AttributeUsage(AttributeTargets.Field)]
#endif
public class EnumNameAttribute : PropertyAttribute
{
    /// <summary> 
    /// 枚舉名稱 
    /// </summary>
    public string name;

    public EnumNameAttribute(string name)
    {
        this.name = name;
    }

}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnumNameAttribute))]
public class EnumNameDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //替換屬性名稱
        EnumNameAttribute rename = (EnumNameAttribute)attribute;
        label.text = rename.name;

        //重繪 GUI
        bool isElement = Regex.IsMatch(property.displayName, "Element \\d+");
        if (isElement) label.text = property.displayName;
        if (property.propertyType == SerializedPropertyType.Enum)
        {
            DrawEnum(position, property, label);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }


    /// <summary>
    /// 繪製枚舉類型
    /// </summary>
    private void DrawEnum(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();

        //獲取枚舉相關屬性
        Type type = fieldInfo.FieldType;
        string[] names = property.enumNames;
        string[] values = new string[names.Length];

        //
        while (type.IsArray)
            type = type.GetElementType();


        // 獲取枚舉相對應名稱
        for (int i = 0; i < names.Length; i++)
        {
            FieldInfo info = type.GetField(names[i]);
            EnumNameAttribute[] atts = (EnumNameAttribute[])info.GetCustomAttributes(typeof(EnumNameAttribute), false);
            values[i] = atts.Length == 0 ? names[i] : atts[0].name;
        }
        // 重繪GUI
        int index = EditorGUI.Popup(position, label.text, property.enumValueIndex, values);
        if (EditorGUI.EndChangeCheck() && index != -1) property.enumValueIndex = index;
    }

}
#endif
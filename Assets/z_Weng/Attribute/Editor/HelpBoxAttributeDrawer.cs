using UnityEngine;
using UnityEditor;


//修改自下方網址六樓回文：
//https://forum.unity.com/threads/helpattribute-allows-you-to-use-helpbox-in-the-unity-inspector-window.462768/


[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
public class HelpBoxAttributeDrawer : DecoratorDrawer
{
    private float BoxHeight;

    public override float GetHeight() {
        HelpBoxAttribute helpBox = (HelpBoxAttribute) attribute;
        //if (helpBox == null) {
        //    return base.GetHeight();
        //}
        //GUIStyle HelpBoxStyle = (GUI.skin != null) ? GUI.skin.GetStyle("helpbox") : null;
        //if (HelpBoxStyle == null) {
        //    return base.GetHeight();
        //}
        //return Mathf.Max(30f, HelpBoxStyle.CalcHeight(new GUIContent(helpBox.text), EditorGUIUtility.currentViewWidth) + 4);
        GUIStyle HelpBoxStyle = GUI.skin.GetStyle("helpbox");
        HelpBoxStyle.fontSize = 12;
        HelpBoxStyle.wordWrap = true;
        BoxHeight = HelpBoxStyle.CalcHeight(new GUIContent(helpBox.text), EditorGUIUtility.currentViewWidth - 64);
        return helpBox.height + BoxHeight + 6;
    }

    public override void OnGUI(Rect position) {
        HelpBoxAttribute helpBox = (HelpBoxAttribute) attribute;
        position.height -= 10;
        position.y = position.y + helpBox.height;
        position = EditorGUI.IndentedRect(position);
        EditorGUI.HelpBox(position , helpBox.text, GetMessageType(helpBox.BoxType));
    }

    private MessageType GetMessageType(HelpBoxType helpBoxType) {
        switch (helpBoxType) {
            default:
            case HelpBoxType.None:    return MessageType.None;
            case HelpBoxType.Info:    return MessageType.Info;
            case HelpBoxType.Warning: return MessageType.Warning;
            case HelpBoxType.Error:   return MessageType.Error;
        }
    }
}
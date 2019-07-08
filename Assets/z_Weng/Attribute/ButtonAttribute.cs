#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Reflection;


/// <summary>
/// This attribute can only be applied to fields because its
/// associated PropertyDrawer only operates on fields (either
/// public or tagged with the [SerializeField] attribute) in
/// the target MonoBehaviour.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class ButtonAttribute : PropertyAttribute
{

    //可調參數
    public static float kDefaultButtonWidth = 80;     //按鈕長度(預設80，可透過 [InspectorButton（“OnButtonClicked”，ButtonWidth = 100）] 調整)
    public readonly string ButtonName;                //調用的方法
    public readonly string MethodName;                //調用的方法名稱



    //內部參數
    private float _buttonWidth = kDefaultButtonWidth; //按鈕長度
    

    public float ButtonWidth{
        get { return _buttonWidth; }
        set { _buttonWidth = value; }
    }


    //[Buttom] 格式
    public ButtonAttribute(string ButtonName, string MethodName){
        this.ButtonName = ButtonName;
        this.MethodName = MethodName;
    }



}



#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ButtonAttribute))]
public class InspectorButtonPropertyDrawer1 : PropertyDrawer
{
    private MethodInfo _eventMethodInfo = null;
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        ButtonAttribute tmp = (ButtonAttribute) attribute;
        Rect buttonRect = new Rect(
            position.x + (position.width - tmp.ButtonWidth) * 0.5f,
            position.y + 5.0f, 
            tmp.ButtonWidth, 
            position.height
            );


        if (GUI.Button(buttonRect, label.text))
        {
            System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();

            string buttonName = tmp.ButtonName;
            string eventName = tmp.MethodName;

            if (_eventMethodInfo == null) {
                _eventMethodInfo = eventOwnerType.GetMethod(
                    eventName, 
                    BindingFlags.Instance | 
                    BindingFlags.Static | 
                    BindingFlags.Public | 
                    BindingFlags.NonPublic);
            }

            if (_eventMethodInfo != null){
                _eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
            } else {
                Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
            } 
        }
    }
}
#endif



//來自
//https://www.reddit.com/r/Unity3D/comments/1s6czv/inspectorbutton_add_a_custom_button_to_your/
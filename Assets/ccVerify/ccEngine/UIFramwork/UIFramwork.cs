using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using ccVerifySDK;




/// <summary>
/// UI组件
/// 所有UI的基类
/// </summary>
public class UIFramwork : ccUIBase
{
    /*
禁止使用，如果需要使用重载f_CustomAwake
void Awake()
{

}
*/

    /*
    protected override void  f_CustomAwake()
    {

    }
    */

    /// <summary>
    /// 定义自己需要处理的消息
    /// UI消息定义放在UIMessageDef中
    /// UI初始时自动调用
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
    }
        
    /// <summary>
    /// UI初始化工作，手工调用
    /// </summary>
    protected override void InitGUI()
    {

    }


    /// <summary>
    /// UI更新工作，手工调用
    /// </summary>
    protected override void UpdateGUI()
    {
        //throw new NotImplementedException();
    }

    // Use this for initialization
    //void Start()
    //

    //}

    // 不建议使用
    //void Update()
    //{

    //}

    public void f_RegClickEvent(string strName, UnityAction tccCallback)
    {
        GameObject tObj = f_GetObject(strName);
        if (tObj == null)
        {
            //MessageBox.ASSERT("没有找到对应的物件，注册失败 " + strName);
        }
        Button tButton = tObj.GetComponent<Button>();
        tButton.onClick.AddListener(tccCallback);
    }

    #region 红点提示，自动调用
    ///// <summary>
    ///// 初始化红点
    ///// </summary>
    //protected virtual void InitRaddot()
    //{

    //}

    ///// <summary>
    ///// 刷新红点提示UI 
    ///// UI_OPEN和UI_UNHOLD时会自动调用刷新方法
    ///// </summary>
    //protected virtual void UpdateReddotUI()
    //{

    //}

    #endregion


}

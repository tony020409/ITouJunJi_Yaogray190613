using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class CharacterInfo : MonoBehaviour
{
    //獲取自己用
    BaseRoleControllV2 _BaseRoleControl;

    [Rename("血量")] public int HP;
    [Rename("陣營")] public string TeamType;
    [Rename("類型")] public string RoleType;
    [Rename("生命")] public int Life;
    [Line()]
    [Rename("條件AI列表")] public string AIList;
    //[Rename("RunAI列表")]  public string AIListRun;
    [Rename("當前AI狀態")] public AI_EM.EM_AIState CurRunAI;
    [Line()]
    [Rename("是否顯示視野範圍")] public bool showViewSize = false;
    [Rename("是否顯示攻擊範圍")] public bool showAttackSize = true;
    [Rename("是否顯示身體佔位")] public bool showBodySize = true;


    //獲取不會更動的參數 ===============================================================
    void Start(){
        if (glo_Main.GetInstance().m_EM_GameStatic != EM_GameStatic.Gaming) {
            return;
        }
        _BaseRoleControl = this.GetComponentInParent<BaseRoleControllV2>(); //獲取自己
        TeamType = _BaseRoleControl.f_GetTeamType().ToString();             //獲取隊伍資訊
        RoleType = _BaseRoleControl.f_GetRoleType().ToString();             //獲取隊伍資訊
        AIList    = _BaseRoleControl.f_GetAI();                             //獲取AI列表
    }

    //獲取會變動的參數 =================================================================
    void Update(){
        if (glo_Main.GetInstance().m_EM_GameStatic != EM_GameStatic.Gaming) {
            return;
        }
        if (_BaseRoleControl == null) {
            return;
        }
        HP = _BaseRoleControl.f_GetHp();           //獲取血量資訊
        CurRunAI = _BaseRoleControl.GetCurRunAI(); //獲取當前RunAI
        Life = _BaseRoleControl.f_GetHaveLife();   //獲取當前生命數
    }


    //顯示視野、攻擊範圍、身體佔位 =====================================================
    private void OnDrawGizmos(){
        if (_BaseRoleControl == null) {
            return;
        }
        #if UNITY_EDITOR
        Gizmos2DLine();
        #endif
    }


    /// <summary>
    /// 以球體或方形模型顯示範圍
    /// </summary>
    void Gizmos3D() {
        //顯示[視野]範圍
        if (showViewSize) {
            Gizmos.color = new Color(1.0f, 2.0f, 1.0f, 0.2f);  //淡綠色
            //Gizmos.DrawSphere(transform.position, _BaseRoleControl.f_GetViewSize() * _BaseRoleControl.m_viewZoom);
        }

        //顯示[攻擊]範圍
        if (showAttackSize)  {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);  //紅色
            Gizmos.DrawCube(transform.position, Vector3.one * _BaseRoleControl.f_GetAttackSize());
            Gizmos.DrawWireCube(transform.position, Vector3.one * _BaseRoleControl.f_GetAttackSize());
        }

        //顯示[身體佔位]
        if (showBodySize) {
            Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.7f);  //藍色
            Gizmos.DrawCube(transform.position, Vector3.one * _BaseRoleControl.f_BodySize());
        }
    }

    #if UNITY_EDITOR
    /// <summary>
    /// 以圓形平面顯示範圍
    /// </summary>
    //void Gizmos2D() {
    //    //顯示[視野]範圍
    //    if (showViewSize) {
    //        Handles.color = new Color(0.07f, 1.0f, 1.0f, 0.2f); //綠色
    //        Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f,0), Vector3.up, Vector3.forward, 360, _BaseRoleControl.f_GetViewSize() * _BaseRoleControl.m_viewZoom);
    //    }
    //
    //    //顯示[攻擊]範圍
    //    if (showAttackSize){
    //        Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);  //紅色
    //        Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, _BaseRoleControl.f_GetAttackSize());
    //    }
    //
    //    //顯示[身體佔位]
    //    if (showBodySize){
    //        Handles.color = new Color(0.0f, 0.0f, 1.0f, 0.7f);  //藍色
    //        Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, _BaseRoleControl.f_BodySize());
    //    }
    //}


    /// <summary>
    /// 以圓形的線顯示範圍
    /// </summary>
    void Gizmos2DLine(){
        //顯示[視野]範圍
        if (showViewSize) {
            Handles.color = Color.green;
            //Handles.DrawWireArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, _BaseRoleControl.f_GetViewSize() * _BaseRoleControl.m_viewZoom);
            
        }

        //顯示[攻擊]範圍
        if (showAttackSize) {
            Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);  //紅色
            Handles.DrawWireArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, _BaseRoleControl.f_GetAttackSize());
        }

        //顯示[身體佔位]
        if (showBodySize) {
            Handles.color = new Color(0.0f, 0.0f, 1.0f, 0.7f);  //藍色
            Handles.DrawWireArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, _BaseRoleControl.f_BodySize());
        }
    }
    #endif
}



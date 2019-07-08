using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class _33_showEnemyHP : MonoBehaviour 
{
	public Image iHP;     //血條圖片
    private float maxHP;    //目標滿血量
    public  float targetHP; //目標當前血量
    BaseRoleControllV2 _BaseRoleControl; //獲取自己用

    [Header("單純拿來看的參數")]
    public string TeamType;   //隊伍
    //public string HP_Percent; //血量百分比


    //初始化 ============================================================
    void Start(){
        _BaseRoleControl = this.GetComponentInParent<BaseRoleControllV2>(); //獲取自己
        maxHP = _BaseRoleControl.f_GetHp();                              //獲取滿血量

        //單純拿來看的參數1 --------------------------------------
        TeamType = _BaseRoleControl.f_GetTeamType().ToString();
    }

    //監控 ==============================================================
    void Update () {
        //血條控制 ------------------------------------------------
	    iHP = iHP.GetComponent<Image>();
        targetHP = _BaseRoleControl.f_GetHp();
	    iHP.fillAmount = ((targetHP * 100) / maxHP) * 0.01f;

        //單純拿來看的參數2 ---------------------------------------
        //HP_Percent = ((targetHP * 100) / maxHP).ToString() + "% (fillAmount: " + iHP.fillAmount +")";
    }
}

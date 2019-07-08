using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 殺怪測試
/// </summary>
public class Test_KillRole : MonoBehaviour {

    [Header("【怪物1】")]
    [Rename(" - ID")]     public int roleId_A = 0;
    [Rename(" - 得到多少傷害")] public int HP_A = 10;

    [Header("【怪物2】")]
    [Rename(" - ID")]     public int roleId_B = 1;
    [Rename(" - 得到多少傷害")] public int HP_B = 10;

	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.O) && StaticValue.m_bIsMaster) {
            Attack(roleId_A, HP_A);
        }

        if (Input.GetKeyDown(KeyCode.P) && StaticValue.m_bIsMaster){
            Attack(roleId_B, HP_B);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpId"></param>
    /// <param name="tDamage"></param>
    void Attack(int tmpId, int tDamage) {
        BaseRoleControllV2 tBaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(tmpId);
        if (tBaseRoleControl != null) {
            RoleHpAction tRoleHpAction = new RoleHpAction();
            tRoleHpAction.f_Hp(0, tBaseRoleControl.m_iId, tDamage, Vector3.zero);
            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleHpAction);
            Debug.LogWarning("代號(" + tmpId + ")受到 " + tDamage + "單位的傷害! 剩餘血量為:" + tBaseRoleControl.f_GetHp());
        }
        else {
            Debug.LogWarning("代號(" + tmpId + ")不存在!");
        } 
    }



}

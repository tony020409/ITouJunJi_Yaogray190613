using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllTools
{

    /// <summary>
    /// 檢查事件類型
    /// </summary>
    /// <param name="tEM_GameControllAction"> 事件id </param>
    public static bool f_CheckStateTypeIsRight(EM_GameControllAction tEM_GameControllAction)
    {
        if (tEM_GameControllAction == EM_GameControllAction.RoleCreate ||    //  (1) 命名...並生成對應id的角色於指定位置
            tEM_GameControllAction == EM_GameControllAction.RoleMove ||      //  (2) 移動...到某個位置
            tEM_GameControllAction == EM_GameControllAction.RoleDie ||       //  (3) 如果...死掉了
            //tEM_GameControllAction == EM_GameControllAction.RoleHp ||        //  (4) 如果...的血量低於某個數值
            tEM_GameControllAction == EM_GameControllAction.PlayMv ||        //  (5) 命名...並生成指定路徑上的Timeline物件
            tEM_GameControllAction == EM_GameControllAction.ChangeWeapon ||  //  (6) 切換...隊伍的槍枝至指定的種類
            tEM_GameControllAction == EM_GameControllAction.RoleIdel ||      //  (7) 把...的AI切換到Idle狀態
            tEM_GameControllAction == EM_GameControllAction.RoleMove2 ||     // (10) 宥翔版移動

            tEM_GameControllAction == EM_GameControllAction.RoleAnimator ||     // (20) 播放...身上Animator的某個動畫(暴龍用，動畫結束才執行下一行)
            tEM_GameControllAction == EM_GameControllAction.SetActive ||        // (21) 開關物件
            tEM_GameControllAction == EM_GameControllAction.ChangeAIState ||    // (22) 切換...的AI至指定狀態
            tEM_GameControllAction == EM_GameControllAction.RoleChangeHP ||     // (23) 給予角色血量變化
            tEM_GameControllAction == EM_GameControllAction.setInv ||           // (24) 設定角色是否為無敵
            tEM_GameControllAction == EM_GameControllAction.RoleAnimatorJust || // (25) 播放...身上Animator的某個動畫(純播動畫，不會去判斷動畫結束了沒)
            tEM_GameControllAction == EM_GameControllAction.MoveAndAnim ||      // (26) 移動到某點後做某動作
            tEM_GameControllAction == EM_GameControllAction.PlaySound ||        // (27) 播放聲音
            tEM_GameControllAction == EM_GameControllAction.RoleJump ||         // (28) 跳躍
            tEM_GameControllAction == EM_GameControllAction.RolePath ||         // (29) 走路徑

            tEM_GameControllAction == EM_GameControllAction.ShowUIText ||        // (30) 显示UI文字
            tEM_GameControllAction == EM_GameControllAction.UIActionShow ||      // (??) ??
            tEM_GameControllAction == EM_GameControllAction.SwitchingTaskAims || // (??) ??
            tEM_GameControllAction == EM_GameControllAction.SmallPterMove   ||   // (??) ??
            tEM_GameControllAction == EM_GameControllAction.BigPterMove ||       // (??) ??

            tEM_GameControllAction == EM_GameControllAction.RoleGrab ||                // (41) 抓玩家
            tEM_GameControllAction == EM_GameControllAction.Role_Position_Rotation ||  // (42) 瞬間修改位置或朝向

            tEM_GameControllAction == EM_GameControllAction.AutoClock ||


            tEM_GameControllAction == EM_GameControllAction.V3_RoleCreateForGameObj || // (2001) 根据场景里的GameObject位置创建角色
            //tEM_GameControllAction == EM_GameControllAction.V3_RoleRangCheck ||      // (2002) 替 Object 添加偵測範圍，敵方陣營靠近後執行腳本下一行
            tEM_GameControllAction == EM_GameControllAction.V3_GameObjectSetAction ||  // (2003) 場景上的 GameObj下的子物件開與關
            tEM_GameControllAction == EM_GameControllAction.V3_GameObjectDestory ||    // (2004) 銷毀場景上的物件
            tEM_GameControllAction == EM_GameControllAction.V3_GameObjectCreate ||     // (2005) 創建 Resource/BattleGameObject 裡的物件

            tEM_GameControllAction == EM_GameControllAction.V3_DoorControl ||          // (2025) 控制鬼屋門開關動畫
            tEM_GameControllAction == EM_GameControllAction.V3_SetParament ||          // (3001) 设置变量的值（参数1为变量名, 参数2为变量值，参数3无效）
            tEM_GameControllAction == EM_GameControllAction.V3_RoleAnimator ||         // (3002) 设置角色動畫
            tEM_GameControllAction == EM_GameControllAction.V3_SectorClear ||          // (3003) 清除某物件附近的所有怪物
            tEM_GameControllAction == EM_GameControllAction.V3_RoleTransfor2Obj ||     // (4000) 将指定KeyId的角色传送到场景里的GameObject的位置（参数1为角色分配的指定KeyId, 参数2为场景里的GameObject的名字）
            tEM_GameControllAction == EM_GameControllAction.V3_FadeScreen ||           // (4001) 畫面單純淡入淡出
            tEM_GameControllAction == EM_GameControllAction.V3_Elevator ||             // (4002) 電梯
            tEM_GameControllAction == EM_GameControllAction.V3_SetParent               // (4003) 變更父物件
            )
        {
            return true;
        }
        return false;
    }


    //↓這裡也要填什麼Action對應什麼類型
    public static GameControllBaseState f_CreateState(EM_GameControllAction tEM_GameControllAction)
    {
        if (tEM_GameControllAction == EM_GameControllAction.End)
        {
            return new GameControllEnd();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.RoleCreate)
        {
            return new GameControllRoleCreate();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.RoleMove)
        {
            return new GameControllRoleMove();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.RoleDie)
        {
            return new GameControllRoleDie();
        }
        //else if (tEM_GameControllAction == EM_GameControllAction.RoleHp)
        //{
        //    return new GameControllRoleHp();
        //}
        else if (tEM_GameControllAction == EM_GameControllAction.PlayMv)
        {
            return new GameControllPlayMv();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.ChangeWeapon)
        {
            return new GameControllChangeGun();
        }
        //else if (tEM_GameControllAction == EM_GameControllAction.RoleIdel)
        //{
        //    return new GameControllRoleIdel();
        //}
        else if (tEM_GameControllAction == EM_GameControllAction.RoleMove2)
        {
            return new GameControllRoleMove2();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.RoleAnimator)
        {
            return new GameControllPlayAnim();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.SetActive)
        {
            return new GameControllSetActive();
        }       
        else if (tEM_GameControllAction == EM_GameControllAction.setInv)
        {
            return new GameControllSetInv();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.RoleAnimatorJust)
        {
            return new GameControllPlayAnimJust();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.MoveAndAnim)
        {
            return new GameControllRoleMoveAndAnim();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.PlaySound)
        {
            return new GameControllPlaySound();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.RoleJump)
        {
            return new GameControl_RoleJump();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.SwitchingTaskAims)
        {
            return new GameControllSwitchingTaskAims();
        }       
        else if (tEM_GameControllAction == EM_GameControllAction.ShowUIText)
        {
            return new GameControllShowUIText();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.UIActionShow)
        {
            return new GameControllUIActionShow();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.V3_RoleCreateForGameObj)
        {
            return new GameControllV3_RoleCreateForGameObj();
        }
        //else if (tEM_GameControllAction == EM_GameControllAction.V3_RoleRangCheck)
        //{
        //    return new GameControllV3_RoleRangCheck();
        //}
        else if (tEM_GameControllAction == EM_GameControllAction.V3_GameObjectSetAction)
        {
            return new GameControllV3_GameObjectSetAction();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.V3_GameObjectDestory)
        {
            return new GameControllV3_GameObjDestory();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.V3_GameObjectCreate)
        {
            return new GameControllV3_GameObjectCreate();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.ServerAction)
        {
            return new GameControllServerAction();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.V3_RoleTransfor2Obj)
        {
            return new GameControllV3_RoleTransform2Obj();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.V3_Elevator)
        {
            return new GameControllV3_Elevator();
        }
        else if (tEM_GameControllAction == EM_GameControllAction.V3_SetParament) {
            return new GameControllV3_SetParent();
        }
        else {
            MessageBox.ASSERT("未注册的状态机 " + tEM_GameControllAction.ToString());
        }

        return null;
    }





}
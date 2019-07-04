using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleTools
{
    /// <summary>
    /// 保存玩家自己的身高
    /// </summary>
    public static void f_SaveMyselfHeight(float fHeight)
    {
        GloData.glo_fMyselfHeight = fHeight;
    }

    
    /// <summary>
    /// 网络调用创建角色接口
    /// </summary>
    /// <param name="iId"></param>
    /// <param name="tTeamType"></param>
    /// <param name="tCharacterDT"></param>
    /// <param name="tTileNode"></param>
    public static BaseRoleControllV2 f_CreateRoleForNetWork(int iId, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight,  Vector3 tSetPos = default(Vector3))
    {
        GameObject oRole = glo_Main.GetInstance().m_ResourceManager.f_CreateRole2(tCharacterDT);
        if (oRole == null) {
            return null;
        }
        oRole.transform.parent = BattleMain.GetInstance().m_oRole.transform;
        oRole.transform.position = tSetPos;
        return f_CreateRoleControll(oRole, iId, tTeamType, tCharacterDT, tTileNode, fHeight);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="oRole"       ></param>
    /// <param name="iId"         ></param>
    /// <param name="tTeamType"   ></param>
    /// <param name="tCharacterDT"></param>
    /// <param name="tTileNode"   ></param>
    public static BaseRoleControllV2 f_CreateRoleControll(GameObject oRole, int iId, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight)
    {
        BaseRoleControllV2 tRoleControl = null;

        
        //--------------------------------------------------------------------- 鬼屋的門
        if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.Door){
            tRoleControl = oRole.GetComponent<DoorRoleControl>();
            if (tRoleControl == null){
                tRoleControl = oRole.AddComponent<DoorRoleControl>();
            }
            tRoleControl.f_Init(iId, new DoorActionController (tRoleControl), tTeamType, tCharacterDT, tTileNode, fHeight);
        }


        //--------------------------------------------------------------------- 空物件(偵測點)
        else if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.CheckObj)  {
            tRoleControl = oRole.GetComponent<NullObjRoleControl>();
            if (tRoleControl == null) {
                tRoleControl = oRole.AddComponent<NullObjRoleControl>();
            }
            tRoleControl.f_Init(iId, new NullObjActionController(tRoleControl), tTeamType, tCharacterDT, tTileNode, fHeight);
        }


        //--------------------------------------------------------------------- 殭屍
        else if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.Zombie) {
            tRoleControl = oRole.GetComponent<ZombieRoleControl>();
            if (tRoleControl == null)  {
                tRoleControl = oRole.AddComponent<ZombieRoleControl>();
            }
            tRoleControl.f_Init(iId, new ZombieActionController(tRoleControl), tTeamType, tCharacterDT, tTileNode, fHeight);
        }


        //--------------------------------------------------------------------- 通用型遠攻
        else if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.Archer){
            tRoleControl = oRole.GetComponent<ArcherRoleControl>();
            if (tRoleControl == null) {
                tRoleControl = oRole.AddComponent<ArcherRoleControl>();
            }
            tRoleControl.f_Init(iId, new ArcherActionController(tRoleControl), tTeamType, tCharacterDT, tTileNode, fHeight);
        }

        //--------------------------------------------------------------------- 通用型拾取品
        else if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.Pickable) {
            tRoleControl = oRole.GetComponent<PickableRoleControl>();
            if (tRoleControl == null) {
                tRoleControl = oRole.AddComponent<PickableRoleControl>();
            }
            tRoleControl.f_Init(iId, new PickableActionController(tRoleControl), tTeamType, tCharacterDT, tTileNode, fHeight);
        }

        //---------------------------------------------------------------------被打死時會給周圍傷害的物件
        else if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.Burst){
            tRoleControl = oRole.GetComponent<BurstRoleControl>();
            if (tRoleControl == null) {
                tRoleControl = oRole.AddComponent<BurstRoleControl>();
            }
            tRoleControl.f_Init(iId, new BurstActionController(tRoleControl), tTeamType, tCharacterDT, tTileNode, fHeight);
        }

        //--------------------------------------------------------------------- 士兵
        else if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.Solider) {
            tRoleControl = oRole.GetComponent<SoliderRoleControl>();
            if (tRoleControl == null) {
                tRoleControl = oRole.AddComponent<SoliderRoleControl>();
            }
            tRoleControl.f_Init(iId, new SoliderActionController(tRoleControl), tTeamType, tCharacterDT, tTileNode, fHeight);
        }

        //--------------------------------------------------------------------- 哥布林
        else if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.Goblin){
            tRoleControl = oRole.GetComponent<GoblinRoleControl>();
            if (tRoleControl == null) {
                tRoleControl = oRole.AddComponent<GoblinRoleControl>();
            }
            tRoleControl.f_Init(iId, new GoblinActionController(tRoleControl), tTeamType, tCharacterDT, tTileNode, fHeight);
        }


        //---------------------------------------------------------------------無人機
        else if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.Drone) {
            tRoleControl = oRole.GetComponent<DroneRoleControl>();
            if (tRoleControl == null) {
                tRoleControl = oRole.AddComponent<DroneRoleControl>();
            }
            tRoleControl.f_Init(iId, new DroneActionController(tRoleControl), tTeamType, tCharacterDT, tTileNode, fHeight);
        }


        //--------------------------------------------------------------------- 無
        else   {
            MessageBox.ASSERT("无此模型资源 " + tCharacterDT.iId + "-" + tCharacterDT.szName + "-" + tCharacterDT.szResName);
        }


        ////--------------------------------------------------------------------- 一出生就直接播放動畫的物件
        //else if ((GameEM.emRoleType)tCharacterDT.iType == GameEM.emRoleType.AnimationObj) {
        //    tRoleControl = oRole.GetComponent<AniObjRoleControl>();
        //    if (tRoleControl == null) {
        //        tRoleControl = oRole.AddComponent<AniObjRoleControl>();
        //    }
        //    tRoleControl.f_Init(iId, new BaseAIControl(tRoleControl), new Old3V3CharActionController(tRoleControl), tTeamType, tCharacterDT, tTileNode);
        //}


        //---------------------------------------------------------------------
        if (tRoleControl == null) {
            MessageBox.ASSERT("获取角色RoleControll失败 " + tCharacterDT.iId);
        }
        BattleMain.GetInstance().f_SaveRole(tRoleControl);
        return tRoleControl;
    }

    /// <summary>
    /// AI的状态枚举转换成Action状态枚举
    /// </summary>
    /// <param name="tEM_AIState"></param>
    /// <returns></returns>
    public static GameEM.EM_RoleAction f_GetAIState2RoleAction(AI_EM.EM_AIState tEM_AIState)
    {
        if (tEM_AIState == AI_EM.EM_AIState.Walk)
        {
            return GameEM.EM_RoleAction.Walk;
        }
        else
        {
            MessageBox.ASSERT("f_GetAIState2RoleAction未找到AI对就Action " + tEM_AIState.ToString());
        }
        return GameEM.EM_RoleAction.Default;
    }

}
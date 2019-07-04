using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏任务主控制器
/// </summary>
public class GameControll
{

    public bool _bIsRuning = false;
    private int _iConditionId;
    private ccMachineManager _GameControllMachineManager = null;
    private GameControllMunClock _GameControllMunClock = null;

    private float tTimeOut = 0;
    GameControllDT tGameControllDT = null;


    public GameControll(int iConditionId)
    {
        _iConditionId = iConditionId;
        _GameControllMunClock = new GameControllMunClock(); //Loop
        _GameControllMachineManager = new ccMachineManager(new GameControllLoop()); //Loop


        // =======================【 除了這邊之外，要到「GameControllTools.cs」填寫對應的類型！！ 】==========================
        _GameControllMachineManager.f_RegState(new GameControllRead(_iConditionId)); //讀取任務
        RegState(new GameControllEnd());          //結束任務
        RegState(new GameControllServerAction()); //同步任務
        //--------------------------------------------------------------------------------------
        RegState(new GameControllRoleCreate());   // 1. 創造角色
        RegState(new GameControllRoleMove());     // 2. 移動
        RegState(new GameControllRoleDie());      // 3. 死亡時事件
        //RegState(new GameControllRoleHp());     // 4. 血量低於xx時事件
        RegState(new GameControllPlayMv());       // 5. 播放Timeline動畫
        RegState(new GameControllChangeGun());    // 6. 換槍
        //--------------------------------------------------------------------------------------
        RegState(new GameControllRoleMove2());    //10. 宥翔版移動
        RegState(new GameControllPlayAnim());     //20. 播放Animator動畫(暴龍用，要暴龍動畫結束後，才執行下一行指令)
        RegState(new GameControllSetActive());    //21. 開關物件
        RegState(new GameControlRoleChangeHP());  //23. 給予角色血量變化
        RegState(new GameControllSetInv());       //24. 設定角色是否為無敵 (限定暴龍)
        RegState(new GameControllPlayAnimJust()); //25. 播放Animator動畫(純粹播動畫，不會去判斷動畫是否結束)
        RegState(new GameControllRoleMoveAndAnim());    //26. 移動到某點後做某動作，做完再移動到某點
        RegState(new GameControllPlaySound());          //27. 播放聲音
        RegState(new GameControl_RoleJump());           //28. 跳躍
        RegState(new GameControl_RolePath());           //29. 走路徑
        //--------------------------------------------------------------------------------------
        RegState(new GameControllShowUIText());         //30. ??
        RegState(new GameControllUIActionShow());       //31. ??
        RegState(new GameControllSwitchingTaskAims());  //32.任務目標
        //--------------------------------------------------------------------------------------
        RegState(new GameControl_RoleGrab());                  //41.抓玩家
        RegState(new GameControll_Role_Position_Rotation());   //41.抓玩家
        //--------------------------------------------------------------------------------------  
        //V3
        RegState(new GameControllV3_RoleCreateForGameObj()); //2001. 以物件位置生怪
        RegState(new GameControllV3_GameObjectSetAction());  //2003. 場景物件的顯示
        RegState(new GameControllV3_GameObjDestory());       //2004. 銷毀場景上的物件
        RegState(new GameControllV3_GameObjectCreate());     //2005. 創建非角色表上的物件

        RegState(new GameControlV3_DoorAnimationControl());  //2025. 控制鬼屋開關門的動畫
        RegState(new GameControllV3_SetParament());          //3001. 设置变量的值（参数1为变量名, 参数2为变量值，参数3无效）
        RegState(new GameControllV3_RoleAnimator());         //3002. 设置角色動畫
        RegState(new GameControlV3_SectorClear());           //3003. 清除某物件指定範圍內，指定隊伍的所有怪物 (參數4可設定忽略特定類型的怪)  (未測試)
        RegState(new GameControllV3_RoleTransform2Obj());    //4000. 移動指定腳色到某物件的位置 (指定腳色、指定位置)
        RegState(new GameControllV3_FadeScreen());           //4001. 畫面單純淡入淡出
        RegState(new GameControllV3_Elevator());             //4002. 移動指定腳色到某物件的位置 (玩家0、1，連同房間整個移動)
        RegState(new GameControllV3_SetParent());            //4003. 改父物件
        //--------------------------------------------------------------------------------------

        //特殊处理定时器动作
        _GameControllMachineManager.f_RegState(new GameControllClock(_GameControllMunClock));
        _GameControllMachineManager.f_ChangeState((int)EM_GameControllAction.Loop);
    }



    void RegState(GameControllBaseState tccMachineStateBase) {
        _GameControllMachineManager.f_RegState(tccMachineStateBase);
        //ccMachineStateBase tNewState = tccMachineStateBase.f_Clone();
    }


    public void f_Start(int iActionId = -99) {
        _bIsRuning = true;
        if (StaticValue.m_bIsMaster && iActionId > 0) {
            _GameControllMachineManager.f_ChangeState((int)EM_GameControllAction.Read, iActionId);
        }
        tGameControllDT = (GameControllDT)glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetSC(iActionId);
    }



    public void f_Stop() {
        _bIsRuning = false;
        _GameControllMachineManager.f_ChangeState((int)EM_GameControllAction.Loop);
    }


    public void f_Update()  {
        _GameControllMachineManager.f_Update();
        _GameControllMunClock.f_Update();
    }


    public void f_RunServerActionState(int iId) {
        if (iId > 100000) {            
            iId = iId / 100000;
            GameControllDT tGameControllDT = (GameControllDT)glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetSC(iId);
            tGameControllDT.iNeedEnd = 0;
            tGameControllDT.fEndSleepTime = 0;
            tGameControllDT.iEndAction = 0;
            DoClockState(tGameControllDT);
            MessageBox.DEBUG("Clock执行Action " + iId + " " + tGameControllDT.szName);
        }
        else {
            _GameControllMachineManager.f_ChangeState((int)EM_GameControllAction.ServerAction, iId);
        }
    }

    void DoClockState(object Obj) {
        GameControllDT tGameControllDT = (GameControllDT)Obj;        
        _GameControllMunClock.f_Execute(tGameControllDT);        
    }



    void f_Avoid_Disconnection() {
        if (!_bIsRuning) {
            return;
        }
        if (tGameControllDT == null) {
            return;
        }
        tTimeOut--;

    }




}
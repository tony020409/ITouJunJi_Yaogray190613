using UnityEngine;
using System.Collections;
using ccU3DEngine;

#region Enum

/// <summary>
/// 聲音
/// </summary>
public enum SoundEM
{
    /// <summary>
    /// 闲置
    /// </summary>
    Idle,
    /// <summary>
    /// 击中
    /// </summary>
    hit,

    /// <summary> 被翼手龍击中 </summary>
    hit_fromPter,
    /// <summary> 被迅猛龍击中 </summary>
    hit_fromRaptor,
    
    /// <summary>
    /// 攻击
    /// </summary>
    Attack,
    /// <summary>
    /// 死亡
    /// </summary>
    Death,
    /// <summary>
    /// 没子弹
    /// </summary>
    Nobullet,
    /// <summary>
    /// 换弹夹
    /// </summary>
    Changeclip,
    /// <summary>
    /// 停止射击
    /// </summary>
    Stop,
}

/// <summary>
/// 玩家畫面
/// </summary>
public enum ScreenEM
{
    /// <summary>
    /// 闲置
    /// </summary>
    Idle,
    /// <summary>
    /// 击中
    /// </summary>
    hit,
    /// <summary>
    /// 死亡
    /// </summary>
    Death,

}
public enum GunEM
{
    /// <summary> 步枪 </summary>
    Rifle = 1,

    /// <summary> 激光枪 </summary>
    Lasergun = 2,

    /// <summary> 刺针枪（榴弹炮） </summary>
    Needlegun = 3,

    /// <summary> 炸彈 </summary>
    Bomb = 4,

    /// <summary> 狙擊槍 </summary>
    Sniper = 5,

    /// <summary> 衝鋒槍 </summary>
    Submachine = 6
}
#endregion


public enum EM_GameResult
{
    /// <summary>
    /// 0未影响
    /// </summary>
    Default = 0,

    /// <summary>
    /// 1死亡游戏失败
    /// </summary>
    Lost = 1,

    /// <summary>
    /// 2死亡游戏胜利
    /// </summary>
    Win = 2,

}



/// <summary>
/// 角色动作控制参数
/// </summary>
public enum EM_RoleAnimator
{
    Idel = 10,
    Run = 20,
    Attack = 30,
    BeAttack = 40,
    Die = 50,
    Roar = 60,
    Rotate = 70,
    PlayAnim = 80,
}


public enum EM_GameControllAction
{
    

    //1.创建角色（参数1为角色分配的指定KeyId, 参数2为角色模板Id，参数3为角色出生坐标）
    RoleCreate = 1,

    //2.角色移动（参数1为角色分配的指定KeyId, 参数2为移动的目标坐标，参数3无效）
    RoleMove = 2,

    //3.角色死亡事件（参数1为角色分配的指定KeyId, 参数23无效）
    RoleDie = 3,

    //4.角色血量事件（参数1为角色分配的指定KeyId, 参数2为触发血量，参数3无效）
    RoleHp = 4,

    //5.播放动画（参数1为动画对象名称，参数23无效）
    PlayMv = 5,

    //6.切换玩家武器，未指定队伍时为切换所有玩家
    ChangeWeapon = 6,

    //7.切换AI为Idel （参数1为角色分配的指定KeyId, 参数23无效）
    RoleIdel = 7,

    //8. 切换AI为Attack （参数1为角色分配的指定KeyId, 参数23无效）
    RoleAttack = 8,

    //10. 角色移动-小翼手龍版（参数1为角色分配的指定KeyId, 参数2为移动的目标坐标，参数3无效）
    RoleMove2 = 10,

    //20.切换AI为Animator，並播放指定動畫，動畫結束後才執行下一行 (目前限定只有暴龍能用)（参数1为暴龍被分配的指定KeyId, 参数2为要播放的動畫名稱, 参数3无效）
    RoleAnimator = 20,

    //21. 開關物件 ()
    SetActive = 21,

    //22.切换AI为指定 （参数1为角色分配的指定KeyId, 参数2为要切換的AI, 参数3无效）
    ChangeAIState = 22,

    //23. 給予角色血量變化（参数1为角色分配的指定KeyId, 参数2为要給予的血量變化值，参数3無效）
    RoleChangeHP = 23,

    //24. 設定角色為無敵狀態 (参数1为角色分配的指定KeyId, 参数2为無敵狀態(1=開啟，0=關閉)，参数3無效）
    setInv = 24,

    //25. 單純讓指定角色，播放指定動畫，不會去判斷動畫是否結束 （参数1为角色分配的指定KeyId, 参数2为要播放的動畫名稱, 参数3无效）
    RoleAnimatorJust = 25,

    //26. 角色移動，並且到達定點後會做一個動畫動作（参数1为角色分配的指定KeyId, 参数2为移動點1+到達後的動作, 参数3为移動點2+到達後的動作）
    MoveAndAnim = 26,

    //27. 播放聲音 (参数1为要播放的音檔路徑, 参数2為音檔名稱, 参数3為音量, 参数4為3D音效時的位置)
    PlaySound = 27,

    //28. 跳躍 (參數1为角色分配的指定KeyId, 參數2是目標座標, 參數3是跳多高、跳多久)
    RoleJump = 28,

    //29. 走路徑 (參數1为角色分配的指定KeyId, 參數2是路徑編號或名稱, 參數3是走到底後的行為)
    RolePath = 29,


    /// <summary>
    /// 显示UI文字
    /// </summary>
    ShowUIText = 30,

    /// <summary>
    /// 31.控制相应的UI组件显示 （参数1有效玩家Id -99表示所有玩家同时有效，参数2组件名称，参数3显示时间）
    /// </summary>
    UIActionShow = 31,

    /// <summary>
    /// 32.切換任務目標（参数1接收指定隊伍名稱，参数2目標ID）
    /// </summary>
    SwitchingTaskAims = 32,

    //抓玩家
    RoleGrab = 41,

    //瞬間修改怪物位置或朝向
    Role_Position_Rotation = 42,

    /// <summary>
    /// 1301.小翼龙盘旋指令(参数1为角色分配的指定KeyId,参数2为盘旋路线，参数3无效)
    /// </summary>
    SmallPterMove = 1301,
    /// <summary>
    /// 1302.大翼龙盘旋指令(参数1为角色分配的指定KeyId,参数2为盘旋路线，参数3无效)
    /// </summary>
    BigPterMove = 1302,

    /// <summary>
    /// 一次性定时器事件(等待操作对此指令无效)
    /// 1303.定时器事件(参数1为定时时间到后执行的下一条指令,参数23无效)
    /// </summary>
    AutoClock = 1303,


    //--------------------------------------------------------------------------
    //新增加指令
    //1.创建位置GameObject
    //2.GameObject范围检测
    //3.GameObject Action
    //4.GameObject Destory
    //5. .

    /// <summary>
    /// 创建位置GameObject
    /// </summary>
    V3_RoleCreateForGameObj = 2001,

    /// <summary>
    /// GameObject异步范围检测
    /// </summary>
    V3_RoleRangCheckAsync = 2002,

    /// <summary>
    /// GameObject Action True False
    /// </summary>
    V3_GameObjectSetAction = 2003,

    /// <summary>
    /// GameObject Destory
    /// </summary>
    V3_GameObjectDestory = 2004,

    /// <summary>
    /// GameObject 创建
    /// </summary>
    V3_GameObjectCreate = 2005,

    ///// <summary>
    ///// GameObject同步范围检测
    ///// </summary>
    //V3_RoleRangCheck = 2006,

    /// <summary>
    /// 2007.当有指定阵营的角色，进入指定"怪物"的指定範圍內，触发指令任务（参数1为位置物件名稱, 参数2为检测队伍, 参数3为检测范围, 参数4进入检测范围内角色的数量）
    /// </summary>
    V3_RoleRangCheckAsyncForTeam = 2007,


    /// <summary>
    /// 2008. 当有指定阵营的角色，进入指定"怪物"的指定範圍內，触发指令任务（参数1为位置物件名稱, 参数2为检测队伍, 参数3为检测范围 (方形檢測，長寬以分號區隔), 参数4进入检测范围内角色的数量)
    /// </summary>
    V3_RoleRangCheckAsyncForTeam_verRect = 2008,

    /// <summary>
    /// 2009. 偵測玩家的手 (参数1为發動偵測的物件id,参数2为偵測距離）
    /// </summary>
    V3_CheckHand = 2009,


    /// <summary>
    /// 2010.当有指定阵营的角色，进入指定"物件"的指定範圍內，触发指令任务（参数1为位置物件名稱, 参数2为检测队伍, 参数3为检测范围, 参数4进入检测范围内角色的数量）
    /// </summary>
    V3_ObjectRangCheckForTeam = 2010,


    /// <summary>
    /// 2011. 当有指定阵营的角色，进入指定"物件"的指定範圍內，触发指令任务（参数1为位置物件名稱, 参数2为检测队伍, 参数3为检测范围 (方形檢測，長寬以分號區隔), 参数4进入检测范围内角色的数量)
    /// </summary>
    V3_ObjectRangCheckForTeam_verRect = 2011,


    /// <summary>
    /// (鬼屋) 開關門用
    /// </summary>
    V3_DoorControl = 2025,


    /// <summary>
    /// 3000.系统初始化指令（参数1无效, 参数2无效，参数3无效，参数4无效）
    /// </summary>
    V3_Init = 3000,

    /// <summary>
    /// 3001.设置变量的值（参数1为变量名, 参数2为变量值，参数3无效）
    /// </summary>
    V3_SetParament = 3001,

    /// <summary>
    /// 3002.設置角色動畫
    /// </summary>
    V3_RoleAnimator = 3002,


    /// <summary>
    /// 清除地區的怪
    /// </summary>
    V3_SectorClear = 3003,



    /// <summary>
    /// 将指定KeyId的角色传送到场景里的GameObject的位置（参数1为角色分配的指定KeyId, 参数2为场景里的GameObject的名字）
    /// </summary>
    V3_RoleTransfor2Obj = 4000,

    /// <summary>
    /// 場景單純淡入淡出
    /// </summary>
    V3_FadeScreen = 4001,

    /// <summary>
    /// 電梯位移
    /// </summary>
    V3_Elevator = 4002,

    /// <summary>
    /// 變更父物件
    /// </summary>
    V3_SetParent = 4003,


    //--------------------------------------------------------------------------
    Loop = 200,
    Read,
    ServerAction,
    End = -99,
}

public enum EM_GameStatic
{
    Waiting = 100,
    Gaming,
    Win,
    Lost,
}

/// <summary>
/// 转定义招唤师招唤类型
/// </summary>
public enum MonsterType
{
    FAT_OGRE_PBR = 2,
    Goblin_Suicide = 3,
    Goblin_Arch = 1,
    /// <summary>
    /// 光球炸弹
    /// </summary>
    Prop_Jar_Explosion = 0,
    /// <summary>
    /// 护盾
    /// </summary>
    Shield = 4,
    Goblin_Ranger = 5,
    /// <summary>
    /// 玩家选择弓箭手射出来箭
    /// </summary>
    Archer_Arrow = 6,


}




public class GameEM
{


    /// <summary>
    /// 槍枝類型
    /// </summary>
    public enum GunEM {
        /// <summary>
        /// 步枪
        /// </summary>
        Rifle = 1,
        /// <summary>
        /// 激光枪
        /// </summary>
        Lasergun = 2,
        /// <summary>
        /// 针击枪
        /// </summary>
        Needlegun = 3,
        /// <summary>
        /// 炸彈
        /// </summary>
        Bomb = 4,
        /// <summary>
        /// 狙擊槍
        /// </summary>
        Sniper = 5,

        /// <summary> 
        /// 衝鋒槍 
        /// </summary>
        Submachine = 6
    }


    /// <summary>
    /// 角色類型 
    /// </summary>
    public enum emRoleType {
        Player = 0,       //玩家
        CheckObj = 1,     //空物件 (偵測點)
        AnimationObj = 2, //單純播放動畫的物件 (一出生就直接播放動畫)
        Archer = 3,       //通用型遠攻
        Pickable = 4,     //通用型拾取品
        Burst = 5,        //被打到會爆炸給周邊傷害的物件

        Door = 3001,      // (走動式遊戲用) 門
        Zombie = 3002,    // 殭屍
        Solider = 3003,   // 士兵
        Goblin = 3004,    // 投石怪
        Drone = 3005,     // 無人機
    }


    /// <summary>
    /// 隊伍
    /// </summary>
    public enum TeamType
    {
        Ero = 0,
        A = 10,
        B = 20,
        C = 30,
        D = 40,
        E = 50,
        Computer = 60,
    }



    public enum MonsterAIState
    {
        Idle,
        FindEnemy,
        Attack,
        Die
    }

    public enum GamePlayAudioClipEnum
    {
        城門炸掉,
        戰鬥開始,
        戰鬥開始2,
        勝利,
        出現玩家角色,
    }

    public enum PlayerJob
    {
        Ero = 0,
        Wizard,
        Summoner,
        Archer
    }


    public enum GamePlayerNum
    {
        OnePlayer = 1,
        TwoPlayers = 2,
        ThreePlayers = 3
    }


    public enum GameScene
    {
        Luncher,
        SelectMenu,
        GameMain,
        GameMainV2,
        BattleMain,
    }



    public enum PlayerMode
    {
        Player,
        Observer
    }


    public enum EM_RoleAction
    {
        //[ProtoInclude(102, typeof(RoleAttackAction))] //攻击
        //[ProtoInclude(103, typeof(RoleDieAction))]    //死亡
        //[ProtoInclude(104, typeof(RoleWaitAction))]   //等待
        //[ProtoInclude(105, typeof(RoleWalkAction))]   //行走
        //[ProtoInclude(106, typeof(RoleBirthAction))]  //出生

        /// <summary>
        /// 不存在
        /// </summary>
        None = 0,

        Idle = 101,
        Attack = 102,
        Die = 103,
        Wait = 104,
        Walk = 105,
        Birth = 106,
        HP = 107,
        ArrowAttack = 108,
        PlayerPush = 109,
        RoleMulHpAction = 110,
        PureHp = 111,
        RoleFlyAction = 120,
        ServerActionState = 130,
        Force_ServerActionState = 131,
        SpiralAction = 140,
        Walk2Tartget = 150,
        SetActive = 160,
        RoomAction = 170,
        Path = 180,
        CreateObj = 190,
        SyncPosition = 200,
        Spring = 201,
        BeAttack = 202,
        ChangeAI = 210,       //切換AI
        CreateResource = 240, //創造Resource資源
        Shoot = 250,          //怪物射擊
        WeaponState = 270,    //枪支状态
        Animator = 280,       //Animator相關
        GetPickable = 290,    //撿到東西
        PushBullet = 300,     //換子彈

        ZombieAI2_Idle = 301,       //殭屍待機
        ZombieAI2_Chase = 302,      //殭屍追擊
        ZombieAI2_NearAttack = 303, //殭屍近戰
        ZombieAI2_FarAttack = 304,  //殭屍遠攻

        ChangePoint = 305,
        ThrowAttack = 306,
        ArcherInit = 307,

        AddGunClip = 308,      //加彈夾
        GetRandomRole = 309,   //(遠攻)取得隨機角色目標
        PlayerChangeGun = 310, //玩家換槍
        GameOver = 311,        //遊戲結束 (測試失敗的指令)
        ChangePlayerPos = 312, //更換玩家位置

        Default = 990,

    }


    /// <summary>
    /// Action_Animator 事件的功能項目
    /// </summary>
    public enum EM_Animator {
        Play = 1,
        CrossFade = 2,
        SetTrigger = 3,
        SetBool = 4,
    }


    /// <summary>
    /// 玩家 Action
    /// </summary>
    public enum EM_PlayerAction {
        Transform = 200,
    }



    public enum EM_MissionEndType
    {
        None = 0,

        /// <summary>
        /// 正常结束
        /// </summary>
        MissionRunEnd = 1,

        /// <summary>
        /// 超时结束
        /// </summary>
        MessionTimeOut = 2,

    }


    /// <summary>
    /// 身体部位 (子彈用)
    /// </summary>
    public enum EM_BodyPart
    {
        Body = 0,
        Head = 1,
        Arm = 2,
        Foot = 3,
    }


}

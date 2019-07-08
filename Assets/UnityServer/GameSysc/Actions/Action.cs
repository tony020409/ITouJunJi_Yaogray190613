using ccU3DEngine;
using ProtoBuf;
using System;


namespace GameSysc
{
    
    [ProtoContract]             //[Serializable]
    [ProtoInclude(100, typeof(NoAction))]
    //[ProtoInclude(101, typeof(PlayerAction))]
    [ProtoInclude(102, typeof(RoleAttackAction))]       //馴僻
    [ProtoInclude(103, typeof(RoleDieAction))]       //侚厗
    [ProtoInclude(104, typeof(RoleWaitAction))]       //脹渾
    [ProtoInclude(105, typeof(RoleWalkAction))]       //俴軗
    [ProtoInclude(106, typeof(RoleBirthAction))]       //堤汜
    [ProtoInclude(107, typeof(RoleHpAction))]       //堤汜
    [ProtoInclude(108, typeof(RoleArrowAttackAction))] //玩家攻擊
    [ProtoInclude(109, typeof(PlayerPushAction))]       //堤汜
    [ProtoInclude(110, typeof(RoleMulHpAction))]
    [ProtoInclude(111, typeof(RolePureHpAction))]      //純加扣血
    //[ProtoInclude(120, typeof(RoleFlyAction))]
    [ProtoInclude(130, typeof(ServerActionState))]       //GameControl 腳本同步執行用
    [ProtoInclude(131, typeof(Force_ServerActionState))] //GameControl 腳本同步執行用 (不管條件表，強制執行某行)
    [ProtoInclude(140, typeof(RoleSpiralAction))]
    [ProtoInclude(150, typeof(RoleWalk2TargetAction))]
    [ProtoInclude(160, typeof(SetActiveAction))]       //開關物件(未完成)
    [ProtoInclude(170, typeof(RoomAction))]            //切換房間
    [ProtoInclude(180, typeof(RolePathAction))]        //走路徑 (沒用到)
    [ProtoInclude(190, typeof(CreateHitEffectAction))] //創造子彈擊中的特效物件
    [ProtoInclude(200, typeof(ChangeAIAction))]        //切換AI
    [ProtoInclude(210, typeof(ChangeAnimatorAction))]  //變更Animator中的某個Int參數值
    [ProtoInclude(220, typeof(ChangeParameterAction))] //變更變數
    //[ProtoInclude(230, typeof(RoleBeAttackAction))]  //同步攻擊
    [ProtoInclude(240, typeof(CreateResourceAction))]  //同步創建Resource資料夾下的物件
    [ProtoInclude(250, typeof(Action_Shoot))]          //同步怪物射擊
    [ProtoInclude(260, typeof(Action_Die))]            //同步死亡
    [ProtoInclude(270, typeof(RoleWeaponStateAction))] //Л盓袨怓
    [ProtoInclude(280, typeof(RoleChangePointAction))] //躲避點

    [ProtoInclude(290, typeof(RoleThrowAttackAction))] //投擲
    [ProtoInclude(300, typeof(Action_PlayerShoot))]    //玩家射擊
    [ProtoInclude(310, typeof(RoleArcherInitAction))]  //ArcherRoleControl - 出生時會先走到附近的躲藏點
    [ProtoInclude(320, typeof(Action_Animator))]       //同步動畫
    [ProtoInclude(330, typeof(Action_GetPickable))]    //同步撿東西
    [ProtoInclude(340, typeof(Action_PushBullet))]     //同步某位玩家換子彈
    [ProtoInclude(350, typeof(Action_AddGunClip))]     //同步某位玩家補彈夾
    [ProtoInclude(360, typeof(Action_SetTarget))]      //同步某隻遠攻怪的攻擊目標
    [ProtoInclude(370, typeof(Action_DelayDie))]       //同步某隻怪物延遲死亡
    [ProtoInclude(380, typeof(Action_PlayerChangeGun))]//同步某玩家換槍了
    [ProtoInclude(400, typeof(Action_Path))]           //同步走路徑
    [ProtoInclude(410, typeof(Action_ChangePlayerPos))]//同步走路徑

    public abstract class Action
    {
        public Action()
        {
            m_iId = ccMath.f_CreateKeyId();
            m_iType = 0;
        }
        /// <summary>
        /// 峔珨Id
        /// </summary>
        [ProtoMember(1)]
        public int m_iId;
        /// <summary>
        /// 垀扽俙模Id
        /// </summary>
        [ProtoMember(2)]
        public int m_iUserId;
        /// <summary>
        /// Action濬倰 0=NoAction 1=む坳濬倰
        /// </summary>
        [ProtoMember(3)]
        public short m_iType;

        [ProtoMember(10)]
        public int m_iRoleId;

        public void f_Save(int iUserId)
        {
            m_iUserId = iUserId;
        }

        ////////////////////////////////////////////////////////////////////
        //眕狟峈赻隅砱杅擂囀��

            

        //[ProtoMember(5)]
        //public int NetworkAverage { get; set; }
        //[ProtoMember(6)]
        //public int RuntimeAverage { get; set; }



        public abstract void ProcessAction();
    }

}
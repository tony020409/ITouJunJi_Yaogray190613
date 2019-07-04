using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 角色控制器基类
/// </summary>
public class BaseRoleControllV2 : MonoBehaviour, IPlayerMono
{
    private GunControll _GunControll = null;

    /// <summary>
    /// 是否显示测试用的Tile信息
    /// </summary>
    public bool m_ShowTile = false;

    [HideInInspector]
    public float fHeight;

    private Transform _RoleModel;
    public Transform m_RoleModel
    {
        get
        {
            return _RoleModel;
        }
    }

    /// <summary>
    /// 当前所在位置
    /// </summary>
    private TileNode m_StayTile = null;

    /// <summary>
    /// AI控制器
    /// </summary>
    BaseAIControllV2 _BaseAI = null;
    BaseActionController _BaseActionController;
    public BaseRoleProtecy _BaseRoleProtecy;

    protected bool _bInitOK = false;

    /// <summary>
    /// 身體被攻擊的碰撞體
    /// </summary>
    private Collider[] _Colliders;


    public int m_iId
    {
        get
        {
            return _BaseRoleProtecy.m_iId;
        }
    }



    void Awake()
    {
        _RoleModel = GetRoleModel();
        //_RoleRangCheck = new RoleRangCheck(this);
    }

    protected virtual Transform GetRoleModel()
    {
        return transform.Find("RoleModel");
    }

    public void f_Update(int iDeltaTime)
    {
        if (!_bInitOK)
        {
            return;
        }
        _BaseAI.f_Update();
    }

    #region 初始相关
    /// <summary>
    /// 初始控制器信息
    /// </summary>
    /// <param name="iId"></param>
    /// <param name="tBasePlayerControll"></param>
    /// <param name="tCharacterDT"></param>
    /// <param name="tTileNode"></param>
    /// <param name="bUpdatePos"></param>
    public virtual void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true)
    {
        _BaseRoleProtecy = new BaseRoleProtecy(iId, tCharacterDT, tTeamType, fHeight);
        gameObject.name = _BaseRoleProtecy.m_iId.ToString();
        _BaseAI = new BaseAIControllV2(this);
        _BaseActionController = tBaseActionController;
        m_bIsComplete = false;
        f_FullHP();  //先補血
        m_StayTile = null;
        f_SetPos(tTileNode, bUpdatePos);
        glo_Main.GetInstance().m_GameSyscManager.f_AddSyscObject(this);
        //initGun();

        f_SetHeight();
        _bInitOK = true;
    }

    private void initGun()
    {
        //if (string.IsNullOrEmpty(_BaseRoleProtecy.f_GetCharacterDT().szGun))
        //{
        //    return;
        //}               
        //_GunControll = new GunControll(this, _BaseRoleProtecy.f_GetCharacterDT().szGun);

    }

    #endregion
    


    #region 动作相关

    /// <summary>
    /// 操作相应动作
    /// </summary>
    /// <param name="tAIState"></param>
    public void f_PlayAction(AI_EM.EM_AIState tAIState)
    {
        _BaseActionController.f_PlayAction(tAIState);
    }
    #endregion


    #region 開槍相關
    public virtual void f_AutoGun(GunEM tGunType)
    {

    }

    public virtual void f_StopGun(GunEM tGunType)
    {

    }
    #endregion


    #region 死亡相关
    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void f_Die()
    {
        ////直接本地死亡
        ////f_ChangeAI2Die();
        Data_Pool.m_PlayerPool.f_Die(m_iId);
        //_bInitOK = false;
        _BaseRoleProtecy.f_LostLife(1);
       
        if (StaticValue.m_bIsMaster)
        {
            UnSetPos();
            Action_Die tRoleDieAction = new Action_Die();
            tRoleDieAction.f_Die(m_iId, f_GetTeamType());
            f_AddMyAction(tRoleDieAction);
        }
    }

    /// <summary>
    /// 是否死亡了
    /// </summary>
    /// <returns></returns>
    /// -----------------------------------------------------
    public bool f_IsDie()
    {
        //if (!_bInitOK)
        //{
        //    return true;
        //}
        if (_BaseRoleProtecy.f_GetHp() <= 0)
        {
            return true;
        }
       
        return false;
    }

    /// <summary>
    /// 执行角色回收销毁
    /// </summary>
    public void f_Destory()
    {
        if (_BaseAI != null)
        {
            _BaseAI = null;
        }
        if (_BaseActionController != null)
        {
            _BaseActionController = null;
        }

        if (this != null)
        {
            glo_Main.GetInstance().m_ResourceManager.f_DestorySD(gameObject);
        }
    }

    #endregion


    #region 血量
    /// <summary>
    /// 被攻击
    /// </summary>
    /// <param name="iHP">受到的傷害</param>
    /// <param name="iRoleId">攻击发起者</param>
    /// <param name="tBodyPart">被攻击部位</param>
    public virtual void f_BeAttack(int iHP, int iRoleId, GameEM.EM_BodyPart tBodyPart)    
    {
        if (f_IsDie())
        {
            return;
        }
        else if (f_IsInv())
        {
            return;
        }
        LostHP(iHP); 
        
        //处理死亡
        if (f_GetHp() <= 0)
        {
            f_Die();
            DispScore(iRoleId, m_iId, tBodyPart, true);
        }
        else
        {
            DispScore(iRoleId, m_iId, tBodyPart, false);
        }
    }

    /// <summary>
    /// 计算角色攻击得分
    /// </summary>
    /// <param name="iRoleid"></param>
    /// <param name="iBeRoleId"></param>
    /// <param name="tBodyPart"></param>
    /// <param name="bDie"></param>
    protected void DispScore(int iRoleid, int iBeRoleId, GameEM.EM_BodyPart tBodyPart, bool bDie)
    {
        if (StaticValue.m_bIsMaster)
        {
            Data_Pool.m_PlayerPool.f_Hit(iRoleid, tBodyPart, bDie);
        }
    }

    /// 加血
    /// </summary>
    /// <param name="iHP"></param>
    public virtual void f_AddHp(int iHP)
    {
        if (f_IsDie())
        {
            return;
        }
        _BaseRoleProtecy.f_AddHp(iHP);
        UpdateHP();
    }

    /// <summary>
    /// 恢复满血
    /// </summary>
    public void f_FullHP()
    {
        _BaseRoleProtecy.f_FullHP();
        UpdateHP();
    }

    /// <summary>
    /// 掉血
    /// </summary>
    /// <param name="iHP"></param>
    protected void LostHP(int iHP)
    {
        _BaseRoleProtecy.f_LostHP(iHP);
        //更新血条
        UpdateHP();
    }

    /// <summary>
    /// 鎖血
    /// </summary>
    /// <param name="iHP"></param>
    public void f_LockHp(float iHP)
    {
        if (f_IsDie())
        {
            return;
        }
        _BaseRoleProtecy.f_LockHp(iHP);
    }

    /// <summary>
    /// 更新血量显示
    /// </summary>
    protected virtual void UpdateHP()
    {

    }

    /// <summary>
    /// 無視無敵強制扣血 (不計分)
    /// </summary>
    /// <param name="iHP"></param>
    public virtual void f_ForceBeAttack(int iHP) {
        LostHP(iHP);
        //处理死亡
        if (f_GetHp() <= 0){
            f_Die();
        }
    }

    /// <summary>
    /// 獲取當前血量
    /// </summary>
    public int f_GetHp()
    {
        return _BaseRoleProtecy.f_GetHp();
    }

    public int f_GetMaxHp()
    {
        return _BaseRoleProtecy.f_GetMaxHp();
    }

    #endregion


    #region 无敌相关
    private bool _bIsInvincible = false;

    /// <summary>
    /// 是否為無敵
    /// </summary>
    public bool f_IsInv() {       
        return _BaseRoleProtecy.f_IsInv();
    }

    /// <summary>
    /// 設定是否為無敵狀態, 此无敌设置权限比脚本中的无敌权限大
    /// </summary>
    public void f_SetInv(bool bInvincible) {
        _BaseRoleProtecy.f_SetInv(bInvincible);
    }
    #endregion


    #region 生命
    /// <summary>
    /// 查询还有几点生命
    /// </summary>
    public int f_GetHaveLife(){
        return _BaseRoleProtecy.f_GetHaveLife();
    }


    /// <summary>
    /// 重生(?)
    /// </summary>
    public virtual void f_Rebirth(){
        m_bIsComplete = false;
        f_FullHP();
        if ((GameEM.emRoleType)_BaseRoleProtecy.f_GetRoleType() != GameEM.emRoleType.Player){
            glo_Main.GetInstance().m_GameSyscManager.f_AddSyscObject(this);
        }
        _bInitOK = true;
    }


    /// <summary>
    /// 失去生命
    /// </summary>
    public void f_LostLife(int value) {
        _BaseRoleProtecy.f_LostLife(value);
    }

    #endregion


    #region 位置相关

    /// <summary>
    /// 设置所在位置
    /// </summary>
    /// <param name="tTileNode"></param>
    /// <param name="bAutoUpdate"></param>
    public void f_SetPos(TileNode tTileNode, bool bAutoUpdate = true)
    {
        if (m_StayTile == tTileNode)
        {
            return;
        }
        UnSetPos();
        m_StayTile = tTileNode;
        m_StayTile.f_Use(m_iId);

        for (int i = 0; i < _BaseRoleProtecy.f_GetCharacterDT().aPos.Length; i++)
        {
            TileNode tBodyTile = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndexXY(m_StayTile.m_iIndexX + (int)_BaseRoleProtecy.f_GetCharacterDT().aPos[i].x, m_StayTile.m_iIndexY + (int)_BaseRoleProtecy.f_GetCharacterDT().aPos[i].y);
            if (tBodyTile != null)
            {
                tBodyTile.f_Use(m_iId);
            }
        }

        if (bAutoUpdate)
        {
            Vector3 Pos = m_StayTile.transform.position;
            //Pos.y = GloData.glo_fDefaultY;
            gameObject.transform.position = Pos;
        }
        //MessageBox.DEBUG(m_iId + " Stay " + m_StayTile.idx);
    }

    public void UnSetPos()
    {
        if (m_StayTile != null)
        {
            //MessageBox.DEBUG("UUU " + m_StayTile.idx);
            m_StayTile.f_UnUse(m_iId);
            for (int i = 0; i < _BaseRoleProtecy.f_GetCharacterDT().aPos.Length; i++)
            {
                TileNode tBodyTile = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndexXY(m_StayTile.m_iIndexX + (int)_BaseRoleProtecy.f_GetCharacterDT().aPos[i].x, m_StayTile.m_iIndexY + (int)_BaseRoleProtecy.f_GetCharacterDT().aPos[i].y);
                if (tBodyTile != null)
                {
                    tBodyTile.f_UnUse(m_iId);
                }
            }
        }
    }

    /// <summary>
    /// 顯示位置
    /// </summary>
    public void f_UpdateWalkInfor()
    {
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(_RoleModel.transform.position);
        if (tTileNode == null)
        {
            MessageBox.DEBUG("f_UpdateWalkInfor角色坐标超出地图计算 " + gameObject.transform.position);
        }
        else if (tTileNode != f_GetPos())
        {
            f_SetPos(tTileNode, false);
        }
        //MessageBox.DEBUG("UUU " + m_StayTile.idx);
    }

    /// <summary>
    /// 获得当前所在位置
    /// </summary>
    /// <returns></returns>
    public TileNode f_GetPos()
    {
        return m_StayTile;
    }

    public void f_StopWalk()
    {
        ForceUpdateStayPos();
    }

    private void ForceUpdateStayPos()
    {
        if (m_StayTile.f_CheckIsFree(null))
        {
            m_StayTile.f_Use(m_iId);
            for (int i = 0; i < _BaseRoleProtecy.f_GetCharacterDT().aPos.Length; i++)
            {
                TileNode tBodyTile = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndexXY(m_StayTile.m_iIndexX + (int)_BaseRoleProtecy.f_GetCharacterDT().aPos[i].x, m_StayTile.m_iIndexY + (int)_BaseRoleProtecy.f_GetCharacterDT().aPos[i].y);
                if (tBodyTile != null)
                {
                    tBodyTile.f_Use(m_iId);
                }
            }
        }
    }

    public bool f_CheckAIIsWalk()
    {
        return _BaseAI.f_CheckAIIsWalk();
    }
    #endregion


    #region 同步相关

    private bool _bIsComplete;
    public bool m_bIsComplete
    {
        get
        {
            return _bIsComplete;
        }

        set
        {
            _bIsComplete = value;
        }
    }

    public void f_AddMyAction(GameSysc.Action action)
    {
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(action);
    }

    public void f_ProcessAction(BaseActionV2 tAction)
    {
        _BaseAI.f_ProcessAction(tAction);
    }

    #endregion

    #region 属性相关

    public GameEM.TeamType f_GetTeamType()  {
        return _BaseRoleProtecy.f_GetTeamType();
    }

    public GameEM.emRoleType f_GetRoleType() {
        return _BaseRoleProtecy.f_GetRoleType();
    }

    public float f_BodySize() {
        return _BaseRoleProtecy.f_BodySize();
    }

    public float f_GetAttackSize() {
        return _BaseRoleProtecy.f_GetAttackSize();
    }

    public float f_GetViewSize() {
        return _BaseRoleProtecy.f_GetViewSize();
    }

    public float f_GetWalkSpeed() {
        return _BaseRoleProtecy.f_GetWalkSpeed();
    }

    public bool f_CheckIsNoFind() {
        return _BaseRoleProtecy.f_CheckIsNoFind();
    }

    public float f_GetBodySize() {
        return _BaseRoleProtecy.f_GetBodySize();
    }

    public string f_GetAI() {
        return _BaseRoleProtecy.f_GetAI();
    }

    public AI_EM.EM_AIState GetCurRunAI() {
        return _BaseAI.GetCurRunAI();
    }




    /// <summary>
    /// 获得攻击力
    /// </summary>
    public string f_GetReadme(){
        return _BaseRoleProtecy.f_GetReadme();
    }


    /// <summary>
    /// 获得攻击力
    /// </summary>
    public int f_GetAttackPower() {
        return _BaseRoleProtecy.f_GetAttackPower();
    }

    #endregion

    #region 角色弹起来
    protected bool _bIsSpring = false;
    protected bool _bIsCatching = false;
    /// <summary>
    /// 判断角色是否被弹起
    /// </summary>
    /// <returns></returns>
    public bool f_IsSpring()
    {
        return _bIsSpring;
    }
    public bool f_IsCatching()
    {
        return _bIsCatching;
    }
    #endregion

    #region 弹跳相关
    /// <summary>
    /// 玩家受到向上弹起的力
    /// </summary>
    /// <param name="fPower">弹起来花费的时间</param>
    /// <param name="fHeight">被弹起来高度</param>
    public virtual void f_Spring(float fPower, float fHeight)
    {

    }

    /// <summary>
    /// 玩家被抓住
    /// </summary>
    /// <param name="fY"></param>
    public virtual void f_SpringPush(float fY)
    {

    }

    /// <summary>
    /// 玩家回地面
    /// </summary>
    /// <param name="fPower"></param>
    public virtual void f_SpringReset(float fPower)
    {

    }

    #endregion

    #region 攻击相关 
    public virtual void f_CallBack_Attacking(object Obj)
    {
        //MessageBox.DEBUG("CallBack_Attacking " + m_iId);
        //if (f_IsDie())
        //{
        //    return;
        //}
        //_BaseAI.f_Attack();
    }

    public virtual void f_CallBack_AttacComplete(object Obj)
    {
        //MessageBox.DEBUG("CallBack_AttacComplete " + m_iId);
        //if (f_IsDie())
        //{
        //    return;
        //}
        //_BaseAI.f_AttackComplete();
    }
    #endregion

    #region AI动作
    /// <summary>
    /// 强制执行AI动作。
    /// </summary>
    /// <param name="tAIState"></param>
    /// <param name="Obj"></param>
    public void f_RunAIState(AI_EM.EM_AIState tAIState, BaseActionV2 tBaseActionV2){
        _BaseAI.f_RunAIState(tAIState, tBaseActionV2);
    }
      

    public void f_RunStateComplete(){
        _BaseAI.f_RunStateComplete();
    }


    /// <summary>
    /// 註冊AI
    /// </summary>
    /// <param name="newRunAI"></param>
    public void f_RegRunAIState(AI_RunBaseStateV2 newRunAI) {
        //AI_RunBaseStateV2 tmpAI = newRunAI.f_Clone();
        //tmpAI.f_Init(this, null);
        //_BaseAI.f_RegRunAIState(tmpAI);
        _BaseAI.f_RegRunAIState(newRunAI);
    }

    #endregion



    #region 碰撞相关
    /// <summary>
    /// 獲取所有碰撞
    /// </summary>
    public void f_GetBodyCollider() {
        _Colliders = GetComponentsInChildren<Collider>(); //獲取所有碰撞
    }

    /// <summary>
    /// 開啟或關閉身體碰撞
    /// </summary>
    /// <param name="tmp"> true=開 / false=關 </param>
    public void f_SetBodyCollider(bool tmp) {
        for (int i = 0; i < _Colliders.Length; i++) {
            _Colliders[i].enabled = tmp;
        }
    }
    #endregion


    #region 枪相关

    #endregion


    #region 丟東西相关
    public virtual void f_CallBack_ThrowStart(object obj){
        //依狀況在各自RoleControl覆寫修改，然後動畫透過 RoleModelCallbackEvent調用
    }

    public virtual void f_CallBack_ThrowEnd(object obj) {
        //依狀況在各自RoleControl覆寫修改，然後動畫透過 RoleModelCallbackEvent調用
    }
    #endregion

    #region 攻擊語音相關
    public virtual void f_AttactVoice()
    {

    }

    #endregion


    #region 身高相关

    /// <summary>
    /// 设置角色的身高
    /// </summary>
    public virtual void f_SetHeight()
    {
        //获取角色的身体，根据身高进行设置操作
        float fHeight = _BaseRoleProtecy.f_GetHeight();
    }

    #endregion


    
}

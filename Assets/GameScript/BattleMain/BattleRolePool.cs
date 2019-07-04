using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRolePool
{
    
    private List<BaseRoleControllV2> _aList = new List<BaseRoleControllV2>();

    public void f_Save(BaseRoleControllV2 tBaseRoleControllV2)
    {
        BaseRoleControllV2 tCheckRole = f_Get(tBaseRoleControllV2.m_iId);
        if (tCheckRole == null)
        { 
            _aList.Add(tBaseRoleControllV2);
        }
    }

    public void f_Die(BaseRoleControllV2 tBaseRoleControllV2)
    {
        _aList.Remove(tBaseRoleControllV2);
    }

    public void f_Clear()
    {
        _aList.Clear();
    }

    public BaseRoleControllV2 f_Get(int iRoleId)
    {
        return _aList.Find(delegate (BaseRoleControllV2 p) { return p.m_iId == iRoleId; });
    }

    public List<BaseRoleControllV2> f_GetRoleList()
    {
        return _aList;
    }

    public int f_GetRoleCount()
    {
        return _aList.Count;
    }


    /// <summary>
    /// 检测角色是否隐身
    /// </summary>
    /// <param name="tBaseRoleControl"></param>
    private bool CheckIsIgnore(BaseRoleControllV2 tBaseRoleControl) {
        if (tBaseRoleControl.f_CheckIsNoFind()) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测角色是否無敵
    /// </summary>
    /// <param name="tBaseRoleControl"></param>
    private bool CheckIsInv(BaseRoleControllV2 tBaseRoleControl) {
        if (tBaseRoleControl.f_IsInv()) {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 檢查所有玩家是否都死了
    /// </summary>
    public bool f_CheckAllPlayerIsDie() {
        int iPlayerLive = 0;
        for (int i = _aList.Count - 1; i >= 0; i--) {
            if (_aList[i].f_GetRoleType() == GameEM.emRoleType.Player) {
                if (_aList[i].f_GetHaveLife() > 0) {
                    iPlayerLive += _aList[i].f_GetHaveLife();
                }
            }
        }
        if (iPlayerLive == 0) {
            return true;
        }
        return false;
    }


    /// <summary>
    /// [PVP模式] 檢查指定隊伍的玩家是否全死光了 (死光表示輸了)
    /// </summary>
    /// <param name="tmpTeam"> 指定的隊伍 </param>
    public bool f_CheckPVP_TeamLost(GameEM.TeamType tmpTeam)
    {
        //int iPlayerLive = 0;
        for (int i = _aList.Count - 1; i >= 0; i--)
        {
            if (_aList[i].f_GetRoleType() == GameEM.emRoleType.Player)
            {
                if (_aList[i].f_GetTeamType() == tmpTeam)
                {
                    if (_aList[i].f_GetHaveLife() > 0)
                    {
                        return false;
                        //iPlayerLive += _aList[i].f_GetHaveLife();
                    }
                }
            }
        }
        //if (iPlayerLive == 0)
        //{
        //    return true;
        //}
        //return false;
        return true;
    }



    #region 查找相关
    /// <summary> 獲取所有指定 Team的角色 </summary> -----------------------------   
    public List<BaseRoleControllV2> f_FindTeamTarget(GameEM.TeamType tTeamType)
    {
        List<BaseRoleControllV2> aData = new List<BaseRoleControllV2>();
        foreach (BaseRoleControllV2 __RoleControl in _aList)
        {
            if (__RoleControl.f_IsDie())
            {
                continue;
            }
            if (tTeamType != __RoleControl.f_GetTeamType())
            {
                continue;
            }
            if (CheckIsIgnore(__RoleControl))
            {
                continue;
            }
            aData.Add(__RoleControl);
        }
        return aData;
    }


    /// <summary>
    /// 寻找范围里其它阵营的目标对象
    /// </summary>
    /// <param name="tRoleControl"></param>
    /// <param name="fSize"></param>
    /// <returns></returns>
    public List<BaseRoleControllV2> f_FindTargetEnemyAll2(BaseRoleControllV2 tRoleControl, float fSize)
    {
        List<BaseRoleControllV2> aData = new List<BaseRoleControllV2>();
        foreach (BaseRoleControllV2 __RoleControl in _aList)
        {
            if (__RoleControl.f_IsDie())
            {
                continue;
            }
            if (tRoleControl == __RoleControl)
            {
                continue;
            }
            if (tRoleControl.f_GetTeamType() == __RoleControl.f_GetTeamType())
            {
                continue;
            }
            if (CheckIsIgnore(__RoleControl))
            {
                continue;
            }
            float f = Vector3.Distance(__RoleControl.transform.position, tRoleControl.transform.position);
            f = f - tRoleControl.f_BodySize();
            if (f <= fSize)
            {
                aData.Add(__RoleControl);
            }
        }
        return aData;
    }

    /// <summary>
    /// 寻找范围里指定阵营的目标对象
    /// </summary>
    /// <param name="tRoleControl"></param>
    /// <param name="fSize"></param>
    public List<BaseRoleControllV2> f_FindTeamTargetAll(BaseRoleControllV2 tRoleControl, GameEM.TeamType tTeamType, float fSize) {
        List<BaseRoleControllV2> aData = new List<BaseRoleControllV2>();
        foreach (BaseRoleControllV2 __RoleControl in _aList)  {
            if (__RoleControl.f_IsDie()) {
                continue;
            }
            if (tRoleControl == __RoleControl) {
                continue;
            }
            if (CheckIsIgnore(__RoleControl)) {
                continue;
            }
            if (tTeamType != __RoleControl.f_GetTeamType()){
                continue;
            }
            float f = Vector3.Distance(__RoleControl.transform.position, tRoleControl.transform.position);
            f = f - tRoleControl.f_BodySize();
            if (f <= fSize) {
                aData.Add(__RoleControl);
            }
        }
        return aData;
    }
        
    public BaseRoleControllV2 f_FindTargetEnemy2(BaseRoleControllV2 tRoleControl, float fSize)
    {
        float __fDis = 1000;
        BaseRoleControllV2 tTartgetRole = null;
        foreach (BaseRoleControllV2 __RoleControl in _aList)
        {
            if (__RoleControl.f_IsDie())
            {
                continue;
            }
            if (tRoleControl == __RoleControl)
            {
                continue;
            }
            if (tRoleControl.f_GetTeamType() == __RoleControl.f_GetTeamType())
            {
                continue;
            }
            if (CheckIsIgnore(__RoleControl))
            {
                continue;
            }
            float f = Vector3.Distance(__RoleControl.transform.position, tRoleControl.transform.position);
            f = f - tRoleControl.f_BodySize();
            if (f <= fSize)
            {
                if (f < __fDis)
                {
                    __fDis = f;
                    tTartgetRole = __RoleControl;
                }
            }
        }
        return tTartgetRole;
    }

    /// <summary>
    /// 搜尋指定範圍內某隊伍的角色
    /// </summary>
    /// <param name="tTeamType"    > 要搜尋的隊伍 </param>
    /// <param name="Pos"          > 搜尋的中心 </param>
    /// <param name="fSize"        > 搜尋的範圍 </param>
    /// <param name="IincludeDie"  > 是否計算死亡的角色 (預設false，不偵測死亡的角色) </param>
    /// <param name="IncludeIgnore"> 是否計算角色表上被設定成無法偵測的角色 (預設false，保留角色表的設定) </param>
    /// <param name="IgnoreY"      > 是否忽略 Y軸 (預設true) </param>
    public List<BaseRoleControllV2> f_FindTargetFriendAll2(GameEM.TeamType tTeamType, Vector3 Pos, float fSize, bool IncludeDie = false, bool IncludeIgnore = false, bool IgnoreY = true) {
        List<BaseRoleControllV2> aData = new List<BaseRoleControllV2>();
        foreach (BaseRoleControllV2 __RoleControl in _aList)  {
            if (__RoleControl.f_IsDie()){
                if (!IncludeDie) {
                    continue;
                }
            }
            if (tTeamType != __RoleControl.f_GetTeamType()){
                continue;
            }
            if (CheckIsIgnore(__RoleControl)){
                if (!IncludeIgnore) {
                    continue;
                }
            }
            float f = 0;
            if (IgnoreY) {
                f = Vector2.Distance( ccMath.IgnoreYAxis(__RoleControl.transform.position), ccMath.IgnoreYAxis(Pos));
            } else {
                f = Vector3.Distance(__RoleControl.transform.position, Pos);
            }
            if (f <= fSize) {
                aData.Add(__RoleControl);
            }
        }
        return aData;
    }



    /// 搜尋指定矩形範圍內某隊伍的角色
    /// </summary>
    /// <param name="tTeamType"  > 要搜尋的隊伍</param>
    /// <param name="CenterPos"  > 搜尋的中心 </param>
    /// <param name="fSizeF"     > 搜尋的範圍 (矩形長) </param>
    /// <param name="fSizeR"     > 搜尋的範圍 (矩形寬) </param>
    /// <param name="IincludeDie"> 是否包括死亡的角色 (預設false，不回傳死亡的角色)</param>
    /// <returns></returns>
    public List<BaseRoleControllV2> f_FindTeamRoleInRectAll(GameEM.TeamType tTeamType, Transform CenterPos, float fSizeF, float fSizeR, bool IincludeDie = false)  {
        List<BaseRoleControllV2> aData = new List<BaseRoleControllV2>();
        foreach (BaseRoleControllV2 __RoleControl in _aList) {
            if (__RoleControl.f_IsDie()) {
                if (!IincludeDie) {
                    continue;
                }
            }
            if (tTeamType != __RoleControl.f_GetTeamType()) {
                continue;
            }
            if (CheckIsIgnore(__RoleControl)) {
                continue;
            }

            Vector3 deltaA = __RoleControl.transform.position - CenterPos.position; //偵測點到目標的向量
            float forwardDotA = Mathf.Abs(Vector3.Dot(CenterPos.forward, deltaA));  //「攻擊點前方向量」與「攻擊點到目標的向量」的????，其值等於兩點距離，且含有方向資訊(?)
            float rightDotA   = Mathf.Abs(Vector3.Dot(CenterPos.right, deltaA));    //「攻擊點右方向量」與「攻擊點到目標的向量」的????，其值等於兩點距離，且含有方向資訊(?)
            if (forwardDotA <= fSizeF){
                if (rightDotA < fSizeR) {
                    aData.Add(__RoleControl);
                }
            }
        }
        return aData;
    }






    public BaseRoleControllV2 f_FindTargetEnemy2(GameEM.TeamType tTeamType, Vector3 Pos, float fSize)
    {
        float __fDis = 1000;
        BaseRoleControllV2 tTartgetRole = null;
        foreach (BaseRoleControllV2 __RoleControl in _aList)
        {
            if (__RoleControl.f_IsDie())
            {
                continue;
            }
            if (tTeamType != __RoleControl.f_GetTeamType())
            {
                continue;
            }
            if (CheckIsIgnore(__RoleControl))
            {
                continue;
            }
            float f = Vector3.Distance(__RoleControl.transform.position, Pos);
            f = f - __RoleControl.f_BodySize();
            if (f <= fSize)
            {
                if (f < __fDis)
                {
                    __fDis = f;
                    tTartgetRole = __RoleControl;
                }
            }
        }
        return tTartgetRole;
    }


    List<BaseRoleControllV2> _aFindRandCatch = new List<BaseRoleControllV2>();
    /// <summary>
    /// 随机返回指定范围内的不同阵营的一个目标
    /// </summary>
    /// <param name="tRoleControl"></param>
    /// <param name="fSize"></param>
    /// <returns></returns>
    public BaseRoleControllV2 f_FindTargetEnemyForRand(BaseRoleControllV2 tRoleControl, float fSize)
    {
        _aFindRandCatch.Clear();
        foreach (BaseRoleControllV2 __RoleControl in _aList)
        {
            if (__RoleControl.f_IsDie())
            {
                continue;
            }
            if (tRoleControl == __RoleControl)
            {
                continue;
            }
            if (tRoleControl.f_GetTeamType() == __RoleControl.f_GetTeamType())
            {
                continue;
            }
            if (CheckIsIgnore(__RoleControl))
            {
                continue;
            }
            float f = Vector3.Distance(__RoleControl.transform.position, tRoleControl.transform.position);
            f = f - tRoleControl.f_BodySize();
            if (f <= fSize)
            {
                _aFindRandCatch.Add(__RoleControl);
            }
        }
        if (_aFindRandCatch.Count == 0)
        {
            return null;
        }
        int iRand = ccMath.f_GetRand(0, _aFindRandCatch.Count);        
        return _aFindRandCatch[iRand];
    }


    #endregion
}


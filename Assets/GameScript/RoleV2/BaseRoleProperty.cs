using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoleProtecy
{
    private int _iHP = 0;

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



    #region 角色視野倍率
    protected float _viewZoom = 1;
    /// <summary>
    /// 偵測範圍倍率
    /// </summary>
    public float m_viewZoom
    {
        get
        {
            return _viewZoom;
        }
        set
        {
            _viewZoom = value;
        }
    }
    #endregion


    #region 角色視野倍率
    /// <summary>
    /// 判斷角色是否受傷
    /// </summary>
    protected bool _bIsHurt = false;
    public bool f_IsHurt
    {
        get
        {
            return _bIsHurt;
        }
        set
        {
            _bIsHurt = value;
        }
    }
    #endregion

    /// <summary>
    /// NPC模板DT
    /// </summary>
    protected CharacterDT _CharacterDT;

    /// <summary>
    /// 阵营
    /// </summary>
    GameEM.TeamType _TeamType;

    private int _iLife = 0;

    private int _iId;
    public int m_iId
    {
        get
        {
            return _iId;
        }
    }

    /// <summary>
    /// 当前所在位置
    /// </summary>
    private TileNode m_StayTile = null;

    private float _fHeight;


    public BaseRoleProtecy(int iId, CharacterDT tCharacterDT, GameEM.TeamType tTeamType, float fHeight)
    {
        _iId = iId;
        _CharacterDT = tCharacterDT;
        _TeamType = tTeamType;
        _fHeight = fHeight;

        _iLife = _CharacterDT.iReBirth;
        if (_iLife <= 0)
        {
            _iLife = 1;
        }

    }

    public CharacterDT f_GetCharacterDT()
    {
        return _CharacterDT;
    }

    #region 血量
    /// <summary>
    /// 加血
    /// </summary>
    /// <param name="iHP"></param>
    public void f_AddHp(int iHP)
    {
        _iHP += iHP;
        if (_iHP > _CharacterDT.iHp)
        {
            _iHP = _CharacterDT.iHp;
        }
    }

    /// <summary>
    /// 恢复满血
    /// </summary>
    public void f_FullHP()
    {
        _iHP = _CharacterDT.iHp;
    }

    /// <summary>
    /// 掉血
    /// </summary>
    /// <param name="iHP"></param>
    public void f_LostHP(int iHP)
    {
        _iHP -= iHP;
        if (_iHP < 0)
        {
            _iHP = 0;
        }
    }

    /// <summary>
    /// 鎖血
    /// </summary>
    /// <param name="iHP"></param>
    public void f_LockHp(float iHP)
    {
        _iHP = (int)iHP;
        if (_iHP > _CharacterDT.iHp)
        {
            _iHP = _CharacterDT.iHp;
        }
    }

    /// <summary>
    /// 獲取當前血量
    /// </summary>
    public int f_GetHp()
    {
        return _iHP;
    }

    public int f_GetMaxHp()
    {
        return _CharacterDT.iHp;
    }
    #endregion


    #region 生命
    /// <summary>
    /// 查询还有几点生命
    /// </summary>
    /// <returns></returns>
    public int f_GetHaveLife()
    {
        return _iLife;
    }

    public void f_LostLife(int iLife)
    {
        _iLife -= iLife;
        if (_iLife < 0)
        {
            _iLife = 0;
        }
    }

    #endregion

    #region 无敌相关
    private bool _bIsInvincible = false;

    /// <summary>
    /// 是否為無敵
    /// </summary>
    /// <returns></returns>
    /// --------------------------------------------------
    public bool f_IsInv()
    {
        if (_bIsInvincible)
        {
            return true;
        }
        if (_CharacterDT != null)
        {
            if (_CharacterDT.iInvincible == 1)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 設定是否為無敵狀態, 此无敌设置权限比脚本中的无敌权限大
    /// </summary>
    /// <returns></returns>
    /// --------------------------------------------------
    public void f_SetInv(bool bInvincible)
    {
        _bIsInvincible = bInvincible;
    }
    #endregion

    #region 属性相关
    public GameEM.TeamType f_GetTeamType()
    {
        return _TeamType;
    }

    public GameEM.emRoleType f_GetRoleType()
    {
        return (GameEM.emRoleType)_CharacterDT.iType;
    }

    public float f_BodySize()
    {
        return _CharacterDT.fBodySize;
    }

    public float f_GetAttackSize()
    {
        return _CharacterDT.fAttackSize;
    }

    public float f_GetViewSize()
    {
        return _CharacterDT.fViewSize;
    }

    public float f_GetWalkSpeed()
    {
        return _CharacterDT.fMoveSpeed;
    }
    public bool f_CheckIsNoFind()
    {
        if (_CharacterDT.iNoFind == 1)
        {
            return true;
        }
        return false;
    }



    public float f_GetBodySize() {
        return _CharacterDT.fBodySize;
    }


    public string f_GetAI()
    {
        return _CharacterDT.szAI;
    }


    /// <summary>
    /// 取得人物說明
    /// </summary>
    /// <returns></returns>
    public string f_GetReadme() {
        return _CharacterDT.strReadme;
    }


    /// <summary>
    /// 获得攻击力
    /// </summary>
    /// <returns></returns>
    public int f_GetAttackPower() {
        return _CharacterDT.iAttackPower;
    }

    /// <summary>
    /// 获取角色身高
    /// </summary>
    /// <returns></returns>
    public float f_GetHeight()
    {
        return _fHeight;
    }

    #endregion
    


}
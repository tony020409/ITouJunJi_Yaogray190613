
//============================================
//
//    Character来自Character.xlsx文件自动生成脚本
//    2018/5/23 20:30:20
//    
//
//============================================
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDT : NBaseSCDT
{

    /// <summary>
    /// 角色名
    /// </summary>
    public string szName;
    /// <summary>
    /// 角色类型
    /// </summary>
    public int iType;
    /// <summary>
    /// 资源名
    /// </summary>
    public string szResName;
    /// <summary>
    /// 人物說明(中文)
    /// </summary>
    public string strReadme;
    /// <summary>
    /// 生命
    /// </summary>
    public int iHp;
    /// <summary>
    /// 攻击力
    /// </summary>
    public int iAttackPower;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public float fAttackSize;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float fMoveSpeed;
    /// <summary>
    /// 视野
    /// </summary>
    public float fViewSize;
    /// <summary>
    /// 攻击目标类型
    /// </summary>
    public string szAttackType;
    /// <summary>
    /// 身体占位
    /// </summary>
    public string szPos;
    /// <summary>
    /// 身体大小
    /// </summary>
    public float fBodySize;
    /// <summary>
    /// 是否可以被侦测0可以被侦测1隐身不能被侦测
    /// </summary>
    public int iNoFind;
    /// <summary>
    /// 是否无敌0正常 1无敌
    /// </summary>
    public int iInvincible;
    /// <summary>
    /// 重生次数
    /// </summary>
    public int iReBirth;

    /// <summary>
    /// AI列表
    /// </summary>
    public string szAI;

    /// <summary>
    /// 装备枪支列表
    /// </summary>
    public string szGun;



    #region  附加缓存信息

    private Vector2[] _aPos = null;
    public Vector2[] aPos
    {
        get
        {
            if (_aPos == null)
            {
                string[] aData = ccMath.f_String2ArrayString(szPos, ":");
                if (aData.Length > 0)
                {
                    _aPos = new Vector2[aData.Length];
                    for (int i = 0; i < aData.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(aData[i]))
                        {
                            int[] aPos = ccMath.f_String2ArrayInt(aData[i], "+");
                            if (aPos.Length == 2)
                            {
                                _aPos[i] = new Vector2(aPos[0], aPos[1]);
                            }
                        }

                    }
                }
            }
            return _aPos;
        }
    }

    #endregion





}

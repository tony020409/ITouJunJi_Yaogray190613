
//============================================
//
//    Gun来自Gun.xlsx文件自动生成脚本
//    2018/10/9 10:27:00
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GunDT : NBaseSCDT
{

    /// <summary>
    /// 名称
    /// </summary>
    public string szName;
    /// <summary>
    /// 子弹资源名
    /// </summary>
    public string szBulletResName;
    /// <summary>
    /// 射击速度
    /// </summary>
    public float fSpeed;
    /// <summary>
    /// 装弹量
    /// </summary>
    public int iMaxBullet;
    /// <summary>
    /// 子弹开始位置
    /// </summary>
    public string szBulletStartPos;
    /// <summary>
    /// 能装载的子弹Id
    /// </summary>
    public int iBulletId;
    /// <summary>
    /// 弹夹
    /// </summary>
    public int iClipNum;
}

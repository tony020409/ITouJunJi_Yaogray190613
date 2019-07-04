using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 飛彈狀態
/// </summary>
public enum MissileEnum
{
    /// <summary>
    /// 直線飛行
    /// </summary>
    StraightLine,
    /// <summary>
    /// 追蹤
    /// </summary>
    Track,
    /// <summary>
    /// 停止
    /// </summary>
    Stop,
}
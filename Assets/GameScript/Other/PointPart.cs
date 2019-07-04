using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 躲藏點資訊
/// </summary>
public enum EM_Hide{
    [Rename("往右躲")]
    /// <summary> 往右躲 </summary>
    RightHide,

    [Rename("往左躲")]
    /// <summary> 往左躲 </summary>
    LeftHide,

    [Rename("蹲下")]
    /// <summary> 蹲下 </summary>
    SquatHide,

    [Rename("原地")]
    /// <summary> 原地 </summary>
    StandHide,

    [Rename("跳躍")]
    /// <summary> 跳躍 </summary>
    JumpHide,
}


public class PointPart : MonoBehaviour {
    [Rename("閃避動畫")] public EM_Hide HideType;
}

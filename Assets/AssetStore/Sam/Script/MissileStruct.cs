using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
/// <summary>
/// 飛彈結構
/// </summary>
public struct MissileStruct {

    /// <summary>
    /// 追蹤時間
    /// </summary>
    /// <remarks>過幾秒後開始追蹤  一開始只直線飛行</remarks>
    public float TrackTime;

    public float m_uptimer;
    /// <summary>
    /// 飛行速度
    /// </summary>
    public float m_speed;
    /// <summary>
    /// 追蹤飛行速度
    /// </summary>
    public float m_Trackspeed;
    /// <summary>
    /// 追蹤速度
    /// </summary>
    public float m_RotSpeed;
    /// <summary>
    /// 追蹤目標
    /// </summary>
    public GameObject m_Target;
    /// <summary>
    /// 飛彈狀態
    /// </summary>
    public MissileEnum m_MissileEnum;

    public Explosion ExplosionPrefab, m_Explosion;

    /// <summary>
    /// 特效
    /// </summary>
    public EllipsoidParticleEmitter[] m_EllipsoidParticleEmitter;

}
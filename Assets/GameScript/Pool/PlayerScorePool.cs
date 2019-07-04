using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScorePool
{
    /// <summary>
    /// 发射子弹的量
    /// </summary>
    private int _iShot = 0;
    public int m_iShot {
        get {
            return _iShot;
        }
    }

    /// <summary>
    /// 发射子弹的命中量
    /// </summary>
    private int _iShotHit = 0;
    public int m_iShotHit  {
        get {
            return _iShotHit;
        }
    }

    /// <summary>
    /// 命中头部
    /// </summary>
    private int _iHeadShot = 0;
    public int m_iHeadShot {
        get {
            return _iHeadShot;
        }
    }

    /// <summary>
    /// 命中头部击杀数
    /// </summary>
    private int _iHeadShotDie = 0;
    public int m_iHeadShotDie {
        get {
            return _iHeadShotDie;
        }
    }

    /// <summary>
    /// 击杀数
    /// </summary>
    private int _iShotDie = 0;
    public int m_iShotDie {
        get {
            return _iShotDie;
        }
    }

    /// <summary>
    /// 自己死亡次数
    /// </summary>
    private int _iDie = 0;
    public int m_iDie {
        get {
            return _iDie;
        }
    }


    /// <summary>
    /// 分數歸零 (重置)
    /// </summary>
    public void f_Reset() {
        _iShot = 0;
        _iShotHit = 0;
        _iHeadShot = 0;
        _iHeadShotDie = 0;
        _iShotDie = 0;
    }


    public void f_Shot() {
        _iShot++;
    }


    /// <summary>
    /// 子弹射中目标
    /// </summary>
    /// <param name="tEM_BodyPart">被击中的部位</param>
    /// <param name="bDie">是否死亡</param>
    public void f_Hit(GameEM.EM_BodyPart tEM_BodyPart, bool bDie)
    {
        _iShotHit++;

        if (bDie == true)
        {
            _iShotDie++;
        }
        if (tEM_BodyPart == GameEM.EM_BodyPart.Head && bDie == true)
        {
            _iHeadShot++;
            _iHeadShotDie++;
        }
        else if (tEM_BodyPart == GameEM.EM_BodyPart.Head)
        {
            _iHeadShot++;
        }         

    }

    public void f_Die()
    {
        _iDie++;
    }



}

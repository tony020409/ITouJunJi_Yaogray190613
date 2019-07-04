using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool
{

    private List<BaseBullet> _aList = new List<BaseBullet>();


    public void f_Save(BaseBullet tBaseBullet)
    {
        BaseBullet tCheckBaseBullet = Get(tBaseBullet);
        if (tCheckBaseBullet != null)
        {
            return;
        }
        _aList.Add(tBaseBullet);
    }

    public void f_Del(BaseBullet tBaseBullet)
    {
        _aList.Remove(tBaseBullet);
    }

    private BaseBullet Get(BaseBullet tBaseBullet)
    {
        return _aList.Find(delegate (BaseBullet p) { return p == tBaseBullet; });
    }

    public BaseBullet f_Get(int iBulletId)
    {
        return _aList.Find(delegate (BaseBullet p) { return p.f_GetId() == iBulletId; });
    }

    public BaseBullet f_CheckIsCollide(BaseBullet tBaseBullet)
    {
        for (int i = 0; i < _aList.Count; i++)
        {
            if (_aList[i] == tBaseBullet)
            {
                continue;
            }
            float f = Vector3.Distance(tBaseBullet.f_GetPosition(), _aList[i].f_GetPosition());
            f = f - tBaseBullet.f_GetAttackRang() - _aList[i].f_GetAttackRang();
            if (f <= 0)
            {
                return _aList[i];
            }
        }
        return null;
    }




}
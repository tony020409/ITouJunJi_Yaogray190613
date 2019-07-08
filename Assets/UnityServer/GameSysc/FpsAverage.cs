using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsAverage
{
    List<int> _aData = new List<int>();
    private int _iMax;
    private int _iAll;
    private int _iAverage;
    public FpsAverage(int iMax = 20)
    {
        _iMax = iMax;
    }


    public void f_Add(int iData)
    {
        if (_aData.Count > _iMax)
        {
            _iAll -= _aData[0];
            _aData.RemoveAt(0);
        }
        _iAll += iData;
        _aData.Add(iData);
        _iAverage = _iAll / _aData.Count;

    }

    public int f_GetAverage()
    {
        return _iAverage;
    }

}
using System.Collections;
using UnityEngine;

public class BuzzFloorAdapter : MonoBehaviour
{
    public enum BuzzFloorType
    {
        HTC,
        BlackTea
    }

    public BuzzFloorType m_BuzzFloorType;

    public BlackTeaBuzzFloor blackTeaBuzzFloor;
    public HTCBuzzFloor htcBuzzFloor;
    public static  BuzzFloorAdapter instance;

    void Start()
    {
        Init();
        instance = this;
    }

    void Init()
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                htcBuzzFloor.gameObject.SetActive(true);
                break;
            case BuzzFloorType.BlackTea:
                blackTeaBuzzFloor.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("Type Error");
                break;
        }
    }

    public void f_setVol(float V)
    {

        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                htcBuzzFloor.f_setVol(V);
                break;
            case BuzzFloorType.BlackTea:
                blackTeaBuzzFloor.f_setVol(V);
                break;
            default:
                Debug.LogError("Type Error");
                break;
        }
    }

    public IEnumerator s1Button(float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s1Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s1Button(WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }

    public IEnumerator s2Button(float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s2Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s2Button(WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }
    public void do_s3Button(float WaitTime)
    {
        StartCoroutine(s3Button(WaitTime));
    }
    public IEnumerator s3Button(float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s3Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s3Button(WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }

    public IEnumerator s4Button(float WaitTime, float Vol)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s4Button(WaitTime, Vol);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s4Button(WaitTime, Vol);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }
    public IEnumerator s9Button(float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s1Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s9Button(WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }
    public IEnumerator s10Button(float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s1Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s10Button(WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }

    public IEnumerator s99Button(float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s1Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s99Button(WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }
    public IEnumerator s12Button(float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s1Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s12Button(WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }
    public IEnumerator s13Button(float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s1Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s13Button(WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }
    public IEnumerator s14Button(float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s1Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.s14Button(WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }

    public IEnumerator Playshock(string AudioName,  float WaitTime)
    {
        switch (m_BuzzFloorType)
        {
            case BuzzFloorType.HTC:
                return htcBuzzFloor.s1Button(WaitTime);
            case BuzzFloorType.BlackTea:
                return blackTeaBuzzFloor.Playshock(AudioName, WaitTime);
            default:
                Debug.LogError("Type Error");
                return null;
        }
    }

}

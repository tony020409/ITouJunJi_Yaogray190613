using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血量控制腳本
/// </summary>
public class HPController : MonoBehaviour
{
    [SerializeField]
    [Header("Default HP")]
    private int DefaultHP;
    [SerializeField]
    [Header("Current HP")]
    private int Hp = 0;
    [SerializeField]
    private bool  UseCD  = false;
    [SerializeField]
    private float CDTime = 0.5f;
    [SerializeField]
    private Text HPText;

    bool NormalSetHP;

    public int HP
    {
        get {
            return Hp;
        }
        set {
            if ( Invincible == true )
                return;

            if ( value <= 0 )
            {
                Hp = 0;

                Invincible = true;
            }
            else
            {
                Hp = value;

                if ( UseCD && NormalSetHP == false )
                {
                    print( Hp );

                    Invincible = true;
                    Invoke( "SetCanSubHP" , CDTime );
                }
            }

            if ( HPText )
                HPText.text = value.ToString();

            if ( HitEvent != null )
            {
                HitEvent( this );
            }

            if ( Hp == 0 )
            {
                if ( DieEvent != null )
                {
                    DieEvent( this );
                }
                RemoveAllEvents();
            }
        }
    }

    public delegate void HPControllerEvent ( HPController sender );
    List<HPControllerEvent> Delegates_Die = new List<HPControllerEvent>();
    List<HPControllerEvent> Delegates_Hit = new List<HPControllerEvent>();

    private event HPControllerEvent DieEvent;
    private event HPControllerEvent HitEvent;

    /// <summary>
    /// 血量大於0才可註冊
    /// </summary>
    public event HPControllerEvent OnDie
    {
        add{
            if(HP >0)
            {
                //Debug.Log("OnDie Reg : " + value.Target);
                DieEvent += value;
                Delegates_Die.Add(value);
            }
        }
        remove{
            DieEvent -= value;
            Delegates_Hit.Add(value);
        }
    }

    /// <summary>
    /// 血量大於0才可註冊
    /// </summary>
    public event HPControllerEvent OnHit
    {
        add{
            if(HP >0)
            {
                HitEvent += value;
                Delegates_Hit.Add(value);
            }
        }
        remove{
            HitEvent -= value;
            Delegates_Hit.Add(value);
        }
    }

    private bool Invincible;

    private void Awake ( )
    {
        if ( HPText == null )
        {
            CanvasRenderer canvasRenderer = GetComponentInChildren<CanvasRenderer>();
            if ( canvasRenderer != null )
            {
                HPText = canvasRenderer.GetComponent<Text>();
            }
        }
        HPControllerInit();
    }

    /// <summary>
    /// 初始化HpController，關閉無敵，並將HP設為預設HP
    /// </summary>
    public void HPControllerInit ( )
    {
        CancelInvoke( "SetCanSubHP" );
        NormalSetHP = true;
        SetInvinvible(false); /// 設定可以扣血，關閉無敵
        if ( DefaultHP >= 0 )
            Hp = DefaultHP;
        NormalSetHP = false;
    }

    /// <summary>
    /// 設定預設HP，並順便初始化
    /// </summary>
    /// <param name="_hp"></param>
    public void SetDefaultHP ( int _hp )
    {
        DefaultHP = _hp;
        HPControllerInit();
    }

    public void SetInvinvible ( bool _Invincible )
    {
        Invincible = _Invincible;
    }

    /// <summary>
    /// 設定目前的血量
    /// </summary>
    /// <param name="Hp">要設定的血量</param>
    public void SetHP ( int _HP )
    {
        HP = _HP;
    }

    public int GetDefaultHP ( )
    {
        return DefaultHP;
    }

    public bool IsInvinvible ( )
    {
        return Invincible;
    }

    public bool IsDie ( )
    {
        return HP == 0;
    }

    public void RemoveAllEvents ( )
    {
        for ( int i = 0 ; i < Delegates_Die.Count ; i++ )
        {
            HPControllerEvent hpEvt = Delegates_Die[ i ];
            DieEvent -= hpEvt;
        }
        Delegates_Die.Clear();

        for ( int i = 0 ; i < Delegates_Hit.Count ; i++ )
        {
            HPControllerEvent hpEvt = Delegates_Hit[ i ];
            HitEvent -= hpEvt;
        }
        Delegates_Hit.Clear();
    }

    void OnDestroy ( )
    {
        RemoveAllEvents();
        HPText = null;
    }

    public void ShowDieEventListener()
    {
        for ( int i = 0 ; i < Delegates_Hit.Count ; i++ )
        {
            Debug.Log( Delegates_Hit [i].Target);
        }
    }
}

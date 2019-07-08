using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 飛彈類型
/// </summary>
public enum MissileEM
{
    Needlegun,
    Lasergun,
}


/// <summary>
/// 刺針狀態
/// </summary>
public enum NeedlegunEM
{
    emission,
    track
}
public class Needlegun : MonoBehaviour
{
    public float emissionSpeed;
    public float trackSpeed;

    public float Timing;
    private float ttime;
    public GameObject Effetcs;
    private GameObject _oTarget;


    private MissileEM _eDoStep = MissileEM.Needlegun;



    private int _fMinDis = 3;
    private GameEM.TeamType _emTeamType;

    private bool _bDoing = false;
    private float _fSpeed = 0;
    private float _fLiveTime = 0;
    private int _iAttackPower = 0;

    private BaseRoleControllV2 _BeAttackRoleControll = null;

    // Use this for initialization
    void Start()
    {


    }
    public void f_Fired(BaseRoleControllV2 tBeAttackRoleControl, GameEM.TeamType tTeamType, float fSpeed, float fLive, int iAttackPower, GameObject oTarget)
    {
        _BeAttackRoleControll = tBeAttackRoleControl;
        _emTeamType = tTeamType;
        _bDoing = true;
        _fSpeed = fSpeed;
        _fLiveTime = fLive;
        _iAttackPower = iAttackPower;
        _oTarget = oTarget;

        _eDoStep = MissileEM.Needlegun;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_bDoing)
        {
            return;
        }

        if (_eDoStep == MissileEM.Needlegun)
        {
            ttime += Time.deltaTime;
            if (ttime >= Timing)
            {
                Track();
                hitjudgment();
            }
            else
            {
                Emission();
            }
        }       
    }
    private void Emission()
    {
        Effetcs.gameObject.SetActive(false);
        transform.Translate(0, 0, emissionSpeed * Time.deltaTime);
    }
    private void Track()
    {
        transform.Translate(0, 0, trackSpeed * Time.deltaTime);
        Effetcs.gameObject.SetActive(true);
        Vector3 targetDir = _oTarget.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 0.2f, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);       
    }

    private void hitjudgment()
    {      
        _fLiveTime -= Time.deltaTime;
        transform.Translate(0, 0, Time.deltaTime * _fSpeed);
        if (_fLiveTime < 0)
        {
            DoDestory();
        }
        else
        {
            //↓这个部分的Debug.Log只有在伺服器模式才能看的到
            if (StaticValue.m_bIsMaster)
            {
                float f = Vector3.Distance(transform.position, _oTarget.transform.position);
                if (f < _fMinDis)
                {
                    //MessageBox.DEBUG("射中!!!!!!!");
                    if (_BeAttackRoleControll != null)
                    {
                        if (!GloData.glo_bCanShootMySelf)
                        {
                            if (_BeAttackRoleControll.f_GetTeamType() == _emTeamType)
                            {  //射中自己人
                               //Debug.LogWarning("射中自己人!!!!!!!!!");
                                return;
                            }
                        }

                        //射中别人
                        //Debug.LogWarning("射中代號" + BeAttackRoleControl.m_iId + "!!");
                        RoleHpAction tRoleHpAction = new RoleHpAction();
                        tRoleHpAction.f_Hp(-99, _BeAttackRoleControll.m_iId, _iAttackPower, transform.position);
                        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleHpAction);
                        //播放命中特效
                        DoDestory();
                    }
                }
            }
        }
    }
    private void DoDestory()
    {
        _bDoing = false;
        var N = glo_Main.GetInstance().m_ResourceManager.f_Needlegunexplosion();
        N.transform.position = _oTarget.transform.position;
        N.transform.rotation = _oTarget.transform.rotation;
        //Destroy(gameObject);
        glo_Main.GetInstance().m_ResourceManager.f_DestorySD(gameObject);
    }
    
}

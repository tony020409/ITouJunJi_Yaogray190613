using GameControllAction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayerTransform : MonoBehaviour
{
    private float _fFpsTime = 0;
    PlayerTransformAction _PlayerTransformAction = new PlayerTransformAction();

    public GameObject m_oHead;
    public GameObject m_oArmL;
    public GameObject m_oArmR;

    public GameObject m_oFootL;
    public GameObject m_oFootR;

    public GameObject m_oOther1;
    public GameObject m_oOther2;

    private bool _bIsComplete = false;
    public bool m_bIsComplete
    {
        get
        {
            return _bIsComplete;
        }

        set
        {
            _bIsComplete = value;
        }
    }

    void Start()
    {
        m_oHead = GameObject.Find("Camera (eye)");
        if (m_oHead == null)
        {
            MessageBox.ASSERT("未找到有效的 Camera (eye)");
        }
    }



    void Update()
    {
        //if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming)
        //{
            if (_fFpsTime <= 0)
            {
                CreateAction();
                _fFpsTime = GloData.glo_fMaxControllFrameTime;
            }
            _fFpsTime -= Time.deltaTime;
        //}
    }

    private void CreateAction()
    {
        //_PlayerTransformAction.m_iPlayerId = StaticValue.m_UserDataUnit.m_PlayerDT.m_iId;
        SaveHead(_PlayerTransformAction);
        //SaveBody(_PlayerTransformAction);
        SaveArmL(_PlayerTransformAction);
        SaveArmR(_PlayerTransformAction);

        SaveFootL(_PlayerTransformAction);
        SaveFootR(_PlayerTransformAction);

        SaveOther1(_PlayerTransformAction);
        SaveOther2(_PlayerTransformAction);
        ControllSocket.GetInstance().f_SendAction(_PlayerTransformAction);
    }

    private void SaveHead(PlayerTransformAction tPlayerTransformAction)
    {
        tPlayerTransformAction.m_fHeadPosX = m_oHead.transform.position.x;
        tPlayerTransformAction.m_fHeadPosY = m_oHead.transform.position.y;
        tPlayerTransformAction.m_fHeadPosZ = m_oHead.transform.position.z;

        //Quaternion tQuaternion = m_oHead.transform.rotation.eulerAngles.y;
        //tPlayerTransformAction.m_fHeadQutnX = m_oHead.transform.rotation.x;
        tPlayerTransformAction.m_fHeadQutnY = m_oHead.transform.rotation.eulerAngles.y;
        //tPlayerTransformAction.m_fHeadQutnZ = m_oHead.transform.rotation.z;
        //tPlayerTransformAction.m_fHeadQutnW = m_oHead.transform.rotation.w;

    }

    //private void SaveBody(PlayerTransformAction tPlayerTransformAction)
    //{
    //    tPlayerTransformAction.m_fBodyPosX = transform.position.x;
    //    tPlayerTransformAction.m_fBodyPosY = transform.position.y;
    //    tPlayerTransformAction.m_fBodyPosZ = transform.position.z;

    //    //Vector3 v3Rotation = transform.rotation.eulerAngles;
    //    //tPlayerTransformAction.m_fBodyQutnX = v3Rotation.x;
    //    //tPlayerTransformAction.m_fBodyQutnY = v3Rotation.y;
    //    //tPlayerTransformAction.m_fBodyQutnZ = v3Rotation.z;
    //    //tPlayerTransformAction.m_fBodyQutnW = transform.rotation.w;
    //}

    private void SaveArmL(PlayerTransformAction tPlayerTransformAction)
    {
        tPlayerTransformAction.m_fArmLPosX = m_oArmL.transform.position.x;
        tPlayerTransformAction.m_fArmLPosY = m_oArmL.transform.position.y;
        tPlayerTransformAction.m_fArmLPosZ = m_oArmL.transform.position.z;

        Vector3 v3Rotation = m_oArmL.transform.rotation.eulerAngles;
        tPlayerTransformAction.m_fArmLQutnX = v3Rotation.x;
        tPlayerTransformAction.m_fArmLQutnY = v3Rotation.y;
        tPlayerTransformAction.m_fArmLQutnZ = v3Rotation.z;
    }

    private void SaveArmR(PlayerTransformAction tPlayerTransformAction)
    {
        tPlayerTransformAction.m_fArmRPosX = m_oArmR.transform.position.x;
        tPlayerTransformAction.m_fArmRPosY = m_oArmR.transform.position.y;
        tPlayerTransformAction.m_fArmRPosZ = m_oArmR.transform.position.z;

        Vector3 v3Rotation = m_oArmR.transform.rotation.eulerAngles;
        tPlayerTransformAction.m_fArmRQutnX = v3Rotation.x;
        tPlayerTransformAction.m_fArmRQutnY = v3Rotation.y;
        tPlayerTransformAction.m_fArmRQutnZ = v3Rotation.z;
    }

    private void SaveFootL(PlayerTransformAction tPlayerTransformAction)
    {
        if (m_oFootL == null)
        {
            return;
        }
        tPlayerTransformAction.m_fFootLPosX = m_oFootL.transform.position.x;
        tPlayerTransformAction.m_fFootLPosY = m_oFootL.transform.position.y;
        tPlayerTransformAction.m_fFootLPosZ = m_oFootL.transform.position.z;

        Vector3 v3Rotation = m_oFootL.transform.rotation.eulerAngles;
        tPlayerTransformAction.m_fFootLQutnX = v3Rotation.x;
        tPlayerTransformAction.m_fFootLQutnY = v3Rotation.y;
        tPlayerTransformAction.m_fFootLQutnZ = v3Rotation.z;
    }

    private void SaveFootR(PlayerTransformAction tPlayerTransformAction)
    {
        if (m_oFootR == null)
        {
            return;
        }
        tPlayerTransformAction.m_fFootRPosX = m_oFootR.transform.position.x;
        tPlayerTransformAction.m_fFootRPosY = m_oFootR.transform.position.y;
        tPlayerTransformAction.m_fFootRPosZ = m_oFootR.transform.position.z;

        Vector3 v3Rotation = m_oFootR.transform.rotation.eulerAngles;
        tPlayerTransformAction.m_fFootLQutnX = v3Rotation.x;
        tPlayerTransformAction.m_fFootLQutnY = v3Rotation.y;
        tPlayerTransformAction.m_fFootLQutnZ = v3Rotation.z;
    }

    private void SaveOther1(PlayerTransformAction tPlayerTransformAction)
    {
        if (m_oOther1 == null)
        {
            return;
        }
        tPlayerTransformAction.m_fOtherPos1X = m_oOther1.transform.position.x;
        tPlayerTransformAction.m_fOtherPos1Y = m_oOther1.transform.position.y;
        tPlayerTransformAction.m_fOtherPos1Z = m_oOther1.transform.position.z;
    }

    private void SaveOther2(PlayerTransformAction tPlayerTransformAction)
    {
        if (m_oOther2 == null)
        {
            return;
        }
        tPlayerTransformAction.m_fOtherPos2X = m_oOther2.transform.position.x;
        tPlayerTransformAction.m_fOtherPos2Y = m_oOther2.transform.position.y;
        tPlayerTransformAction.m_fOtherPos2Z = m_oOther2.transform.position.z;
    }



}
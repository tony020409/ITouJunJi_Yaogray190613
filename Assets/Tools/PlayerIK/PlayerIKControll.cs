using GameControllAction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIKControll : MonoBehaviour
{
    public GameObject m_oHead;
    public GameObject m_oArmL;
    public GameObject m_oArmR;

    //void Start()
    //{

    //}


    //void Update()
    //{

    //}

    /// <summary>
    /// 接收来至玩家动作Socket的动作数据
    /// </summary>
    /// <param name="tPlayerTransformAction"></param>
    public void f_UpdatePlayerTransform(PlayerTransformAction tPlayerTransformAction)
    {
        UpdateHead(tPlayerTransformAction);
        //UpdateBody(tPlayerTransformAction);
        UpdateArmL(tPlayerTransformAction);
        UpdateArmR(tPlayerTransformAction);
    }

    private void UpdateHead(PlayerTransformAction tPlayerTransformAction)
    {
        Vector3 Pos = new Vector3(tPlayerTransformAction.m_fHeadPosX, tPlayerTransformAction.m_fHeadPosY, tPlayerTransformAction.m_fHeadPosZ);
        transform.position = Pos;
        //iTween.MoveTo(gameObject, Pos, GloData.glo_fMaxControllFrameTime);

        Vector3 v3Quaternion = transform.rotation.eulerAngles;
        Quaternion tQuaternion = Quaternion.Euler(transform.rotation.eulerAngles.x, tPlayerTransformAction.m_fHeadQutnY, transform.rotation.eulerAngles.z);
        //tQuaternion.x = tPlayerTransformAction.m_fHeadQutnX;
        //v3Quaternion.y = tPlayerTransformAction.m_fHeadQutnY;
        //tQuaternion.z = tPlayerTransformAction.m_fHeadQutnZ;
        //tQuaternion.w = tPlayerTransformAction.m_fHeadQutnW;
        transform.rotation = tQuaternion;
        //iTween.RotateTo(gameObject, v3Quaternion, GloData.glo_fMaxControllFrameTime);

       
        
    }

    //private void UpdateBody(PlayerTransformAction tPlayerTransformAction)
    //{
    //    Vector3 Pos = new Vector3(tPlayerTransformAction.m_fBodyPosX, tPlayerTransformAction.m_fBodyPosY, tPlayerTransformAction.m_fBodyPosZ);
        
    //    //transform.position = Pos;
    //    //transform.rotation = tQuaternion;
    //    iTween.MoveTo(gameObject, Pos, GloData.glo_fMaxControllFrameTime);

    //    //Quaternion tQuaternion = Quaternion.Euler(tPlayerTransformAction.m_fBodyQutnX, tPlayerTransformAction.m_fBodyQutnY, tPlayerTransformAction.m_fBodyQutnZ);
    //    //iTween.RotateTo(gameObject, tQuaternion.eulerAngles, GloData.glo_fMaxControllFrameTime);
    //}

    private void UpdateArmL(PlayerTransformAction tPlayerTransformAction)
    {
        Vector3 Pos = new Vector3(tPlayerTransformAction.m_fArmLPosX, tPlayerTransformAction.m_fArmLPosY, tPlayerTransformAction.m_fArmLPosZ);
        m_oArmL.transform.position = Pos;
        Quaternion tQuaternion = Quaternion.Euler(tPlayerTransformAction.m_fArmLQutnX, tPlayerTransformAction.m_fArmLQutnY, tPlayerTransformAction.m_fArmLQutnZ);
        m_oArmL.transform.rotation = tQuaternion;
        //iTween.MoveTo(m_oArmL, Pos, GloData.glo_fMaxControllFrameTime);
    }

    private void UpdateArmR(PlayerTransformAction tPlayerTransformAction)
    {
        Vector3 Pos = new Vector3(tPlayerTransformAction.m_fArmRPosX, tPlayerTransformAction.m_fArmRPosY, tPlayerTransformAction.m_fArmRPosZ);
        m_oArmR.transform.position = Pos;
        Quaternion tQuaternion = Quaternion.Euler(tPlayerTransformAction.m_fArmRQutnX, tPlayerTransformAction.m_fArmRQutnY, tPlayerTransformAction.m_fArmRQutnZ);
        m_oArmR.transform.rotation = tQuaternion;
        //iTween.MoveTo(m_oArmR, Pos, GloData.glo_fMaxControllFrameTime);
    }

    private void LookAt(Vector3 Pos)
    {
        Hashtable args = new Hashtable(); 
        args.Add("looktarget", Pos);
        //args.Add("time", _fMaxFpsTime);
        args.Add("loopType", "none");
        //iTween.LookTo(_RoleControl.gameObject, args);
    }

}
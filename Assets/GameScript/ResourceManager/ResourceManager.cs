using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ccU3DEngine;
using ccU3DEngine.ccEngine.ThingManager;


/// <summary>
/// 异步加载资源回调
/// </summary>
/// <param name="Obj">申请资源对象</param>
/// <param name="asset">异步加载成功返回后的资源，根据需要通过GameObject.Instantiate创建</param>
public delegate void ResourceCatchDelegate(object Obj, UnityEngine.Object asset);

public class ResourceManager
{  
    
    public ResourceManager()
    {
    }


    public void f_Update()
    {
           
    }



    #region 创建资源

    public GameObject f_Animator(string AnimatorPath)
    {
        UnityEngine.Object tModel = Resources.Load(AnimatorPath);
        if (tModel == null)
        {
            MessageBox.ASSERT("加载指定动画失败。" + AnimatorPath);
            return null;
        }
        GameObject f_AnimatorGameObject = GameObject.Instantiate(tModel) as GameObject;
        return f_AnimatorGameObject;
    }



    public GameObject f_CreateBullet()
    {
        string ppSQL = "Model/Bullet/bullet";
        if (!ccResourceManager.GetInstance().f_CheckIsHave(ppSQL))
        {
            GameObject oProfab = (GameObject)Resources.Load(ppSQL);
            if (oProfab == null)
            {
                MessageBox.ASSERT("Profab没找到 " + ppSQL);
                return null;
            }
            GameObject oModel = (GameObject)GameObject.Instantiate(oProfab, Vector3.zero, Quaternion.identity);
            ccResourceManager.GetInstance().f_RegResource(ppSQL, oModel, null, null, true);
        }
        GameObject oBullet = ccResourceManager.GetInstance().f_Instantiate(ppSQL);
        return oBullet;
    }

    public BaseBullet f_CreateBullet(BulletDT tBulletDT)
    {
        string ppSQL = "Model/Bullet/" + tBulletDT.szBulletResName;
        if (!ccResourceManager.GetInstance().f_CheckIsHave(ppSQL))
        {
            GameObject oProfab = (GameObject)Resources.Load(ppSQL);
            if (oProfab == null)
            {
                MessageBox.ASSERT("Profab没找到 " + ppSQL);
                return null;
            }
            GameObject oModel = (GameObject)GameObject.Instantiate(oProfab, Vector3.zero, Quaternion.identity);
            ccResourceManager.GetInstance().f_RegResource(ppSQL, oModel, null, null, true);
        }
        GameObject oBullet = ccResourceManager.GetInstance().f_Instantiate(ppSQL);
        BaseBullet tBaseBullet = oBullet.GetComponent<BaseBullet>();
        return tBaseBullet;
    }

    /// <summary>
    /// 產生在 Resource資料夾下的資源 (e.g.  (Model/Bullet/, bullet)  )
    /// </summary>
    /// <param name="resPath"> 資源路徑 (範例: Model/Bullet/ ) </param>
    /// <param name="resName"> 資源名稱 (範例: bullet )        </param>
    public GameObject f_CreateResource(string resPath ,string resName) {
        string ppSQL = resPath + resName;
        if (!ccResourceManager.GetInstance().f_CheckIsHave(ppSQL)){
            GameObject oProfab = (GameObject)Resources.Load(ppSQL);
            if (oProfab == null)  {
                MessageBox.ASSERT("Profab没找到 " + ppSQL);
                return null;
            }
            GameObject oModel = (GameObject)GameObject.Instantiate(oProfab, Vector3.zero, Quaternion.identity);
            ccResourceManager.GetInstance().f_RegResource(ppSQL, oModel, null, null, true);
        }
        GameObject oBullet = ccResourceManager.GetInstance().f_Instantiate(ppSQL);
        return oBullet;
    }

    /// <summary>
    /// 產生在 Resource資料夾下的資源 (e.g. Model/Bullet/bullet)  )
    /// </summary>
    /// <param name="resPath"> 資源路徑 (範例: Model/Bullet/bullet ) </param>
    public GameObject f_CreateResource(string resPath)
    {
        string ppSQL = resPath;
        if (!ccResourceManager.GetInstance().f_CheckIsHave(ppSQL))
        {
            GameObject oProfab = (GameObject)Resources.Load(ppSQL);
            if (oProfab == null){
                MessageBox.ASSERT("Profab没找到 " + ppSQL);
                return null;
            }
            GameObject oModel = (GameObject)GameObject.Instantiate(oProfab, Vector3.zero, Quaternion.identity);
            ccResourceManager.GetInstance().f_RegResource(ppSQL, oModel, null, null, true);
        }
        GameObject oBullet = ccResourceManager.GetInstance().f_Instantiate(ppSQL);
        return oBullet;
    }



    public GameObject f_NeedlegunBullet()
    {
        string ppSQL = "Model/Bullet/Needlegun";
        if (!ccResourceManager.GetInstance().f_CheckIsHave(ppSQL))
        {
            GameObject oProfab = (GameObject)Resources.Load(ppSQL);
            if (oProfab == null)
            {
                MessageBox.ASSERT("Profab没找到 " + ppSQL);
                return null;
            }
            GameObject oModel = (GameObject)GameObject.Instantiate(oProfab, Vector3.zero, Quaternion.identity);
            ccResourceManager.GetInstance().f_RegResource(ppSQL, oModel, null, null, true);
        }
        GameObject oBullet = ccResourceManager.GetInstance().f_Instantiate(ppSQL);     
        return oBullet;

        //UnityEngine.Object tModel = Resources.Load("Model/Bullet/Needlegun");
        //GameObject oBullet = GameObject.Instantiate(tModel) as GameObject;
        //return oBullet;
    }

    public GameObject f_Needlegunexplosion()
    {
        UnityEngine.Object tModel = Resources.Load("Model/Bullet/NeedlegunexplosionEffect");
        GameObject oBullet = GameObject.Instantiate(tModel) as GameObject;
        return oBullet;
    }
    
    /// <summary>
    /// 计算身高对应的模型
    /// </summary>
    /// <param name="fHeight"></param>
    /// <returns></returns>
    string GetOtherPlayerModelNameForHeight(float fHeight)
    {
        MessageBox.DEBUG(fHeight.ToString());
        string strModel = "";
        //預設模型(0cm)
        if (fHeight==0){
            strModel = "Model/Player/OtherPlayer_0cm";
        }
        //0~200cm
        if (0 < fHeight && fHeight < 200) {
            strModel = "Model/Player/OtherPlayer_167cm";
        }
        return strModel;
    }

    public OtherPlayerControll2 f_CreateOtherPlayer(float fHeight)
    {
        string strModel = GetOtherPlayerModelNameForHeight(fHeight);
        UnityEngine.Object tModel = Resources.Load(strModel);
        if (tModel == null)
        {
            MessageBox.ASSERT("f_CreateOtherPlayer指定身体的模型加载失败 " + fHeight);
        }
        GameObject oPlayer = GameObject.Instantiate(tModel) as GameObject;
        OtherPlayerControll2 tOtherPlayerControll2 = oPlayer.GetComponent<OtherPlayerControll2>();
        return tOtherPlayerControll2;
    }


    //播放音效用 =========================================================================
    public GameObject f_CreateSoundPlayer(){
        UnityEngine.Object tModel = Resources.Load("Model/SoundObj/SoundPlayer");
        if (tModel == null) {
            MessageBox.ASSERT("Excel聲音播放元件加载失败!  請確定 Resource/Model/SoundObj 裡有 SoundPlayer2 物件或路徑是否正確。");
            return null;
        }
        GameObject osp = GameObject.Instantiate(tModel) as GameObject;
        return osp;
    }


    //擊中特效 ===========================================================================
    public GameObject f_CreateFX_Blood_Small(){
        UnityEngine.Object tModel = Resources.Load("Model/HitEffect/FX_BloodSpray_02");
        GameObject oBullet = GameObject.Instantiate(tModel) as GameObject;
        return oBullet;
    }
    public GameObject f_CreateFX_Blood_Big(){
        UnityEngine.Object tModel = Resources.Load("Model/HitEffect/FX_Blood_Burst_Omni");
        GameObject oBullet = GameObject.Instantiate(tModel) as GameObject;
        return oBullet;
    }
    public GameObject f_CreateFX_Terrain(){
        UnityEngine.Object tModel = Resources.Load("Model/HitEffect/WFXMR_BImpact Dirt + Hole");
        GameObject oBullet = GameObject.Instantiate(tModel) as GameObject;
        return oBullet;
    }
    public GameObject f_CreateFX_Wood(){
        UnityEngine.Object tModel = Resources.Load("Model/HitEffect/WFX_BImpact Wood");
        GameObject oBullet = GameObject.Instantiate(tModel) as GameObject;
        return oBullet;
    }
    public GameObject f_CreateFX_Metal(){
        UnityEngine.Object tModel = Resources.Load("Model/HitEffect/WFX_BImpact Metal");
        GameObject oBullet = GameObject.Instantiate(tModel) as GameObject;
        return oBullet;
    }



    public GameObject f_CreateBattleGameObj(string strName) {
        UnityEngine.Object tModel = Resources.Load("BattleGameObj/" + strName);
        if (tModel == null)
        {
            MessageBox.ASSERT("无此模型资源 " + "BattleGameObj/" + strName);
        }
        GameObject oGameObj = GameObject.Instantiate(tModel) as GameObject;
        oGameObj.name = strName;
        return oGameObj;
    }


    /// <summary>
    /// 创建角色,回收通过f_Destory
    /// </summary>
    /// <param name="iResId"></param>
    /// ======================================================================================
    public GameObject f_CreateRole2(CharacterDT tCharacterDT, bool bCreate = true)
    {
        if (tCharacterDT == null){
            MessageBox.ASSERT("Profab对应的角色脚本未找到");
            return null;
        }

        string ppSQL = string.Format("Role/{0}", tCharacterDT.szResName);
        if (!ccResourceManager.GetInstance().f_CheckIsHave(ppSQL)) {
            GameObject oProfab = (GameObject)Resources.Load(ppSQL);
            if (oProfab == null) {
                MessageBox.ASSERT("Profab没找到 " + ppSQL);
                return null;
            }
            GameObject oModel = (GameObject)GameObject.Instantiate(oProfab, Vector3.zero, Quaternion.identity);
            ccResourceManager.GetInstance().f_RegResource(ppSQL, oModel, null, null, true);           
        }

        if (!bCreate){
            return null;
        }

        GameObject oRole = ccResourceManager.GetInstance().f_Instantiate(ppSQL);
        oRole.transform.position = Vector3.zero;
        oRole.transform.rotation = Quaternion.identity;
        //MessageBox.DEBUG("f_CreateRole End");
        return oRole;
    }


    /// <summary>
    /// 创建的GameObject通过此方法进行回收
    /// </summary>
    /// <param name="Obj"></param>
    public void f_DestorySD(GameObject Obj)
    {
        try
        {
            if (!ccResourceManager.GetInstance().f_UnInstantiate(Obj))
            {
                GameObject.Destroy(Obj);
            }
        }
        catch 
        {
            GameObject.Destroy(Obj);
        }

    }

    
    /// <summary>
    /// 获取音效
    /// </summary>
    /// <param name="ButtleOrBg">按钮 或者背景音乐 0是按钮  1为特效 其他为背景音乐</param>
    /// <param name="MusicType">音乐类型</param>
    /// <returns></returns>
    public AudioClip f_GetAudioClip(int ButtleOrBg, int MusicType)
    {
        return null;
    }
    

#endregion



}

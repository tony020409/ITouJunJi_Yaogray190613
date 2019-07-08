using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ccU3DEngine;

public class Anim_Control : MonoBehaviour
{


    #region 參數區 =============================================
    //動畫用
    //[Header("放入要控制的動畫物件、特效產生位置")]
    //public Animator anim;  //用來控制其他物件的動畫
    //public Transform self; //特效產生位置

    //特效用
    [Header("放入特效物件")]
    public GameObject[] fxObj;

    //聲音用
    [Header("放入相關音檔 (播放一次用 ex:爆炸)")]
    public AudioClip[] clip;           //音檔
    private AudioSource audioOne;      //用PlayOneShot播放的 AudioSource

    [Header("放入相關音檔 (掛身上的AudioSource)")]
    public AudioSource[] audioX;       //直接丟 AudioSource 控制的 AudioSource
    #endregion


    // Start ===========================================
    void Start(){
        audioOne = this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 3D的聲音重置
    /// </summary>
    public void SoundInit() {
        for (int i = 0; i < audioX.Length; i++) {
            audioX[i].Stop();
        }
    }


    #region 動畫功能區 ==================================================================
    //動畫 Animation Key 事件，用來切換指定物件的動畫用 =================================
    //public void ChangeAnimation(string animName){
    //    anim.Play(animName);
    //}
    #endregion


    #region 聲音功能區 ==================================================================
    //動畫 Animation Key 事件，用 PlayOneShot播放某聲音一次 =============================
    public void SoundOne(int clipIndex)
    {
        if (clip.Length < clipIndex)
        {
            MessageBox.ASSERT("播放的声音越界 " + clipIndex + " / " + this.gameObject.name);
        }
        else
        {
            audioOne.PlayOneShot(clip[clipIndex]);
        }
    }

    //動畫 Animation Key 事件，用來播放身上 AudioSource的聲音 ===========================           
    public void PlaySound(int clipIndex)
    {
        if (clip.Length < clipIndex)
        {
            MessageBox.ASSERT("播放的声音越界 " + clipIndex);
        }
        else
        {
            audioX[clipIndex].Play();
        }
    }

    //動畫 Animation Key 事件，用來停止身上 AudioSource的聲音 =========================== 
    public void StopSound(int clipIndex)
    {
        if (clip.Length < clipIndex)
        {
            MessageBox.ASSERT("播放的声音越界 " + clipIndex);
        }
        else
        {
            audioX[clipIndex].Stop();
        }
    }


    //動畫 Animation Key 事件，用來開啟身上 AudioSource的聲音 ===========================           
    public void AudioOn(int clipIndex)
    {
        if (clip.Length < clipIndex)
        {
            MessageBox.ASSERT("播放的声音越界 " + clipIndex);
        }
        else
        {
            audioX[clipIndex].gameObject.SetActive(true);
        }
    }

    //動畫 Animation Key 事件，以八分之一機率用 PlayOneShot播放某聲音一次 =============================
    public void RandomSound(int clipIndex)
    {
        if (clip.Length < clipIndex)
        {
            MessageBox.ASSERT("播放的声音越界 " + clipIndex + " / " + this.gameObject.name);
        }
        else
        {
            int i = UnityEngine.Random.Range(1, 8);
            if (i == 1)
            {
                audioOne.PlayOneShot(clip[clipIndex]);
            }
        }
    }

    #region ____用創造 AudioSource的方式播聲音(已取消)___
    //動畫 Animation Key 事件，用來播放聲音 =============================================
    //public void cPlaySound(int clipIndex){
    //    CreateAudioSource(clipIndex);                                    //創立音檔(後來改直接放物件身上)
    //    audio[clipIndex].Play();                                         //播放音檔
    //    StartCoroutine(DestroyAudio(clip[clipIndex].length, clipIndex)); //播放結束後
    //}

    //動畫 Animation Key 事件，用來停止聲音 =============================================
    //public void cStopSound(int clipIndex){
    //    audioX[clipIndex].Stop();                                         //停止音檔
    //    Destroy(audio[clipIndex].gameObject, 1.0f);
    //}

    //創立音檔
    //void CreateAudioSource(int clipIndex){
    //    GameObject oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateAudioSource(); //產生播聲音物件
    //    oBullet.transform.parent = this.transform;                       //成為子物件
    //    oBullet.transform.position = Vector3.zero;                       //成為子物件
    //    oBullet.transform.rotation = Quaternion.identity;                //成為子物件
    //    audio[clipIndex] = oBullet.GetComponent<AudioSource>();          //獲取 AudioSource
    //    audio[clipIndex].clip = clip[clipIndex];                         //指定音檔
    //    audio[clipIndex].loop = true;                                    //預設是循環 (不循環的直接用AudioSource掛身上，或用這裡的Sound())
    //}

    //銷毀音檔 ==========================================================================
    //IEnumerator DestroyAudio(float delay, int index){
    //    yield return new WaitForSeconds(delay);
    //    Destroy(audio[index].gameObject, 1.0f);
    //}
    #endregion

    #endregion


    #region 特效產生區 ==================================================================
    //動畫 Animation Key 事件，用來關閉身上的特效物件 (像是煙塵) ========================
    public void ani_ShowFX(int objIndex)
    {
        fxObj[objIndex].SetActive(true);
    }

    //動畫 Animation Key 事件，用來關閉身上的特效物件 (像是煙塵) ========================
    public void ani_HideFX(int objIndex)
    {
        fxObj[objIndex].SetActive(false);
    }

    //動畫 Animation Key 事件，用來產生額外的特效物件 (像是爆炸) ========================
    public void ani_CreateFX(int objIndex)
    {
        GameObject oBullet = (GameObject)Instantiate(fxObj[objIndex]);
        oBullet.transform.position = this.transform.position;
    }
    #endregion


    #region 震動地板 ====================================================================

    public void getBuzzFloorAdapter()
    {
        if (BuzzFloorAdapter.instance != null)
        {
            StartCoroutine(BuzzFloorAdapter.instance.Playshock("013", 0.1f));
        }
    }

    public void FrankRun()
    {
        if (BuzzFloorAdapter.instance != null)
        {
            StartCoroutine(BuzzFloorAdapter.instance.Playshock("014", 0.1f));
        }

    }
    #endregion

}


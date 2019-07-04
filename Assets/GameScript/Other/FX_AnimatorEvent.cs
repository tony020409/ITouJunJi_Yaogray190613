using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_AnimatorEvent : MonoBehaviour {



    [HelpBox("Mv_Show() 開啟身上的某物件\n" +
             "Mv_Hide()   關閉身上的某物件\n" + 
             "(int參數填入物件編號)\n" +
             "_____________________\n" +
             "Mv_Sound() 播放聲音\n" +
             "(int參數填入聲音編號)\n"+
             "_____________________\n" +
             "Mv_Create() 產生特效\n" +
             "(int參數填入特效編號)",
        HelpBoxType.Info)]


    [Header("身上要開關的物件")]
    [Rename("物件編號")]
    public GameObject[] _Obj;

    [Header("要播放的聲音")]
    [Rename("聲音編號")]
    public AudioClip[] _Clip;
    private AudioSource _audio;

    [Header("要產生的特效")]
    [Rename("特效編號")]
    public GameObject[] _FX;



	// Use this for initialization
	void Start () {
        if (this.GetComponent<AudioSource>() != null) {
            _audio = this.GetComponent<AudioSource>();
        }
        else {
            this.gameObject.AddComponent<AudioSource>();
            _audio = this.GetComponent<AudioSource>();
        }
    }


    /// <summary>
    /// 顯示身上物件
    /// </summary>
    /// <param name="tmp"> 物件編號 </param>
    public void Mv_Show(int tmp) {
        if (_Obj[tmp] != null) {
            _Obj[tmp].SetActive(true);
        }
    }

    /// <summary>
    /// 關閉身上物件
    /// </summary>
    /// <param name="tmp"> 物件編號 </param>
    public void Mv_Hide(int tmp) {
        if (_Obj[tmp] != null) {
            _Obj[tmp].SetActive(false);
        }
    }


    /// <summary>
    /// 播放聲音
    /// </summary>
    /// <param name="tmp"> 聲音編號 </param>
    public void Mv_Sound(int tmp) {
        if (_Clip[tmp] != null) {
            _audio.PlayOneShot(_Clip[tmp], 1);
        }
    }


    /// <summary>
    /// 產生特效物件
    /// </summary>
    /// <param name="tmp"> 特效編號 </param>
    public void Mv_Create(int tmp) {
        if (_FX[tmp] != null) {
            Instantiate(_FX[tmp], transform.position, transform.rotation);
        }
    }


}

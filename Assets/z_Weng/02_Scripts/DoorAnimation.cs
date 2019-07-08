using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 開門的方式
/// </summary>
public enum EM_DoorAnimation
{
   Push,
   Slide,
}


[RequireComponent(typeof(AudioSource))]
public class DoorAnimation : MonoBehaviour {

    //[HideInInspector()]
    [Rename("打勾為開，沒勾為關")]
    public bool isOpen;

    [Rename("預設門一開始是打開的")]
    public bool OpenOnStart = false;

    [Rename("選擇開門的方式")]
    public EM_DoorAnimation DoorStyle;

    [Header("拉門")]
    public Transform SlideDoor;
    private Vector3 Origin_SliderDoor;

    [Header("推門的左右邊")]
    public Transform DoorL;
    public Transform DoorR;
    private Vector3 Origin_DoorL; //原始的朝向(左)
    private Vector3 Origin_DoorR; //原始的朝向(右)

    [Header("- [參數] -")]
    [Rename("開門的方向")]
    public Vector3 Dir = new Vector3(0, 90 ,0); //推門的方向

    [Rename("開關的速度 (越小越快,1=1秒完成)")]
    public float OpenTime = 1.0f;

    [Header("- [音效] -")]

    [Rename("開門聲 (Open)")]
    public AudioClip DoorSound_Open;

    [Rename("關門聲 (Close)")]
    public AudioClip DoorSound_Close;

    /// <summary>
    /// 播聲音用
    /// </summary>
    private AudioSource _audioSource;


    // Use this for initialization
    void Start ()
    {
        if (DoorStyle == EM_DoorAnimation.Slide) {
            Origin_SliderDoor = SlideDoor.localPosition;
        }
        if (DoorStyle == EM_DoorAnimation.Push){
            if (DoorL != null) {
                Origin_DoorL = DoorL.localEulerAngles;
            }
            if (DoorR != null) {
                Origin_DoorR = DoorR.localEulerAngles;
            }
        }

        //開場開門
        if (OpenOnStart) {
            OpenDoor(false);
        }

        _audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.KeypadPeriod) && Input.GetKeyDown(KeyCode.Keypad5)){
            OpenDoor();
        }
        if (Input.GetKey(KeyCode.KeypadPeriod) && Input.GetKeyDown(KeyCode.Keypad6)){
            CloseDoor();
        }
    }



    /// <summary>
    /// 開門
    /// </summary>
    /// <param name="playAudio"> 是否播放開門聲 </param>
    public void OpenDoor(bool playAudio = true) {
        //如果門打開了，就不能再開門
        if (isOpen) {
            return;
        }
        if (DoorStyle == EM_DoorAnimation.Slide) {
            if (SlideDoor != null) {
                SlideDoor.DOLocalMove(SlideDoor.localPosition + Dir, OpenTime);
            }
        }
        if (DoorStyle == EM_DoorAnimation.Push) {
            if (DoorL != null) {
                DoorL.DOLocalRotate(DoorL.localEulerAngles + Dir, OpenTime);
            }
            if (DoorR != null) {
                DoorR.DOLocalRotate(DoorR.localEulerAngles - Dir, OpenTime);
            }
        }
        isOpen = true;
        if (DoorSound_Open!=null) {
            if (playAudio) {
                if (this.gameObject.activeInHierarchy) {
                    if (_audioSource != null){
                        _audioSource.PlayOneShot(DoorSound_Open);
                    }
                }
            }
        }
    }


    /// <summary>
    /// 關門
    /// </summary>
    public void CloseDoor(bool playAudio = true) {
        //如果門關閉了，就不能再關門
        if (!isOpen) {
            return;
        }
        if (DoorStyle == EM_DoorAnimation.Slide){
            if (SlideDoor != null) {
                SlideDoor.DOLocalMove(Origin_SliderDoor, OpenTime);
            }
        }
        if (DoorStyle == EM_DoorAnimation.Push) {
            if (DoorL != null) {
                DoorL.DOLocalRotate(DoorL.localEulerAngles - Dir, OpenTime);
            }
            if (DoorR != null) {
                DoorR.DOLocalRotate(DoorR.localEulerAngles + Dir, OpenTime);
            }
        }
        isOpen = false;
        if (DoorSound_Close != null) {
            if (playAudio) {
                if (this.gameObject.activeInHierarchy) {
                    _audioSource.PlayOneShot(DoorSound_Close);
                }
            }
        }
    }



}

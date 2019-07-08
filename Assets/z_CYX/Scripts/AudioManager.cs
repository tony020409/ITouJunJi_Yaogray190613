using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager _ins;
    private AudioSource _audioSource;
    List<AudioInfo> _AudioInfo = new List<AudioInfo>();

    void Awake () {
        _ins = this;
        _audioSource = this.GetComponent<AudioSource>();
    }

    void FixedUpdate () {
        //Debug.LogWarning(_AudioInfo.Count);

        //for (int i = 0; i < _AudioInfo.Count; i++) {
        //    if (Time.time >= _AudioInfo[i]._AudioLength + _AudioInfo[i]._AudioStartTime) {
        //        Destroy(_AudioInfo[i]._GameObject);
        //        _AudioInfo.RemoveAt(i);
        //        i--;
        //    }
        //}
    }



    public void PlayOneShot(AudioClip tClip, float volume = 1, string DebugWhoCall = "") {
        if (tClip == null) {
            Debug.LogWarning(DebugWhoCall + " 播放空音效");
            return;
        }
        if (volume > 1) {
            volume = 1;
        }
        if (volume < 0) {
            volume = 0;
        }
        _audioSource.PlayOneShot(tClip, volume);
    }



    public void PlayAudioClip3D (string _WhoCall, Vector3 _ObjectPosition, AudioClip _AudioClip, bool _Is3D = false, float _AudioVolume = 1) {
        if (_AudioClip == null) {
            return;
        }

        //
        //GameObject _GameObject = new GameObject("AudioPlaying:" + _AudioClip.name + "-" + _WhoCall);
        //_GameObject.transform.position = _ObjectPosition;
        //AudioSource _AudioSource = _GameObject.AddComponent<AudioSource>();
        //
        //_AudioSource.clip = _AudioClip;
        //_AudioSource.volume = _AudioVolume;
        //_AudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        //_AudioSource.minDistance = 0.5f;
        //_AudioSource.maxDistance = 13f;
        //if (_Is3D)
        //{
        //    _AudioSource.spatialBlend = 1f;
        //}
        //_AudioSource.Play();
        //
        //RegistAudio(_AudioClip, _GameObject);
    }

    public void PlayAudioClip (string _WhoCall, Vector3 _ObjectPosition, string _AudioName, float _AudioVolume = 1)
    {
        AudioClip _AudioClip = null;
        _AudioClip = Resources.Load<AudioClip>(_AudioName);

        if (_AudioClip == null)
        {
            Debug.LogWarning("找無音效:" + _AudioName + "-" + _WhoCall);
            return;
        }

        GameObject _GameObject = new GameObject("AudioPlaying:" + _AudioName + "-" + _WhoCall);
        _GameObject.transform.position = _ObjectPosition;
        AudioSource _AudioSource = _GameObject.AddComponent<AudioSource>();

        _AudioSource.clip = _AudioClip;
        _AudioSource.volume = _AudioVolume;
        _AudioSource.Play();

        RegistAudio(_AudioClip, _GameObject);
    }

    public void RegistAudio (AudioClip _AudioClip, GameObject _GameObject) {
        if (_AudioClip == null)
            return;

        _AudioInfo.Add(new AudioInfo(_AudioClip.name, _AudioClip.length, Time.time, _GameObject));
    }

    public void ClearRegistAudio () {
        for (int i = 0; i < _AudioInfo.Count; i++) {
            Destroy(_AudioInfo[i]._GameObject);
            _AudioInfo.RemoveAt(i);
            i--;
        }
    }
}

class AudioInfo {
    public string _AudioName;       // 音效名稱
    public float _AudioLength;      // 音效長度
    public float _AudioStartTime;   // 開始時間
    public GameObject _GameObject;  // 音效物件

    public AudioInfo (string _AudioName, float _AudioLength, float _AudioStartTime, GameObject _GameObject) {
        this._AudioName = _AudioName;
        this._AudioLength = _AudioLength;
        this._AudioStartTime = _AudioStartTime;
        this._GameObject = _GameObject;
    }
}

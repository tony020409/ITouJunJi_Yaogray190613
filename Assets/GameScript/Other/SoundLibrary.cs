using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour {


    AudioSource _AudioSource;


    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class SoundGroup {
        public string groupName;
        public AudioClip[] group;
    }

    /// <summary>
    /// 
    /// </summary>
    public SoundGroup[] soundGroup;

    /// <summary>
    /// 
    /// </summary>
    Dictionary<string, AudioClip[]> SoundLib = new Dictionary<string, AudioClip[]>();
    void Awake() {
        for (int i = 0; i< soundGroup.Length; i++) {
            SoundLib.Add(soundGroup[i].groupName, soundGroup[i].group);
        }
        _AudioSource = this.GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

       
    /// <summary>
    /// 取得某群組裡的隨機一個音樂
    /// </summary>
    /// <param name="groupName"> 群組名稱 </param>
    public AudioClip Lib_GetRandomClip(string groupName) {
        if (SoundLib.ContainsKey(groupName)) {
            AudioClip[] sounds = SoundLib[groupName];
            return sounds[Random.Range(0, sounds.Length)];
        }
        return null;
    }


    /// <summary>
    /// 播放某群組裡的某首歌
    /// </summary>
    /// <param name="groupName"> 群組名稱 </param>
    /// <param name="clipName" > 音檔名稱 </param>
    /// <param name="volume"   > 音量 </param>
    public void Lib_PlayOneShot(string groupName, string clipName, float volume) {
        if (SoundLib.ContainsKey(groupName)) {
            for (int i = 0; i < SoundLib[groupName].Length; i++) {
                if (SoundLib[groupName][i].name == clipName) {
                    _AudioSource.PlayOneShot(SoundLib[groupName][i]);
                    break;
                }
            }
        }
    }





}

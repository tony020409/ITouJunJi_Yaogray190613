using UnityEngine;



/// <summary> 音效配置文件 </summary>
public class AudioEvent : ScriptableObject {

    /// <summary> 播放 </summary>
    /// <param name="source"> 播放器 </param>
	public virtual void Play(AudioSource source) {

    }


    /// <summary> 播放一次 </summary>
    /// <param name="source"> 播放器 </param>
    public virtual void PlayOneShot(AudioSource source) {

    }


}
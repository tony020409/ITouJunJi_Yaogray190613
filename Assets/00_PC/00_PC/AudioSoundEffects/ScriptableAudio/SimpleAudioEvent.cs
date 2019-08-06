using UnityEngine;
using Random = UnityEngine.Random;


/// <summary> 基本音效配置檔 </summary>
[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent {

    /// <summary> 音源集</summary>
	public AudioClip[] clips;

    /// <summary> 聲音範圍 </summary>
	public RangedFloat volume;

    /// <summary> pitch 範圍 </summary>
	[MinMaxRange(0, 2)]
	public RangedFloat pitch;


    /// <summary> 播放 </summary>
    /// <param name="source"> 負責播放聲音的AudioSource </param>
    public override void Play(AudioSource source) {
        if (!CanPlay(source)) {
            return;
        }
        source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.minValue, volume.maxValue);
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.Play();
	}


    /// <summary> 播放一次 </summary>
    /// <param name="source"> 負責播放聲音的AudioSource </param>
    public override void PlayOneShot(AudioSource source) {
        if (!CanPlay(source)) {
            return;
        }
        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.PlayOneShot(source.clip, source.volume);
    }



    /// <summary> 檢查播放條件是否達成 </summary>
    /// <param name="source"> 負責播放聲音的AudioSource </param>
    private bool CanPlay(AudioSource source) {
        if (clips.Length == 0) {
            return false;
        }
        if (source == null) {
            return false;
        }
        return true;
    }



}
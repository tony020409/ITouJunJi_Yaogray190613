using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_SoundPlayer : MonoBehaviour {

    AudioClip clip = null;         //播放的音檔
    public  bool audioEnd = false; //判斷聲音是否結束      
    private new AudioSource audio; //播放聲音用


    // Use this for initialization
    void Start(){
    }


    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clipPath"> 音效在 Resource下的路徑 </param>
    /// <param name="cVolume"> 音量 </param>
    public void s_Play(string clipPath, float cVolume){
        audio = this.GetComponent<AudioSource>();
        clip  = (AudioClip) Resources.Load(clipPath, typeof(AudioClip));

        if (audio == null) {
            MessageBox.ASSERT("p_SoundPlayer.cs AudioSource 獲取失敗!");
        }
        if (clip == null)  {
            if (BattleMain.GetInstance().Debug_Object_isNull) {
                Debug.LogWarning("p_SoundPlayer.cs 音檔獲取失敗! / " + clipPath + " / 看看是不是要播放的音檔不在 Resources資料夾裡或路徑錯誤");
            }
            EndSound();
            return;
        }

        audio.clip = clip;
        audio.volume = cVolume;
        audio.Play();
        Invoke("EndSound", clip.length);
    }

    public void EndSound(){
        audioEnd = true;
        Destroy(gameObject,5.0f);
    }


}

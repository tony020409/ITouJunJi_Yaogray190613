using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Sound : MonoBehaviour {

    [Header("會隨機挑一首播放")]
    public AudioClip[] clip;
    private new AudioSource audio;

    [Header("是否延遲播放、延遲多久")]
    public bool useDelay;
    public float dTime;

    void Awake(){
        audio = this.GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        if (!useDelay)
        { PlayRandomSound(); }
        else
        { Invoke("PlayRandomSound", dTime); }
    }

    void PlayRandomSound(){
        audio.PlayOneShot(clip[Random.Range(0, clip.Length)], 1f);
    }
}

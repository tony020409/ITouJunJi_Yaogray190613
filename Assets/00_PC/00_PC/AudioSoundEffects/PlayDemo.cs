using UnityEngine;

public class PlayDemo : MonoBehaviour
{

    //dada
    public AudioSource m_ExplosionAudio;
    [Header("Tank")]
    public AudioEvent m_ExplosionAudioEvent1;
    [Header("Footstep")]
    public AudioEvent m_ExplosionAudioEvent2;
    [Header("Dog")]
    public AudioEvent m_ExplosionAudioEvent3;
    [Header("Sheep")]
    public AudioEvent m_ExplosionAudioEvent4;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            m_ExplosionAudioEvent1.Play(m_ExplosionAudio);
        }
        if (Input.GetKeyDown("2"))
        {
            m_ExplosionAudioEvent2.Play(m_ExplosionAudio);
        }
        if (Input.GetKeyDown("3"))
        {
            m_ExplosionAudioEvent3.Play(m_ExplosionAudio);
        }
        if (Input.GetKeyDown("4"))
        {
            m_ExplosionAudioEvent4.Play(m_ExplosionAudio);
        }
    }
}

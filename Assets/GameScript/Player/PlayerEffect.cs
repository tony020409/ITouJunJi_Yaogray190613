using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using UnityEngine;
using UnityEngine.UI;

namespace Sam
{
    #region Class
    /// <summary>
    /// 玩家效果  音效  画面效果
    /// </summary>
    public class PlayerEffect : MonoBehaviour
    {

        public Soundstruct m_Soundstruct;
        public static PlayerEffect _Instance;
        void Awake(){
            _Instance = this;
        }

        private void Update() {
            
        }


        /// <summary>
        /// 声音效果
        /// </summary>
        public class SoundEffect {
            public SoundEffect(SoundEM S, ref AudioSource audioSource) {
                switch (S) {
                    case SoundEM.Idle:
                        audioSource.PlayOneShot(_Instance.m_Soundstruct.Idle, 1f);
                        break;
                    case SoundEM.hit:
                        if (_Instance.m_Soundstruct.playerhitSound.hit.Length > 0){
                            audioSource.PlayOneShot(_Instance.m_Soundstruct.playerhitSound.hit[Random.Range(0, _Instance.m_Soundstruct.playerhitSound.hit.Length)], 1f);
                        }
                        break;
                    case SoundEM.Attack:
                        //audioSource.PlayOneShot(_Instance.m_Soundstruct.Attack, 1f);
                        //_Instance.m_Soundstruct.Ani_Attack1.Play();
                        break;
                    case SoundEM.Death:
                        audioSource.PlayOneShot(_Instance.m_Soundstruct.Death, 1f);
                        break;
                    case SoundEM.Nobullet:
                        audioSource.PlayOneShot(_Instance.m_Soundstruct.Nobullet, 1f);
                        break;
                    case SoundEM.Changeclip:
                        audioSource.PlayOneShot(_Instance.m_Soundstruct.Changeclip, 1f);
                        break;
                    case SoundEM.Stop:
                        _Instance.m_Soundstruct.Ani_Attack1.Stop();
                        break;

                    //被各種怪物擊中的特殊音效 ============================================================
                    case SoundEM.hit_fromPter:    //被翼手龍攻擊
                        if (_Instance.m_Soundstruct.playerhitSound.hit.Length > 0){
                            audioSource.PlayOneShot(_Instance.m_Soundstruct.playerhitSound.hit[1], 1f);
                        }
                        break;
                    case SoundEM.hit_fromRaptor:  //被迅猛龍攻擊
                        if (_Instance.m_Soundstruct.playerhitSound.hit.Length > 0){
                            audioSource.PlayOneShot(_Instance.m_Soundstruct.playerhitSound.hit[2], 1f);
                        }
                        break;


                    default:
                        break;
                }
            }
        }


        /// <summary> 
        /// 画面效果
        /// </summary>
        public class ScreenEffect {
            public ScreenEffect(ScreenEM S, MySelfPlayerControll2 M) {
                switch (S) {
                    case ScreenEM.Idle:
                        break;
                    case ScreenEM.hit:
                        M.screen_Hit.GetComponent<Animator>().Play("Hit");
                        break;
                    case ScreenEM.Death:
                        break;
                    default:
                        break;
                }
            }

            private void PlayerScreen(GameObject G){
                G.SetActive(false);
                G.SetActive(true);
            }

        }
    }
    #endregion



    #region Struct
    /// <summary>
    /// 玩家被击中的音效  因为需要不一样的声音  所以才开结构出来
    /// </summary>
    [System.Serializable]
    public struct PlayerhitSound
    {
        public AudioClip[] hit;
    }



    [System.Serializable]
    public struct Soundstruct
    {
        /// <summary>
        /// 闲置
        /// </summary>
        public AudioClip Idle;
        /// <summary>
        /// 攻击
        /// </summary>
        public AudioClip Attack;
        /// <summary>
        /// 死亡
        /// </summary>
        public AudioClip Death;
        /// <summary>
        /// 没子弹
        /// </summary>
        public AudioClip Nobullet;
        /// <summary>
        /// 换弹夹
        /// </summary>
        public AudioClip Changeclip;
        /// <summary>
        /// 插件音效(開槍)
        /// </summary>
        public AimSound.GunSoundSource Ani_Attack1;
        // <summary>
        /// 插件音效(開槍)
        /// </summary>
        public AimSound.GunSoundSource Ani_Attack2;
        /// <summary>
        /// 击中
        /// </summary>
        public PlayerhitSound playerhitSound;
    }
    #endregion
}

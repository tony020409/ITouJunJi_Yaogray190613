using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sam
{

    /// <summary>
    /// 追瞄狀態
    /// </summary>
    public enum TrackEM
    {
        /// <summary>
        /// 追踪中
        /// </summary>
        IsTrack,
        /// <summary>
        /// 未追踪
        /// </summary>
        NotTrack,
        /// <summary>
        /// 追踪完毕
        /// </summary>
        Trackcomplete
    }
    public class Track : MonoBehaviour
    {
        public float TrackTime;
        public float NowTrackTime;
        public Image im;
        public Color TrackColor;
        public TrackEM trackEM=TrackEM.NotTrack;


        public Animator m_Animator;
        // Use this for initialization
        void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Animator.enabled = false;
            im.color = TrackColor;
        }
        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 追踪开始
        /// </summary>
        public void TracksStart()
        {
            trackEM = TrackEM.IsTrack;
            if (NowTrackTime <= TrackTime)
            {
                NowTrackTime += Time.deltaTime;
                display();
            }
            else
            {
                m_Animator.enabled = true;
                trackEM = TrackEM.Trackcomplete;
                if (GetComponent<Animator>()!=null)
                {
                    GetComponent<Animator>().enabled = true;
                }
                print("追瞄成功");
            }
        }
        /// <summary>
        /// 追踪停止
        /// </summary>
        public void TrackStop()
        {
            trackEM = TrackEM.NotTrack;
            m_Animator.enabled = false;
            im.color = TrackColor;
            NowTrackTime = 0;
            display();
        }
        /// <summary>
        /// 追踪UI显示
        /// </summary>
        public void display()
        {
            im.fillAmount = NowTrackTime / TrackTime;
        }

    }
}
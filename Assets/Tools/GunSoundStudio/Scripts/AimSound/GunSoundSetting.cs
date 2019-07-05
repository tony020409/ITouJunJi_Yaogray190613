using UnityEngine;

namespace AimSound
{
    public class GunSoundSetting : MonoBehaviour
    {
        AudioClipList audioClipList;
        public void InitCheck()
        {
            if(!audioClipList)
            {
                _Load();
            }
        }

        [ContextMenu("reload")]
        public void _Load()
        {
            audioClipList = ScriptableObject.CreateInstance<AudioClipList>();
            audioClipList.hideFlags = HideFlags.DontSave;
            audioClipList.Load(data);

            for(int i=0;i<sounds.Length;++i)
            {
                sounds[i].Load(audioClipList);
            }
        }
	    public float shotLoopInterval;
#if UNITY_EDITOR
        public bool needRepack = false;
        public void GetEditorClips()
        {
            if(audioClipList)
                DestroyImmediate(audioClipList);
            audioClipList = ScriptableObject.CreateInstance<AudioClipList>();
            audioClipList.hideFlags = HideFlags.DontSave;
            for(int i = 0;i<sounds.Length;++i)
            {
                sounds[i].GetEditorClips();
            }
        }
#endif

        [System.Serializable]
        public class GunSoundElement
        {
            public string name = "new";
            // public AudioSource audioSource;
#if UNITY_EDITOR
            public AudioClip[] loopEditorClips = new AudioClip[]{};
            public AudioClip[] endEditorClips = new AudioClip[]{};
            public void GetEditorClips()
            {
                this.loopClips = this.loopEditorClips;
                this.endClips = this.endEditorClips;
            }
#endif
            public AudioClip[] loopClips
            {
                get;
                private set;
            }
            public AudioClip[] endClips
            {
                get;
                private set;
            }
            public int[] loopClipIDs;
            public int[] endClipIDs;
            public bool useLowPassFilter;
            public bool isReverb;
            public float volume = 1;
            public AnimationCurve volumeCurve = AnimationCurve.Linear(0,1,1,0);
            public AnimationCurve spatialBlendCurve = AnimationCurve.Linear(0,0,1,1);
            public AnimationCurve lowPassFilterCurve = AnimationCurve.Linear(0,1,1,1);
            public float lowpassResonanceQ = 1;

            public void Load(AudioClipList audioClipList)
            {
                var audioClips = audioClipList.audioClips;
                // Clear(loopClips);
                // Clear(endClips);
                loopClips = new AudioClip[loopClipIDs.Length];
                endClips = new AudioClip[endClipIDs.Length];

                for(int i=0;i < loopClipIDs.Length ; ++i)
                    if(loopClipIDs[i]>=0)
                        loopClips[i] = audioClips[loopClipIDs[i]];

                for(int i=0;i < endClipIDs.Length ; ++i)
                    if(endClipIDs[i]>=0)
                        endClips[i] = audioClips[endClipIDs[i]];
            }
        }
        [HideInInspector]
        public byte[] data = new byte[]{};
        public GunSoundElement[] sounds;
        public ulong[] environments = new ulong[2];
        public ulong GetEnvironmentMask(EnvironmentType environmentType)
        {
            var index = (int)environmentType;
            if(index>=0 && index<environments.Length)
            {
                return environments[index];
            }
            return environments[0];
        }
        public bool isOneShot
        {
            get
            {
                return shotLoopInterval == 0;
            }
        }

    }
}
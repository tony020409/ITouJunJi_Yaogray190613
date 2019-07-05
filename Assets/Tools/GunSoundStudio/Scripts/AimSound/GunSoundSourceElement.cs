using UnityEngine;
using UnityEngine.Audio;

namespace AimSound
{
    [System.Serializable]
    internal class GunSoundSourceElement
    {
        GunSoundSetting soundSetting;
        public GunSoundSetting.GunSoundElement setting;
        public GunSoundSource source;
        public AudioSource loopAudioSource;
        public AudioSource endAudioSource;
        // public AudioSource lastEndAudioSource;
        public GameObject gameObject;
        
        float _maxEndSoundDuration;

        public float maxEndSoundDuration
        {
            get
            {
                return _maxEndSoundDuration;
            }
        }

        public AudioMixerGroup outputAudioMixerGroup
        {
            set
            {
                loopAudioSource.outputAudioMixerGroup = value;
                if(endAudioSource)
                    endAudioSource.outputAudioMixerGroup = value;
            }
        }
        public int priority
        {
            set
            {
                loopAudioSource.priority = value;
                if(endAudioSource)
                    endAudioSource.priority = value;
            }
        }
        public float maxDistance
        {
            set
            {
                loopAudioSource.maxDistance = value;
                if(endAudioSource)
                    endAudioSource.maxDistance = value;
            }
        }
        public float minDistance
        {
            set
            {
                loopAudioSource.minDistance = value;
                if(endAudioSource)
                    endAudioSource.minDistance = value;
            }
        }
        public float volume
        {
            set
            {
                var volume = setting.volume * value;
                loopAudioSource.volume = volume;
                if(endAudioSource)
                    endAudioSource.volume = volume;
            }
        }
        public bool isReverb
        {
            get
            {
                return setting.isReverb;
            }
        }
        public GunSoundSourceElement(GunSoundSetting soundSetting,GunSoundSetting.GunSoundElement setting,GunSoundSource source)
        {
            this.soundSetting = soundSetting;
            this.setting = setting;
            this.source = source;
            gameObject = new GameObject(setting.name);
            #if AimSoundInternal
            // gameObject.hideFlags = HideFlags.DontSave;
            #else
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            #endif
            gameObject.transform.SetParent(source.transform,false);
            loopAudioSource = CloneAudioSource(setting,gameObject);
            loopAudioSource.loop = true;
            var endClips = setting.endClips;
            for(int i=0;i<endClips.Length;++i)
                if(endClips[i])
                    _maxEndSoundDuration = Mathf.Max(_maxEndSoundDuration,endClips[i].length);

            if(setting.useLowPassFilter)
            {
                var audioLowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
                audioLowPassFilter.lowpassResonanceQ = setting.lowpassResonanceQ;
                audioLowPassFilter.customCutoffCurve = setting.lowPassFilterCurve;
            }
            // endAudioSource = CloneAudioSource(soundSetting.audioSource,gameObject);
        }
        public void UpdateLowPassFilter()
        {
            if(setting.useLowPassFilter)
            {
                var audioLowPassFilter = gameObject.GetComponent<AudioLowPassFilter>();
                if(!audioLowPassFilter)
                    audioLowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
                audioLowPassFilter.lowpassResonanceQ = setting.lowpassResonanceQ;
                audioLowPassFilter.customCutoffCurve = setting.lowPassFilterCurve;
            }
            else
            {
                var audioLowPassFilter = gameObject.GetComponent<AudioLowPassFilter>();
                if(audioLowPassFilter)
                    Object.DestroyImmediate(audioLowPassFilter);
            }
        }
        public void UpdateAudioVolume()
        {
            loopAudioSource.volume = setting.volume * source.volume;
        }
        public void UpdateAudioSource()
        {
            ParseAudioSource(setting,loopAudioSource,source.volume);
            UpdateLowPassFilter();
        }
        public void Destroy()
        {
#if UNITY_EDITOR
            if(Application.isEditor)
                Object.DestroyImmediate(gameObject);
            else
                Object.Destroy(gameObject);
#else
                Object.Destroy(gameObject);
#endif
        }
        public bool looping
        {
            get;
            private set;
        }

        public void PlayLoopSound(double startTime, float shotLoopInterval)
        {
            loopAudioSource.Stop();
            if(endAudioSource)
                source._FadeOutAndDestroyObject(endAudioSource,shotLoopInterval);
            endAudioSource = null;
            var clips = setting.loopClips;
            if(clips.Length==0)
                return;
            var clip = setting.loopClips[ Random.Range(0,clips.Length)];
		    var loopShotCount = Mathf.RoundToInt(clip? clip.length/shotLoopInterval : 1 );
            var startPosition = Random.Range(0,loopShotCount)*shotLoopInterval;
            loopAudioSource.clip = clip;
            loopAudioSource.time = startPosition;
            // Debug.Log("GunSoundSourceElement.PlayLoopSound loopAudioSource.PlayScheduled "+startTime
            // +" loopShotCount "+loopShotCount+" startPosition "+ startPosition
            // +" dspTime "+ AudioSettings.dspTime);
            loopAudioSource.PlayScheduled(startTime);
            looping = true;
        }

        public void MakeLoopEndSwitch(double switchTime)
        {
            MakeLoopOver(switchTime);
            endAudioSource = CreateAudioSourceObject(setting.endClips,"end");
            endAudioSource.PlayScheduled(switchTime);
            // Debug.Log("GunSoundSourceElement.MakeLoopEndSwitch endAudioSource.PlayScheduled "+switchTime+" dspTime "+ AudioSettings.dspTime);
        }
        public void MakeLoopOver(double switchTime)
        {
            loopAudioSource.SetScheduledEndTime(switchTime);
            looping = false;
        }
        public void StopEnd()
        {
            if(endAudioSource)
                endAudioSource.Stop();
        }
        
        AudioSource CreateAudioSourceObject(AudioClip[] clips,string name)
        {
            var gameObject = new GameObject(name);
            gameObject.transform.SetParent(this.gameObject.transform,false);
            var source = CloneAudioSource(setting,gameObject);
            if(clips.Length>0)
            {
                var clip = clips[ Random.Range(0,clips.Length)];
                source.clip = clip;
                if(setting.useLowPassFilter)
                {
                    var audioLowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
                    audioLowPassFilter.lowpassResonanceQ = setting.lowpassResonanceQ;
                    audioLowPassFilter.customCutoffCurve = setting.lowPassFilterCurve;
                }
            }
            return source;
        }
        AudioSource CreateAudioSource(AudioClip[] clips)
        {
            var source = CloneAudioSource(setting,gameObject);
            if(clips.Length>0)
            {
                var clip = clips[ Random.Range(0,clips.Length)];
                source.clip = clip;
            }
            return source;
        }
        public void PlayOneShotSound()
        {
            if(endAudioSource)
                source._FadeOutAndDestroyObject(endAudioSource,0.1f);
            endAudioSource = CreateAudioSourceObject(setting.endClips,"end");
            endAudioSource.Play();
            if(endAudioSource.clip)
                this.source._DestroyAudioSourceObject(endAudioSource.gameObject,endAudioSource.clip.length  + 0.1f);
            else
                this.source._DestroyAudioSourceObject(endAudioSource.gameObject,0.1f);
        }


        public AudioSource CloneAudioSource(GunSoundSetting.GunSoundElement from,GameObject to)
        {
            var audioSource = to.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.dopplerLevel = 0;
            ParseAudioSource(from,audioSource,source.volume);
            audioSource.outputAudioMixerGroup = source.outputAudioMixerGroup;
            audioSource.priority = source.priority;
            // audioSource.minDistance = source.minDistance;
            audioSource.maxDistance = source.maxDistance;
            return audioSource;
        }
        public static void ParseAudioSource(GunSoundSetting.GunSoundElement from,AudioSource to,float volume)
        {
            to.volume = from.volume * volume;
            to.rolloffMode = AudioRolloffMode.Custom;
            AddCurveEnd(from.volumeCurve);
            AddCurveEnd(from.spatialBlendCurve);
            AddCurveEnd(from.lowPassFilterCurve);
            to.SetCustomCurve(AudioSourceCurveType.CustomRolloff,from.volumeCurve);
            to.SetCustomCurve(AudioSourceCurveType.SpatialBlend,from.spatialBlendCurve);
        }

        static void AddCurveEnd(AnimationCurve curve)
        {
            var lastKeyframe = curve.keys[curve.length-1];
            if(lastKeyframe.time<=1f)
            {
                lastKeyframe.outTangent = 0;
                curve.RemoveKey(curve.length-1);
                var Keyframe = new Keyframe(1f,lastKeyframe.value,0f,0f);
                curve.AddKey(lastKeyframe);
                curve.AddKey(Keyframe);
            }
        }

    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace AimSound
{
	/// <summary>A sound player for gun sound</summary>
    public class GunSoundSource : MonoBehaviour
    {
		#region property setting
	    /// <summary>The sound setting will to be play</summary>
        public GunSoundSetting setting;
        [SerializeField]
        AudioMixerGroup _outputAudioMixerGroup;
	    /// <summary>The count of shot you want to play in continuous shots. 0 is infinite until you call stop </summary>
        public int shot = 0;
        [SerializeField]
        float _volume = 1;
        [SerializeField]
        public AnimationCurve rolloff = AnimationCurve.Linear(0,1,1,0);
        [SerializeField]
	    /// <summary>The volume of reverb sound in the gun sound source (0.0 to 1.0).</summary>
        float _reverbVolume = 1;

        [SerializeField]
        EnvironmentType _environmentType;
        [SerializeField]
        int _priority = 128;
        public bool playOnAwake;
        
        [SerializeField]
        float _maxDistance = 500f;
	    /// <summary>Destroy the game object after the sound play over</summary>
        public bool destroyAfterStop;
		#endregion

		#region property
        public AudioMixerGroup outputAudioMixerGroup
        {
            get
            {
                return _outputAudioMixerGroup;
            }
            set
            {
                _outputAudioMixerGroup = value;
                for(int i = 0;i<elements.Length;++i)
                    elements[i].outputAudioMixerGroup = value;
            }
        }
        public float volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = Mathf.Clamp01(value);
                UpdateVolume();
            }
        }
        public float reverbVolume
        {
            get
            {
                return _reverbVolume;
            }
            set
            {
                _reverbVolume = Mathf.Clamp01(value);
                UpdateVolume();
            }
        }
	    /// <summary>Include indoor and outdoor environment type, choose by where you play the gun sound</summary>
        public EnvironmentType environmentType
        {
            get
            {
                return _environmentType;
            }
            set
            {
                // EnvironmentTypeCrossFade(value);
                _environmentType = value;
            }
        }
        
        public int priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                for(int i = 0;i<elements.Length;++i)
                    elements[i].priority = value;
            }
        }

        public float maxDistance
        {
            get
            {
                return _maxDistance;
            }
            set
            {
                _maxDistance = value;
                for(int i = 0;i<elements.Length;++i)
                    elements[i].maxDistance = value;
            }
        }

        float _maxEndSoundDuration;
        public float maxEndSoundDuration
        {
            get
            {
                return _maxEndSoundDuration;
            }
        }
	    /// <summary>The interval between every shot, it would be zero in single shot gun</summary>
        public float shotLoopInterval
        {
            get
            {
                return setting.shotLoopInterval;
            }
        }
        bool _needStop = false;
	    /// <summary>Is it need called stop to stop play.
        ///  It would be true only when shotLoopInterval > 0 and shot == 0 in the playing sound.</summary>
        public bool needStop
        {
            get
            {
                return _needStop;
            }
        }
        #endregion

		#region function interface
	    /// <summary>Play the gun sound with shot value in member.</summary>
        public void Play()
        {
            Play(shot);
        }

	    /// <summary> Play the gun sound with shot value from parameter. </summary>
        public void Play(int shot)
        {
            if(_needStop)
                return;
            if(lastSetting!=setting)
            {
                SetSetting(setting);
            }
            if(setting)
            {
                startTime = AudioSettings.dspTime;
                var shotLoopInterval = setting.shotLoopInterval;
                // var environmentMask = setting.GetEnvironmentMask(environmentType);
                if(setting.shotLoopInterval>0)
                {
                    for(int i=0;i<elements.Length;++i)
                    {
                        if(_NeedPlayInCurrentEnvironmentType(i))
                            elements[i].PlayLoopSound(startTime,shotLoopInterval);
                    }
                    if(shot>0)
                    {
                        switchTime = startTime + shot*shotLoopInterval;
                        for(int i=0;i<elements.Length;++i)
                        {
                            if(_NeedPlayInCurrentEnvironmentType(i))
                                MakeLoopEndSwitch(elements[i],switchTime);
                        }
                        if(needDestroy)
                            Destroy(gameObject,shotLoopInterval*shot + maxEndSoundDuration+1f);
                    }
                    else
                    {
                        switchTime = double.MinValue;
                        _needStop = true;
                    }
                }
                else
                {
                    for(int i=0;i<elements.Length;++i)
                    {
                        if(_NeedPlayInCurrentEnvironmentType(i))
                            elements[i].PlayOneShotSound();
                    }
                    if(needDestroy)
                        Destroy(gameObject, maxEndSoundDuration+1f);
                }
                lastEnvironmentMask = setting.GetEnvironmentMask(environmentType);
                UpdateDistanceAndVolume();
            }
        }

	    /// <summary>Switch the sound to end sound at right time,only need to be call when needStop is true</summary>
        public void Stop(){
            if(!_needStop)
                return;
            _needStop = false;

            var shotLoopInterval = setting.shotLoopInterval;
            var dspTime = AudioSettings.dspTime;
            double time = dspTime - startTime ;
            var index = (int)(time/shotLoopInterval);
            switchTime = startTime + (index+1)*shotLoopInterval;
            // print(" startTime "+startTime+" index "+(index+1)+" shotLoopInterval "+shotLoopInterval 
            // +" ->switchTime "+switchTime);
            for(int i=0;i<elements.Length;++i)
            {
                if( (lastEnvironmentMask & (1ul<<i))!=0 )
                    MakeLoopEndSwitch(elements[i],switchTime);
                else if(elements[i].looping)
                {
                    elements[i].MakeLoopOver(switchTime);
                }
            }
            if(needDestroy)
            {
                Destroy(gameObject, (float)(switchTime - dspTime) + maxEndSoundDuration + 1f);
            }
        }
        #endregion
		#region private


        float lastElementVolume = 1f;
        GunSoundSourceElement[] elements = new GunSoundSourceElement[0]{};
        float lastDistanceFromListener;
        ulong lastEnvironmentMask;
        double switchTime;
        GunSoundSetting lastSetting;
        double startTime;
        static AudioListener _audioListener;

        AudioListener audioListener
        {
            get
            {
                if(!_audioListener ||! _audioListener.gameObject.activeSelf)
                {
                    _audioListener = (AudioListener)GameObject.FindObjectOfType(typeof(AudioListener));
                }
                return _audioListener;
            }
        }

        bool needDestroy
        {
            get
            {
                #if UNITY_EDITOR
                return destroyAfterStop && Application.isPlaying;
                #else
                return destroyAfterStop;
                #endif
            }
        }
        void OnValidate()
        {
            outputAudioMixerGroup = _outputAudioMixerGroup;
            priority = _priority;
            // minDistance = _minDistance;
            maxDistance = _maxDistance;
            _reverbVolume = Mathf.Clamp01(_reverbVolume);
            volume = _volume;
        }
        void Awake()
        {
            if(setting)
                setting.InitCheck();
            if(playOnAwake)
                Play();
        }

        public void UpdateDistanceAndVolume()
        {
            var audioListener = this.audioListener;
            if(audioListener)
            {
                lastDistanceFromListener = (audioListener.transform.position - transform.position).magnitude;
                UpdateVolume();
            }
        }
        void Update()
        {
            UpdateDistanceAndVolume();
        }
        void UpdateVolume()
        {
            var normalized = Mathf.Clamp01( lastDistanceFromListener / maxDistance );
            var volume = _volume * rolloff.Evaluate(normalized);
            for(int i = 0;i<elements.Length;++i)
            {
                if(elements[i].isReverb)
                    elements[i].volume = volume * reverbVolume;
                else
                    elements[i].volume = volume;
            }
            lastElementVolume = volume;
        }

        public bool _NeedPlayInEnvironmentType(EnvironmentType environment, int index)
        {
            return (setting.environments[(int)environment]  & (1ul<<index))!=0;;
        }
        public bool _NeedPlayInCurrentEnvironmentType(int index)
        {
            return _NeedPlayInEnvironmentType(_environmentType,index);
        }
        void MakeLoopEndSwitch(GunSoundSourceElement element, double switchTime)
        {
            element.MakeLoopEndSwitch(switchTime);
            if(element.isReverb)
                element.volume = lastElementVolume * reverbVolume;
            else
                element.volume = lastElementVolume;
            element.priority = _priority;
            element.maxDistance = _maxDistance;
            element.outputAudioMixerGroup = _outputAudioMixerGroup;
        }
        void SetSetting(GunSoundSetting setting)
        {
            lastSetting = setting;
            for(int i=0;i<elements.Length;++i)
            {
                elements[i].Destroy();
            }
            if(setting)
            {
                if(setting)
                    setting.InitCheck();
                var sounds = setting.sounds;
                elements = new GunSoundSourceElement[ sounds.Length ];
                _maxEndSoundDuration = 0;
                for(int i=0;i<sounds.Length;++i)
                {
                    elements[i] = new GunSoundSourceElement(setting, sounds[i],this);
                    _maxEndSoundDuration = Mathf.Max(_maxEndSoundDuration,elements[i].maxEndSoundDuration);
                }
            }
        }
        public void _Clear()
        {
            Stop();
            SetSetting(null);
        }
        public AudioSource _UpdateElementAudioSource(int index)
        {
            if(index>=0 && index<elements.Length)
            {
            // print("_UpdateElement "+index);
                elements[index].UpdateAudioSource();
                return elements[index].loopAudioSource;
            }
            return null;
        }

        public void _UpdateElementAudioVolume(int index){
            if(index>=0 && index<elements.Length){
                elements[index].UpdateAudioVolume();
            }
        }

        public void _UpdateElementLowpassFilter(int index){
            if(index>=0 && index<elements.Length){
            // print("_UpdateElement "+index);
                elements[index].UpdateLowPassFilter();
            }
        }


        //---------------------------------------------------------------------
        public void _DestroyAudioSourceObject(GameObject source,float time){
            #if UNITY_EDITOR
            if(Application.isPlaying)
                Destroy(source,time);
            else
                StartCoroutine(_DestroyInTime(source,time));
            #else
            Destroy(source,time);
            #endif
        }
        IEnumerator _DestroyInTime(GameObject source,float time){
            yield return new WaitForSeconds(time);
            DestroyImmediate(source);
        }

        //-------------------------------------------------------------------
        public void _FadeOutAndDestroyObject(AudioSource source,float time){
            this.StartCoroutine(_FadeOut(source,time,true));
        }
        IEnumerator _FadeOut(AudioSource source,float time,bool destroy){
            var volume = source.volume;
            var startTime = Time.time;
            var finalTime = startTime + time;
            while(Time.time<finalTime)
            {
                if(!source)
                    yield break;
                source.volume = (finalTime - Time.time)/time * volume;
                yield return null;
            }

            if(!source)
                yield break;
            source.volume = 0;
            if(destroy)
            {
                #if UNITY_EDITOR
                if(Application.isPlaying)
                    Destroy(source.gameObject);
                else
                    DestroyImmediate(source.gameObject);
                #else
                Destroy(source.gameObject);
                #endif
            }
        }
        
        void OnDrawGizmosSelected(){
            Gizmos.color = new Color(0.5f,0.5f,0.8f,0.9f);
            Gizmos.DrawWireSphere(transform.position,maxDistance);
        }
        #endregion
    }

}
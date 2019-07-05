using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDemo:MonoBehaviour
{
    public AimSound.GunSoundSource gunSoundSource;
    public UIGunButton gunButtonPrefab;
    public float maxListenDistance = 500;
    public Transform gunTransfrom;
    public UICoordinateDrag coordinateDrag;
    public AimSound.GunSoundSetting[] gunSettings;
    public GameObject shotCountObject;
    public Text shotCountText;
    public Slider shotCountSlider;
    public ToggleGroup toggleGroup;
    public Toggle[] environmentToggles;
    List<UIGunButton> gunButtons = new List<UIGunButton>();

    //-----------------------------------------------------------
    public void OnShotCountSliderChanged(int count)
    {
        gunSoundSource.shot = count;
        if(count == 0)
        {
            shotCountText.text = "∞";
        }
        else
        {
            shotCountText.text = count.ToString();
        }
    }
    public RectTransform gunButtonParent;
    void AddGunButtons(AimSound.GunSoundSetting[] settings)
    {
        for(int i=0;i<settings.Length;++i)
            AddGunButton(settings[i]);
    }
    void AddGunButton(AimSound.GunSoundSetting setting)
    {
        var button = Instantiate(gunButtonPrefab);
        button.text.text = setting.name;
        button.transform.SetParent(gunButtonParent);
        button.toggle.group = toggleGroup;
        button.toggle.onValueChanged.AddListener( (value)=>
        {
            SetGun(setting);
        });
        gunButtons.Add(button);
    }
    void Awake()
    {
        AddGunButtons(gunSettings);
        shotCountSlider.onValueChanged.AddListener((f)=> OnShotCountSliderChanged((int)f) );
        coordinateDrag.onPositionChanged=(position)=>gunTransfrom.localPosition = position*maxListenDistance*2;
    }
    void Start()
    {
        if(gunButtons.Count>0)
        {
            var selection = gunButtons[0];
            selection.toggle.isOn = true;
            toggleGroup.NotifyToggleOn(selection.toggle);

            SetGun(gunSettings[0]);
        }
        for(int i=0;i < environmentToggles.Length;++i)
        {
            var index = i;
            environmentToggles[index].onValueChanged.AddListener((f)=>{
                if(f)
                    gunSoundSource.environmentType = (AimSound.EnvironmentType)index;
            });
        }
    }
    void SetGun(AimSound.GunSoundSetting setting)
    {
        gunSoundSource.setting = setting;
        shotCountObject.SetActive(!setting.isOneShot);
    }
	public float nextAvailableTime;
    public bool playing;
    
	void Update () 
	{
		if(!playing && Input.GetKey(KeyCode.Space) &&  Time.time > nextAvailableTime)
		{
            playing = true;
			if(gunSoundSource.shot>0)
			{
				nextAvailableTime = gunSoundSource.setting.shotLoopInterval * gunSoundSource.shot + Time.time;
			}
			gunSoundSource.Play();
		}
		if(playing && !Input.GetKey(KeyCode.Space))
		{
            playing = false;
			gunSoundSource.Stop();
		}
	}
}
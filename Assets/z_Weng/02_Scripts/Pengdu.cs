using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pengdu : MonoBehaviour {

    [TextArea(1, 2)]
    public string Note =
        "這程式是用來自動播放旁白\n" +
        "每唸完一句就換下一個句子";


	[Header("#顯示是否開啟了對話、對話的序號")]
	public bool CanTalk = true;    //是否可以下一句對話
	private bool startNext = true; //是否顯示下一句對話
	[SerializeField] 
	private int index = 0;         //對話序號
    private AudioSource audio;     //播放聲音用


	//[Header("#這裡放入對話語音")]
	[System.Serializable]
	public class MessageBoxSetting{
        //public bool  useAuto;
		//public float TextTime;
		public AudioClip PSound; 
	}
	[Header("#這裡放入對話語音")]
	public List<MessageBoxSetting> diaSet = new List<MessageBoxSetting>();

	[Header("●對話結束後執行的行為")]
	public GameObject[] ObjectToStart;

    // ======================================================
	void Start (){ audio = this.GetComponent<AudioSource>();}

	void Update (){
		if (CanTalk && startNext) {
			StartCoroutine ("NextText");
			startNext = false;
		}
	}


    //開啟對話 =============================================
    public void OpenTalk()
    { CanTalk = true; }


	//播放對話 =============================================
	IEnumerator NextText()
	{
		//未播放完 ----------------------------------
		if (index < diaSet.Count){
            audio.clip = diaSet[index].PSound;
            audio.Play();
            //audio.PlayOneShot(diaSet[index].PSound,0.9F);
            yield return new WaitForSeconds(audio.clip.length);
            //yield return new WaitForSeconds(diaSet[index].TextTime);
            index++;
			startNext = true;
		}

		//播放完所有對話 ----------------------------
		if (index == diaSet.Count){
            audio.clip = null;
            MessageBox.DEBUG("語音結束");
            for (int i = 0; i < ObjectToStart.Length; i++) {
				ObjectToStart [i].SetActive(true); //開啟所有想要開啟的東西
				if (i == ObjectToStart.Length)     //所有東西都開啟後
                { Destroy (gameObject);}           //銷毀自己
			}
		}
	}	
}
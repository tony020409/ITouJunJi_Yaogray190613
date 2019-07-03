using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI; //使用Unity UI程式庫。

public class time : MonoBehaviour {

	int time_int = 0;

	public Text time_UI;

	void Start(){


		InvokeRepeating("timer", 0.5f, 0.02f);

	}

	void timer(){

		time_int += 1;

		time_UI.text = time_int + ""+"%";

		if (time_int == 101) {

			time_UI.text = "READY!";

			CancelInvoke("timer");


		}

	}

}
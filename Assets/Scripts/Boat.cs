using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {
	float orgY = 0;
	public float waveSpeed = 1.7f;
	public float moveDis = 0.1f;
  public float LeftRight = 1.0f;
	// Use this for initialization
	void Start () {
		orgY = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(transform.localPosition.x, orgY+moveDis*Mathf.Sin(Time.time*waveSpeed), transform.localPosition.z);
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y, LeftRight * Mathf.Sin(Time.time*(waveSpeed)+1.5f));
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGunBullets : MonoBehaviour {

	public GameObject pistolShell;
	public GameObject pistolShellPoint;

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space))
		{
			CreateBullets ();
			animator.Play ("Shoot",-1,0f);
		}
	}

	public void CreateBullets()
	{
		GameObject _pistolShell = Instantiate(pistolShell,pistolShellPoint.transform.position,pistolShell.transform.rotation) as GameObject;

		Destroy(_pistolShell,2.0f);
	}

}

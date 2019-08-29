using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tree_lightevent : MonoBehaviour {

    public GameObject light_image;
    public Transform camerarig;
    public float a;
    public float b;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void light_open()
    {
        light_image.SetActive(true);
        camerarig.DOShakePosition(a, b);
    }
}

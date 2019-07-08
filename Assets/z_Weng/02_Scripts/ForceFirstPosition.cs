using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFirstPosition : MonoBehaviour
{
    [Header("要改變的項目")]
    public bool usePosition  = true;
    public bool useDirection = true;

    [Header("強制移動出生時的位置、轉向")]
    public Vector3 FirstPosition;
    public Vector3 FirstDirection;

    [Header("??????")]
    public AudioSource _AudioSource;
    public float f;

    //=======================================================
    void Start () {

        //改變初始位置
        if (usePosition){
            this.transform.position = FirstPosition;
        }

        //改變初始朝向
        if (useDirection){
            this.transform.eulerAngles = FirstDirection;
        }
        
        //
        if (_AudioSource!=null){
            StartCoroutine(PlayAudioSource());
        }
    }

    //======================================================
    public IEnumerator PlayAudioSource()
    {
        yield return new WaitForSeconds(f);
        _AudioSource.Play();

    }
}

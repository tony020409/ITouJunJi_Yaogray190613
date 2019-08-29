using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl_OnEnable : MonoBehaviour {

    public float _time;

    void OnEnable () {
        //Debug.Log("test");
        StartCoroutine(UnEnable());
    }

    IEnumerator UnEnable () {
        yield return new WaitForSeconds(_time);

        gameObject.SetActive(false);
    }
}

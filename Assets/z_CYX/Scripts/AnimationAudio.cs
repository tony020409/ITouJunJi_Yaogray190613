using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudio : MonoBehaviour {

    public void PlayAudio (string _AudioName) {
        if (_AudioName != null && _AudioName != "") {
            AudioManager._ins.PlayAudioClip("AnimationAudio", transform.position, _AudioName);
        }
    }
}

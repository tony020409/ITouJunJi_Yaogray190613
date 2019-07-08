using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerSound : MonoBehaviour {

    public bool play1;
    public bool play2;
    public AimSound.GunSoundSource Ani_Attack1;
    public AimSound.GunSoundSource Ani_Attack2;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update () {
        //
        if (Input.GetKeyDown(KeyCode.Keypad0) && play1){
            Ani_Attack1.Play();
        }
        if (Input.GetKeyUp(KeyCode.Keypad0)){
            Ani_Attack1.Stop();
        }

        //
        if (Input.GetKeyDown(KeyCode.Keypad0) && play2){
            Ani_Attack2.Play();
        }
        if (Input.GetKeyUp(KeyCode.Keypad0)){
            Ani_Attack2.Stop();
        }
    }
}

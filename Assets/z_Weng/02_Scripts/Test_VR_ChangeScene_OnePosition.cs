using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// VR 測試用的切換場景
/// </summary>
public class Test_VR_ChangeScene_OnePosition : MonoBehaviour {


    [Header("場景清單")]

    /// <summary>
    /// 場景清單
    /// </summary>
    [Rename("場景編號")]
    public GameObject[] SceneList;


    /// <summary>
    /// 當前場景編號
    /// </summary>
    [Rename("當前場景編號")]
    public int current_Index = -1;



    // Update is called once per frame
    void Update() {

        if (BattleMain.GetInstance() != null) {
            if ( !BattleMain.GetInstance().Debug_Mode) {
                return;
            }
        }


        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Grip)) {
            if (current_Index >= 0) {
                if (SceneList[current_Index] != null) {
                    SceneList[current_Index].SetActive(false);
                }
            }
            current_Index += 1;
            if (current_Index >= SceneList.Length) {
                current_Index = 0;
            }
            if (SceneList[current_Index] != null) {
                SceneList[current_Index].SetActive(true);
            }
        }


        if (Input.GetKeyDown(KeyCode.M)) {
            if (SceneList[current_Index] != null) {
                SceneList[current_Index].SetActive(false);
            }
            current_Index += 1;
            if (current_Index >= SceneList.Length) {
                current_Index = 0;
            }
            if (SceneList[current_Index] != null) {
                SceneList[current_Index].SetActive(true);
            }
        }

    }



}

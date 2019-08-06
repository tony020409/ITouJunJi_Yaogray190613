using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve;

public class PC_PlayerCamera : MonoBehaviour {


    /// <summary> 玩家物件(Transform) </summary>  
    [Rename("放入玩家節點 (Player[CameraRig])")] public Transform player;
    /// <summary> 玩家攝影機 </summary> 
    /// <summary> 玩家攝影機 </summary> 
    [Rename("放入玩家攝影機 (Camera(eye))")] public Transform m_Camera;
    /// <summary> 播音效用的 </summary> 
    [Rename("AudioSource")] public AudioSource _audioSource;


    [Line()]
    public bool enableMouseLook = true;
    public bool LookSmoothing = false;
    public float LookSmoothFactor = 10.0f;

    [Line()]
    public bool clampVerticalRotation = true;
    public float MinimumX = -80.0f;
    public float MaximumX = 80.0f;

    // 查看特定的東西 (用於「停靠(docked)」類型交互)
    private bool restrictLook = false;
    private Vector2 lookRestrictionAngles = Vector2.zero;
    private Vector2 rotationSum = Vector2.zero;
    private Vector2 lastRotationChanges = Vector2.zero;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;

    void Awake(){
        UnityEngine.VR.InputTracking.disablePositionalTracking = true;
        UnityEngine.VR.VRSettings.enabled = false;
        if (player == null) {
            player = this.transform;
        }
        if (player != null) {
            m_CharacterTargetRot = player.localRotation;
        }
        if (m_Camera.transform != null) {
            m_CameraTargetRot = m_Camera.transform.localRotation;
        }
        if (_audioSource == null) {
            _audioSource = this.GetComponent<AudioSource>();
        }
        Cursor.lockState = CursorLockMode.Locked;
    }



    void Update(){
        LookRotation(player, m_Camera.transform);
        if (Input.GetKeyDown(KeyCode.L)) {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
 

     /// <summary> 調整視角 </summary>
    /// <param name="character"> 玩家 </param>
    /// <param name="camera"   > 玩家攝影機 </param>
    public void LookRotation(Transform character, Transform camera) {

        if (enableMouseLook) {
            lastRotationChanges.x = Input.GetAxis("Mouse Y");
            lastRotationChanges.y = Input.GetAxis("Mouse X");

            // 限制視角的情況下
            if (restrictLook) {
                if ((rotationSum.y + lastRotationChanges.y) > lookRestrictionAngles.y) {
                    lastRotationChanges.y = (lookRestrictionAngles.y - rotationSum.y);
                }
                else if ((rotationSum.y + lastRotationChanges.y) < -lookRestrictionAngles.y) {
                    lastRotationChanges.y = (-lookRestrictionAngles.y - rotationSum.y);
                }
                rotationSum.y += lastRotationChanges.y;
                if ((rotationSum.x + lastRotationChanges.x) > lookRestrictionAngles.x) {
                    lastRotationChanges.x = (lookRestrictionAngles.x - rotationSum.x);
                }
                else if ((rotationSum.x + lastRotationChanges.x) < -lookRestrictionAngles.x) {
                    lastRotationChanges.x = (-lookRestrictionAngles.x - rotationSum.x);
                }
                rotationSum.x += lastRotationChanges.x;
            }

            m_CharacterTargetRot *= Quaternion.Euler(0.0f, lastRotationChanges.y, 0.0f);
            m_CameraTargetRot    *= Quaternion.Euler(-lastRotationChanges.x, 0.0f, 0.0f);

            // Only clamp when not restricting look
            // 只有在不限制視角時，才能 clmap (???)
            if (!restrictLook && clampVerticalRotation) {
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
            }

            if (LookSmoothing) {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot, LookSmoothFactor * Time.deltaTime);
                camera.localRotation    = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot, LookSmoothFactor * Time.deltaTime);
            }
            else {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

        }

    }



    
    /// <summary> 沿著X軸周圍夾具旋轉 </summary>
    /// <param name="q"></param>
    Quaternion ClampRotationAroundXAxis(Quaternion q) {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }

       
    
    /// <summary> 讓玩家相機聚焦在某一個位置 </summary>
    /// <param name="character" > 玩家位置 </param>
    /// <param name="camera"    > 玩家相機位置 </param>
    /// <param name="focalPoint"> 距焦點的世界座標 </param>
    public void LookAtPosition(Transform character, Transform camera, Vector3 focalPoint) {
        // 使角色面對目標
        Vector3 relativeCharPosition = focalPoint - character.position;
        Quaternion rotation = Quaternion.LookRotation(relativeCharPosition);
        Vector3 flatCharRotation = rotation.eulerAngles;
        flatCharRotation.x = 0.0f;
        flatCharRotation.z = 0.0f;
        character.localRotation = Quaternion.Euler(flatCharRotation);
        // 使目標物的旋轉與我們當前的旋轉相同
        m_CharacterTargetRot = character.localRotation;

        // 使相機面對目標
        Vector3 relativeCamPosition = focalPoint - camera.position;
        Quaternion camRotation = Quaternion.LookRotation(relativeCamPosition);
        Vector3 flatCamRotation = camRotation.eulerAngles;
        flatCamRotation.y = 0.0f;
        flatCamRotation.z = 0.0f;
        camera.localRotation = Quaternion.Euler(flatCamRotation);
        // Key: Make cam target rotation our current cam rotation
        m_CameraTargetRot = camera.localRotation;
    }


    public void enableLookRestriction(Vector2 maxAnglesFromOrigin) {
        // Note: we flip these because 'x' rotation means 'horizontal' to the user, but it really translates to 'tilt' which is vertical
        // 我們翻轉它們是因為“x”旋轉對用戶來說意味著“水平”，但它實際上轉換為垂直的“傾斜”
        lookRestrictionAngles.x = maxAnglesFromOrigin.y;
        lookRestrictionAngles.y = maxAnglesFromOrigin.x;
        rotationSum = Vector2.zero;
        restrictLook = true;
    }


    public void disableLookRestriction() {
        restrictLook = false;
    }



}

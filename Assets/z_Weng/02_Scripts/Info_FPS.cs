
using UnityEngine;
using UnityEngine.UI;


public class Info_FPS : MonoBehaviour
{

    int frameCount;
    float nextTime;
    public Text _Text_FPS;

    private int GUI_fps;
    private Rect GUI_rect;



    //void Awake() {
    //    Application.targetFrameRate = 300;
    //}


    // Use this for initialization
    void Start() {
        nextTime = Time.time + 1;
        GUI_rect = new Rect(Screen.width / 2, 0, 100, 100);
    }
    
    // Update is called once per frame
    void Update() {
        frameCount++;
    
        if (Time.time >= nextTime) {
            // 1秒後顯示fps
            //Debug.LogWarning("FPS : " + frameCount);
            if (_Text_FPS != null) {
                GUI_fps = frameCount;
                _Text_FPS.text = "FPS: " + frameCount;
            }
            frameCount = 0;
            nextTime += 1;
        }
    }


#if UNITY_EDITOR
    void OnGUI()
    {
        GUI.Label(GUI_rect, "FPS: " + GUI_fps);
    }
#endif



}

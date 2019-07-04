using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public Camera[] _aCamera;
    public GameObject m_CameraUI;

    private int _iCameraIndex = 0;
    // Use this for initialization
    void Start()
    {
        //_aCamera = gameObject.GetComponentsInChildren< Camera>();
        if (GloData.glo_iGameModel == 1)
        {
            m_CameraUI.SetActive(true);
        }
        else
        {
            m_CameraUI.SetActive(false);
            CloseAll();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
            {
            m_CameraUI.gameObject.SetActive(false);
        }


            if (Input.GetKeyDown(KeyCode.Space))
        {
            _iCameraIndex++;
            if (_iCameraIndex >= _aCamera.Length)
            {
                _iCameraIndex = 0;
            }
            CloseAll();
            _aCamera[_iCameraIndex].gameObject.SetActive(true);
        }

    }

    private void CloseAll()
    {
        for (int i = 0; i < _aCamera.Length; i++)
        {
            _aCamera[i].gameObject.SetActive(false);
        }
    }

}

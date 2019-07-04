using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllPlaySound : GameControllBaseState{

    private string soundPath;                  //聲音路徑和檔案名稱
    private GameObject oBullet = null;         //聲音控制器物件
    private p_SoundPlayer soundControl = null; //聲音控制器程式
    private float _iVolume = 0.9f;             //聲音音量
    private Vector3 _iPos;                     //聲音位置(3D音效時)
    private bool is3D = false;                 //是否為3D音

    public GameControllPlaySound() : 
    base((int)EM_GameControllAction.PlaySound){ }


    public override void f_Enter(object Obj){
        _CurGameControllDT = (GameControllDT)Obj;

        //設定音效路徑
        if (_CurGameControllDT.szData1 != "") {
            soundPath = _CurGameControllDT.szData1 + "/" + _CurGameControllDT.szData2;
        } else {
            soundPath = _CurGameControllDT.szData2;
        }

        //如果有指定音量就設定音量
        if (_CurGameControllDT.szData3 != "") {
            _iVolume = ccMath.atof(_CurGameControllDT.szData3);    
        } 

        //如果有指定座標，就是3D音效
        if (_CurGameControllDT.szData4 != ""){
            float[] aPos = ccMath.f_String2ArrayFloat(_CurGameControllDT.szData4, ";");
            _iPos = new Vector3(aPos[0], aPos[1], aPos[2]);
            is3D = true;
        }
        
        StartRun();
    }


    //27. 播放聲音 (參數1=音效路徑, 參數2=音效名稱，參數3=音量，參數4=3D音效時的位置) (註：「Resource/Voice/早安.mp3」的參數1為「Voice」,參數2為「早安」)
    protected override void Run(object Obj){
        base.Run(Obj);
        oBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateSoundPlayer(); //產生播聲音物件
        soundControl = oBullet.GetComponent<p_SoundPlayer>();                     //獲取聲音播放控制器
        
        //如果聲音播放控制器生產出來了
        if (soundControl != null) {

            //如果要3D音效，就設定成3D音(spatialBlend = 1)，並給予音效位置
            if (is3D) {
                oBullet.gameObject.GetComponent<AudioSource>().spatialBlend = 1.0f; //3D
                oBullet.transform.position = _iPos;
                is3D = false;
            }
            else {
                oBullet.gameObject.GetComponent<AudioSource>().spatialBlend = 0.0f; //2D
            }

            //播放聲音
            soundControl.s_Play(soundPath, _iVolume);  
        }

        EndRun();
    }

    //public override void f_Execute(){
    //    base.f_Execute();
    //
    //    //如果聲音播放控制器生產出來了
    //    if (soundControl != null){
    //
    //        //如果是3D音就給予位置
    //        if (is3D){
    //            oBullet.gameObject.GetComponent<AudioSource>().spatialBlend = 1.0f; //設成3D音 (0.0f = 2D音)
    //            oBullet.transform.position = _iPos;
    //            is3D = false;
    //        }
    //
    //        //如果不等聲音結束就直接執行下一行腳本
    //        if (_CurGameControllDT.iNeedEnd == 0) {
    //            EndRun();
    //        }
    //
    //        //如果要等聲音結束，就等聲音結束時執行下一行腳本
    //        else if (_CurGameControllDT.iNeedEnd == 1){
    //            if (soundControl.audioEnd){
    //                EndRun();
    //            }
    //        }
    //
    //    }
    //}



}

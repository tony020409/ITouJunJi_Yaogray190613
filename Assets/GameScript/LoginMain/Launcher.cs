using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;
using ccU3DEngine;

using ccGameControllSDK;

namespace y_Network.Launcher
{
    public class Launcher : UIFramwork
    {

        [Space(5)]
        private bool isLoadingScene = true;
        private AsyncOperation async;
        public Image LoadingSlider;

        public  static Launcher _instance;
        public static Launcher GetIinstance()
        {
            return _instance;
        }

        protected override void f_CustomAwake()
        {

            _instance = this;
            //if (GloData.glo_iGameModel == 0){
            //    UITools.f_SetText(f_GetObject("GameModel"), "Player");
            //}
            //else if (GloData.glo_iGameModel == 1) {
            //    UITools.f_SetText(f_GetObject("GameModel"), "Master");
            //}
            //else {
            //    UITools.f_SetText(f_GetObject("GameModel"), "Mode Ero");
            //}
            //ccTimeEvent.GetInstance().f_RegEvent(3, false, null, Connect);

            isLoadingScene = false;
            if (LoadingSlider != null) {
                LoadingSlider.fillAmount = 0;
            }

        }



        private void Update() {
            //如果按右邊Alt + 左邊橫排數字0，清空 PlayerPrefs的記錄
            if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.Alpha0)){
                PlayerPrefs.DeleteAll();
                MessageBox.DEBUG("清空 PlayerPrefs紀錄!");
            }

            if (isLoadingScene) {
                LoadSceneProgressUI();
            }
        }



        protected override void f_InitMessage()
        {
            base.f_InitMessage();
            //f_RegClickEvent("LoginBtn", UI_BtnConnect);
        }


        private bool _bConnect = false;
        public void UI_BtnConnect()
        {
            if (!_bConnect)
            {
                Connect(null);
                _bConnect = true;
            }
        }


        public void Connect (object Obj)
        {
            MessageBox.DEBUG("Connect");
            GameSocket.GetInstance().f_Login(Callback_LoginSuc);
            ControllSocket.GetInstance().f_Login(Callback_GameControllLoginSuc);
        }


        private void Callback_GameControllLoginSuc(object Obj)
        {
            MessageBox.DEBUG("GameControll登陆");
            eMsgOperateResult teMsgOperateResult = (eMsgOperateResult) Obj;
            if (teMsgOperateResult == (int)eMsgOperateResult.OR_Succeed)
            {
                MessageBox.DEBUG("GameControll登陆成功");
            }
            else
            {
                MessageBox.DEBUG("登陆失败 " + teMsgOperateResult.ToString());
            }
        }


        private void Callback_LoginSuc(object Obj)
        {
            MessageBox.DEBUG("GameStep登陆");
            eMsgOperateResult teMsgOperateResult = (eMsgOperateResult) Obj;
            if (teMsgOperateResult == (int)eMsgOperateResult.OR_Succeed)
            {
                MessageBox.DEBUG("GameStep登陆成功!");                
                if (GloData.glo_iGameModel == 1)
                {
                    StaticValue.m_bIsMaster = true;
                }
                else
                {
                    StaticValue.m_bIsMaster = false;
                }


                SceneManager.LoadScene(GameEM.GameScene.BattleMain.ToString());


                //用這個好像會造成進 BattleMain場景時，機率性當機，所以用回原本的方式
                //StartCoroutine(loadScene());
                //isLoadingScene = true;
            }
            else {
                MessageBox.DEBUG("登陆失败 " + teMsgOperateResult.ToString());

                //使用控制台
                if (ccVerifyMain2.GetInstance().Controller_bool) {
                    ccGameControll.GetInstance().f_SendErroInfor(EM_ErrorType.GamingError, "Game登陆失败:" + teMsgOperateResult.ToString());
                }

            }
        }


        /// <summary>
        /// 異步讀取場景
        /// </summary>
        IEnumerator loadScene(){
            async = SceneManager.LoadSceneAsync("BattleMain"); //異步讀取場景
            async.allowSceneActivation = true;                 //自動跳轉
            //async.allowSceneActivation = false;              //不自動跳轉
            yield return null;
        }

        /// <summary>
        /// 延遲進場景
        /// </summary>
        IEnumerator DelayGotoScene(float delayTime) {
            yield return new WaitForSeconds(delayTime);
            async.allowSceneActivation = true;
        }


        /// <summary>
        /// 顯示讀取進度
        /// </summary>
        private void LoadSceneProgressUI() {
            if (LoadingSlider == null) {
                return;
            }
            LoadingSlider.fillAmount = async.progress;
            if (LoadingSlider.fillAmount == 0.9f) {
                LoadingSlider.fillAmount = 1.0f;
            }
        }


    }
}
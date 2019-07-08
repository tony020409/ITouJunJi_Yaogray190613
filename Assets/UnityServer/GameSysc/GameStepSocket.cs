using ccU3DEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameSysc
{
    public class GameStepSocket : GameSysc.ServiceBase
    {

        private GameSyscManager _GameSyscManager;
        public GameStepSocket(GameSyscManager tGameSyscManager)
        {
            _GameSyscManager = tGameSyscManager;
        }

        protected override void InitMessage()
        {
            base.InitMessage();

            CTS_GameData tCTS_GameData = new CTS_GameData();
            _ccSocketMessagePoolV2.f_AddListener_Buf_V2((int)SocketCommand.CTS_GameData, tCTS_GameData, On_CTS_GameData);

            STC_BroadCastAction tSTC_BroadCastAction = new STC_BroadCastAction();
            _ccSocketMessagePoolV2.f_AddListener_Buf_V2((int)SocketCommand.STC_BroadCastAction, tSTC_BroadCastAction, On_STC_BroadCastAction);

            STC_SyscUpdate tSTC_SyscUpdate = new STC_SyscUpdate();
            _ccSocketMessagePoolV2.f_AddListener((int)SocketCommand.STC_SyscUpdate, tSTC_SyscUpdate, On_STC_SyscUpdate);

            CTS_StartGame tCTS_StartGame = new CTS_StartGame();
            _ccSocketMessagePoolV2.f_AddListener((int)SocketCommand.CTS_StartGame, tCTS_StartGame, On_CTS_StartGame);

            CTS_ServerActionConfirm tCTS_ServerActionConfirm = new CTS_ServerActionConfirm();
            _ccSocketMessagePoolV2.f_AddListener((int)SocketCommand.CTS_ServerActionConfirm, tCTS_ServerActionConfirm, On_CTS_ServerActionConfirm);

            CTS_GameOver tCTS_GameOver = new CTS_GameOver();
            _ccSocketMessagePoolV2.f_AddListener((int)SocketCommand.CTS_GameOver, tCTS_GameOver, On_CTS_GameOver);

            GameSocket.GetInstance().f_RegOtherRouter(_ccSocketMessagePoolV2.f_Router);

            CTS_StartGame tCTS_RestartGame = new CTS_StartGame();
            _ccSocketMessagePoolV2.f_AddListener((int)SocketCommand.CTS_RestartGame, tCTS_RestartGame, On_CTS_RestartGame);


        }

        private void On_CTS_ServerActionConfirm(object Obj)
        {
            CTS_ServerActionConfirm tCTS_ServerActionConfirm = (CTS_ServerActionConfirm)Obj;
            _GameSyscManager.f_ServerActionConfirm(tCTS_ServerActionConfirm);
        }

        /// <summary>
        /// 开始游戏，同时推送其它玩家的信息
        /// </summary>
        /// <param name="Obj"></param>
        private void On_CTS_StartGame(object Obj)
        {
            CTS_StartGame tCTS_StartGame = (CTS_StartGame)Obj;
            _GameSyscManager.On_Start(tCTS_StartGame);
        }

        private void On_CTS_GameOver(object Obj)
        {
            CTS_GameOver tCTS_GameOver = (CTS_GameOver)Obj;
            _GameSyscManager.On_GameOver(tCTS_GameOver);
        }

        private void On_CTS_RestartGame(object Obj)
        {
            MessageBox.DEBUG("强制结束游戏");
            ccTimeEvent.GetInstance().f_RegEvent(15, false, null, QuitGame);
        }


        void QuitGame(object o)
        {           
            glo_Main.GetInstance().f_Destroy();            
        }


        private void On_CTS_GameData(int iLen, byte[] aBuf)
        {
            //MainLogic.GetInstance().f_Router(_ClientSocket, aBuf);
        }

        private void On_STC_BroadCastAction(int iLen, byte[] aBuf)
        {
            if (iLen == aBuf.Length)
            {
                _GameSyscManager.f_ServerBroadCastAction(aBuf);
            }
            else
            {
                byte[] aTTT = new byte[iLen];
                Array.Copy(aBuf, aTTT, iLen);
                _GameSyscManager.f_ServerBroadCastAction(aTTT);
            }
        }

        private void On_STC_SyscUpdate(object Obj)
        {
            STC_SyscUpdate tSTC_SyscUpdate = (STC_SyscUpdate)Obj;
            _GameSyscManager.f_SyscUpdate(tSTC_SyscUpdate.iCurGameSyscFrame, tSTC_SyscUpdate.fCurFrameTime, tSTC_SyscUpdate.iGameFpsRunTime);
        }

        public void f_SendPlayerAction(byte[] aBuf)
        {
            int i = GameSocket.GetInstance().f_SendBuf((int)SocketCommand.STC_PlayerAction, aBuf);
            //MessageBox.DEBUG("f_SendPlayerAction:" + i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iActionId"></param>
        /// <param name="iActionFrame">当前运行的总工作帧数</param>
        /// <param name="iUserId"></param>
        /// <param name="iLastGameFps">当前工作帧剩余未完成的渲染次数</param>
        /// <param name="iCurAverageGameFpsRunTime">当前平均运行时间</param>
        public void f_SendPlayerActionConfirm(int iActionFrame, int iUserId, int iLastGameFps, int iCurAverageGameFpsRunTime)
        {
            //MessageBox.DEBUG("回复确认:" + iActionId + " " + iActionFrame + " " + iUserId);
            CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
            tCreateSocketBuf.f_Add(iActionFrame);
            tCreateSocketBuf.f_Add(iUserId);
            tCreateSocketBuf.f_Add(iLastGameFps);
            tCreateSocketBuf.f_Add(iCurAverageGameFpsRunTime);
            GameSocket.GetInstance().f_SendBuf((int)SocketCommand.STC_PlayerActionConfirm, tCreateSocketBuf.f_GetBuf());
        }

    }

}
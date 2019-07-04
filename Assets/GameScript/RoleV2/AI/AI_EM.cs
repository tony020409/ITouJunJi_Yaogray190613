using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_EM
{
    public enum EM_AIState
    {
        None = 0,
        //OffLine,
        Idle = 100,
        /// <summary>
        /// 可视范围内搜索目标
        /// </summary>
        Search ,
        CheckCanAttack,
        AI_WaitAction2,
        WaitAction,

        Walk,
        GotoNextRandPath,
        Attack,
        Die = 5,
        Walk2Target,
        Move,
        TrexWait,
        TrexMove,
		Fly,
        Super,
        Roar,
        Rotate,
        Timeline,

        //其他--------------------------
        PlayAnim,
        PlayerObj,

        //龍----------------------------
        Path,       //依路徑移動
        Jump,       //跳
        Grab,       //抓玩家
        BeAttack,



        //殭屍AI
        ZombieAI2_Idle,       //殭屍待機
        ZombieAI2_Chase,      //殭屍追擊
        ZombieAI2_NearAttack, //殭屍近戰
        ZombieAI2_FarAttack,  //殭屍遠攻

        //士兵AI
        ArcherAI_Init,


        //其他 RunAI
        Throw,
        AI_ChangePoint,
        AI_FarAttack,      //遠攻-攻擊中
        AI_Reload,         //遠攻-換子彈
        MoveToHidePos,     //遠攻-從出生點移到最近躲避點

        //條件AI
        ViewSize_In,       //視野有敵人
        ViewSize_Out,      //視野沒敵人
        AttackSize_In,     //攻擊範圍內有敵人
        AttackSize_Out,    //攻擊範圍內沒敵人
        CheckHP,           //檢測血量
        CheckBulletAmount, //檢查彈藥量
        ChangePoint,
        ArcherInit,         //遠攻-初始化
        CheckPlayerHand,    //檢查玩家的手
        CheckPlayerTracker, //檢查玩家的Tracker
        GotoCustomInitAI,   //前往自定義的初始AI動作

        StoneAttack,

    }

}
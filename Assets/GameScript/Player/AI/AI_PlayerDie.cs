using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AI_PlayerDie : AI_RunBaseStateV2
{

    MySelfPlayerControll2 _MySelfPlayerControll2;
    public AI_PlayerDie()
        : base(AI_EM.EM_AIState.Die) {
    }


    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);
        MessageBox.DEBUG("玩家[" + _BaseRoleControl.m_iId + "] 死亡");

        //自己
        if (_BaseRoleControl.m_iId == StaticValue.m_UserDataUnit.m_PlayerDT.m_iId)  {
            _MySelfPlayerControll2 = (MySelfPlayerControll2) _BaseRoleControl;

            //如果玩家沒有復活次數了
            if (_MySelfPlayerControll2.f_GetHaveLife() == 0) {
                MessageBox.DEBUG("玩家[" + _BaseRoleControl.m_iId + "] 生命用完无法复活");
                _MySelfPlayerControll2._Lutifybool = true;                                                                          //不到數 (因為沒有生命了，所以沒有倒數復活的必要)
                _MySelfPlayerControll2.DieTipObj.SetActive(true);                                                                   //開生命耗盡的提示訊息
                ccTimeEvent.GetInstance().f_RegEvent(ccMath.atof(_CharacterAIRunDT.szData1), false, null, CallBack_ToBeSoulFriend); //指定秒數後，保持死亡，但清除死亡的畫面，讓玩家看其他玩家玩
            }

            //如果玩家還有生命的話
            else {
                _MySelfPlayerControll2.fDelayTime = 0;                                                                       //重生倒數歸零
                _MySelfPlayerControll2._Lutifybool = false;                                                                  //重生倒數開始
                ccTimeEvent.GetInstance().f_RegEvent(ccMath.atof(_CharacterAIRunDT.szData1), false, null, CallBack_Rebirth); //指定秒數後復活
            }


            //如果遊戲失敗了
            if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Lost){
                _MySelfPlayerControll2._Lutify.Blend = 1;                          //畫面變黑
                _MySelfPlayerControll2.DeathreciprocalGameObject.SetActive(false); //不顯示重生倒數UI
            }

            //如果遊戲仍在進行中
            if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
                _MySelfPlayerControll2._Lutify.Blend = 1;         //開黑畫面濾鏡
                _MySelfPlayerControll2.Riflebool = false;         //???
                _MySelfPlayerControll2.f_StopGun(_MySelfPlayerControll2.GunEM); //停火
                _MySelfPlayerControll2.HaveLifeText.text = "Life:" + _MySelfPlayerControll2.f_GetHaveLife(); //刷新生命數UI
            }

        }

        //其它玩家死亡
        else {
            OtherPlayerControll2 _OtherPlayerControll2 = (OtherPlayerControll2)_BaseRoleControl;
            if (_OtherPlayerControll2.f_GetHaveLife() != 0) {
                ccTimeEvent.GetInstance().f_RegEvent(ccMath.atof(_CharacterAIRunDT.szData1), false, null, CallBack_Rebirth);
            } 
        }

        //遊戲任務-檢查其他玩家是否也都死透了
        //BattleMain.GetInstance()._GameMission.f_CheckAllPlayerIsDie();  
        BattleMain.GetInstance()._GameMission.f_CheckPVP_TeamLost();
    }


    /// <summary>
    /// 復活
    /// </summary>
    private void CallBack_Rebirth(object Obj) {
        if (glo_Main.GetInstance().m_EM_GameStatic != EM_GameStatic.Gaming) {
            return;
        }
        _BaseRoleControl.f_Rebirth();
        ChangeAIAction tmpAction = new ChangeAIAction();
        tmpAction.f_SetAI(_BaseRoleControl.m_iId, "Idle");
        _BaseRoleControl.f_AddMyAction(tmpAction);
    }



    /// <summary>
    /// 保持死亡，但清除復活倒數的畫面，讓玩家可以看其他玩家玩
    /// </summary>
    private void CallBack_ToBeSoulFriend(object Obj){
        if (glo_Main.GetInstance().m_EM_GameStatic != EM_GameStatic.Gaming){
            return;
        }
        _MySelfPlayerControll2 = (MySelfPlayerControll2)_BaseRoleControl;
        _MySelfPlayerControll2._Lutifybool = true;                         //停止倒數
        _MySelfPlayerControll2._Lutify.Blend = 0;                          //畫面變亮
        _MySelfPlayerControll2.DeathreciprocalGameObject.SetActive(false); //關閉重生倒數UI
    }


}
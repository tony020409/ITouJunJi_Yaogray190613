using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 變更父物件
/// </summary>
public class GameControllV3_SetParent : GameControllBaseState
{


    public GameObject m_Object;
    public GameObject m_NewParent;


      public GameControllV3_SetParent() :
    base((int)EM_GameControllAction.V3_SetParent)
    { }




    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }



    protected override void Run(object Obj) {
        base.Run(Obj);

        //預設抓玩家
        m_Object = BattleMain.GetInstance().m_oMySelfPlayer_VR;

        //玩家的特殊判斷
        if (_CurGameControllDT.szData1 == "PlayerAuto") {
            m_Object = BattleMain.GetInstance().m_oMySelfPlayer_VR;
        }

        //如果不是玩家就找場景上的物件
        else {
            m_Object = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData1);
        }

        //如果找不到玩家或物件就結束指令
        if (m_Object == null) {
            EndRun();
            return;
        }


        //找新父物件，如果找不到新父物件就結束指令
        m_NewParent = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData2);
        if (m_NewParent == null){
            EndRun();
            return;
        }

        //變更父物件
        m_Object.transform.parent = m_NewParent.transform;

        EndRun();

    }


 

}

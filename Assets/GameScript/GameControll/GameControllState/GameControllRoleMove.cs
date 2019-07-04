using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllRoleMove : GameControllBaseState
{

    public GameControllRoleMove(): 
        base((int)EM_GameControllAction.RoleMove){ }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }

    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //2.角色移动（参数1为角色分配的指定KeyId,参数2为移动的目标坐标，参数3无效）
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1));
        if (tRoleControl == null)
        {
            MessageBox.ASSERT("- 任務[" + _CurGameControllDT.iId + "] 未找到指定的角色: " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }
        float[] aPos1 = ccMath.f_String2ArrayFloat(_CurGameControllDT.szData2, ";");
        float[] aPos2 = ccMath.f_String2ArrayFloat(_CurGameControllDT.szData3, ";");
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetNodeForIndexXY((int)aPos1[0], (int)aPos1[1]);
        ccCallback tccCallback = CallBack_WalkComplete;



        if (aPos1.Length == 2){
            //MessageBox.ASSERT("移動模式1");
            //tRoleControl.f_Walk2Target(tTileNode, tccCallback);
        }
        else {
            //MessageBox.ASSERT("移動模式2");
            Vector3 newPos1 = new Vector3(aPos1[0], aPos1[1], aPos1[2]);
            Vector3 newPos2 = new Vector3(aPos2[0], aPos2[1], aPos2[2]);
            if (tRoleControl != null){
                //tRoleControl.f_Walk3Target(newPos1, newPos2, tccCallback);
            }
        }


        if (_CurGameControllDT.iNeedEnd == 0){
            tccCallback = null;
            EndRun();
        }

    }

    private void CallBack_WalkComplete(object Obj)
    {
        if (_CurGameControllDT.iNeedEnd == 1)
        {
            Debug.LogWarning(_CurGameControllDT.iId + "執行CallBack");
            EndRun();
        }
    }





}

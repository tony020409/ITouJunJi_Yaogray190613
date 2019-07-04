using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 電梯位移
/// </summary>
public class GameControllV3_Elevator : GameControllBaseState {


    public GameObject _elevator;
    public GameObject _target_Pos;

    /// <summary>
    /// 乘客清單
    /// </summary>
    private List<Transform> PassengerList = new List<Transform>();

    /// <summary>
    /// 保存乘客進電梯前的父物件位置用
    /// </summary>
    private List<Transform> Passenger_OriginParentList = new List<Transform>();



    public GameControllV3_Elevator() :
    base((int)EM_GameControllAction.V3_Elevator)
    { }




    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj;
        StartRun();
    }



    protected override void Run(object Obj) {
        base.Run(Obj);

        //找尋電梯
        _elevator = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData1);
        if (_elevator == null) {
            EndRun();
        }


        //參數2 若等於「Only_Attach」表示單純上電梯
        if (_CurGameControllDT.szData2 == "Only_Attach") {
            Passenger_Attach();
            EndRun();
            return;
        }


        //參數2 若等於「Only_Detach」表示單純下電梯
        if (_CurGameControllDT.szData2 == "Only_Detach"){
            Passenger_Detach();
            EndRun();
            return;
        }



        //參數2 若填寫「位置」，則玩家連同電梯一起瞬間移過去
        _target_Pos = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData2);
        if (_target_Pos != null){
            //抓乘客
            Passenger_Attach();

            //移動電梯
            _elevator.transform.position = _target_Pos.transform.position;
            _elevator.transform.rotation = _target_Pos.transform.rotation;

            //釋放乘客
            Passenger_Detach();
        }
        EndRun();

    }


     /// <summary>
    /// 把偵測範圍內的乘客抓上電梯
    /// </summary>
    void Passenger_Attach() {
        Transform tmp = BattleMain.GetInstance().m_oMySelfPlayer_VR.transform;
        if (tmp != null) {
            PassengerList.Add(tmp.transform);                     //加入至乘客清單內
            Passenger_OriginParentList.Add(tmp.transform.parent); //記錄原本的父物件
            tmp.transform.parent = _elevator.transform;           //加乘客拉到電梯下
        }
    }


    /// <summary>
    /// 把乘客釋放出電梯
    /// </summary>
    void Passenger_Detach() {
        Transform tmp = BattleMain.GetInstance().m_oMySelfPlayer_VR.transform;
        if (tmp != null) {
            tmp.transform.parent = Passenger_OriginParentList[0];
        }
        PassengerList.Clear();
        Passenger_OriginParentList.Clear();
    }






}

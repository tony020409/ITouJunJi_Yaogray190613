using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using ccU3DEngine;



//記得到Action.cs加
[ProtoContract]
public class Action_ChangePlayerPos : BaseActionV2 //GameSysc.Action
{

    public Action_ChangePlayerPos()
        : base(GameEM.EM_RoleAction.ChangePlayerPos)
    { }


    //玩家id、玩家畫面過場過程的時間、完全黑畫面的時間
    [ProtoMember(41001)] public int m_PlayerId;
    [ProtoMember(41002)] public float m_FadeTime; 
    [ProtoMember(41003)] public float m_BlackTime;


    //因為[ProtoMember]不支援 Vector3、Quaternion，所以拆開來
    [ProtoMember(41004)] public float newPos_x;
    [ProtoMember(41005)] public float newPos_y;
    [ProtoMember(41006)] public float newPos_z;

    //因為[ProtoMember]不支援 Vector3、Quaternion，所以拆開來
    [ProtoMember(41007)] public float newRot_x;
    [ProtoMember(41008)] public float newRot_y;
    [ProtoMember(41009)] public float newRot_z;


    /// <summary>
    /// 設定
    /// </summary>
    public void f_Change(int iRoleId, float fadeTime, float blackTime, Vector3 tPos, Vector3 tRot) {
        m_PlayerId = iRoleId;
        m_FadeTime = fadeTime;
        m_BlackTime = blackTime;
        newPos_x = tPos.x;
        newPos_y = tPos.y;
        newPos_z = tPos.z;
        newRot_x = tRot.x;
        newRot_y = tRot.y;
        newRot_z = tRot.z;
    }


    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction() {
        BattleMain.GetInstance().SceneFadeTo(1, m_FadeTime);                                  //玩家畫面慢慢黑掉
        ccTimeEvent.GetInstance().f_RegEvent(m_FadeTime, false, null, CallBack_FadeComplete); //0.8秒後，待畫面變暗完成再移位置，然後恢復畫面
    }


    /// <summary>
    /// 玩家畫面變暗完畢後，移動玩家
    /// </summary>
    private void CallBack_FadeComplete(object obj) {
        BaseRoleControllV2 tmpPlayer = BattleMain.GetInstance().f_GetRoleControl2(m_PlayerId);
        if (tmpPlayer != null) {

            //移動玩家
            MySelfPlayerControll2 _MySelfPlayerControll2 = tmpPlayer.GetComponent<MySelfPlayerControll2>();
            if (_MySelfPlayerControll2 != null) {
                Vector3 tmpPos = new Vector3(newPos_x, newPos_y, newPos_z); //取得新位置
                Vector3 tmpRot = new Vector3(newRot_x, newRot_y, newRot_z); //取得新朝向
                _MySelfPlayerControll2.f_SetPos(tmpPos); //改玩家的位置     
                _MySelfPlayerControll2.f_SetRot(tmpRot); //改玩家的朝向
            } 
        }

        //指定秒數後，恢復畫面明亮
        ccTimeEvent.GetInstance().f_RegEvent(m_BlackTime, false, null, CallBack_MoveComplete);
    } 


    /// <summary>
    /// 移動玩家後，在指定的時間內保持黑畫面，時間過後畫面變亮
    /// </summary>
    private void CallBack_MoveComplete(object obj) {
        BattleMain.GetInstance().SceneFadeTo(0, m_FadeTime);
    }



}

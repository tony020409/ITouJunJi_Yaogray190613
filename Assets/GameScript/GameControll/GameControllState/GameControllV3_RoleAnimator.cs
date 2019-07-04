using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3002.设置角色動畫（参数1为角色id, 参数2为動作類型，参数3為參數, 參數4為參數(參數4有需要時才填寫))
/// </summary>
public class GameControllV3_RoleAnimator : GameControllBaseState
{
    private GameObject tmpObj;
    private Animator _animator;


    public GameControllV3_RoleAnimator()
        : base((int)EM_GameControllAction.V3_RoleAnimator)
    { }



    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj;

        //取得要播放動畫的物件 (可能是角色，也可能是 BattainMain裡存放的物件)
        try {
            tmpObj = BattleMain.GetInstance().f_GetRoleControl2(int.Parse(_CurGameControllDT.szData1)).gameObject;
        }
        catch {
            tmpObj = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData1);
        }

        //獲取動畫控制件
        if (tmpObj != null) {
            _animator = tmpObj.GetComponent<Animator>();
        }
        StartRun();
    }



    protected override void Run(object Obj) {
        if (tmpObj == null) {
            if (BattleMain.GetInstance().Debug_Object_isNull) {
                Debug.LogWarning("【警告】腳本[" + _CurGameControllDT.iId + "] 未找到指定角色或物件:" + _CurGameControllDT.szData1 + "   // " + _CurGameControllDT.szName);
            }
            EndRun();
            return;
        }

        if (_animator == null){
            if (BattleMain.GetInstance().Debug_Object_isNull) {
                Debug.LogWarning("【警告】腳本[" + _CurGameControllDT.iId + "] 指定的角色:" + _CurGameControllDT.szData1 + " 身上沒有掛 Animator，故這行跳過不執行！");
            }
            EndRun();
            return;
        }


        if (!_animator.enabled){
            EndRun();
            return;
        }


        if (_animator.runtimeAnimatorController == null){
            EndRun();
            return;
        }

        if (_CurGameControllDT.szData2 == "Play") {
            _animator.Play(_CurGameControllDT.szData3);
        }

        else if (_CurGameControllDT.szData2 == "CrossFade") {
            _animator.CrossFade(_CurGameControllDT.szData3, 0.25f);
        }

        else if (_CurGameControllDT.szData2 == "SetInt")  {
            _animator.SetInteger(_CurGameControllDT.szData3, ccMath.atoi(_CurGameControllDT.szData4));
        }

        else if (_CurGameControllDT.szData2 == "SetFloat")  {
            _animator.SetFloat(_CurGameControllDT.szData3, ccMath.atoi(_CurGameControllDT.szData4));
        }

        else if (_CurGameControllDT.szData2 == "SetTrigger") {
            _animator.SetTrigger(_CurGameControllDT.szData3);
        }

        else if (_CurGameControllDT.szData2 == "SetBool")  {
            _CurGameControllDT.szData4 = _CurGameControllDT.szData4.ToLower(); //將文字一律轉成小寫
            if (_CurGameControllDT.szData4 == "true") {
                _animator.SetBool(_CurGameControllDT.szData3, true);
            } else {
                _animator.SetBool(_CurGameControllDT.szData3, false);
            }
        }
        else {
            MessageBox.ASSERT("腳本[" + _CurGameControllDT.iId + "] 動畫指令錯誤:" + _CurGameControllDT.szData2);
        }

        EndRun();
    }

}


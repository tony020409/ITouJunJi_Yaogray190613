

/// <summary>
/// 無人機遠攻
/// </summary>
public class DroneAI_FarAttack : AI_RunBaseStateV2 {

    private DroneRoleControl _Drone;               //無人機模組
    private BaseRoleControllV2 _ReadyAttackTarget; //攻擊目標


    public DroneAI_FarAttack()
        : base(AI_EM.EM_AIState.Attack) { }


    public override void f_Enter(object Obj) {
        base.f_Enter(Obj);
        _Drone = _BaseRoleControl.GetComponent<DroneRoleControl>();       //取得無人機模組
        if (_Drone != null) {
            _Drone.BulletAmount = ccMath.atoi(_CharacterAIRunDT.szData1); //取得攜彈量
        }

    }


    public override void f_Execute() {
        base.f_Execute();
        //.........做什麼
    }


    public override void f_Exit() {
        base.f_Exit();
        //.........做什麼
    }


}

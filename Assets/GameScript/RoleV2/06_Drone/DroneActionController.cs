

/// <summary>
/// 無人機 ActionController
/// </summary>
public class DroneActionController : BaseActionController {

    BaseRoleControllV2 _BaseRoleControl;
    public DroneActionController (BaseRoleControllV2 tBaseRoleControl)
        : base(tBaseRoleControl, GameEM.emRoleType.Drone) {
        _BaseRoleControl = tBaseRoleControl;
    }

    public override void f_PlayAction(AI_EM.EM_AIState tAIState) {
        if (animator == null) {
            return;
        }

        if (m_CurAIStatic != tAIState) {

            // 待機
            if (AI_EM.EM_AIState.Idle == tAIState) {
                animator.SetBool("IsIdle", true);
                animator.SetBool("CanAttack", false);
            }

            // 死亡
            else if (AI_EM.EM_AIState.Die == tAIState) {
                animator.Play("Die");
            }

            m_CurAIStatic = tAIState;
        }

    }

}
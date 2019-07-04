using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherActionController : BaseActionController
{
    BaseRoleControllV2 _BaseRoleControl;
    public ArcherActionController (BaseRoleControllV2 tBaseRoleControl) 
        :base(tBaseRoleControl, GameEM.emRoleType.Archer) {
        _BaseRoleControl = tBaseRoleControl;
    }


    public override void f_PlayAction(AI_EM.EM_AIState tAIState)
    {

        if (animator == null)  {
            return;
        }

        //if (tAIState == AI_EM.EM_AIState.WaitAction) {
        //    animator.CrossFade("Idle", 0.25f);
        //    return;
        //}


        if (m_CurAIStatic != tAIState) {

            //Debug.LogAssertion("Archer換動作!");

            // 播放指定動畫 ========================================
            if (AI_EM.EM_AIState.PlayAnim == tAIState) {
                //照 AI_Animator.cs 收到的動畫名稱去播放
            }

            // 待機 ================================================  
            else if (AI_EM.EM_AIState.Idle == tAIState){
                //animator.SetInteger("control", 71);
                //animator.CrossFade("Idle", 0.25f);
                animator.SetBool("IsIdle", true);
                animator.SetBool("CanAttack", false);
                GetHideInfo();
                //Debug.Log(" <color=yellow> 待機動畫 </color>");
            }


            // 待機(殭屍) ==========================================
            else if (AI_EM.EM_AIState.ZombieAI2_Idle == tAIState) {
                //animator.SetInteger("control", 71);
                //animator.CrossFade("Idle", 0.25f);
                animator.SetBool("IsIdle", true);
                animator.SetBool("CanAttack", false);
                GetHideInfo();
                //Debug.Log(" <color=yellow> 待機動畫-殭屍 </color>");
            }

            // 追擊 ================================================
            else if (AI_EM.EM_AIState.ZombieAI2_Chase == tAIState) {
                //animator.SetInteger("control", 71);
                animator.CrossFade("Run", 0.25f);
            }

            // 近戰 ================================================        
            else if (AI_EM.EM_AIState.ZombieAI2_NearAttack == tAIState) {
                //animator.SetInteger("control", 71);
                animator.SetBool("IsIdle", false);
                animator.CrossFade("Attack", 0.25f);
            }

            // 遠攻 ================================================        
            else if (AI_EM.EM_AIState.ZombieAI2_FarAttack == tAIState) {
                animator.SetBool("IsIdle", false);
                if (animator.GetBool("attackInit") == true){
                    animator.SetBool("CanAttack", true);
                }
                else {
                    animator.SetBool("attackInit", true);
                    //GetHideInfo();
                    //Debug.Log(" <color=yellow> 遠攻動畫 </color>");
                }
                //由主玩家亂數決定延遲播放攻擊動作 (改由表控制)
                //if (StaticValue.m_bIsMaster){
                //    float i = UnityEngine.Random.Range(0.1f, 0.5f);
                //    ccTimeEvent.GetInstance().f_RegEvent(i, false, null, CallBack_DelayAttack);
                //}
            }

            // 死亡 ================================================        
            else if (AI_EM.EM_AIState.Die == tAIState) {
                //animator.SetInteger("control", 71);
                animator.Play("Die");
                animator.SetBool("attackInit", false);
            }

            // 換點 ================================================ 
            else if (AI_EM.EM_AIState.AI_ChangePoint == tAIState){
                animator.CrossFade("Run", 0.25f);
            }

            // 初始化 ==============================================
            else if (AI_EM.EM_AIState.ArcherInit == tAIState){
                animator.Play("Run");
            }
            else if (AI_EM.EM_AIState.MoveToHidePos == tAIState) {
                animator.Play("Run");
            }


            m_CurAIStatic = tAIState;
        }

    }


    private void CallBack_DelayAttack(object Obj) {
        Action_Animator tmpAction = new Action_Animator();
        tmpAction.f_SetAnimation(_BaseRoleControl.m_iId, GameEM.EM_Animator.SetBool, "CanAttack", true);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tmpAction);
    }



    /// <summary>
    /// 以躲藏點來決定動作
    /// </summary>
    private void GetHideInfo(){
        ArcherRoleControl _Solider = _BaseRoleControl.GetComponent<ArcherRoleControl>();
        if (_Solider == null){
            animator.CrossFade("Idle", 0.25f);
            return;
        }
        if (_Solider.HidePos.Count == 0){
            animator.Play("Reload");
            return;
        }
        PointPart tmp = _Solider.HidePos[_Solider.CurHidePos].GetComponent<PointPart>();
        if (tmp == null){
            animator.CrossFade("Idle", 0.25f);
            return;
        }

        if (tmp.HideType == EM_Hide.LeftHide) {
            animator.Play("Reload_L");
        }
        else if (tmp.HideType == EM_Hide.RightHide) {
            animator.Play("Reload_R");
        }
        else if (tmp.HideType == EM_Hide.StandHide) {
            animator.Play("Reload");
        }
        else if (tmp.HideType == EM_Hide.SquatHide) {
            animator.Play("SquatHide");
        }
        else {
            animator.CrossFade("Idle", 0.25f);
        }
    }






}

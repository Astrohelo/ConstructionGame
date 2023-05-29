using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    
    [SerializeField] private Animator animator;
    enum ActiveAnim { IDLE,RUN,JUMP,FALL};
    private ActiveAnim activeAnim;
    private const string IS_IDLE = "IsIdle";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_JUMPING = "IsJumping";
    private const string IS_FALLING = "IsFalling";




    public void SetIdle() {
        if (IsOwner && activeAnim != ActiveAnim.IDLE) {
            SetAllFalse();
            activeAnim = ActiveAnim.IDLE;
            animator.SetBool(IS_IDLE,true);
        }
    }
    public void SetRunning() {
        if (IsOwner && activeAnim != ActiveAnim.RUN) {
            SetAllFalse();
            activeAnim = ActiveAnim.RUN;
            animator.SetBool(IS_RUNNING, true);
        }
    }
    public void SetJumping() {
        if (IsOwner &&  activeAnim != ActiveAnim.JUMP) {
            SetAllFalse();
            activeAnim = ActiveAnim.JUMP;
            animator.SetBool(IS_JUMPING, true);
        }
    }
    public void SetFalling() {
        if (IsOwner &&  activeAnim != ActiveAnim.FALL) {
            SetAllFalse();
            activeAnim = ActiveAnim.FALL;
            animator.SetBool(IS_FALLING, true);
        }
    }
    public void SetAllFalse() {
        animator.SetBool(IS_IDLE, false);
        animator.SetBool(IS_RUNNING, false);
        animator.SetBool(IS_JUMPING, false);
        animator.SetBool(IS_FALLING, false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationStateAction : StateMachineBehaviour
{
    public UnityAction<Animator,AnimatorStateInfo,int> OnStateEnterAction;
    public UnityAction<Animator,AnimatorStateInfo,int> OnStateUpdateAction;
    public UnityAction<Animator,AnimatorStateInfo,int> OnStateExitAction;
    public UnityAction<Animator,AnimatorStateInfo,int> OnStateMoveAction;
    public UnityAction<Animator,AnimatorStateInfo,int> OnStateIKAction;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        OnStateEnterAction?.Invoke(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        OnStateUpdateAction?.Invoke(animator, stateInfo, layerIndex);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        OnStateExitAction?.Invoke(animator, stateInfo, layerIndex);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);
        OnStateMoveAction?.Invoke(animator, stateInfo, layerIndex);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateIK(animator, stateInfo, layerIndex);
        OnStateIKAction?.Invoke(animator, stateInfo, layerIndex);
    }
}

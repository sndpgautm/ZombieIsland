﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningBehaviour : StateMachineBehaviour {
	
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Player.Instance.IsRunning = true;


	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (animator.GetFloat("speed")>0.01) {
			Debug.Log ("run" + animator.GetBool ("run"));
			animator.SetFloat ("movementSpeed", Player.Instance.WalkingSpeed * 2);
		} else {
			Debug.Log ("run when speed is zero" + animator.GetBool ("run"));
			animator.SetBool ("run", false);
			animator.SetFloat("movementSpeed", Player.Instance.WalkingSpeed);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Player.Instance.IsRunning = false;
		animator.SetBool ("run", false);
		animator.SetFloat("movementSpeed", Player.Instance.WalkingSpeed);
		Debug.Log (" end run" + animator.GetBool ("run"));
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}

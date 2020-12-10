using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetup {

	public float speedDampTime = 0.2f, angularSpeedDampTime = 0.7f, angleResponseTime = 0.2f;

	private Animator anim;

	public AnimatorSetup (Animator animator) {
		anim = animator;
	}

	public void Setup (float speed, float angle) {
		float angularSpeed = angle / angleResponseTime;
		anim.SetFloat("Speed", speed, speedDampTime, Time.deltaTime);
		anim.SetFloat("Angular Speed", angularSpeed, angularSpeedDampTime, Time.deltaTime);

	}
}

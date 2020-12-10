using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashIDs : MonoBehaviour {

	// Player animation states:
	public int groundedState;
	public int airBorneState;
	public int crouchingState;

	// Enemy animation states:
	public int locomotionState;
	public int chargeLaserState;
	public int fireLaserState;

	// Player animation parameters:
	public int forwardFloat;
	public int turnFloat;
	public int onGroundBool;
	public int jumpFloat;
	public int jumpLegFloat;
	public int crouchBool;

	// Enemy animation parameters:
	public int speedFloat;
	public int angularSpeedFloat;
	public int playerInSightBool;

	// Use this for initialization
	void Awake () {
		// Player animation states:
		groundedState = Animator.StringToHash("Base Layer.Grounded");
		airBorneState = Animator.StringToHash("Base Layer.Airborne");
		crouchingState = Animator.StringToHash("Base Layer.Crouching");

		// Enemy animation states:
		locomotionState = Animator.StringToHash("Base Layer.Locomotion");
		chargeLaserState = Animator.StringToHash("ShootPlayer.ChargeLaser");
		fireLaserState = Animator.StringToHash("ShootPlayer.FireLaser");

		// Player animation parameters:
		forwardFloat = Animator.StringToHash("Forward");
		turnFloat = Animator.StringToHash("Turn");
		onGroundBool = Animator.StringToHash("OnGround");
		jumpFloat = Animator.StringToHash("Jump");
		jumpLegFloat = Animator.StringToHash("JumpLeg");
		crouchBool = Animator.StringToHash("Crouch");

		// Enemy animation parameters:
		speedFloat = Animator.StringToHash("Speed");
		angularSpeedFloat = Animator.StringToHash("Angular Speed");
		playerInSightBool = Animator.StringToHash("PlayerInSight");
	}
}

using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public PlayerCharacter pc;
	public Camera camera;
	public Rigidbody2D rigidbody;
	public Animator animator;

	private Vector2 actorFacing;
	private Vector2 dashDirection;
	private float timeDashed = 0;
	private const float dashDistance = 0.16f;

	void Start() {

	}

	//Updates the player internal state and sets the correct animation
	// transitions
	private void setState(CharacterState state) {
		pc.State = state;
		animator.SetBool("Moving", state == CharacterState.Moving);
		animator.SetBool("Blocking", state == CharacterState.Blocking);
		//animator.SetBool("Idle", state == CharacterState.Idle);
		if(state == CharacterState.Dashing)
			animator.SetTrigger("Dashing");
	}

	//Rotates the player to face the mouse
	// also normalizes the magnitude
	private void recordActorFacing() {
		Vector2 characterPos = rigidbody.position;
		Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
		actorFacing = mousePos - characterPos;
		actorFacing = Vector2.ClampMagnitude(actorFacing, 1);
		rigidbody.rotation = Vector2.Angle(Vector2.up, actorFacing);
		if(actorFacing.x > 0)
			rigidbody.rotation *= -1;
	}

	void Update() {
		//Debug.Log(rigidbody.velocity.magnitude);

		//Check to see if the player is dashing
		// If we aren't and we're not cooling down or anything
		// communicate the dash to the player controller,
		// and save the direction we're facing
		if(Input.GetAxis("Dash") > 0) {
			if(pc.canDash()) {
				setState(CharacterState.Dashing);
				pc.dash();
				recordActorFacing();
				dashDirection = actorFacing.normalized;
			}
		}

		//If the player is already dashing we need to lock up their controls
		// and complete our dash.
		if(pc.State == CharacterState.Dashing) {
			//Debug.Log(timeDashed);
			//if we haven't been dashing for too long keep a constant
			// velocity
			//otherwise communicate to the player we're now idle
			// and reset the dash timer;
			timeDashed += Time.deltaTime;
			if(timeDashed < dashDistance) {
				rigidbody.velocity = dashDirection * pc.dashSpeed();
			} else if(rigidbody.velocity.magnitude < 0.3f) {
				setState(CharacterState.Idle);
				timeDashed = 0;
			}
		} else if(Input.GetAxis("Block") > 0) {
			//if the player wants to block, stop the player from moving
			setState(CharacterState.Blocking);
			rigidbody.velocity = Vector2.zero;
		} else {
			//if the player doesn't want to do anything fancy, update the rotation
			// and listen for the player move button
			recordActorFacing();
			if(Input.GetAxis("Move") > 0) {
				setState(CharacterState.Moving);
				rigidbody.velocity = actorFacing * pc.MoveSpeed;
			} else {
				setState(CharacterState.Idle);
			}
		}
		animator.SetFloat("Speed", rigidbody.velocity.magnitude);
	}
}

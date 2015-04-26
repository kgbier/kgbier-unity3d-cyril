using UnityEngine;
using System.Collections;

public class PlayerCharacterController : MonoBehaviour {

	private PlayerCharacter	pc;
	private Rigidbody2D			rigidbody;
	private Animator				animator;

	private Vector2		actorFacing;
	private Vector2		dashDirection;
	private float			timeDashed = 0.0f;

	void Awake() {
		pc = GetComponent<PlayerCharacter>();
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	//Updates the player internal state and sets the correct animation
	// transitions
	private void setState(CharacterState state) {
		pc.State = state;
		animator.SetBool("Moving", state == CharacterState.Moving);
		animator.SetBool("Blocking", state == CharacterState.Blocking);
		animator.SetBool("Idling", state == CharacterState.Idle);
		if(state == CharacterState.Dashing)
			animator.SetTrigger("Dashing");
	}

	//Rotates the player to face the mouse
	// also normalizes the magnitude
	private void recordActorFacing(Vector2 mousePos) {
		Vector2 characterPos = rigidbody.position;
		actorFacing = mousePos - characterPos;
		actorFacing = Vector2.ClampMagnitude(actorFacing, 1);
		rigidbody.rotation = Vector2.Angle(Vector2.up, actorFacing);
		if(actorFacing.x > 0)
			rigidbody.rotation *= -1;
	}

	//Check to see if the player is dashing
	// If we aren't and we're not cooling down or anything
	// communicate the dash to the player object
	public void dash(Vector2 mousePos) {
		if(pc.canDash()) {
			setState(CharacterState.Dashing);
			pc.dash();
			recordActorFacing(mousePos);
			dashDirection = actorFacing.normalized;
		}
	}

	public void continueDash() {
		//if we haven't been dashing for too long keep a constant
		// velocity
		timeDashed += Time.deltaTime;
		if(timeDashed < pc.getDashDistance()) {
			rigidbody.velocity = dashDirection * pc.getDashSpeed();
		} else if(rigidbody.velocity.magnitude < 0.3f) {
			//otherwise communicate to the player we're now idle
			// and reset the dash timer;
			setState(CharacterState.Idle);
			//rigidbody.velocity = Vector2.zero;
			timeDashed = 0;
		}
	}

	public void block() {
		//if the player wants to block, stop the player from moving
		setState(CharacterState.Blocking);
		rigidbody.velocity = Vector2.zero;
	}

	public void idle(Vector2 mousePos) {
		//if the player doesn't want to do anything fancy, update the rotation
		setState(CharacterState.Idle);
		recordActorFacing(mousePos);
	}

	public void move(Vector2 mousePos) {
		setState(CharacterState.Moving);
		recordActorFacing(mousePos);
		rigidbody.velocity = actorFacing * pc.MoveSpeed;
	}

	void Update() {
		animator.SetFloat("Speed", rigidbody.velocity.magnitude);
	}
}

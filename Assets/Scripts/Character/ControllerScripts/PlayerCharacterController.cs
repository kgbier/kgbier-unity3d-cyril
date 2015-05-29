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

	//Updates the player internal state and sets the correct animation transitions
	private void setState(CharacterState state) {
		pc.State = state;
		animator.SetBool("Moving", state == CharacterState.Moving);
		animator.SetBool("Blocking", state == CharacterState.Blocking);
		animator.SetBool("Idling", state == CharacterState.Idle);
		if(state == CharacterState.Attacking)
			animator.SetTrigger("Attacking");
		if(state == CharacterState.Dashing)
			animator.SetTrigger("Dashing");
	}

	//Rotates the player to face the mouse, also clamps the magnitude for 
	// analog movement up to a point.
	private void recordActorFacing(Vector2 mousePos) {
		actorFacing = mousePos - rigidbody.position;
		actorFacing = Vector2.ClampMagnitude(actorFacing, 1);
		rigidbody.rotation = Vector2.Angle(Vector2.up, actorFacing);
		if(actorFacing.x > 0)
			rigidbody.rotation *= -1;
	}

	//Check to see if the player is dashing If we aren't and we're not
	// cooling down or anything communicate the dash to the player object
	public void dash(Vector2 mousePos) {
		if(pc.canDash()) {
			setState(CharacterState.Dashing);
			pc.dash();
			//As this is a movement based action, update our position
			recordActorFacing(mousePos);
			//Record the direction of the dash as this needs to be referenced for 
			// the duration of the action and cannot change
			dashDirection = actorFacing.normalized;
		}
	}

	public void continueDash() {
		//if we haven't been dashing for too long keep a constant velocity
		timeDashed += Time.deltaTime;
		if(timeDashed < pc.getDashDistance()) {
			rigidbody.velocity = dashDirection * pc.getDashSpeed();
		} else if(rigidbody.velocity.magnitude < 0.3f) {
			//otherwise communicate to the player we're now idle and reset 
			// the dash timer;
			setState(CharacterState.Idle);
			//rigidbody.velocity = Vector2.zero;
			timeDashed = 0;
		}
	}

	//if the player wants to block, stop the player from moving and forget to 
	// register orientation
	public void block() {
		setState(CharacterState.Blocking);
		rigidbody.velocity = Vector2.zero;
	}

	//if the player doesn't want to do anything fancy, simply update the rotation
	public void idle(Vector2 mousePos) {
		setState(CharacterState.Idle);
		recordActorFacing(mousePos);
	}

	//move in the direction of the mouse pointer
	public void move(Vector2 mousePos) {
		setState(CharacterState.Moving);
		recordActorFacing(mousePos);
		rigidbody.velocity = actorFacing * pc.MoveSpeed;
	}

	//if the player can attack, perform the action in the current direction
	public void attack() {
		if(pc.canAttack()) {
			setState(CharacterState.Attacking);
			pc.attack();
		}
	}

	void Update() {
		animator.SetFloat("Speed", rigidbody.velocity.magnitude);
	}
}

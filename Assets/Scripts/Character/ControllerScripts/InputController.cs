using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	private PlayerCharacterController controller;
	private PlayerCharacter						pc;

	// Use this for initialization
	void Awake() {
		pc = GetComponent<PlayerCharacter>();
		controller = GetComponent<PlayerCharacterController>();
	}

	// Update is called once per frame
	void Update() {
		switch(pc.State) {
			case CharacterState.Idle:
			case CharacterState.Moving:
			case CharacterState.Attacking:
			case CharacterState.Blocking:
				if(Input.GetButtonDown("Attack")) {
					controller.attack();
				} else if(Input.GetButtonDown("Dash")) {
					controller.dash(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				} else if(Input.GetButton("Block")) {
					controller.block();
				} else if(Input.GetButton("Move")) {
					controller.move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				} else {
					controller.idle(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				}

				break;
			case CharacterState.Dashing:
				controller.continueDash();
				break;
		}

	}
}

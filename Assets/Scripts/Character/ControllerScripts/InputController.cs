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
			case CharacterState.Blocking:
			case CharacterState.Attacking:
				if(Input.GetAxis("Dash") > 0) {
					controller.dash(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				} else if(Input.GetAxis("Block") > 0) {
					controller.block();
				} else if(Input.GetAxis("Move") > 0) {
					controller.move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				} else {
					controller.idle(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				}

				if(Input.GetAxis("Attack") > 0) {
					controller.attack();
				}

				break;
			case CharacterState.Dashing:
				controller.continueDash();
				break;
		}

	}
}

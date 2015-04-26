using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	private PlayerCharacterController controller;
	private PlayerCharacter						pc;

	public Camera		camera;

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
				if(Input.GetAxis("Dash") > 0) {
					controller.dash(camera.ScreenToWorldPoint(Input.mousePosition));
				} else if(Input.GetAxis("Block") > 0) {
					controller.block();
				} else if(Input.GetAxis("Move") > 0) {
					controller.move(camera.ScreenToWorldPoint(Input.mousePosition));
				} else {
					controller.idle(camera.ScreenToWorldPoint(Input.mousePosition));
				}
				break;
			case CharacterState.Dashing:
				controller.continueDash();
				break;
		}

	}
}

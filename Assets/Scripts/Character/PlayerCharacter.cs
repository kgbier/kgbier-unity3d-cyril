using UnityEngine;
using System.Collections.Generic;

public enum CharacterState {
	Idle, Moving, Blocking, Dashing
};

public class PlayerCharacter : MonoBehaviour {

	private const float moveSpeed = 5;

	private float stamina = 100;
	private float maxStamina = 100;
	private float staminaRegenRate = 15;

	private CharacterState currentState = CharacterState.Idle;

	private List<Ability> abilities = new List<Ability>();

	public float MoveSpeed {
		get { return moveSpeed; }
	}

	public float Stamina {
		get { return stamina; }
	}
	public CharacterState State {
		get { return currentState; }
		set { currentState = value; }
	}

	private void regenerateStamina() {
		if(stamina < maxStamina) {
			stamina += staminaRegenRate * Time.deltaTime;
		} else {
			stamina = maxStamina;
		}
	}

	public bool canDash() {
		if(currentState != CharacterState.Dashing) {
			return abilities[0].canUse(this);
		}
		return false;
	}

	public float dashSpeed() {
		return abilities[0].DashSpeed;
	}

	public void dash() {
		useAbility(abilities[0]);
	}

	private void cooldownAbilities() {
		//Debug.Log(dashCooldown);
		foreach(Ability a in abilities) {
			a.runCooldown();
		}
	}

	public void useAbility(Ability a) {
		stamina -= a.staminaCost;
		a.use();
	}

	// Use this for initialization
	void Start() {
		abilities.Add(new Ability("Dash", 1));
	}

	// Update is called once per frame
	void Update() {
		regenerateStamina();
		cooldownAbilities();
	}
}

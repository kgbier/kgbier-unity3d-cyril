using UnityEngine;
using System.Collections.Generic;

public enum CharacterState {
	Idle, Moving, Blocking, Dashing
};

public class PlayerCharacter : MonoBehaviour {

	private float		moveSpeed = 5.0f;
	private float		stamina = 100.0f;
	private float		maxStamina = 100.0f;
	private float		staminaRegenRate = 15.0f;

	private CharacterState currentState = CharacterState.Idle;

	private List<Ability>		abilities = new List<Ability>();
	private DashAbility			dashAbility;
	private AttackAbility		attackAbility;

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

	//Asks if the player character can use dash
	// (is not already dashing? if not ask the ability)
	public bool canDash() {
		if(currentState != CharacterState.Dashing) {
			return dashAbility.canUse(stamina);
		}
		return false;
	}
	public float getDashSpeed() {
		return dashAbility.Speed;
	}
	public float getDashDistance() {
		return dashAbility.Distance;
	}
	public void dash() {
		useAbility(dashAbility);
	}

	//Regenerates stamina, uses deltaTime, only call in Update();
	private void regenerateStamina() {
		if(stamina < maxStamina) {
			stamina += staminaRegenRate * Time.deltaTime;
		} else {
			stamina = maxStamina;
		}
	}

	//Runs the cooldown for each of the player's abilities
	private void cooldownAbilities() {
		//Debug.Log(dashCooldown);
		foreach(Ability a in abilities) {
			if(a.cooldown > 0) {
				a.cooldown -= Time.deltaTime;
			} else {
				a.cooldown = 0;
			}
		}
	}

	//Calculate stamina cost of an ability and use it.
	public void useAbility(Ability a) {
		stamina -= a.staminaCost;
		a.use();
	}

	// Use this for initialization
	void Start() {
		dashAbility = new DashAbility("Dash", 1.0f, 40.0f);
		attackAbility = new AttackAbility("Attack", 1.0f, 10.0f);
		abilities.Add(dashAbility);
		abilities.Add(attackAbility);
	}

	// Update is called once per frame
	void Update() {
		regenerateStamina();
		cooldownAbilities();
	}

}

using UnityEngine;
using System.Collections.Generic;

public enum CharacterState {
	Idle, Moving, Blocking, Dashing, Attacking
};

public class PlayerCharacter : MonoBehaviour {

	private float		moveSpeed = 5.0f;
	private float		stamina = 100.0f;
	private float		maxStamina = 100.0f;
	private float		baseStaminaRegenRate = 15.0f;

	//because Malus is a better word than penalty :p
	private const float movementStaminaMalus = 8.0f;
	private const float blockingStaminaMalus = 12.0f;

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

	//Asks if the player character can use dash (is not already dashing)
	public bool canDash() {
		if(currentState != CharacterState.Dashing) {
			return dashAbility.canUse(stamina);
		}
		return false;
	}

	//Asks if the player can attack (idle or blocking)
	public bool canAttack() {
		if(currentState == CharacterState.Idle
			|| currentState == CharacterState.Blocking) {
			return attackAbility.canUse(stamina);
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
	public void attack() {
		useAbility(attackAbility);
	}

	//Regenerates stamina and applies stamina regen Malus' uses deltaTime,
	// only call in Update().
	private void regenerateStamina() {
		float regenRate = baseStaminaRegenRate;
		switch(currentState) {
			case CharacterState.Moving:
				regenRate -= movementStaminaMalus;
				break;
			case CharacterState.Blocking:
				regenRate -= blockingStaminaMalus;
				break;
		}
		if(stamina < maxStamina) {
			stamina += regenRate * Time.deltaTime;
		} else {
			stamina = maxStamina;
		}
	}

	//Runs the cooldown for each of the player's abilities uses deltaTime, 
	// only call in Update().
	private void cooldownAbilities() {
		foreach(Ability a in abilities) {
			if(a.cooldown > 0) {
				a.cooldown -= Time.deltaTime;
			} else {
				a.cooldown = 0;
			}
		}
	}

	//Apply stamina cost of an ability and use it.
	public void useAbility(Ability a) {
		stamina -= a.staminaCost;
		a.use();
	}

	//Initialization. Creates test abilities dash and attack.
	void Awake() {
		dashAbility = new DashAbility("Dash", 0.5f, 38.0f);
		attackAbility = new AttackAbility("Attack", 0.0f, 12.0f);
		abilities.Add(dashAbility);
		abilities.Add(attackAbility);
	}

	//Regenerate player stamina and run cooldown for all player abilities.
	void Update() {
		regenerateStamina();
		cooldownAbilities();
	}

}

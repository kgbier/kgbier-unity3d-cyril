using UnityEngine;
using System.Collections;

public class Ability {

	public string name;
	public float cooldownDuration;

	public float staminaCost = 40;
	public const float dashSpeed = 20;

	public float cooldown = 0;

	public float DashSpeed {
		get { return dashSpeed; }
	}

	public Ability(string name, float cooldownDuration) {
		this.name = name;
		this.cooldownDuration = cooldownDuration;
	}

	public bool canUse(PlayerCharacter pc) {
		if(cooldown == 0
			&& pc.Stamina > staminaCost) {
			return true;
		}
		return false;
	}

	public void runCooldown() {
		if(cooldown > 0) {
			cooldown -= Time.deltaTime;
		} else {
			cooldown = 0;
		}
	}

	public void use() {
		cooldown = cooldownDuration;
	}

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class AttackAbility : Ability {

	private float damage = 1.0f;
	private float range = 1.0f;

	public AttackAbility(string name, float cooldownDuration, float staminaCost) :
		base(name, cooldownDuration, staminaCost) { }

	public float Damage {
		get { return damage; }

	}

	public float Range {
		get { return range; }

	}

}
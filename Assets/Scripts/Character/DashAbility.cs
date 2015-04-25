using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class DashAbility : Ability {

	public const float	speed = 20.0f;
	private const float distance = 0.16f;

	public DashAbility(string name, float cooldownDuration, float staminaCost) :
		base(name, cooldownDuration, staminaCost) { }

	public float Speed {
		get { return speed; }

	}
	public float Distance {
		get { return distance; }

	}

}
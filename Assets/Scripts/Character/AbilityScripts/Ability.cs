using System.Collections;

public class Ability {

	public string		name;
	public float		cooldownDuration;
	public float		staminaCost;

	public float		cooldown = 0.0f;

	public string Name {
		get { return name; }
	}

	public float Cooldown {
		get { return cooldown; }
		set { cooldown = value; }
	}

	public Ability(string name, float cooldownDuration, float staminaCost) {
		this.name = name;
		this.cooldownDuration = cooldownDuration;
		this.staminaCost = staminaCost;
	}

	public bool canUse(float stamina) {
		if(cooldown == 0
			&& stamina > staminaCost) {
			return true;
		}
		return false;
	}

	public void use() {
		cooldown = cooldownDuration;
	}

}

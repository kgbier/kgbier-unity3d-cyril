using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaBar : MonoBehaviour {

	public Slider						slider;
	public PlayerCharacter	pc;

	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		slider.value = pc.Stamina;
	}
}

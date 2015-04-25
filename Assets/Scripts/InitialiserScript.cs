using UnityEngine;
using System.Collections;

public class InitialiserScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = -1;
	}
}

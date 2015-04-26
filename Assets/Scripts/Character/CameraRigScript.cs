using UnityEngine;
using System.Collections;

public class CameraRigScript : MonoBehaviour {

	public GameObject character;

	// Update is called once per frame
	void LateUpdate() {
		//Debug.Log(character.transform.position);
		transform.position = new Vector3(character.transform.position.x, character.transform.position.y, transform.position.z);
	}
}

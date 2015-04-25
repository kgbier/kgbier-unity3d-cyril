using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {
	void LateUpdate () {
		transform.rotation = Quaternion.identity;
	}
}

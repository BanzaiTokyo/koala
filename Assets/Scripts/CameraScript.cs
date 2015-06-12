using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	Vector3 beforeShakePos;
	Quaternion beforeShakeRot;
	float shakeDecay, shakeIntensity;
	bool shaking = false;
	Vector3 shakeDir = Vector3.zero, prevShakeDir = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (shakeIntensity > 0f) {
			prevShakeDir = shakeDir;
			shakeDir = new Vector3(Random.value * shakeIntensity, Random.value * shakeIntensity, 0f);
			transform.position = beforeShakePos + shakeDir;
			foreach (Transform t in transform) {
				t.position = t.position - prevShakeDir + shakeDir;
			}
			/*transform.rotation = new Quaternion (
				beforeShakeRot.x + Random.Range (-shakeIntensity, shakeIntensity) * .2f*0f,
				beforeShakeRot.y + Random.Range (-shakeIntensity, shakeIntensity) * .2f*0f,
				beforeShakeRot.z + Random.Range (-shakeIntensity, shakeIntensity) * .2f,
				beforeShakeRot.w + Random.Range (-shakeIntensity, shakeIntensity) * .2f*0f);*/
			shakeIntensity -= shakeDecay;
		}
		else if (shaking) {
			transform.position = beforeShakePos;
			foreach (Transform t in transform) {
				t.position = t.position - shakeDir;
			}
			shaking = false;
		}
	}

	public void Shake() {
		beforeShakePos = transform.position;
		beforeShakeRot = transform.rotation;
		shakeIntensity = 0.3f;
		shakeDecay = 0.02f;
		shaking = true;
	}
}

using UnityEngine;
using System.Collections;

public class BeeAnimator : MonoBehaviour {
	public float speed;
	public float radius;
	public bool reverse;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		Vector3 v = transform.localPosition;
		v.x = radius;
		transform.localPosition = v;
		if (reverse) {
			v = transform.localScale;
			v.x = -v.x;
			transform.localScale = v;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 axis = reverse ? Vector3.back : Vector3.forward;
		transform.RotateAround(transform.parent.position, axis, speed * Time.deltaTime);
	}
}

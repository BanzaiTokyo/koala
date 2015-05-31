using UnityEngine;
using System.Collections;

public class BeeAnimator : MonoBehaviour {
	public float speed;
	public float radius;
	public bool reverse;
	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().castShadows = true;
		transform.localPosition = new Vector2 (radius, 0f);
		if (reverse) {
			Vector3 scale = transform.localScale;
			transform.localScale = new Vector3 (scale.x, -scale.y, scale.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 axis = reverse ? Vector3.back : Vector3.forward;
		transform.RotateAround(transform.parent.position, axis, speed * Time.deltaTime);
	}
}

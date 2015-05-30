using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {
	private bool clockwise;
	public float MAXANGLE = 55f;
	public float DAYDURATION = 60f;

	// Use this for initialization
	void Start () {
		clockwise = true;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = transform.eulerAngles;
		if (clockwise && (v.y >= MAXANGLE) && (v.y < 360 - MAXANGLE) ||
		    !clockwise && (v.y <= 360 - MAXANGLE) && (v.y > MAXANGLE))
			clockwise = !clockwise;
		float angle = MAXANGLE * 2 / DAYDURATION * Time.deltaTime;
		if (!clockwise)
			angle = -angle;
		v.y += angle;
		transform.eulerAngles = v;
	}
}

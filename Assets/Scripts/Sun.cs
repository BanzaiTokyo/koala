using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {
	private bool clockwise;
	public float dayLength = 60f;
	private float startIntensity;
	public float endIntensity = 0.5f;
	public Transform waypoints;
	private int currentWaypointIdx;
	private GameObject shineTo;
	private Vector3 sunVelocity;
	private float pathLength = 0f;
	private float speed;
	private float deltaIntensity;
	private Light thisLight;
	private float passedPath = 0f;
	// Use this for initialization
	void Start () {
		clockwise = true;
		currentWaypointIdx = 0;
		Transform prevTransform = null;
		foreach (Transform wp in waypoints.transform) {
			wp.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			if (prevTransform)
			  pathLength += (wp.position - prevTransform.position).magnitude;
			prevTransform = wp;
		}
		speed = pathLength / dayLength;
		thisLight = GetComponent<Light>();
		startIntensity = thisLight.intensity;
		deltaIntensity = (endIntensity - startIntensity) / dayLength;
		shineTo = new GameObject ();
		shineTo.transform.parent = waypoints.transform;
		shineTo.transform.localPosition = waypoints.GetChild (0).localPosition;
	}

	void Update() {
		if (clockwise && currentWaypointIdx < waypoints.transform.childCount || !clockwise && currentWaypointIdx >= 0) {
			Vector3 target = waypoints.transform.GetChild(currentWaypointIdx).localPosition;
			shineTo.transform.forward = Vector3.RotateTowards(shineTo.transform.forward, target - shineTo.transform.localPosition, speed*Time.deltaTime, 0.0f);
			shineTo.transform.localPosition = Vector3.MoveTowards(shineTo.transform.localPosition, target, speed*Time.deltaTime);
			Vector3 step = target - shineTo.transform.localPosition;

			if (step.magnitude < Mathf.Epsilon) {
				currentWaypointIdx += clockwise ? 1 : -1;
			}
			else {
				passedPath += step.normalized.magnitude*speed * Time.deltaTime;
				sunVelocity = step.normalized*speed * Time.deltaTime;
				transform.LookAt(shineTo.transform.position);
				thisLight.intensity += deltaIntensity*Time.deltaTime*(clockwise ? 1f : -1f);
			}

		}
		if (clockwise && currentWaypointIdx >= waypoints.transform.childCount || !clockwise && currentWaypointIdx < 0) {
			clockwise = !clockwise;
			currentWaypointIdx += clockwise ? 1 : -1;
			transform.LookAt(waypoints.transform.GetChild(currentWaypointIdx).position);
			thisLight.intensity = clockwise ? startIntensity : endIntensity;
			Debug.Log ("sun turns back "+Time.time+" fin intensity "+thisLight.intensity+" passed "+passedPath);
			passedPath = 0f;
		}
	}
}

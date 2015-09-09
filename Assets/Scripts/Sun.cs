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
		shineTo.transform.position = waypoints.GetChild (0).position;
	}

	void Update() {
		if (clockwise && currentWaypointIdx < waypoints.transform.childCount || !clockwise && currentWaypointIdx >= 0) {
			speed = pathLength / dayLength;
			thisLight = GetComponent<Light>();
			deltaIntensity = (endIntensity - startIntensity) / dayLength;
			float deltaSpeed = speed * Time.deltaTime;
			Vector3 target = waypoints.transform.GetChild(currentWaypointIdx).position;
			shineTo.transform.forward = Vector3.RotateTowards(shineTo.transform.forward, target - shineTo.transform.position, deltaSpeed, 0.0f);
			shineTo.transform.position = Vector3.MoveTowards(shineTo.transform.position, target, deltaSpeed);
			Vector3 step = target - shineTo.transform.position;

			if (step.magnitude < Mathf.Epsilon) {
				currentWaypointIdx += clockwise ? 1 : -1;
			}
			else {
				passedPath += step.normalized.magnitude * deltaSpeed;
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

using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {
	private bool clockwise;
	public float SUNSPEED = 1f;
	public Transform waypoints;
	private int currentWaypointIdx;
	private GameObject shineTo;
	private Vector3 sunVelocity;
	// Use this for initialization
	void Start () {
		clockwise = true;
		currentWaypointIdx = 0;
		shineTo = new GameObject ();
		shineTo.transform.parent = waypoints.transform;
		shineTo.transform.localPosition = waypoints.GetChild (0).localPosition;
		foreach (Transform wp in waypoints.transform) {
			wp.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	void Update() {
		if (clockwise && currentWaypointIdx < waypoints.transform.childCount || !clockwise && currentWaypointIdx >= 0) {
			Vector3 target = waypoints.transform.GetChild(currentWaypointIdx).localPosition;
			Vector3 moveDirection = target - shineTo.transform.localPosition;

			if (moveDirection.magnitude < 1f) {
				currentWaypointIdx += clockwise ? 1 : -1;
			}
			else {
				sunVelocity = moveDirection.normalized*SUNSPEED * Time.deltaTime;
				shineTo.transform.localPosition += sunVelocity;
				transform.LookAt(shineTo.transform.position);
			}
		}
		if (clockwise && currentWaypointIdx >= waypoints.transform.childCount || !clockwise && currentWaypointIdx < 0) {
			clockwise = !clockwise;
			currentWaypointIdx += clockwise ? 1 : -1;
		}
	}
}

using UnityEngine;
using System.Collections;

public class ThumbController : MonoBehaviour {

	[HideInInspector] public RectTransform r;
	public float CONTROLLER_RETURN_SPEED;
	public RectTransform leftBars;
	public RectTransform rightBars;
	public GameObject button;

	// Use this for initialization
	void Start () {
		r = GetComponent<RectTransform>();
		r.position = new Vector3 (Screen.width / 2, r.position.y, r.position.z);
		button.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 size = r.sizeDelta;
		if (button.activeSelf)
			size = button.GetComponent<RectTransform> ().sizeDelta;
		if (!Input.GetMouseButton (0) && (Input.touchCount == 0)) { //no touch, return controller to center
			Vector3 pos = r.position;
			float leftOrRight = Screen.width/2 - pos.x;
			float dx = CONTROLLER_RETURN_SPEED*Time.deltaTime;
			if (Mathf.Abs(leftOrRight) > dx) {
				dx *= leftOrRight;
				r.Translate(new Vector3(dx, 0, 0));
			}
		}
		leftBars.sizeDelta = new Vector2 (r.position.x - size.x / 2 - leftBars.position.x - 20, leftBars.sizeDelta.y);
		rightBars.sizeDelta = new Vector2 (Screen.width - leftBars.position.x - r.position.x - size.x / 2 - 20, rightBars.sizeDelta.y);
	}
}

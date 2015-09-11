using UnityEngine;
using UnityEngine.UI;

public class ThumbController : MonoBehaviour {

	[HideInInspector] public RectTransform r;
	public float CONTROLLER_RETURN_SPEED;
	public RectTransform leftBars;
	public RectTransform rightBars;
	public GameObject button;

	// Use this for initialization
	void Start () {
		r = GetComponent<RectTransform>();
		r.position = new Vector3 (Screen.width / 2f, r.position.y, r.position.z);
		float cm2i = 2.54f * 2f;
		float width = 1.53f * Screen.dpi / cm2i;
		r.sizeDelta = new Vector2 (width, width);
		Vector2 barsSize = new Vector2 (2.3f * Screen.dpi / cm2i, r.sizeDelta.y * 0.8f);
		float barsScale = barsSize.y / leftBars.FindChild ("big").GetComponent<RectTransform> ().sizeDelta.y;
		foreach (Transform child in leftBars.transform)
		{
			RectTransform br = child.gameObject.GetComponent<RectTransform>();
			br.localScale = new Vector3(barsScale, barsScale, barsScale);
		}
		foreach (Transform child in rightBars.transform)
		{
			RectTransform br = child.gameObject.GetComponent<RectTransform>();
			br.localScale = new Vector3(barsScale, barsScale, barsScale);
		}
		leftBars.sizeDelta = barsSize;
		rightBars.sizeDelta = barsSize;
		leftBars.position = new Vector3 (r.position.x - barsSize.x - 20f, leftBars.position.y, 0f);
		rightBars.position = new Vector3 (r.position.x + barsSize.x + 20f, rightBars.position.y, 0f);
		button.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		Vector2 size = r.sizeDelta;
		//if (button.activeSelf)
		//	size = button.GetComponent<RectTransform> ().sizeDelta;
		if (!Input.GetMouseButton (0) && (Input.touchCount == 0)) { //no touch, return controller to center
			Vector3 pos = r.position;
			float leftOrRight = Screen.width/2 - pos.x;
			float dx = CONTROLLER_RETURN_SPEED*Time.deltaTime;
			if (Mathf.Abs(leftOrRight) > dx) {
				dx *= leftOrRight;
				r.Translate(new Vector3(dx, 0, 0));
			}
		}
		leftBars.sizeDelta = new Vector2 (r.position.x - size.x / 2f - leftBars.position.x - 20f, leftBars.sizeDelta.y);
		rightBars.sizeDelta = new Vector2 (Screen.width - leftBars.position.x - r.position.x - size.x / 2f - 20f, rightBars.sizeDelta.y);
	}

	public void setColorAlpha(float a) {
		Color c = GetComponent<Image>().color;
		c.a = a;
		GetComponent<Image>().color = c;

	}
}

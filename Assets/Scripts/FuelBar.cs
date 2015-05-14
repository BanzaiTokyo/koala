using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FuelBar : MonoBehaviour {
	public float percentage;

	public void setPercentage(float percentage) {
		RectTransform canvasRect = GameObject.Find ("Canvas").GetComponent<RectTransform>();
		RectTransform rt = GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (canvasRect.rect.width * percentage, rt.rect.height);
		Color c;
		if (percentage < 0.2f)
			c = new Color (1f, 0f, 0f);
		else
			c = new Color (0f, 1f, 0f);
		GetComponent<Image> ().color = c;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

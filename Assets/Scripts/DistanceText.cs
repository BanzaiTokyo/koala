using UnityEngine;
using System.Collections;

public class DistanceText : MonoBehaviour {
	public float distanceTextStep = 50f;
	private float minY;
	// Use this for initialization
	void Start () {
		//GetComponent<Renderer>().sortingOrder = 0; 
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		if (pos.y > Camera.main.transform.position.y - Camera.main.orthographicSize / 2f - GetComponent<Renderer> ().bounds.size.y)
			return;
		pos.y = (Mathf.Floor(Camera.main.transform.position.y / distanceTextStep) + 1f) * distanceTextStep;
		transform.position = pos;
		GetComponent<TextMesh>().text = Mathf.RoundToInt(pos.y).ToString();
	}
}

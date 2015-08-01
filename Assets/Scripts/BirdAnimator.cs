using UnityEngine;
using System.Collections;

public class BirdAnimator : MonoBehaviour {
	public bool reverse;
	public float MINBIRDSPEED = 1f;
	public float MAXBIRDSPEED = 3f;
	public float BIRDREPEATTIME = 15f;

	private SpriteRenderer spriteRenderer;
	private bool flying = false;
	private float speed;
	private float _start_time = -1f;
	private float oldZ;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
		oldZ = transform.localPosition.z;
	}

	public void Fly() {
		if (_start_time > 0f) {
			Vector3 pos = transform.localPosition;
			Vector3 scale = transform.localScale;
			pos.y = Random.Range (-Camera.main.orthographicSize, 0f);
			pos.z = oldZ;
			speed = Random.Range (MINBIRDSPEED, MAXBIRDSPEED);
			float halfSize = spriteRenderer.bounds.size.x / 2f;
			if (reverse) {
				pos.x = (Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0, 0))).x + halfSize;
				speed = -speed;
				transform.localScale = new Vector3 (Mathf.Abs (scale.x), scale.y, scale.z);
			} else {
				transform.localScale = new Vector3 (-Mathf.Abs (scale.x), scale.y, scale.z);
				pos.x = (Camera.main.ScreenToWorldPoint (Vector3.zero)).x - halfSize;
			}
			transform.localPosition = pos;
			flying = true;
		}
		_start_time = Time.time;
	}

	// Update is called once per frame
	void Update () {
		if (!flying) {
			if ((_start_time > 0f) && (Time.time - _start_time >= BIRDREPEATTIME))
				Fly ();
		}
		else
			transform.Translate (speed * Time.deltaTime, 0f, 0f);
	}

	void OnBecameInvisible() {
		flying = false;
		_start_time = Time.time;
		Vector3 pos = transform.localPosition;
		oldZ = pos.z;
		pos.z = Camera.main.farClipPlane + 1f;
		transform.localPosition = pos;
	}
}

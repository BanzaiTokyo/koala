using UnityEngine;
using System.Collections;

public class BeeAnimator : MonoBehaviour {
	public float speed;
	public float radius;
	public bool reverse;
	private Sprite[] sprites;
	public float framesPerSecond;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().castShadows = true;
		spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
		sprites = Resources.LoadAll<Sprite>("Sprites/bee");

		GetComponent<Renderer>().castShadows = true;
		transform.localPosition = new Vector2 (radius, 0f);
		if (reverse) {
			Vector3 scale = transform.localScale;
			transform.localScale = new Vector3 (-scale.x, scale.y, scale.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
		int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
		index = index % sprites.Length;
		spriteRenderer.sprite = sprites[ index ];

		Vector3 axis = reverse ? Vector3.back : Vector3.forward;
		transform.RotateAround(transform.parent.position, axis, speed * Time.deltaTime);
	}
}

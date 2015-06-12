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
		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
		sprites = Resources.LoadAll<Sprite>("Sprites/bee");

		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		Vector3 v = transform.localPosition;
		v.x = radius;
		transform.localPosition = v;
		if (reverse) {
			v = transform.localScale;
			v.x = -v.x;
			transform.localScale = v;
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

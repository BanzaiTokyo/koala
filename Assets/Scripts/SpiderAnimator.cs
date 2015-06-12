using UnityEngine;
using System.Collections;

public class SpiderAnimator : Actor {
	public float MINSPIDERTIME = 1f;
	public float MAXSPIDERTIME = 5f;
	public float SPIDERREPEATTIME = 11f;

	private Sprite[] sprites;
	public float framesPerSecond;
	private SpriteRenderer spriteRenderer;
	private float _start_time = -1f;
	private Vector3 offScreenPos;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
		sprites = Resources.LoadAll<Sprite>("Sprites/spider");
	}
	
	// Update is called once per frame
	void Update () {
		int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
		index = index % sprites.Length;
		spriteRenderer.sprite = sprites[ index ];

		if ((_start_time > 0f) && ((Time.time - _start_time) >= SPIDERREPEATTIME)) {
			ShowSpider ();
			float timeToMoveDown = Random.Range (MINSPIDERTIME, MAXSPIDERTIME),
			timeToWait = Random.Range (MINSPIDERTIME, MAXSPIDERTIME),
			timeToMoveUp = Random.Range (MINSPIDERTIME, MAXSPIDERTIME);
			AddAction(new Sequence(
				new MoveLocalTo(new Vector3(offScreenPos.x, 0f, offScreenPos.z), timeToMoveDown),
				new SleepFor (timeToWait),
				new MoveLocalTo(offScreenPos, timeToMoveUp)
			));
			_start_time += timeToMoveDown + timeToWait + timeToMoveUp;
		} else
			UpdateActions ();
	}

	public void ShowSpider() {
		float minX = Camera.main.ScreenToWorldPoint (Vector3.zero).x,
		maxX = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0f, 0f)).x,
		x = Random.Range (minX, maxX);
		offScreenPos = transform.localPosition;
		offScreenPos.x = x;
		transform.localPosition = offScreenPos;
		_start_time = Time.time;
	}

}

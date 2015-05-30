
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameLogic : MonoBehaviour {
	private static readonly bool DEBUGSTEER = false;

	public BeeAnimator bee;
	public SpiderAnimator spider;
	public GameObject[] trees;
	public GameObject[] clouds;
	public BirdAnimator bird;
	public GameObject powerup;
	public Text scoreLabel;
	public FuelBar fuelBar;
	public ThumbController thumb;
	public GameObject leftEar;
	public GameObject rightEar;
	public ParticleSystem fire;
	public ParticleSystem lastSmoke;


	private int NUMLEVELS = 4;
	public int[] levelTargets = new int[]{100, 200, 300, 0};
	
	private float SPOTGRIDSIZE = 2.10f;
	private int[] obstacleDistance = new int[]{7, 7, 5, 4}; //multiplies of SPOTGRIDSIZE
	private float[] damperAppearanceProbability = new float[]{0.0f, 0.3f, 0.75f, 0.75f};

	public float BEEPROBABILITY = 0.3f;

	public float ACCELERATION_UP = 30.0f;
	public float ACCELERATION_DOWN = 7.000f;
	public float[] maxVelocity = new float[]{10f, 15f, 20f, 20f};
	
	private int INITIALFUEL = 20;
	private int MAXFUEL = 20;
	public float[] fuelCPL = new float[]{1f, 1.5f, 2f, 3f}; //consumption per level
	private float[] fuelInPowerup = new float[]{10f, 10f, 8f, 5f};
	
	private float THUMBMARGIN = 90f;
	private float CONTROLLER_DEAD_ZONE = 0.1f;
	public float STEER_FORCE = 0.2f;
	private float MAXBODYROLL = 150f;
	private float MAXEARANGLE = 20f;

	private int BACKGROUNDLAYER = 0;
	private int OBSTACLESLAYER = -1;
	private int POWERUPSLAYER = -2;
	private int BODYLAYER = 0;
	private int DAMPERSLAYER = -6;
	private int SPIDERLAYER = -7;
	private int BIRDLAYER = -8;
	private int HUDLAYER = -9;

	private float scale = 1f;
	Vector2 midScreen;
	bool accelerating;
	float damperSize;
	float score;
	GameObject bg1;
	GameObject bg2;
	bool isGameOver;
	float thumbMargin;
	float massScale;
	float fuel;
	float prevTime;
	float bgSize;
	int globalGridY;
	int level;
	static bool oddRow;
	Rigidbody2D rigidBody;
	float leftMargin, rightMargin;

	private bool isTouchDevice = false;
	void Awake() {
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
			isTouchDevice = true; 
		else
			isTouchDevice = false;
	}

	float randomFloat(float from, float to) {
		return Random.value * (to - from) + from;
	}

	public GameObject AddSprite (string texName) {
		Texture2D _texture = Resources.Load<Texture2D>(texName);
		Sprite newSprite = Sprite.Create(_texture, new Rect(0f, 0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 128f);
		GameObject sprGameObj = new GameObject();
		sprGameObj.name = texName;
		sprGameObj.AddComponent<SpriteRenderer>();
		SpriteRenderer sprRenderer = sprGameObj.GetComponent<SpriteRenderer>();
		sprRenderer.sprite = newSprite;
		return sprGameObj;
	}

	GameObject generateBackgroundWithObstacles(bool withObstacles) {
		GameObject result = new GameObject();
		float x, y = 0,
		kGrid = SPOTGRIDSIZE * scale,
		screenWidth = rightMargin-leftMargin;
		Debug.Log (leftMargin + " " + rightMargin + " "+kGrid);
		int obstacleIdx = (int)(screenWidth / kGrid) + 1;
		obstacleIdx = (int)(Random.value * obstacleIdx);
		while (y < bgSize) {
			x = oddRow ? 0f : kGrid/2f;
			oddRow = !oddRow;
			int iX = 0;
			bool obstacleAdded = false;
			while (x < screenWidth) {
				string spotName = Mathf.RoundToInt(Random.value*44 + 1).ToString("D2")+"@2x";
				GameObject spot = this.AddSprite("Sprites/spots/"+spotName);

				spot.transform.position = new Vector3(x, y, BACKGROUNDLAYER);
				SpriteRenderer r = spot.GetComponent<SpriteRenderer>();
				Color c = r.color;
				c.a = 0.4f;
				r.color = c;
				spot.transform.parent = result.transform;
				if (withObstacles && (globalGridY % obstacleDistance[level] == 0) && !obstacleAdded && (iX == obstacleIdx)) {
					obstacleAdded = true;
					GameObject o = this.generateObstacle();
					o.transform.parent = result.transform;
					o.transform.localPosition = new Vector3(x, y, OBSTACLESLAYER);
				}
				x = x + kGrid;
				iX++;
			}
			globalGridY++;
			y = y + kGrid/2f;
		}
		return result;
	}

	GameObject generateObstacle() { 
		if (Random.value <= damperAppearanceProbability[level]) {
			GameObject damper = this.generateDamper();
			return damper;
		}
		else {
			int treeIdx = (int)Random.value*trees.Length;
			GameObject obstacle = Instantiate(trees[treeIdx]);
			this.generatePowerupAtPalm(obstacle);

			if (Random.value <= BEEPROBABILITY) {
				BeeAnimator abee = Instantiate<BeeAnimator>(bee);
				abee.reverse = Random.value > 0.5f;
				Vector3 v = abee.transform.position;
				v.z = POWERUPSLAYER;
				abee.transform.parent = obstacle.transform;
				abee.transform.localPosition = v;
			}
			return obstacle;
		}
	}

	GameObject generateDamper() {
		int cloudIdx = (int)Random.value*clouds.Length;
		GameObject damper = Instantiate(clouds[cloudIdx]);
		return damper;
	}

	void generatePowerupAtPalm(GameObject obstacle) {
		GameObject fuel = Instantiate (powerup);
		fuel.transform.parent = obstacle.transform;
		fuel.transform.localPosition = new Vector3 (1.206f, 0.33f, 0);
	}

	void setScore(float _score) {
		if (_score > score || Mathf.Approximately(_score, 0f)) {
			score = _score;
			scoreLabel.text = "Score: " + score;
			if (level < NUMLEVELS - 1 && score > levelTargets [level]) {
				level++;
				Debug.Log ("level: " + level);
			}
		}
	}
	
	public void AddFuel() {
		setFuel (fuel + fuelInPowerup [level]);
	}
	
	void setFuel(float _fuel) {
		if (_fuel < 0f)
			_fuel = 0f;
		else if (_fuel > MAXFUEL)
			_fuel = MAXFUEL;
		fuel = _fuel;
		fuelBar.setPercentage (fuel / MAXFUEL);
	}
	
	public void restartGame(bool realRestart) {
		accelerating = false;
		isGameOver = !realRestart;
		level = 0;
		setScore (0f);
		setFuel (INITIALFUEL);
		rigidBody.gravityScale = 0f;
		rigidBody.velocity = Vector2.zero;
		gameObject.layer = LayerMask.NameToLayer ("Body");
		Camera.main.transform.position = new Vector3 (0f, 0f, Camera.main.transform.position.z);
		transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, BODYLAYER);
		globalGridY = 0;
		fire.Stop ();
		lastSmoke.Stop ();
		thumb.button.SetActive (!realRestart);
		if (DEBUGSTEER)
			return;
		if (bg1)
			Destroy (bg1);
		if (bg2)
			Destroy (bg2);
		bg1 = generateBackgroundWithObstacles (realRestart);
		bg1.transform.position = new Vector3(leftMargin, -Camera.main.orthographicSize/2f, BACKGROUNDLAYER);
		bg2 = generateBackgroundWithObstacles (realRestart);
		bg2.transform.position = new Vector3(leftMargin, bg1.transform.position.y+bgSize, BACKGROUNDLAYER);
		if (realRestart) {
			spider.ShowSpider ();
			bird.Fly ();
		}
	}

	void gameOver() {
		rigidBody.velocity = Vector2.zero;
		rigidBody.gravityScale = 0f;
		accelerating = false;
		isGameOver = true;
		fire.Stop ();
		thumb.button.SetActive (true);
		thumb.button.GetComponentInChildren<Text>().text = "Again?";
		/*[self runAction:[SKAction waitForDuration:1.0] completion:^(void) {
			[_viewController performSelector:@selector(gameOver)];
		 }];*/

	}

	// Use this for initialization
	void Start () {
		isGameOver = true;
		scale = Screen.width / 320f;
		thumbMargin = THUMBMARGIN * scale;
		bgSize = SPOTGRIDSIZE * Camera.main.orthographicSize*2f * scale;
		midScreen = new Vector2 (Screen.width / 2, Screen.height / 2);
		Camera.main.transform.position = new Vector3 (0f, 0f, Camera.main.transform.position.z);
		leftMargin = Camera.main.ScreenToWorldPoint (Vector2.zero).x;
		rightMargin = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width, 0)).x;
		RectTransform r = fuelBar.GetComponent<RectTransform> ();
		r.sizeDelta = new Vector2(Screen.width, r.sizeDelta.y);
		Debug.Log ("start "+scale + " " + bgSize+" "+Camera.main.aspect);
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		GetComponent<Renderer>().castShadows = true;
		restartGame (false);
	}
	
	// Update is called once per frame
	void touchesBegan(Vector3 position) {

		if (!DEBUGSTEER)
			rigidBody.gravityScale = 1.0f;

		//[_thumb removeAllActions];
		accelerating = true;
		if (fuel <= 0f)
			return;
		//burst & smoke
		lastSmoke.Clear ();
		lastSmoke.Play ();
		fire.Clear ();
		fire.Play ();
	}
	void touchesMoved(Vector2 position) {
		if (accelerating) {
			float dx = Input.GetAxis ("Horizontal");
			float x = thumb.r.transform.position.x + dx;
			if (x < thumbMargin + 1f || x > Screen.width - thumbMargin - 1f)
				return;
			thumb.r.Translate(new Vector3(dx, 0, 0));
			//Debug.Log (thumb.r.transform.position);
		}
	}
	void touchesEnded() {
		accelerating = false;
		fire.Stop ();
		//[_thumb runAction:putInCenter];
	}
	void Update () {
		Vector3 pos;

		if (isGameOver)
			return;
		if (!isTouchDevice) {
			//If mouse button 0 is down
			if (Input.GetMouseButton (0)) { //finger pressed
				if (!accelerating)
					touchesBegan(Input.mousePosition);
				else
					touchesMoved(Input.mousePosition);
			} else if (!Input.GetMouseButton (0) && accelerating) { //finger released
				touchesEnded();
			}
		} else {
			//If a touch is detected
			if (Input.touchCount >= 1) {
				Touch touch = Input.touches [0];
				if (!accelerating) //finger pressed
					touchesBegan(touch.position);
				else
					touchesMoved(touch.position);
			} else if (Input.touchCount == 0 && accelerating) { //finger released
				touchesEnded();
			}
		}

		if (!DEBUGSTEER) {
			if (rigidBody.gravityScale > 0f && fuel > 0f && accelerating) {
				float df = 1f;
				if (rigidBody.velocity.y > 0.2f * maxVelocity [level])
					df = 1f - (rigidBody.velocity.y - 0.1f * maxVelocity [level]) / maxVelocity [level] * 0.5f;
				df = df * fuelCPL [level];
				setFuel (fuel - Time.deltaTime * df);
			}
		}
		float a;
		if (accelerating && fuel > 0f) {
			if (!DEBUGSTEER) {
				a = rigidBody.mass * ACCELERATION_UP*scale;
				if (rigidBody.velocity.y < maxVelocity[level])
					rigidBody.AddForce(new Vector2(0f, a));
			}
		}
		else {
			if (rigidBody.velocity.y > 0f) //reduce upward speed faster
				rigidBody.AddForce(new Vector2(0f, -rigidBody.mass * ACCELERATION_DOWN*scale * rigidBody.velocity.y / maxVelocity[level]));
			else if (fuel <= 0f)
				gameObject.layer = 0;
			
			/*float dx = midScreen.x - thumb.transform.position.x;
			if (Mathf.Abs(dx) > 0f)
				[self positionThumbAndBars];*/
		}

		a = MAXEARANGLE*rigidBody.velocity.y/maxVelocity[level];
		if (a < 0f)
			a = 0f;
		rightEar.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -a));
		leftEar.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, a));

		if (fuel > 0f) {
			float d = STEER_FORCE;
			float dx = (thumb.r.transform.position.x - midScreen.x), adx = Mathf.Abs(dx);
			if (adx < CONTROLLER_DEAD_ZONE)
				dx = 0f;
			if (dx < 0f)
				dx = -Mathf.Pow(-dx, 0.8f);
			else
				dx = Mathf.Pow(dx, 0.8f);
			d = d * dx;
			gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, d/(thumbMargin-midScreen.x)/STEER_FORCE * MAXBODYROLL));
			rigidBody.velocity = new Vector2(d, rigidBody.velocity.y);
		}
		else if (accelerating) {
			setFuel(0f);
			fire.Stop();
		}
		
		/*_fire.particlePosition = [self convertPoint:CGPointMake(0, -47) fromNode:_body];
		_lastSmoke.particlePosition = _fire.particlePosition;*/

		if (gameObject.transform.position.y > Camera.main.transform.position.y)
			Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);

		if (gameObject.transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize - GetComponent<SpriteRenderer>().bounds.size.y*2f  
		    && (fuel <= 0f || !accelerating)) {
			Debug.Log ("Gameover");
			gameOver ();
		}

		pos = gameObject.transform.position;
		if (pos.x < leftMargin) {
			pos.x += rightMargin*2.2f;
		}
		else if (pos.x > rightMargin) {
			pos.x -= rightMargin*2.2f;
		}
		gameObject.transform.position = pos;

		if (DEBUGSTEER)
			return;

		float minBgY = Camera.main.transform.position.y - Camera.main.orthographicSize / 2f - bgSize;
		if (bg1.transform.position.y < minBgY) {
			pos = bg1.transform.position;
			pos.y = bg2.transform.position.y + bgSize;
			Destroy(bg1);
			bg1 = generateBackgroundWithObstacles(true);
			bg1.transform.position = pos;
		}
		if (bg2.transform.position.y < minBgY) {
			pos = bg2.transform.position;
			pos.y = bg1.transform.position.y + bgSize;
			Destroy(bg2);
			bg2 = generateBackgroundWithObstacles(true);
			bg2.transform.position = pos;
		}
		setScore (transform.position.y);
		//bird test
		/*
		if (false && Input.GetButtonDown ("Fire1")) {
				BirdAnimator abird = Instantiate<BirdAnimator>(bird);
				abird.reverse = Random.value > 0.5f;
				abird.speed = 10f;
				pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.z = 0f;
				abird.transform.position = pos;
				Debug.Log(abird.transform.position);
			}
		}*/
	}

	void OnBecameInvisible() {
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.gameObject.layer == LayerMask.NameToLayer ("Powerup")) {
			AddFuel ();
			Destroy (other.transform.gameObject);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.transform.gameObject.layer == LayerMask.NameToLayer ("Damper")) {
			float l1 = (new Vector2(transform.position.x-other.transform.position.x, transform.position.y - other.transform.position.y)).magnitude,
			l2 = other.bounds.size.magnitude/2f,
			x = other.gameObject.GetComponent<Damper>().maxResistance * (1f - l1/l2);
			rigidBody.drag = x;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.transform.gameObject.layer == LayerMask.NameToLayer ("Damper")) {
			rigidBody.drag = 0f;
		}
	}
}

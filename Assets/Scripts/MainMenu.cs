using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	GameObject g;
	GameLogic gl;
	GameObject bg1;
	GameObject bg2;
	public MeshRenderer background;

	// Use this for initialization
	void Start () {
		g = new GameObject ();
		gl = g.AddComponent<GameLogic>();
		g.SetActive(false);
		gl.leftMargin = Camera.main.ScreenToWorldPoint (Vector2.zero).x;
		gl.rightMargin = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width, 0)).x;
		gl.scale = Screen.width / 320f;
		gl.bgSize = gl.SPOTGRIDSIZE.y * Camera.main.orthographicSize*2f;
		bg1 = gl.generateBackgroundWithObstacles (false);
		bg1.transform.position = new Vector3(gl.leftMargin, -Camera.main.orthographicSize, gl.BACKGROUNDLAYER);
		bg2 = gl.generateBackgroundWithObstacles (false);
		bg2.transform.position = new Vector3(gl.leftMargin, bg1.transform.position.y+gl.bgSize, gl.BACKGROUNDLAYER);
		GameObject.Find ("Background").transform.localScale = new Vector3(Camera.main.orthographicSize/4f * Camera.main.aspect, 1f, Camera.main.orthographicSize/4f);
	}
	
	// Update is called once per frame
	void Update () {
		bg1.transform.position += Vector3.down * Time.deltaTime;
		bg2.transform.position += Vector3.down * Time.deltaTime;
		background.material.SetTextureOffset("_MainTex", new Vector2(0f, -Time.timeSinceLevelLoad/Camera.main.orthographicSize/2.5f));
		float minBgY = Camera.main.transform.position.y - Camera.main.orthographicSize / 2f - gl.bgSize;
		Vector3 pos;
		if (bg1.transform.position.y < minBgY) {
			pos = bg1.transform.position;
			pos.y = bg2.transform.position.y + gl.bgSize;
			Destroy(bg1);
			bg1 = gl.generateBackgroundWithObstacles(false);
			bg1.transform.position = pos;
		}
		if (bg2.transform.position.y < minBgY) {
			pos = bg2.transform.position;
			pos.y = bg1.transform.position.y + gl.bgSize;
			Destroy(bg2);
			bg2 = gl.generateBackgroundWithObstacles(false);
			bg2.transform.position = pos;
		}

	}

	public void StartGame() {
		Application.LoadLevel ("GamePlay");
	}

	public void ExitGame() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif

	}
}

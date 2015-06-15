using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSScript : MonoBehaviour {
	float deltaTime = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}
	
	void OnGUI()
	{
		float fps = 1.0f / deltaTime;
		GetComponent<Text>().text = string.Format("{0:0.} fps", fps);
	}
}

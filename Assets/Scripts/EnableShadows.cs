using UnityEngine;
using System.Collections;

public class EnableShadows : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().castShadows = true;
	}
}

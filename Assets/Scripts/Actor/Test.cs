using UnityEngine;
using System.Collections;

public class Splash : Actor {
	
	// Initialization
	void Start () {
		
		// Add single actions (run in sequence)
		AddAction(new FadeTo(0.5f, 1f));
		AddAction(new ScaleTo(new Vector3(1.1f, 1.1f, 1f), 0.25f));
		AddAction(new ScaleTo(new Vector3(0.9f, 0.9f, 1f), 0.25f));
		AddAction(new ScaleTo(new Vector3(1f, 1f, 1f), 0.25f));
		
		// Add sequence of actions (run in sequence)
		AddAction(new Sequence(
					new ScaleTo(new Vector3(2f, 2f, 1f), 1f),
					new RotateTo(new Vector3(0f, 0f, 90f), 1f),
					new MoveTo(new Vector3(1f, 1f, 0f), 2f),
					new SleepFor(2f),
					new ScaleTo(new Vector3(1f, 1f, 1f), 0.25f),
					new RotateTo(new Vector3(0f, 0f, 00f), 0.25f),
					new MoveTo(new Vector3(0f, 0f, 0f), 0.25f)
				));
		
		// Add parallel	actions (run at same time)
		AddAction(new Parallel(
					new FadeTo(1.0f, 2f),
					new ScaleTo(new Vector3(2f, 2f, 1f), 1f),
					new RotateTo(new Vector3(0f, 0f, 90f), 1f),
					new MoveTo(new Vector3(1f, 1f, 0f), 2f)
				));

	}
	
	// Update
	void Update () {
		
		// If you wanna override Update method, you need
		// to call this method bellow
		
		// Update Actions
		UpdateActions();
		
	}
	
}

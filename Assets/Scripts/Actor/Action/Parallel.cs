using UnityEngine;
using System.Collections.Generic;

public class Parallel : Action {
	
	// Action list
	private List<Action> actions = new List<Action>();
	
	// Constructor
	public Parallel(params Action[] action_list)
	{
		// add actions to list
		for (int i = 0; i < action_list.Length; i++) actions.Add(action_list[i]);
	}
	
	// Init
	public override void Init () {

		initialized = true;
	}

	// Update
	public override void Update () {
		
		// Not completed
		if(!completed)
		{
			// removal list
			List<Action> to_remove = new List<Action>();
			
			// Run all actions in parallel
			foreach(Action action in actions)
			{
				
				// Initialize action
				if(!action.IsInitialized()) {
					// Assing parent
					action.parent = parent;
					// Initialize action
					action.Init();
				}

				// Update action
				action.Update();
				
				// Remove action when completed
				if(action.IsCompleted()) to_remove.Add(action);				
				
			}
			
			// Remove finished actions
			foreach(Action action in to_remove) actions.Remove(action);
			
			// No more actions
			if(actions.Count == 0) {
				
				Debug.Log("Parallel completed");
				EndAction();
			}
		
		}
		
	}

}

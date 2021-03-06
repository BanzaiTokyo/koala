using UnityEngine;
using System.Collections.Generic;

public class Sequence : Action {
	
	// Action list
	private List<Action> actions = new List<Action>();
	
	// Constructor
	public Sequence(params Action[] action_list)
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
			
			// Run actions
			if(actions.Count>0)
			{
				// Get current action instance
				Action action=actions[0];
				
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
				if(action.IsCompleted()) actions.Remove(action);

			} else {
				
				Debug.Log("Sequence completed");
				
				// No more actions
				EndAction();
			}
		}
		
	}

}

using UnityEngine;
using System.Collections.Generic;

public class Actor : MonoBehaviour
{
	// Action list
	private List<Action> actions = new List<Action>();
	
	// Update is called once per frame
	void Update () {
		
		// Run actions
		UpdateActions();
	}
	
	// Update actions
	protected void UpdateActions() {
		
		// Run actions
		if(actions.Count>0)
		{
			// Get current action instance
			Action action=actions[0];
			
			// Initialize action
			if(!action.IsInitialized()) action.Init();
			
			// Update action
			action.Update();
			
			// Remove action when completed
			if(action.IsCompleted()) actions.Remove(action);
		}
		
	}
	
	// Get amount of actions
	public int GetActionAmount() {
		// return actions count
		return actions.Count;
	}
	
	// Add Action
	public void AddAction(Action action)
	{
		// Add action to list
		actions.Add(action);
		// Assign parent to action
		action.parent = this;
	}
	
	// Clear all Actions
	public void ClearAllActions()
	{
		actions.Clear();
	}
}

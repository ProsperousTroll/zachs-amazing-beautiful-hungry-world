using Godot;
using System;

public partial class Globals : Node
{
	
	public enum PLRSTATES 
	{
		IDLE,
		WALKING,
		INAIR,
		CRUMP,
		PLIP
	};
	
	[Export]
	public PLRSTATES STATE;

	public bool Plip = false;
	public bool Crump = false;

	public float inDir()
	{
		float dir = 0;
		if (Input.IsActionPressed("Left") && Input.IsActionPressed("Right"))
		{
			dir = 0;
		}
		else if (Input.IsActionPressed("Right"))
		{
			dir += 1;
		}
		else if (Input.IsActionPressed("Left"))
		{
			dir -= 1;
		}
		return dir;
	}
	
	// Helper functions because I'm a lazy ######
	
	public void changeState(ref PLRSTATES currentState, string requestedState)
	{
		Enum.TryParse<PLRSTATES>(requestedState, out var value);
		currentState = value; 
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// RESTART GAME //
		if(Input.IsActionJustPressed("ui_cancel"))
		{
			GetTree().CallDeferred("reload_current_scene");
		}
	}
	
}

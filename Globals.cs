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


	public float inDir()
	{
		float dir = 0;
		if (Input.IsActionPressed("Left") && Input.IsActionPressed("Right"))
		{
			dir = 0;
		}
		else if (Input.IsActionPressed("Right"))
		{
			dir = 1;
		}
		else if (Input.IsActionPressed("Left"))
		{
			dir = -1;
		}
		return dir;
	}
	
}

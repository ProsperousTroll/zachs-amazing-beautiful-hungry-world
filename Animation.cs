using Godot;
using System;

public partial class Animation : AnimatedSprite2D
{
	private CharacterBody2D Char;
	private Globals Globals;
	
	private bool Direction = false;
	private bool lastDirection = false;
	
	private bool lookDir()
	{
		if (Input.IsActionPressed("Left") && Input.IsActionPressed("Right"))
		{
			lastDirection = Direction;
		}
		else if (Input.IsActionPressed("Right"))
		{
			Direction = false;
			lastDirection = true;
		}
		else if (Input.IsActionPressed("Left"))
		{
			Direction = true;
			lastDirection = false;
		}
		return Direction;
	}
	
	public override void _Ready()
	{
		Globals = GetNode<Globals>("/root/World/Globals");
		Char = GetNode<CharacterBody2D>("/root/World/Player/CharacterBody2D");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		
		FlipH = lookDir();

		// Lean in movement direction. Holy magic numbers.  
		RotationDegrees = Mathf.Lerp(Globals.inDir(), Char.Velocity.X / 65F, 0.5F);
		
		switch(Globals.STATE)
		{
			case Globals.PLRSTATES.IDLE:
				Play("IDLE");
				break;
			case Globals.PLRSTATES.WALKING:
				Play("WALKING");
				break;
			case Globals.PLRSTATES.INAIR:
				if (Char.Velocity.Y < 0)
				{
					Play("AIR_UP");
				} else Play ("AIR_DOWN");
				break;
			case Globals.PLRSTATES.CRUMP:
				Play("CRUMP");
				break;
			case Globals.PLRSTATES.PLIP:
				Play("PLIP");
				break;
		}
	}
}

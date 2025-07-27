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
		
		FlipH = (bool)lookDir();
		
		switch((int)Globals.STATE)
		{
			case 0:
				Play("IDLE");
				break;
			case 1:
				Play("WALKING");
				break;
			case 2:
				if (Char.Velocity.Y < 0)
				{
					Play("AIR_UP");
				} else Play ("AIR_DOWN");
				break;
			case 3:
				Play("CRUMP");
				break;
			case 4:
				Play("PLIP");
				break;
		}
	}
}

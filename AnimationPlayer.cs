using Godot;
using System;

public partial class AnimationPlayer : Godot.AnimationPlayer
{
	private CharacterBody2D Player;
	private Globals Globals;
	
	private bool hasPlayed = false;

	public override void _Ready()
	{
		Player = GetNode<CharacterBody2D>("/root/World/Player/CharacterBody2D");
		Globals = GetNode<Globals>("/root/World/Globals");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("Jump") && Globals.STATE != Globals.PLRSTATES.INAIR)
		{
			Play("Jump");
		}
	
		if(Globals.Plip == true && Player.Velocity.Y >= 1 && hasPlayed == false)
		{
			Play("Flip");
			hasPlayed = true;
		}
		else if (Globals.STATE != Globals.PLRSTATES.INAIR)
		{
			hasPlayed = false;
		}
		}
	}

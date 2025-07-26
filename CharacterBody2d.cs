using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
	
	[Export]
	public float gravity = 1800;
	[Export]
	public float speed = 750;
	[Export]
	public float accel = 0.25F;
	[Export]
	public float friction = 0.25F;
	[Export]
	public float jumpPower = -800;
	[Export]
	public float drag = 0.005F;
	

	
	private Vector2 vel = Vector2.Zero;
	private Globals Globals;
	

	public override void _Ready()
	{
		Globals = GetNode<Globals>("/root/World/Globals");
	}

	public override void _PhysicsProcess(double delta)
	{
		
		// Ground movement
		if (Globals.inDir() != 0)
		{
			Globals.STATE = Globals.PLRSTATES.WALKING;
			vel.X = Mathf.Lerp(vel.X, (Globals.inDir() * speed), accel);
		}
		else if (IsOnFloor())
		{
			Globals.STATE = Globals.PLRSTATES.IDLE;
			vel.X = Mathf.Lerp(vel.X, 0.0F, friction);
		}

		// Plip
		if (Input.IsActionPressed("Down"))
			Globals.STATE = Globals.PLRSTATES.CRUMP;


		// Jumping
		if (!IsOnFloor())
		{
		Globals.STATE = Globals.PLRSTATES.INAIR;
		vel.Y += gravity * (float)delta;
		vel.X = Mathf.Lerp(vel.X, Globals.inDir() * speed, drag);
		}
		
		if (IsOnFloor() && Input.IsActionJustPressed("Jump"))
		{
			vel.Y = jumpPower;
		}
		
		Velocity = vel;
		MoveAndSlide();
	}
	
}

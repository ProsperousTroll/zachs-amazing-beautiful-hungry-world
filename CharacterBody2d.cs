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
	[Export]
	
	// Multipliers for special 'crump' move
	public float crumpSpeed = 1.7F;
	[Export]
	public float crumpHeight = 0.7F;
	

	
	private Vector2 vel = Vector2.Zero;
	private Globals Globals;
	
	private void playerStates()
	{
		if (!IsOnFloor())
		{
			Globals.STATE = Globals.PLRSTATES.INAIR;
		}
		else if (IsOnFloor() && Globals.inDir() == 0)
		{
			Globals.STATE = Globals.PLRSTATES.IDLE;
		}
		else if (Globals.inDir() != 0)
		{
			Globals.STATE = Globals.PLRSTATES.WALKING;
		}

		if (Input.IsActionPressed("Down"))
		{
			Globals.STATE = Globals.PLRSTATES.CRUMP;
		}
		else if (Input.IsActionPressed("Up"))
		{
			Globals.STATE = Globals.PLRSTATES.PLIP;
		}
	}
	
	
	public override void _Ready()
	{
		Globals = GetNode<Globals>("/root/World/Globals");
	}



	public override void _PhysicsProcess(double delta)
	{
		
		playerStates();
		GD.Print(Velocity.X + Velocity.Y);
		
		// Gravity / jumping
		if (Input.IsActionJustPressed("Jump"))
		{
			if (Globals.STATE == Globals.PLRSTATES.IDLE || Globals.STATE == Globals.PLRSTATES.WALKING)
			{
				vel.Y = jumpPower;
			}
			else if (Globals.STATE == Globals.PLRSTATES.CRUMP && IsOnFloor())
			{
				vel.Y = jumpPower * crumpHeight;
			}
		}

		
		// Movement depending on player state
		switch((int)Globals.STATE)
		{
			// IDLE
			case 0:
				vel.X = Mathf.Lerp(vel.X, 0.0F, friction);
				break;
			// WALKING
			case 1:
				vel.X = Mathf.Lerp(vel.X, (Globals.inDir() * speed), accel);
				break;
			// INAIR
			case 2:
				vel.Y += gravity * (float)delta;
				vel.X = Mathf.Lerp(vel.X, Globals.inDir() * speed, drag);
				break; 
			// CRUMP
			case 3:
				if (Velocity.Y < 0)
				{
					vel.Y += gravity * (float)delta;
				}
				else
				{
					vel.Y += (gravity * 2) * (float)delta;
				}
				if (IsOnFloor())
				{
					vel.X = Mathf.Lerp(vel.X, 0.0F, friction);
				} else vel.X = Mathf.Lerp(vel.X, Globals.inDir() * (speed * crumpSpeed), accel);
				break;
			// PLIP
			case 4:
				break;
		}
		
		/*
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
		*/
		
		Velocity = vel;
		MoveAndSlide();
	}
	
}

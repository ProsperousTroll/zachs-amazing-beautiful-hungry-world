using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
	/////////////////
	/// VARIABLES /// 
	/////////////////

	// Public properties 
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
	public float slide = 0.025F;
	[Export]
	public float crumpSpeed = 1.8F;
	[Export]
	public float crumpHeight = 0.5F;
	[Export]
	public float plipSpeed = 0.5F;
	[Export]
	public float plipHeight = 1.5F;
	
	// Private variables 
	private Vector2 vel = Vector2.Zero;
	private Vector2 multipliers = Vector2.Zero;
	private Globals Globals;
	
	/////////////////
	/// FUNCTIONS ///
	/////////////////
	
	private void Multipliers()
	{
		if(Globals.Crump == true)	
		{
			multipliers.X = crumpSpeed;
			multipliers.Y = crumpHeight; 
			drag = 0.5F;
		}
		else if(Globals.Plip == true)
		{
			multipliers.X = plipSpeed;
			multipliers.Y = plipHeight;
			drag = 0.05F;
		}
		else
		{
			multipliers.X = 1.0F;
			multipliers.Y = 1.0F;
			drag = 0.02F;
		} 
	}

	private void playerStates()
	{
		
		if (!IsOnFloor())
		{
			Globals.changeState(ref Globals.STATE, "INAIR");
		}
		else if (IsOnFloor() && Globals.inDir() == 0)
		{
			Globals.changeState(ref Globals.STATE, "IDLE");
			Globals.Crump = false;
			Globals.Plip = false;
		}
		else
		{
			Globals.changeState(ref Globals.STATE, "WALKING");
			Globals.Crump = false;
			Globals.Plip = false;
		}

		if (Input.IsActionPressed("Down") && IsOnFloor())
		{
			Globals.changeState(ref Globals.STATE, "CRUMP");
			Globals.Crump = true;
		}
		else if (Input.IsActionPressed("Up") && IsOnFloor())
		{
			Globals.changeState(ref Globals.STATE, "PLIP");
			Globals.Plip = true;
		}

	}
	
	/////////////
	/// READY ///
	/////////////
	
	public override void _Ready()
	{
		Globals = GetNode<Globals>("/root/World/Globals");
	}
	
	///////////////////////
	/// PHYSICS PROCESS ///
	///////////////////////
	
	public override void _PhysicsProcess(double delta)
	{
		// This order matters
		playerStates();
		Multipliers();

		// Fix fast fall bug. Wouldn't want this here normally. 
		if (IsOnFloor())
		{
			vel.Y = 0;
		}

		// Jump
		if (Input.IsActionJustPressed("Jump"))
		{
			if (IsOnFloor())
			{
				vel.Y = jumpPower * multipliers.Y;
			}
			
		}

		
		switch(Globals.STATE)
		{
			case Globals.PLRSTATES.IDLE:
				vel.X = Mathf.Lerp(vel.X, 0.0F, friction);
				break;
			case Globals.PLRSTATES.WALKING:
				vel.X = Mathf.Lerp(vel.X, (Globals.inDir() * speed), accel);
				break;
			case Globals.PLRSTATES.INAIR:
				// I don't *WANT* this here
				if (IsOnCeiling())
				{
					vel.Y = 0;
				}
				// I thought the engine would do this for me ^
				vel.Y += gravity * (float)delta;
				vel.X = Mathf.Lerp(vel.X, Globals.inDir() * (speed * multipliers.X), drag);
				vel.Y = Math.Clamp(vel.Y, -gravity, gravity);
				break; 
			case Globals.PLRSTATES.CRUMP:
				vel.X = Mathf.Lerp(vel.X, 0.0F, slide);
				break;
			case Globals.PLRSTATES.PLIP:
				vel.X = Mathf.Lerp(vel.X, 0.0F, slide);
				break;
 		}

		// Fix arial momentum bug, prevents from gettings stuck on walls in the air.
		if(Velocity.X == 0 && Globals.inDir() == 0)
		{
			vel.X = 0;
		}	

		Velocity = vel;
		MoveAndSlide();
	}
	
}

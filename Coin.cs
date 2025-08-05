using Godot;
using System;

public partial class Coin : Area2D
{
	/////////////////
	/// VARIABLES ///
	/////////////////
	
	private Globals Globals;
	private CharacterBody2D Char;
	private AnimatedSprite2D Anim;


	/////////////
	/// READY ///
	/////////////
	
	public override void _Ready()
	{
		Globals = GetNode<Globals>("/root/World/Globals");
		Char = GetNode<CharacterBody2D>("/root/World/Player/CharacterBody2D");
		Anim = GetNode<AnimatedSprite2D>("/root/World/Coin/Area2D/AnimatedSprite2D");
	}
	
	///////////////////////
	/// PHYSICS PROCESS ///
	///////////////////////
	
	public override void _PhysicsProcess(double delta)
	{
		Anim.Play("SPIN");
	}

}

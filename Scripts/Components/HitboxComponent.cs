using Godot;
using System;

public partial class HitboxComponent : Area2D
{
	[Signal]
    public delegate void DamageTakenEventHandler(int DamageTaken);

	// Adjust for different hitboxes to allow for extra headshot damage etc.
	[Export] private float _damageMultiplier = 1;

	public void Hit(int Damage)
	{
		EmitSignal(SignalName.DamageTaken, Damage * _damageMultiplier);
	}
}

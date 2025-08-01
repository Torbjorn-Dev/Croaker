using Godot;
using System;

public partial class Killzone : Node2D
{
	public void OnHitboxEntered2D(HitboxComponent Hitbox)
	{
		GameManager.Instance.LoseLevel();
	}
}

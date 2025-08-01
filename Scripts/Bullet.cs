using Godot;
using System;

public partial class Bullet : Node2D
{
	[Export] private int _damage = 1;
	[Export] private float _speed = 200;

	public override void _PhysicsProcess(double delta)
	{
		Position += Transform.X * _speed * (float)delta;
	}

	// If bullet hits a node with a hitbox attached.
	public void onAreaEntered2D(Area2D Area)
	{
		if (Area is HitboxComponent)
		{
			HitboxComponent Hitbox = Area as HitboxComponent;
			Hitbox.Hit(_damage);
		}
		GetParent<Player>().GetBullet();
		QueueFree();
	}

	// If bullet hits terrain or other solid objects.
	public void OnCollisionEntered2D(Node2D Object)
	{
		QueueFree();
	}
}

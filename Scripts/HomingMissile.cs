using Godot;
using System;

public partial class HomingMissile : Node2D
{
	[Export] private int _damage = 1;
	[Export] private float _speed = 200;
	private Node2D _target;
	[ExportCategory("PackedScenes")]
	[Export] private PackedScene _explosionScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		FindTarget();
	}

	public override void _PhysicsProcess(double delta)
	{
		// This entire if statement might have issues.
		if (_target == null)
		{
			Position += _speed * Vector2.Right.Rotated(10) * (float)delta;
			return;
		}

		LookAt(_target.GlobalPosition);
		Position = Position.MoveToward(_target.GlobalPosition, _speed * (float)delta);
	}

	private void FindTarget()
	{
		foreach (Node node in GetTree().Root.GetChildren())
		{
			if (node.Name.ToString().StartsWith("Level"))
			{
				_target = (Node2D)node.GetNode("Player");
				break;
			}
		}
	}

	// If missile hits a node with a hitbox attached.
	public void OnAreaEntered2D(Area2D Area)
	{
		if (Area is HitboxComponent)
		{
			HitboxComponent Hitbox = Area as HitboxComponent;
			Hitbox.Hit(_damage);
		}

		Explode();
	}

	// If missile hits terrain or other solid objects.
	public void OnCollisionEntered2D(Node2D Object)
	{
		Explode();
	}

	private void Explode()
	{
		Node2D Explosion = (Node2D)ResourceLoader.Load<PackedScene>(_explosionScene.ResourcePath).Instantiate();
		GetTree().Root.AddChild(Explosion);
		Explosion.Transform = GlobalTransform;
		Explosion.GetChild<CpuParticles2D>(0).Emitting = true;
		SetPhysicsProcess(false);
		QueueFree();
	}

	public void OnTimerTimeout()
	{
		Explode();
	}

}

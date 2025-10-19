using Godot;
using System;

public partial class GunToad : Creature
{
    [Export] private PackedScene _bulletScene;
    [Export] private PackedScene _deathParticleScene;
    [Export] private Node2D _fireLocation;
    private Node2D _target;
    private Timer _timer;


    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = GetNode<PatrolAIComponent>("PatrolAI").PatrolVelocity;
        MoveAndSlide();
    }

    public override void BecomeAlert()
    {
        IsAlert = true;
        _timer.Start();
    }

    public void OnTimerTimeout()
    {
        FireBullet();
        _timer.Start();
    }

    public void FireBullet()
    {
        foreach (Node node in GetTree().Root.GetChildren())
        {
            if (node.Name.ToString().StartsWith("Level"))
            {
                _target = (Node2D)node.GetNode("Player");
                break;
            }
        }
        Node2D bullet = (Node2D)ResourceLoader.Load<PackedScene>(_bulletScene.ResourcePath).Instantiate();
        AddChild(bullet);
        bullet.Position = Position.Lerp(_target.Position, 0.125f);
        bullet.LookAt(_target.Position);
    }

    public void Die()
    {
        GD.Print("Toad died!");
        Node2D DeathParticles = (Node2D)ResourceLoader.Load<PackedScene>(_deathParticleScene.ResourcePath).Instantiate();
        GetTree().Root.AddChild(DeathParticles);
        DeathParticles.Transform = GlobalTransform;
        DeathParticles.GetChild<CpuParticles2D>(0).Emitting = true;
        GameManager.Instance.CheckIfWon();
        QueueFree();
    }
}

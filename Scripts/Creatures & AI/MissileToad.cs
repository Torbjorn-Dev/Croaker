using Godot;
using System;

public partial class MissileToad : Creature
{
    [Export] private PackedScene _missileScene;
    [Export] private PackedScene _deathParticleScene;
    [Export] private Node2D _fireLocation;
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
        FireMissile();
        _timer.Start();
    }

    public void FireMissile()
    {
        Node2D Missile = (Node2D)ResourceLoader.Load<PackedScene>(_missileScene.ResourcePath).Instantiate();
        AddChild(Missile);
        Missile.Transform = _fireLocation.GlobalTransform;
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
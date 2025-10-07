using Godot;
using System;

public partial class MissileToad : Creature
{
    [Export] private PackedScene _missileScene;
    [Export] private PackedScene _deathParticleScene;
    [Export] private Node2D _fireLocation;
    private Timer _timer;
    private Area2D _playerArea2D;

    private bool _playerInVisionCone;
    private bool _isAlert = false;


    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_playerInVisionCone && !_isAlert)
        {
            CheckPlayerVisible();
        }

        Velocity = GetNode<PatrolAIComponent>("PatrolAI").PatrolVelocity;
        MoveAndSlide();
    }

    private void OnVisionEntered(Area2D PlayerArea)
    {
        _playerArea2D = PlayerArea;
        _playerInVisionCone = true;
    
    }

    private void OnVisionExited(Area2D PlayerArea)
    {
        _playerInVisionCone = false;
    }

    private void CheckPlayerVisible()
    {
        PhysicsDirectSpaceState2D DirectState = GetWorld2D().DirectSpaceState;
        var RayQuery = PhysicsRayQueryParameters2D.Create(this.GlobalPosition, _playerArea2D.GlobalPosition);
        var RayCollidedObject = DirectState.IntersectRay(RayQuery);
        Node IntersectedNode = (Node)RayCollidedObject["collider"];

        if (IntersectedNode.Name.Equals("Player"))
        {
            Player player = (Player)IntersectedNode;
            if (!player.IsStealthed)
            {
                _isAlert = true;
                GD.Print(this + " was alerted!");
                _timer.Start();
            }
        }
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
using Godot;
using System;

public partial class Turret : Creature
{
    [Export] private PackedScene _bulletScene;
    [Export] private PackedScene _deathParticleScene;
    [Export] private Node2D _fireLocation;

    [ExportCategory("Vision")]
    [Export] private Node2D _visionCone;
    [Export] private float _minRotation;
    [Export] private float _maxRotation;
    [Export] private float _rotationSpeed;
    private bool _isIdling = false;
    
    private Node2D _target;
    private Timer _fireRateTimer;
    private Timer _idleTimer;


    public override void _Ready()
    {
        _fireRateTimer = GetNode<Timer>("FireRateTimer");
        _idleTimer = GetNode<Timer>("IdleTimer");
    }

    public override void _PhysicsProcess(double delta)
    {
        RotateTurretIdle();
    }

    private void RotateTurretIdle()
    {
        if (_isIdling == false)
        {
            _visionCone.Rotate(_rotationSpeed * (float)GetPhysicsProcessDeltaTime());
        }

        if (_rotationSpeed > 0)
        {
            //GD.Print("Rotation counterclockwise!");
            if (_visionCone.Rotation >= _maxRotation)
            {
                _rotationSpeed = -_rotationSpeed;
                GD.Print("Changed direction. Rotation speed: " + _rotationSpeed);
                _isIdling = true;
                _idleTimer.Start();
            }
        }
        else
        {
            //GD.Print("Rotation clockwise!");
            if (_visionCone.Rotation <= _minRotation)
            {
                _rotationSpeed = -_rotationSpeed;
                GD.Print("Changed direction. Rotation speed: " + _rotationSpeed);
                _isIdling = true;
                _idleTimer.Start();
            }
        }
    }

    public override void BecomeAlert()
    {
        IsAlert = true;
        _fireRateTimer.Start();
    }

    public void OnIdleTimerTimeout()
    {
        _isIdling = false;
    }

    public void OnFireRateTimeout()
    {
        FireBullet();
        _fireRateTimer.Start();
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
        bullet.Transform = _fireLocation.GlobalTransform;
    }

    public void Die()
    {
        GD.Print("Turret destroyed!");
        Node2D DeathParticles = (Node2D)ResourceLoader.Load<PackedScene>(_deathParticleScene.ResourcePath).Instantiate();
        GetTree().Root.AddChild(DeathParticles);
        DeathParticles.Transform = GlobalTransform;
        DeathParticles.GetChild<CpuParticles2D>(0).Emitting = true;
        GameManager.Instance.CheckIfWon();
        QueueFree();
    }

    private void ClampVision()
    {
        _visionCone.Rotation = Math.Min(_visionCone.Rotation, _maxRotation);
        _visionCone.Rotation = Math.Max(_visionCone.Rotation, _minRotation);
    }
}

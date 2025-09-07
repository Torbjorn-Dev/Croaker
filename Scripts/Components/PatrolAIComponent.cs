using Godot;
using System;

public partial class PatrolAIComponent : Node2D
{
    [Export] public float PatrolSpeed;
    [Export] public float IdleTime;
    public bool IsPatrolling = true;
    public bool IsIdling = false;
    public Vector2 PatrolVelocity;
    private CharacterBody2D _patrollingParent;
    private Timer _idleTimer;


    [Export] private Marker2D[] _patrolPoints;
    private int _currentPatrolPointIndex;

    public override void _Ready()
    {
        _patrollingParent = GetParent<CharacterBody2D>();
        _idleTimer = GetNode<Timer>("IdleTimer");
        _idleTimer.WaitTime = IdleTime;

        if (_patrolPoints.Length <= 0)
        {
            GD.Print("No patrol points were set!");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsIdling)
        {
            Patrol();
        }
    }

    public void Patrol()
    {
        if (_patrolPoints.Length > 0)
        {
            Vector2 target = _patrolPoints[_currentPatrolPointIndex].Position;
            PatrolVelocity = (target - _patrollingParent.Position).Normalized() * PatrolSpeed;

            if (_patrollingParent.Position.DistanceTo(target) < 10)
            {
                _currentPatrolPointIndex++;
                if (_currentPatrolPointIndex >= _patrolPoints.Length)
                {
                    _currentPatrolPointIndex = 0;
                }
                _idleTimer.Start();
                PatrolVelocity = new Vector2(0, 0);
                IsIdling = true;
            }
        }
    }

    public void OnIdleTimerTimeout()
    {
        IsIdling = false;
    }

}

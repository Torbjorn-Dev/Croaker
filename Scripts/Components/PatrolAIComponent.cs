using Godot;
using System;
using System.Numerics;

public partial class PatrolAIComponent : Node2D
{
    [Export] public float PatrolSpeed;
    public bool IsPatrolling = true;
    public Godot.Vector2 PatrolVelocity;
    private CharacterBody2D _patrollingParent;


    [Export] private Marker2D[] _patrolPoints;
    private int _currentPatrolPointIndex;

    public override void _Ready()
    {
        _patrollingParent = GetParent<CharacterBody2D>();

        if (_patrolPoints.Length <= 0)
        {
            GD.Print("No patrol points were set!");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsPatrolling)
        {
            Patrol();
        }
    }

    public void Patrol()
    {
        if (_patrolPoints.Length > 0)
        {
            Godot.Vector2 target = _patrolPoints[_currentPatrolPointIndex].Position;
            PatrolVelocity = (target - _patrollingParent.Position).Normalized() * PatrolSpeed;

            if (_patrollingParent.Position.DistanceTo(target) < 10)
            {
                _currentPatrolPointIndex++;
                if (_currentPatrolPointIndex >= _patrolPoints.Length)
                {
                    _currentPatrolPointIndex = 0;
                }
            }
        }
    }

}

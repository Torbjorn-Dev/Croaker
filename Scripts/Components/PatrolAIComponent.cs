using Godot;
using System;

public partial class PatrolAIComponent : Node2D
{

    private Node2D PatrollingCreature; // The creature that should be patrolling.

    [Export] private float _patrolSpeed;
    [Export] private Node2D[] _patrolPoints;
    private int _nextPatrolPoint;

    public override void _Ready()
    {
        PatrollingCreature = (Node2D)GetParent();
        _nextPatrolPoint = 0;
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        PatrollingCreature.Position.Lerp(_patrolPoints[_nextPatrolPoint].Position, _patrolSpeed);

        if (PatrollingCreature.Position == _patrolPoints[_nextPatrolPoint].Position)
        {
            DecideNextPoint();
        }
    }

    private void DecideNextPoint()
    {
        _nextPatrolPoint++;
        GD.Print("Next patrol point: " + _nextPatrolPoint);
    }

}

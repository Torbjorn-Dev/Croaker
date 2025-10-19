using Godot;
using System;
using System.Numerics;

public partial class Multiplayer_GameCamera : Camera2D
{
    private Player _player;

    public override void _Ready()
    {
        _player = (Player)GetTree().GetNodesInGroup("Player")[0];
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("Aim"))
        {
            FollowMouse();
        }
        else
        {
            FollowPlayer();
        }
    }

    public void FollowMouse()
    {
        Position = Position.Lerp(_player.Position.Lerp(GetGlobalMousePosition(), 0.35f), 0.05f);
    }

    public void FollowPlayer()
    {
        float DistanceMultiplier = Math.Min(25, Position.DistanceTo(_player.Position) * 0.01f);
        Position = Position.Lerp(_player.Position, 0.01f * DistanceMultiplier);
    }

}

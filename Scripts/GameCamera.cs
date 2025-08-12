using Godot;
using System;
using System.Numerics;

public partial class GameCamera : Camera2D
{

    private Player _player;

    public override void _Ready()
    {
        _player = (Player)GetTree().GetNodesInGroup("Player")[0];
    }

    public override void _PhysicsProcess(double delta)
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        //float DistanceMultiplier = Math.Min(25, Position.DistanceTo(_player.Position) * 0.01f);
        //Position = Position.Lerp(_player.Position.Lerp(GetGlobalMousePosition(), 0.33f), 0.01f * DistanceMultiplier);
        Position = Position.Lerp(_player.Position.Lerp(GetGlobalMousePosition(), 0.25f), 0.05f);
    }

}

using Godot;
using System;

public partial class GameCamera3D : Camera3D
{
    private Player _player;
    private Camera2D _mouseTracker;

    public override void _Ready()
    {
        _player = (Player)GetTree().GetNodesInGroup("Player")[0];
        _mouseTracker = GetNode<Camera2D>("MouseTracker");
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
        GD.Print("Camera following mouse!");
        Vector3 mousePosition = new Vector3(_mouseTracker.GetGlobalMousePosition().X, _mouseTracker.GetGlobalMousePosition().Y, 0);
        Position = Position.Lerp(new Vector3(_player.Position.X, _player.Position.Y, 0).Lerp(mousePosition, 0.35f), 0.05f);
        GD.Print("Mouse position: " + mousePosition);
    }

    public void FollowPlayer()
    {
        float DistanceMultiplier = Math.Min(25, Position.DistanceTo(new Vector3(_player.Position.X, _player.Position.Y, 0) * 0.01f));
        Position = Position.Lerp(new Vector3(_player.Position.X, _player.Position.Y, 0), 0.01f * DistanceMultiplier);
    }
}

using Godot;
using System;

public partial class AIVision : Area2D
{

    private bool _playerInVisionCone;
    private Area2D _playerArea2D;

    public override void _PhysicsProcess(double delta)
    {
        if (_playerInVisionCone && !GetParent<Creature>().IsAlert)
        {
            CheckPlayerVisible();
        }
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
                GetParent<Creature>().BecomeAlert();
                GD.Print(GetParent().Name + " was alerted!");
            }
        }
    }
}

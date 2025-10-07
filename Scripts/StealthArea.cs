using Godot;
using System;

public partial class StealthArea : Area2D
{

    public void OnArea2DEnter(Area2D playerArea)
    {
        Player player = (Player)playerArea.GetParent();
        player.IsStealthed = true;
        GD.Print("Player is hiding!");
    }

    public void OnArea2DExit(Area2D playerArea)
    {
        Player player = (Player)playerArea.GetParent();
        player.IsStealthed = false;
        GD.Print("Player stopped hiding!");
    }

}

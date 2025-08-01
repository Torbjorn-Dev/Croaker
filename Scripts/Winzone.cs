using Godot;
using System;

public partial class Winzone : Node
{
    public void OnHitboxEntered2D(HitboxComponent Hitbox)
    {
        GameManager.Instance.WinLevel();
    }

}

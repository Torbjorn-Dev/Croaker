using Godot;
using System;

public partial class MeleeComponent : Area2D
{
    // If enemy is blocking, take damage. Else do damage and bounce off.

    [Export] private int _meleeDamage;
    [Export] private float _bouncePower;

    [Export] private HealthComponent _healthComponent;

    private Creature _meleedCreature;

    public void onAreaEntered2D(Area2D Area)
    {
        _meleedCreature = (Creature)Area.GetParent();

        if (_meleedCreature.IsBlocking)
        {
            _healthComponent.TakeDamage(1);
            GD.Print("Was blocked!");
            Bounce();
        }
        else
        {
            HealthComponent creatureHealth = _meleedCreature.GetNode<HealthComponent>("Health");
            creatureHealth.TakeDamage(_meleeDamage);
            Bounce();
        }
    }

    private void Bounce()
    {
        var Direction = GlobalPosition.DirectionTo(_meleedCreature.GlobalPosition);
        GD.Print("Direction: " + Direction.Normalized());
        Player player = (Player)GetParent();
        player.Velocity = -Direction.Normalized() * _bouncePower;
    }

}

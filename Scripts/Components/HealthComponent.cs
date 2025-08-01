using Godot;
using System;

public partial class HealthComponent : Node
{

    [Signal]
    public delegate void NoHealthEventHandler();

    [Export] private int _maxHealth;
    private int _currentHealth;

    public override void _Ready()
    {
        _currentHealth = _maxHealth;
    }


    public void TakeDamage(int DamageTaken)
    {
        GD.Print("Took " + DamageTaken + " damage!");
        _currentHealth -= DamageTaken;
        if (_currentHealth <= 0)
        {
            EmitSignal(SignalName.NoHealth);
        }
    }


}

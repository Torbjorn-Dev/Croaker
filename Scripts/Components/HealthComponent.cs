using Godot;
using System;

public partial class HealthComponent : Node
{

    [Signal]
    public delegate void NoHealthEventHandler();

    [Export] private int _maxHealth;
    private int _currentHealth;

    private bool _isInvincible = false;

    public override void _Ready()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int DamageTaken)
    {
        if (!_isInvincible)
        {
            GD.Print("Took " + DamageTaken + " damage!");
            _currentHealth -= DamageTaken;

            if (_currentHealth <= 0)
            {
                EmitSignal(SignalName.NoHealth);
            }
            StartIFrames();
        }
    }

    private void StartIFrames()
    {
        _isInvincible = true;
        GetChild<Timer>(0).Start();
    }

    private void OnInvincibilityTimerTimeout()
    {
        _isInvincible = false;
    }

}

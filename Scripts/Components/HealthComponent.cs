using Godot;
using System;

public partial class HealthComponent : Node
{

    [Signal]
    public delegate void NoHealthEventHandler();

    [Export] private int _maxHealth;
    private int _currentHealth;

    private bool _isInvincible = false;

    // References needed for accessing HitFlash shader.
    [Export] private Sprite2D[] _sprites;

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
        GD.Print(_sprites.Length);
        for (int i = 0; i < _sprites.Length; i++)
        {
            GD.Print("Sprite: " + _sprites[i].Name);
            var material = _sprites[i].GetMaterial();
            GD.Print("Material: " + material);
            ((ShaderMaterial)material).SetShaderParameter("hit_effect", 1);
        }

        GetChild<Timer>(0).Start();
    }

    private void OnInvincibilityTimerTimeout()
    {
        _isInvincible = false;

        foreach (var Sprite in _sprites)
        {
            var material = Sprite.GetMaterial();
            ((ShaderMaterial)material).SetShaderParameter("hit_effect", 0);
        }
    }

}

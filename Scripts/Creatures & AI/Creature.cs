using Godot;
using System;

public partial class Creature : CharacterBody2D
{
    [Export] private int _maxHealth;
    public int CurrentHealth;

    public bool IsBlocking = false;


    public override void _Ready()
    {
        CurrentHealth = _maxHealth;
    }

}

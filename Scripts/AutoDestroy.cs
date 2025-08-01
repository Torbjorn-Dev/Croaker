using Godot;
using System;

public partial class AutoDestroy : Node2D
{

    public override void _Ready()
    {
		GetNode<Timer>("Timer").Start();
    }

	public void OnTimerTimeout()
	{
		QueueFree();
	}

}

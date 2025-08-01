using Godot;
using System;

public partial class MainMenu : Control
{

    public void OnPlayButton()
    {
        GameManager.Instance.LoadScene("res://Scenes/Level1.tscn");
    }

    public void OnQuitButton()
    {
        GetTree().Quit();
    }

}

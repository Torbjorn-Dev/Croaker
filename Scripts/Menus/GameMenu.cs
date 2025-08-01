using Godot;
using System;

public partial class GameMenu : Control
{

    public void NextLevelButton()
    {
        string currentName = GetTree().CurrentScene.Name;

        // Assuming the format is always "Level" followed by a number
        string Prefix = "Level";
        string NumberPart = currentName.Substring(Prefix.Length);

        if (int.TryParse(NumberPart, out int currentLevelNumber))
        {
            int NextLevelNumber = currentLevelNumber + 1;
            string NextSceneName = $"{Prefix}{NextLevelNumber}";

            // Load the next scene
            string NextScenePath = $"res://Scenes/{NextSceneName}.tscn";
            GameManager.Instance.LoadScene(NextScenePath);
        }
        else
        {
            GD.PrintErr($"Failed to parse level number from scene name: {currentName}");
        }
    }

    public void RetryButton()
    {
        GD.Print(GetTree().CurrentScene.Name);
        string CurrentScenePath = $"res://Scenes/{GetTree().CurrentScene.Name}.tscn";
        GameManager.Instance.LoadScene(CurrentScenePath);
    }

    public void PauseButton()
    {
        GameManager.Instance.PauseGame();
    }

    public void BackToMenuButton()
    {
        GameManager.Instance.LoadScene("res://Scenes/MainMenu.tscn");
    }

}

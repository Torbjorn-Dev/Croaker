using Godot;
using System;

public partial class GameManager : Node
{

    [ExportCategory("Menus")]
    [Export] private PackedScene _winMenuScene;
    [Export] private PackedScene _loseMenuScene;
    [Export] private PackedScene _pauseMenuScene;
    private Control _winMenu, _loseMenu, _pauseMenu;

    public static GameManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;

        _winMenu = (Control)ResourceLoader.Load<PackedScene>(_winMenuScene.ResourcePath).Instantiate();
        AddChild(_winMenu);
        _loseMenu = (Control)ResourceLoader.Load<PackedScene>(_loseMenuScene.ResourcePath).Instantiate();
        AddChild(_loseMenu);
        _pauseMenu = (Control)ResourceLoader.Load<PackedScene>(_pauseMenuScene.ResourcePath).Instantiate();
        AddChild(_pauseMenu);

        CloseGameMenus();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Pause"))
        {
            PauseGame();
        }
    }

    public void CheckIfWon()
    {
        // It has been set to "1", as the enemy scripts check for enemies left before they themselves die. I will fix this later.
        if (EnemiesLeft() <= 1)
        {
            WinLevel();
        }
    }

    public void WinLevel()
    {
        GameStateManager.Instance.SetState(GameState.LevelWon, true);
        GetTree().Paused = true;
        //Engine.TimeScale = 0;
        _winMenu.Visible = true;
    }

    public void LoseLevel()
    {
        GameStateManager.Instance.SetState(GameState.LevelLost, true);
        GD.Print(GameStateManager.Instance.GetState(GameState.LevelLost));
        GetTree().Paused = true;
        //Engine.TimeScale = 0;
        _loseMenu.Visible = true;
    }

    private int EnemiesLeft()
    {
        return GetTree().GetNodesInGroup("Enemies").Count;
    }

    public void PauseGame()
    {
        // If paused, unpause. If not paused, pause.
        if (GameStateManager.Instance.GetState(GameState.Paused))
        {
            GameStateManager.Instance.SetState(GameState.Paused, false);
            GetTree().Paused = false;
            //Engine.TimeScale = 1;
            _pauseMenu.Visible = false;
        }
        else
        {
            GameStateManager.Instance.SetState(GameState.Paused, true);
            GetTree().Paused = true;
            //Engine.TimeScale = 0;
            _pauseMenu.Visible = true;
        }
    }


    private void CloseGameMenus()
    {
        _winMenu.Visible = false;
        _loseMenu.Visible = false;
        _pauseMenu.Visible = false;

        GetTree().Paused = false;
    }

    public void LoadScene(String ScenePath)
    {
        GetTree().ChangeSceneToFile(ScenePath);
        CloseGameMenus();
        GameStateManager.Instance.ResetAllStates();
    }

    public void LoadScene(PackedScene ScenePackedScene)
    {
        GetTree().ChangeSceneToPacked(ScenePackedScene);
        CloseGameMenus();
        GameStateManager.Instance.ResetAllStates();
    }

}

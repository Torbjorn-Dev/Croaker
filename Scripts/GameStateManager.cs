using Godot;
using Godot.Collections;
using System;

public enum GameState
{
    LevelWon,
    LevelLost,
    InCutscene,
    Paused
}

public partial class GameStateManager : Node
{
    public Dictionary<GameState, bool> States = new();

    public static GameStateManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        InitializeStates();
        ResetAllStates();
    }
    
    public void InitializeStates()
    {
        foreach (GameState state in Enum.GetValues(typeof(GameState)))
        {
            if (!States.ContainsKey(state))
            {
                States[state] = false;
            }
        }
    }

    public void SetState(GameState state, bool value)
    {
        if (States.ContainsKey(state))
        {
            States[state] = value;
        }
        GD.Print("State '" + state + "' set to " + value);
    }

    public bool GetState(GameState state)
    {
        return States.TryGetValue(state, out bool value) && value;
    }

    public void ResetAllStates()
    {
        foreach (var key in States.Keys)
        {
            States[key] = false;
        }

        GD.Print("All states have been reset!");
    }

    public void PrintAllStates()
    {
        foreach (var pair in States)
        {
            GD.Print($"{pair.Key}: {pair.Value}");
        }
    }

}
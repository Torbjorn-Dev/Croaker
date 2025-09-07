using Godot;
using System;

public partial class DialogueSystem : CanvasLayer
{

    [Export(PropertyHint.File)] private String _dialogueFile;
    private DialogueEntry[] Dialogues;
    private int currentIndex = 0;

    public override void _Ready()
    {
        PlayDialogue();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("DialogueNextLine"))
        {
            NextLine();
        }
    }

    private void NextLine()
    {
        if (currentIndex < Dialogues.Length - 1)
        {
            currentIndex++;
            ShowLine();
        }
        else
        {
            GD.Print("End of dialogue.");
        }
    }

    private void PlayDialogue()
    {
        Dialogues = LoadDialogue();
        ShowLine();
    }

    private DialogueEntry[] LoadDialogue()
    {
        var file = Godot.FileAccess.Open(_dialogueFile, Godot.FileAccess.ModeFlags.Read);
        string jsonText = file.GetAsText();

        return System.Text.Json.JsonSerializer.Deserialize<DialogueEntry[]>(jsonText);
    }

    private void ShowLine()
    {
        var entry = Dialogues[currentIndex];
        GD.Print($"{entry.name}: {entry.message}");
    }

}

public class DialogueEntry
{
    public string name { get; set; }
    public string message { get; set; }
}

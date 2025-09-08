using Godot;
using System;

public partial class DialogueSystem : CanvasLayer
{

    [Export(PropertyHint.File)] public String DialogueFile;
    [Export] private RichTextLabel _nameLabel;
    [Export] private RichTextLabel _messageLabel;
    private DialogueEntry[] Dialogues;
    private int currentIndex = 0;

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
            // ADD ANIMATION FOR CLOSING CALL
            GetTree().Paused = false;
            QueueFree();
        }
    }

    public void PlayDialogue()
    {
        GetTree().Paused = true;
        Dialogues = LoadDialogue();
        ShowLine();
    }

    private DialogueEntry[] LoadDialogue()
    {
        var file = Godot.FileAccess.Open(DialogueFile, Godot.FileAccess.ModeFlags.Read);
        string jsonText = file.GetAsText();

        return System.Text.Json.JsonSerializer.Deserialize<DialogueEntry[]>(jsonText);
    }

    private void ShowLine()
    {
        var entry = Dialogues[currentIndex];
        _nameLabel.Text = entry.name;
        _messageLabel.Text = entry.message;
    }

}

public class DialogueEntry
{
    public string name { get; set; }
    public string message { get; set; }
}

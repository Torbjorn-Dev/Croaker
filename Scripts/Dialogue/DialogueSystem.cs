using Godot;
using System;

public partial class DialogueSystem : CanvasLayer
{

    [Export(PropertyHint.File)] public String DialogueFile;
    [Export] private RichTextLabel _nameLabel, _messageLabel;
    [Export] private TextureRect _portrait1, _portrait2;
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

        ChangePortraits();
    }

    private void ChangePortraits()
    {
        var entry = Dialogues[currentIndex];
        try
        {
            Texture2D portraitTexture = (Texture2D)GD.Load("res://Sprites/Dialogue Portraits/" + entry.portrait1 + ".png");
            _portrait1.Texture = portraitTexture;
            GD.Print("Changed portrait1 to: " + entry.portrait1);
        }
        catch (Exception e)
        {
            GD.Print("No portrait1 declared in JSON file. ERROR: " + e);
        }

        try
        {
            Texture2D portraitTexture = (Texture2D)GD.Load("res://Sprites/Dialogue Portraits/" + entry.portrait2 + ".png");
            _portrait2.Texture = portraitTexture;
            GD.Print("Changed portrait2 to: " + entry.portrait2);
        }
        catch (Exception e)
        {
            GD.Print("No portrait2 declared in JSON file. ERROR: " + e);
        }
    }

}

public class DialogueEntry
{
    public string name { get; set; }
    public string message { get; set; }
    public string portrait1 { get; set; }
    public string portrait2 { get; set; }
}

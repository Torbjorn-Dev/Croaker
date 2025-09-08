using Godot;
using System;

public partial class DialogueTrigger : Area2D
{
    [Export] private PackedScene _dialogueSystemPackedScene;
    [Export(PropertyHint.File)] private String _dialogueFile;

    public void OnAreaEntered2D(Area2D Area)
    {
        DialogueSystem DialogueSystemScene = (DialogueSystem)ResourceLoader.Load<PackedScene>(_dialogueSystemPackedScene.ResourcePath).Instantiate();
        GetTree().Root.AddChild(DialogueSystemScene);
        DialogueSystemScene.DialogueFile = _dialogueFile;
        DialogueSystemScene.PlayDialogue();
        QueueFree();
    }
}

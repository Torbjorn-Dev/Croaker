extends Control

@export var next_level : String

func _on_play_button_button_down():
	get_tree().change_scene_to_file(next_level)

func _on_quit_button_button_down():
	get_tree().quit()

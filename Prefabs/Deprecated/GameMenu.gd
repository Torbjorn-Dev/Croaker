extends Control

@export var next_level : String
@export var this_level : String

func _on_next_level_button_button_down():
	get_tree().change_scene_to_file(next_level)


func _on_back_to_menu_button_button_down():
	get_tree().change_scene_to_file("res://Scenes/MainMenu.tscn")


func _on_back_to_menu_button_down():
	get_tree().change_scene_to_file("res://Scenes/MainMenu.tscn")


func _on_retry_button_button_down():
	get_tree().change_scene_to_file(this_level)

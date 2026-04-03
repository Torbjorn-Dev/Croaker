extends Control

@export var menu_camera : Camera2D

func _on_server_pressed() -> void:
	HighLevelNetworkHandler.start_server()
	##visible = false


func _on_client_pressed() -> void:
	HighLevelNetworkHandler.start_client()
	menu_camera.queue_free()
	visible = false


func _on_host_pressed() -> void:
	HighLevelNetworkHandler.start_host()
	menu_camera.queue_free()
	visible = false

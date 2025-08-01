extends Node2D

var speed = 3000

func _physics_process(delta):
	position += transform.x * speed * delta


func _on_area_2d_area_entered(area):
	print(area, " area")
	if area.is_in_group("enemies") or area.is_in_group("missiles"):
		if area.is_in_group("enemies"):
			get_parent().enemy_hit()
			area.get_parent().die()
		get_parent().get_bullet()
	queue_free()


func _on_area_2d_body_entered(body):
	print(body, " body")
	if body.is_in_group("enemies") or body.is_in_group("missiles"):
		if body.is_in_group("enemies"):
			get_parent().enemy_hit()
			body.get_parent().die()
		get_parent().get_bullet()
	queue_free()

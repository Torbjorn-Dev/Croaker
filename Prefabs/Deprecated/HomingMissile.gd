extends Node2D

@export var speed = 200

@onready var target = get_parent().get_parent().get_node("Frog_player")
@export var explosion : PackedScene

func _physics_process(delta):
	if not target:
		position += speed * Vector2.RIGHT.rotated(10) * delta
		return
	
	look_at(target.global_position)
	position = position.move_toward(target.global_position, speed * delta)

func _on_area_2d_area_entered(area):
	if area.is_in_group("player"):
		area.take_damage()
	if area.is_in_group("enemies") or area.is_in_group("missiles"):
		print("Don't explode!")
	else:
		explode()
		


func _on_area_2d_body_entered(body):
	if body.is_in_group("player"):
		body.take_damage()
	if body.is_in_group("enemies") or body.is_in_group("missiles"):
		print("Don't explode!")
	else:
		explode()

func explode():
	var ex = explosion.instantiate()
	get_parent().add_child(ex)
	ex.transform = global_transform
	ex.get_child(0).emitting = true
	set_physics_process(false)
	queue_free()

func _on_timer_timeout():
	explode()

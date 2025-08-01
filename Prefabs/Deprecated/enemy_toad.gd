extends Node2D

@onready var cooldown_timer = $Timer
@export var missile : PackedScene
@onready var fire_location = $FireLocation
@export var KillParticles : PackedScene

# Called when the node enters the scene tree for the first time.
func _ready():
	cooldown_timer.start()

func die():
	print("Toad died")
	var particles = KillParticles.instantiate()
	get_parent().add_child(particles)
	particles.transform = global_transform
	particles.get_child(0).emitting = true
	queue_free()

func _on_timer_timeout():
	fire_missile()
	cooldown_timer.start()

func fire_missile():
	var homingmissile = missile.instantiate()
	add_child(homingmissile)
	homingmissile.transform = fire_location.global_transform

func _on_area_2d_area_entered(area):
	if area.is_in_group("player"):
		area.take_damage()


func _on_area_2d_body_entered(body):
	if body.is_in_group("player"):
		body.take_damage()

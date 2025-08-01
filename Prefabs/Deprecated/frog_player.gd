extends CharacterBody2D

@export var enemies_left : int
var game_won = false
var dead = false

var current_slowmotion
var is_time_slowed = false
var is_charging = false

var can_move = true
var movement_stopped = false

const SPEED = 300
const JUMP_VELOCITY = 1000

const PROJECTION_SPEED = 1.1
var projection_flipped = false

var has_bullet = true
var bullets = 2
@onready var bulletIcon1 = get_parent().get_node("UI").get_node("BulletCount1")
@onready var bulletIcon2 = get_parent().get_node("UI").get_node("BulletCount2")

@onready var jump_projection = $JumpProjection
@onready var jump_target = $JumpProjection/JumpTarget
@onready var jump_projection_sprite = $JumpProjection/JumpProjectionSprite
@onready var timer = $Timer

@onready var aim_projection = $AimProjection
@onready var aim_projection_sprite = $AimProjection/AimProjectionSprite
@onready var fire_location = $AimProjection/FireLocation
@export var bullet : PackedScene

@onready var win_menu = get_parent().get_node("UI").get_node("WinMenu")
@onready var lose_menu = get_parent().get_node("UI").get_node("LoseMenu")
@onready var pause_menu = get_parent().get_node("UI").get_node("Menu")

@onready var charge_sprite = $Sprites/ChargeSprite
@onready var idle_sprite = $Sprites/IdleSprite
@onready var flipping_sprite = $Sprites/FlippingSprite
@onready var jumping_sprite = $Sprites/JumpingSprite

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

func _ready():
	Engine.set_time_scale(1)
	aim_projection_sprite.visible = false
	is_time_slowed = false

func _process(delta):
	if is_on_floor() and !game_won and !dead:
		if not is_charging:
			sprite_idle()
		if bullets < 2:
			restore_bullets()
		if is_time_slowed:
			Engine.set_time_scale(1)
			aim_projection_sprite.visible = false
			is_time_slowed = false
		if can_move == false:
			velocity.x = 0
		if Input.is_action_pressed("ui_accept"):
			is_charging = true
			sprite_charge()
			jump_projection_sprite.visible = true
			if jump_projection.rotation <= -2.7:
				projection_flipped = true
			elif jump_projection.rotation >= -0.3:
				projection_flipped = false
			if projection_flipped:
				jump_projection.rotation_degrees += PROJECTION_SPEED
			else:
				jump_projection.rotation_degrees -= PROJECTION_SPEED
		elif Input.is_action_just_released("ui_accept"):
			sprite_jumping()
			can_move = true
			timer.start()
			jump_projection_sprite.visible = false
			var direction = position.direction_to(jump_target.global_position)
			velocity = direction.normalized() * JUMP_VELOCITY
			if velocity.x > 0:
				jumping_sprite.scale.x = 2.3
			else:
				jumping_sprite.scale.x = -2.3
	if not is_on_floor() and !game_won and !dead:
		velocity.y += gravity * delta # Add gravity
		if Input.is_action_pressed("ui_accept"):
			if bullets > 0:
				if !is_time_slowed:
					Engine.set_time_scale(0.20)
					is_time_slowed = true
					aim_projection_sprite.visible = true
				if velocity.x > 0:
					sprite_flip()
					flipping_sprite.flip_h
					flipping_sprite.rotation_degrees += PROJECTION_SPEED * 0.5
					aim_projection.rotation_degrees += PROJECTION_SPEED * 0.5
				else:
					sprite_flip()
					flipping_sprite.rotation_degrees -= PROJECTION_SPEED * 0.5
					aim_projection.rotation_degrees -= PROJECTION_SPEED * 0.5
			else:
				aim_projection_sprite.visible = false
				if is_time_slowed:
					Engine.set_time_scale(1)
					is_time_slowed = false
		elif Input.is_action_just_released("ui_accept"):
			if is_time_slowed:
				shoot()
				Engine.set_time_scale(1)
				is_time_slowed = false
			aim_projection_sprite.visible = false

func _physics_process(delta):
	move_and_slide()

func shoot():
	if has_bullet:
		lose_bullet()
		var b = bullet.instantiate()
		add_child(b)
		b.transform = fire_location.global_transform
		var direction = position.direction_to(fire_location.global_position)
		velocity = direction.normalized() * -JUMP_VELOCITY

func enemy_hit():
	enemies_left -= 1
	if enemies_left <= 0:
		win_level()
	
func get_bullet():
	bullets += 1
	check_bullets()
	
func restore_bullets():
	bullets = 2
	check_bullets()
	
func lose_bullet():
	bullets -= 1
	check_bullets()
	
func check_bullets():
	if bullets <= 0:
		bullets = 0
		has_bullet = false
	elif bullets > 0:
		has_bullet = true
	if bullets >= 2:
		bullets = 2
	update_UI()
	
func update_UI():
	match bullets:
		0:
			bulletIcon1.visible = false
			bulletIcon2.visible = false
		1:
			bulletIcon1.visible = true
			bulletIcon2.visible = false
		2:
			bulletIcon1.visible = true
			bulletIcon2.visible = true

func take_damage():
	lose_menu.visible = true
	dead = true
	Engine.set_time_scale(0)
	
func win_level():
	win_menu.visible = true
	Engine.set_time_scale(0)

func _on_timer_timeout():
	can_move = false
	is_charging = false

func sprite_charge():
	charge_sprite.visible = true
	jumping_sprite.visible = false
	idle_sprite.visible = false
	flipping_sprite.visible = false

func sprite_flip():
	charge_sprite.visible = false
	jumping_sprite.visible = false
	idle_sprite.visible = false
	flipping_sprite.visible = true
	
func sprite_idle():
	charge_sprite.visible = false
	jumping_sprite.visible = false
	idle_sprite.visible = true
	flipping_sprite.visible = false
	
func sprite_jumping():
	charge_sprite.visible = false
	jumping_sprite.visible = true
	idle_sprite.visible = false
	flipping_sprite.visible = false

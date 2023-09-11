extends StaticBody3D

var grapple_point
var grappling = false

@onready var character = get_parent().get_parent()
@onready var grapple_cast = %RayCast3D#get_parent().get_child(2).get_child(0).get_child(0)
@onready var timer = $grappleTimer

func _physics_process(_delta):
	if Input.is_action_just_pressed("fire") and not grappling and grapple_cast.is_colliding():
		grappling = true
		character.yes = false
		character.rotation = Vector3.ZERO
		timer.start()
		grapple_point = grapple_cast.get_collision_point()
		var distance = grapple_point - character.position
		character.set_linear_velocity(distance.normalized() * 300.0)
	#if grappling and Input.is_action_just_released("fire"):
		#character.velocity = Vector3.ZERO

func _on_grapple_timer_timeout():
	grappling = false
	character.yes = true

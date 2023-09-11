extends CharacterBody3D

const SPEED = 10.0
const QUICK_SPEED = 20.0
const JUMP_VELOCITY = 5.0
const X_LOOK_SENS = 0.08
const Y_LOOK_SENS = 0.05

@export var move_speed = SPEED
@export var grappling = false
var direction
# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = 9.8

@onready var springNode = $SpringNode3D
@onready var camera = $SpringNode3D/Camera3D

func _input(event):
	if event is InputEventMouseMotion:
		springNode.rotation.x -= deg_to_rad(event.relative.y * Y_LOOK_SENS)
		springNode.rotation.x = clamp(springNode.rotation.x, deg_to_rad(-90), deg_to_rad(90))
		rotation.y -= deg_to_rad(event.relative.x * X_LOOK_SENS)
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var input_dir = Input.get_vector("move_left", "move_right", "move_forward", "move_back")
	direction = (transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	
	if Input.get_action_strength("move_quick"):
		move_speed = QUICK_SPEED
	if Input.is_action_just_released("move_quick"):
		move_speed = SPEED
	if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED and Input.is_action_just_released("escape"):
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	elif Input.is_action_just_released("escape"):
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	

func _physics_process(delta):
	if is_on_floor() and Input.is_action_just_pressed("jump"):
		velocity.y += JUMP_VELOCITY
	
	if is_on_wall() and grappling:
		velocity.x = 0
		velocity.y -= 20
		velocity.z = 0
	
	if direction:
		velocity.x = direction.x * move_speed
		velocity.z = direction.z * move_speed
	else:
		velocity.x = move_toward(velocity.x, 0, move_speed)
		velocity.z = move_toward(velocity.z, 0, move_speed)
	
	if not is_on_floor():
		velocity.y -= gravity * delta
	
	move_and_slide()

extends RigidBody3D

const SPEED = 50.0
const QUICK_SPEED = 100.0
const HORI_LOOK_SENS = 0.08
const VERTI_LOOK_SENS = 0.05

var move_speed = SPEED

var direction
# Get the gravity from the project settings to be synced with RigidBody nodes.
@onready var bodymesh = %bodymesh
@onready var cam_pivot = %campivot
@onready var camera = %campivot/Camera3D
@onready var holster = $holster

var forwback
var leftright
var updown

@export var yes = true
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass

func _input(event):
	if event is InputEventMouseMotion:
		cam_pivot.rotation.x += deg_to_rad(event.relative.y * VERTI_LOOK_SENS)
		cam_pivot.rotation.x = clamp(cam_pivot.rotation.x, deg_to_rad(-90), deg_to_rad(90))
		cam_pivot.rotation.y -= deg_to_rad(event.relative.x * HORI_LOOK_SENS)
		forwback = cam_pivot.transform.basis.z.normalized()
		updown = Vector3(1,0,1)
		leftright = cam_pivot.transform.basis.x.normalized()
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
		
	if Input.get_action_strength("move_quick"):
		move_speed = QUICK_SPEED
		print(get_linear_velocity())
	if Input.is_action_just_released("move_quick"):
		move_speed = SPEED
	
	if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED and Input.is_action_just_released("escape"):
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	elif Input.is_action_just_released("escape"):
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	

func _physics_process(_delta):
	rotation = Vector3.ZERO
	apply_force(Vector3.DOWN * 100)
	if yes:
		movement()
	elif not yes:
		bodymesh.look_at(position - get_linear_velocity())
		rotation = Vector3.ZERO
	if position.y < 0:
		position = Vector3(20, 20, 20)

func movement():
		
	if Input.is_action_just_pressed("jump"):
		apply_force(Vector3.UP * 5000)
		
	if not (Input.get_action_strength("move_left") or Input.get_action_strength("move_right") or Input.get_action_strength("move_forward") or Input.get_action_strength("move_back")):
		set_linear_velocity(Vector3(0, get_linear_velocity().y, 0))
	
	if Input.get_action_strength("move_left"):
		set_axis_velocity(Vector3(leftright * updown * move_speed))
		bodymesh.look_at(Vector3(position.x-get_linear_velocity().x, position.y, position.z-get_linear_velocity().z))
		holster.look_at(Vector3(position.x-get_linear_velocity().x, position.y, position.z-get_linear_velocity().z))
	if Input.is_action_just_released("move_left"):
		set_linear_velocity(Vector3(0, get_linear_velocity().y, 0))
		
	if Input.get_action_strength("move_right"):
		set_axis_velocity(-leftright * updown * move_speed)
		bodymesh.look_at(Vector3(position.x-get_linear_velocity().x, position.y, position.z-get_linear_velocity().z))
		holster.look_at(Vector3(position.x-get_linear_velocity().x, position.y, position.z-get_linear_velocity().z))
	if Input.is_action_just_released("move_right"):
		set_linear_velocity(Vector3(0, get_linear_velocity().y, 0))
		
	if Input.get_action_strength("move_forward"):
		set_axis_velocity(forwback * updown * move_speed)
		bodymesh.look_at(Vector3(position.x-get_linear_velocity().x, position.y, position.z-get_linear_velocity().z))
		holster.look_at(Vector3(position.x-get_linear_velocity().x, position.y, position.z-get_linear_velocity().z))
	if Input.is_action_just_released("move_forward"):
		set_linear_velocity(Vector3(0, get_linear_velocity().y, 0))
		
	if Input.get_action_strength("move_back"):
		set_axis_velocity(-forwback * updown * move_speed)
		bodymesh.look_at(Vector3(position.x-get_linear_velocity().x, position.y, position.z-get_linear_velocity().z))
		holster.look_at(Vector3(position.x-get_linear_velocity().x, position.y, position.z-get_linear_velocity().z))
	if Input.is_action_just_released("move_back"):
		set_linear_velocity(Vector3(0, get_linear_velocity().y, 0))

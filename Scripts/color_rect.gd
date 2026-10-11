extends ColorRect


# Called when the node enters the scene tree for the first time.
func _ready():
	# Get the ViewportTexture from the camera's viewport
	var viewport = get_viewport()
	material.set_shader_parameter("screen_texture", viewport.get_texture())


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

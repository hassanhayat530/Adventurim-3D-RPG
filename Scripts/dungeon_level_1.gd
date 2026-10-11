extends Node3D


# Called when the node enters the scene tree for the first time.
# Add to your main scene
func _ready():
	var post_process = ColorRect.new()
	post_process.size = get_viewport().size
	post_process.material = preload("res://Shaders/dungeon_level_1.gdshader")
	add_child(post_process)
	
	# Update size when window resized
	get_viewport().connect("size_changed", Callable(post_process, "set_size"))


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

extends TextureRect


# Called when the node enters the scene tree for the first time.
func _ready():
	texture = get_parent().get_texture()
	
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

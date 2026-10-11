@tool
extends DungeonRoom3D


# Called when the node enters the scene tree for the first time.
func _ready():
	super._ready()
	dungeon_done_generating.connect(remove_unused_doors_and_walls)

func remove_unused_doors_and_walls():
	_handle_door($"DOOR?_BACK", $Walls/B_WALL)
	_handle_door($"DOOR?_LEFT", $Walls/L_WALL)
	_handle_door($"DOOR?_LEFT2", $Walls/L_WALL2)
	_handle_door($"DOOR?_RIGHT", $Walls/R_WALL)
	_handle_door($"DOOR?_RIGHT2", $Walls/R_WALL2)
	for door in get_doors():
		if door.get_room_leads_to() == null and is_instance_valid(door.door_node):
			door.door_node.queue_free()

func _handle_door(door_node: Node3D, wall_model: Node3D) -> void:
	var door_obj = get_door_by_node(door_node)
	if door_obj == null:
		# door_node reference is stale — treat as unused
		if is_instance_valid(wall_model): wall_model.visible = true
		return
	if door_obj.get_room_leads_to() == null:
		if is_instance_valid(wall_model): wall_model.visible = true
	else:
		if is_instance_valid(wall_model): wall_model.queue_free()
		if is_instance_valid(door_node): door_node.visible = true

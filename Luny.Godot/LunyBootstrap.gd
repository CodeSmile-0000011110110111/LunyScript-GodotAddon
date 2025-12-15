extends Node

# Ensures GodotLifecycleAdapter gets instantiated when launching player

const GODOT_LIFECYCLE_ADAPTER = preload("uid://esdc77w7ncq8")

func _enter_tree() -> void:
    # Instantiate the adapter via a PackedScene bound to the C# script and add to the scene.
    if GODOT_LIFECYCLE_ADAPTER:
        var adapter: Node = GODOT_LIFECYCLE_ADAPTER.instantiate()
        get_parent().add_child.call_deferred(adapter)
        queue_free() # bootstrap no longer needed
    else:
        push_error("Failed to load GodotLifecycleAdapter.tscn")

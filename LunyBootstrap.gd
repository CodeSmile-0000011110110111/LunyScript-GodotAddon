#extends Node
#
#func _ready() -> void:
    #print("LunyBootstrap _ready")
    #
    ## Instantiate the adapter via a PackedScene bound to the C# script and add to the scene.
    #var packed := load("res://addons/lunyscript/Luny.Godot/GodotLifecycleAdapter.tscn")
    #if packed:
        #var adapter: Node = packed.instantiate()
        ## Defer to avoid 'Parent node is busy setting up children' during scene setup
        #get_tree().root.call_deferred("add_child", adapter)
    #else:
        #push_error("Failed to load GodotLifecycleAdapter.tscn")

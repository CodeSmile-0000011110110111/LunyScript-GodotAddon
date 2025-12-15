@tool
extends EditorPlugin

func _enter_tree():
    pass

func _exit_tree():
    pass

#const LUNY_AUTOLOAD_NAME := "LunyBootstrap"
#const LUNY_BOOTSTRAP_PATH := "res://addons/lunyscript/LunyBootstrap.gd"
#const LUNY_PS_KEY := "luny/bootstrap/registered"
#
#func _enable_plugin() -> void:
#    print("_enable_plugin")
#    _ensure_luny_autoload()
#
#func _ensure_luny_autoload() -> void:
#    print("_ensure_luny_autoload")
#    # Idempotent via our own flag; Godot’s API call is safe to repeat too.
#    if not ProjectSettings.has_setting(LUNY_PS_KEY):
#        add_autoload_singleton(LUNY_AUTOLOAD_NAME, LUNY_BOOTSTRAP_PATH)
#        print("[Luny] Registered autoload:", LUNY_AUTOLOAD_NAME)
#    else:
#        # Optional: ensure entry is present if flag exists but entry missing (rare)
#        add_autoload_singleton(LUNY_AUTOLOAD_NAME, LUNY_BOOTSTRAP_PATH)
#
#    ProjectSettings.set_setting(LUNY_PS_KEY, true)
#    ProjectSettings.save()

@tool
extends EditorPlugin

# Ensures Luny Bootstrap is set as autoload singleton

const LUNY_AUTOLOAD_NAME := "LunyBootstrap"
const LUNY_BOOTSTRAP_UID := "uid://bumivynfk8i2q"

func _ensure_luny_autoload() -> void:
    print("[Luny] Registering autoload:", LUNY_AUTOLOAD_NAME)
    var res_path := ResourceUID.uid_to_path(LUNY_BOOTSTRAP_UID)
    add_autoload_singleton(LUNY_AUTOLOAD_NAME, res_path)

    ProjectSettings.save()

func _enable_plugin() -> void:
    _ensure_luny_autoload()

func _enter_tree() -> void:
    _ensure_luny_autoload()

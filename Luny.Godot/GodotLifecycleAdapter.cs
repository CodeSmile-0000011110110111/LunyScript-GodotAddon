namespace Luny.Godot
{
    using global::Godot;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Ultra-thin Godot adapter: auto-initializes and forwards lifecycle to EngineLifecycleDispatcher.
    /// </summary>
    public sealed partial class GodotLifecycleAdapter : Node
    {
        // static GodotLifecycleAdapter()
        // {
        //     var asm = typeof(GodotLifecycleAdapter).Assembly.GetName().Name;
        //     GD.Print($"static ctor GodotLifecycleAdapter from assembly: {asm}");
        // }

        public static Node Create()
        {
            GD.Print("GodotLifecycleAdapter.Create()");
            return new GodotLifecycleAdapter();
        }

        private static GodotLifecycleAdapter _instance;

        private Luny.IEngineLifecycleDispatcher _dispatcher;

// Suppress analyzer warning about using ModuleInitializer in libraries; we intentionally use it for zero-config init.
#pragma warning disable CA2255
        [ModuleInitializer]
#pragma warning restore CA2255
        internal static void AutoInitialize()
        {
            var asm = typeof(GodotLifecycleAdapter).Assembly.GetName().Name;
            GD.Print($"AutoInitialize GodotLifecycleAdapter from assembly: {asm}");

            // Deferred creation until Godot main loop exists
            if (_instance == null)
            {
                Callable.From(() =>
                {
                    GD.Print($"AutoInitialize GodotLifecycleAdapter creating new instance");
                    _instance = new GodotLifecycleAdapter { Name = nameof(GodotLifecycleAdapter) };

                    var tree = Engine.GetMainLoop() as SceneTree;
                    // Use engine-native method name string for deferred call
                    tree?.Root.CallDeferred("add_child", _instance);
                }).CallDeferred();
            }
        }


        public GodotLifecycleAdapter()
        {
            GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) ctor");
            if (_instance != null)
            {
                Luny.EngineLifecycleDispatcher.ThrowDuplicateAdapterException(nameof(GodotLifecycleAdapter),
                    _instance.Name, unchecked((long)_instance.GetInstanceId()), Name, unchecked((long)GetInstanceId()));
            }
        }

        public override void _Ready()
        {
            GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) _Ready");
            _dispatcher = Luny.EngineLifecycleDispatcher.Instance;
        }

        public override void _Process(double delta)
        {
            //GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) processing with delta: {delta}");
            _dispatcher?.OnUpdate(delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            _dispatcher?.OnFixedStep(delta);
        }

        public override void _ExitTree()
        {
            GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) _ExitTree");
            if (_instance != null)
            {
                Luny.EngineLifecycleDispatcher.ThrowAdapterPrematurelyRemovedException(nameof(GodotLifecycleAdapter));
            }
        }

        public override void _Notification(int what)
        {
            if (what == NotificationWMCloseRequest)
            {
                GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) _Notification: NotificationWMCloseRequest");
                _dispatcher?.OnShutdown();
                _dispatcher = null;
                _instance = null;
            }
        }
    }
}

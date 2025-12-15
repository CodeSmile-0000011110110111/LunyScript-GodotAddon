using Godot;
using System;

namespace Luny.Godot
{
    /// <summary>
    /// Ultra-thin Godot adapter: auto-initializes and forwards lifecycle to EngineLifecycleDispatcher.
    /// </summary>
    /// <remarks>
    /// Gets auto-instantiated by LunyBootstrap.gd which itself gets auto-added to autoload via plugin.gd.
    /// </remarks>
    internal sealed partial class GodotLifecycleAdapter : Node
    {
        private static GodotLifecycleAdapter _instance;

        private IEngineLifecycleDispatcher _dispatcher;

        private GodotLifecycleAdapter()
        {
            if (_instance != null)
            {
                Throw.LifecycleAdapterSingletonDuplicationException(nameof(GodotLifecycleAdapter),
                    _instance.Name, unchecked((Int64)_instance.GetInstanceId()), Name, unchecked((Int64)GetInstanceId()));
            }

            _instance = this;
            _dispatcher = EngineLifecycleDispatcher.Instance;
        }

        public override void _Ready() => GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) _Ready");

        public override void _Process(Double delta) => _dispatcher?.OnUpdate(delta);

        public override void _PhysicsProcess(Double delta) => _dispatcher?.OnFixedStep(delta);

        public override void _ExitTree()
        {
            GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) _ExitTree");
            if (_instance != null)
            {
                Shutdown();
                Throw.LifecycleAdapterPrematurelyRemovedException(nameof(GodotLifecycleAdapter));
            }
        }

        public override void _Notification(Int32 what)
        {
            switch ((Int64)what)
            {
                case NotificationCrash:
                case NotificationWMCloseRequest:
                    GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) _Notification: {GetNotificationString(what)}");
                    Shutdown();
                    break;

                default:
                    if (what != NotificationProcess && what != NotificationPhysicsProcess)
                        GD.Print($"GodotLifecycleAdapter ({GetInstanceId()}) _Notification: {GetNotificationString(what)}");
                    break;
            }
        }

        private String GetNotificationString(Int32 what) => (Int64)what switch
        {
            NotificationWMCloseRequest => "NotificationWMCloseRequest",
            var _ => what.ToString(),
        };

        private void Shutdown()
        {
            if (_instance != null)
            {
                _dispatcher?.OnShutdown();
                _dispatcher = null;
                _instance = null;
            }
        }
    }
}

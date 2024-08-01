using DiscordLogHook.Utilities;

namespace DiscordLogHook
{
    public class ModApi : IModApi
    {
        public static bool DebugMode { get; set; } = false;

        public void InitMod(Mod _modInstance)
        {
            DiscordLogger.Init();
            Log.LogCallbacks += DiscordLogger.LogCallbackDelegate;
            ModEvents.GameAwake.RegisterHandler(DiscordLogger.OnGameAwake);
            ModEvents.GameShutdown.RegisterHandler(DiscordLogger.OnGameShutdown);
        }
    }
}

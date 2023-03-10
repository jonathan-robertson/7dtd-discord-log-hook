using DiscordLogHook.Utilities;

namespace DiscordLogHook
{
    public class ModApi : IModApi
    {
        public void InitMod(Mod _modInstance)
        {
            DiscordLogger.Init();
            Log.LogCallbacks += DiscordLogger.LogCallbackDelegate;
            ModEvents.GameAwake.RegisterHandler(DiscordLogger.OnGameAwake);
            ModEvents.GameStartDone.RegisterHandler(DiscordLogger.OnGameStartDone);
            ModEvents.GameShutdown.RegisterHandler(DiscordLogger.OnGameShutdown);
        }
    }
}

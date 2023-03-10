using DiscordLogHook.Utilities;
using HarmonyLib;
using System.Reflection;

namespace DiscordLogHook
{
    public class ModApi : IModApi
    {
        public static bool DebugMode { get; private set; } = false;

        public void InitMod(Mod _modInstance)
        {
            DiscordLogger.Init();
            new Harmony(GetType().ToString()).PatchAll(Assembly.GetExecutingAssembly());
            Log.LogCallbacks += DiscordLogger.LogCallbackDelegate;
            ModEvents.GameAwake.RegisterHandler(DiscordLogger.OnGameAwake);
            ModEvents.GameShutdown.RegisterHandler(DiscordLogger.OnGameShutdown);
        }
    }
}

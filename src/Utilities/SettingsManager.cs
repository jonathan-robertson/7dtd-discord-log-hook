using DiscordLogHook.Data;
using System;
using System.IO;

namespace DiscordLogHook.Utilities
{
    internal class SettingsManager
    {
        private static readonly ModLog<SettingsManager> log = new ModLog<SettingsManager>();
        private static readonly string filename = Path.Combine(GameIO.GetSaveGameDir(), "discord-log-hook.json");

        internal static Settings Load()
        {
            CreatePathIfMissing();
            Settings settings;
            try
            {
                var input = File.ReadAllText(filename);
                if (input != null)
                {
                    settings = Json<Settings>.Deserialize(input);
                    log.Info($"Successfully loaded settings for Discord Log Hook mod; filename: {filename}.");
                    return settings;
                }
                log.Warn($"Unable to load settings for Discord Log Hook mod; falling back to default values; filename: {filename}");
            }
            catch (FileNotFoundException)
            {
                return CreateFirstFile();
            }
            catch (Exception e)
            {
                log.Warn($"Unhandled exception encountered when attempting to load settings for Discord Log Hook mod; filename: {filename}", e);
            }
            return null;
        }

        internal static void CreatePathIfMissing()
        {
            _ = Directory.CreateDirectory(GameIO.GetSaveGameDir());
        }

        private static Settings CreateFirstFile()
        {
            log.Info($"No file detected for Discord Log Hook mod; creating a config with defaults in {filename}");
            var settings = new Settings();
            Save(settings);
            return settings;
        }

        internal static void Save(Settings settings)
        {
            try
            {
                if (!Directory.Exists(GameIO.GetSaveGameDir()))
                {
                    _ = Directory.CreateDirectory(GameIO.GetSaveGameDir());
                }
                File.WriteAllText(filename, Json<Settings>.Serialize(settings));
            }
            catch (Exception e)
            {
                log.Error($"Unable to save settings to {filename}.", e);
            }
        }
    }
}

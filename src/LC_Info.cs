using BepInEx.Bootstrap;
using LC_API.ServerAPI;

namespace GhoulMage.LethalCompany
{
    /// <summary>
    /// Info about Lethal Company's current context.
    /// </summary>
    public class LC_Info
    {
        /// <summary>
        /// v{versionNumber}
        /// </summary>
        public const char VersionPrefix = 'v';

        /// <summary>
        /// Lethal Company game version.<br/>Syntax '<inheritdoc cref="LC_Info.VersionPrefix"/>'.<br/>Might return "unknown" if the GameNetworkManager isn't initialized yet.
        /// </summary>
        public static string GameVersion
        {
            get
            {
                if (GameNetworkManager.Instance == null)
                    return "unknown";

                if (HasLoadedMod("LC_API"))
                {
                    if (ModdedServer.ModdedOnly)
                        return $"v{ModdedServer.GameVersion}";
                }

                return $"v{GameNetworkManager.Instance.gameVersionNum}";
            }
        }

        /// <summary>
        /// Lethal Company game version.<br/>Might return -1 if the GameNetworkManager isn't initialized yet.
        /// </summary>
        public static int GameVersionNumber
        {
            get
            {
                if (GameNetworkManager.Instance == null)
                    return -1;

                if (HasLoadedMod("LC_API"))
                {
                    if (ModdedServer.ModdedOnly)
                        return ModdedServer.GameVersion;
                }

                return GameNetworkManager.Instance.gameVersionNum;
            }
        }

        /// <summary>
        /// Is the specified GUID in the Chainloader.PluginInfos dictionary?
        /// </summary>
        public static bool HasLoadedMod(string guid)
        {
            return Chainloader.PluginInfos.ContainsKey(guid);
        }
    }
}

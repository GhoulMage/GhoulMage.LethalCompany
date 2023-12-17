using BepInEx.Bootstrap;

namespace GhoulMage.LethalCompany
{
    public class LethalApp
    {
        /// <summary>
        /// Syntax v{VersionNumber}. Might return "unknown" if the GameNetworkManager isn't initialized yet.
        /// </summary>
        public static string Version
        {
            get
            {
                if (GameNetworkManager.Instance == null)
                    return "unknown";

                //I don't know what the fuck 16440 means. Modded version number?
                //It's done in LC_API, so it's common enough to add this edge case...
                //This is pure evil. Wonder what happens when the official version 16440 comes around...
                if (GameNetworkManager.Instance.gameVersionNum > 16440)
                    return $"v{GameNetworkManager.Instance.gameVersionNum - 16440}";

                return $"v{GameNetworkManager.Instance.gameVersionNum}";
            }
        }

        /// <summary>
        /// Might return -1 if the GameNetworkManager isn't initialized yet.
        /// </summary>
        public static int PureVersionNumber
        {
            get
            {
                if (GameNetworkManager.Instance == null)
                    return -1;

                //I hate this.
                if (GameNetworkManager.Instance.gameVersionNum > 16440)
                    return GameNetworkManager.Instance.gameVersionNum - 16440;

                return GameNetworkManager.Instance.gameVersionNum;
            }
        }

        public static bool HasLoadedMod(string guid)
        {
            return Chainloader.PluginInfos.ContainsKey(guid);
        }
    }
}

using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using UnityEngine;
using System;
using System.Reflection;

namespace GhoulMage.LethalCompany
{
    // Game version does not come up until the main menu is loaded.
    // This is because for some reason it is inside a Singleton (GameNetworkManager), it's not even a const.
    // We need to workaround this by waiting until the singleton instance exists to check the version number and dispatch if that's a compatible version to load the mod in.
    [BepInDependency("LC_API", BepInDependency.DependencyFlags.SoftDependency)]
    public abstract class GhoulMagePlugin : BaseUnityPlugin
    {
        /// <summary>
        /// API version of the GhoulMagePlugin
        /// </summary>
        protected const string Version = "1.0.1";

        /// <summary>
        /// Version of GhoulMage.LethalCompany. Should match Assembly version.
        /// </summary>
        public const string APIVersion = "2.0.0";

        /// <summary>
        /// Override this to allow compatibility with game versions. Use the version number as in steam patches (40, 45, etc...)
        /// </summary>
        protected abstract LethalGameVersions GameCompatibility { get; }
        protected Harmony HarmonyInstance { get; private set; }
        /// <summary>
        /// Override this with Assembly.GetExecutingAssembly() (your plugin assembly, usually) to correctly tell Harmony which assembly to patch the game with.
        /// </summary>
        protected abstract Assembly AssemblyToPatch { get; }

        private Action _onSuccesfulLoad, _onFailedLoad;
        private bool _intializing, _initialized;

        /// <summary>
        /// Logs a useful message and creates a new Harmony instance.
        /// Then it waits for the Main menu to end loading to check if the game version is compatible.
        /// </summary>
        /// <param name="loadCallback">Called when the main menu loads, the game is connected, and version number is compatible with GameCompatiblity</param>
        /// <param name="patchImmediate">Should it call HarmonyInstance.PatchAll() after a succesful load?</param>
        protected void Startup(string GUID, string NAME, string VERSION, Action loadCallback, bool patchImmediate = true)
        {
            if (_intializing)
            {
                Logger.LogWarning("Trying to startup multiple times..? Aborting.");
                return;
            }
            _intializing = true;

            Logger.LogInfo($"Mod {NAME} ver {VERSION} ({GUID}) is loaded!");
            _onSuccesfulLoad = loadCallback;
            HarmonyInstance = new Harmony(GUID);

            if (patchImmediate)
            {
                PatchAllASAPIfCompatible(NAME);
            }
            else
            {
                WaitToCheckCompatibility();
            }
            Logger.LogInfo("Created Loader. Will patch all? " + (patchImmediate ? "Yes" : "No"));
        }

        /// <summary>
        /// Logs a useful message and creates a new Harmony instance.
        /// Then it waits for the Main menu to end loading to check if the game version is compatible.
        /// </summary>
        /// <param name="loadCallback">Called when the main menu loads, the game is connected, and version number is compatible with GameCompatiblity</param>
        /// <param name="failedLoadCallback">Called when the version number is incompatible with GameCompatiblity</param>
        /// <param name="patchImmediate">Should it call HarmonyInstance.PatchAll() after a succesful load?</param>
        protected void Startup(string GUID, string NAME, string VERSION, Action loadCallback, Action failedLoadCallback, bool patchImmediate = true)
        {
            if (_intializing)
            {
                Logger.LogWarning("Trying to startup multiple times..? Aborting.");
                return;
            }
            _intializing = true;

            Logger.LogInfo($"Mod {NAME} ver {VERSION} ({GUID}) is loaded!");

            _onSuccesfulLoad = loadCallback;
            _onFailedLoad = failedLoadCallback;

            HarmonyInstance = new Harmony(GUID);

            if (patchImmediate)
            {
                PatchAllASAPIfCompatible(NAME);
            }
            else
            {
                WaitToCheckCompatibility();
            }
            Logger.LogInfo("Created Loader. Will patch all? " + (patchImmediate ? "Yes" : "No"));
        }

        private void PatchAllASAPIfCompatible(string name)
        {
            GameObject patcher = new GameObject($"{name} Loader");
            DontDestroyOnLoad(patcher);

            LatePatcher patchScript = patcher.AddComponent<LatePatcher>();
            patchScript.Assembly = AssemblyToPatch;
            patchScript.Harmony = HarmonyInstance;
            patchScript.compatibleVersions = GameCompatibility;
            patchScript.Logger = Logger;
            patchScript.VersionCompatibleCallback = _onSuccesfulLoad;
            patchScript.VersionIncompatibleCallback = _onFailedLoad;
        }
        private void WaitToCheckCompatibility()
        {
            GameObject loaderGameObject = new GameObject($"{name} Loader");
            DontDestroyOnLoad(loaderGameObject);
            loaderGameObject.hideFlags = HideFlags.HideAndDontSave;

            LatePatcher patchScript = loaderGameObject.AddComponent<LatePatcher>();
            patchScript.compatibleVersions = GameCompatibility;
            patchScript.Logger = Logger;
            patchScript.VersionCompatibleCallback = _onSuccesfulLoad;
            patchScript.VersionIncompatibleCallback = _onFailedLoad;
        }

        internal void FireSuccesfulLoad()
        {
            if (_initialized)
            {
                Logger.LogWarning("Somehow firing OnSuccesfulLoad multiple times..? Aborting.");
                return;
            }

            _initialized = true;
            _onSuccesfulLoad();
        }

        private void Start()
        {
            if (!_intializing)
                Initialize();
        }
        private void OnDestroy()
        {
            if (!_intializing)
                Initialize();
        }

        /// <summary>
        /// Use this instead of Awake. Call Startup from here after doing your initialization.
        /// </summary>
        protected abstract void Initialize();
    }

    /// <summary>
    /// Waits until GameNetworkManager.Instance exists to check if the game version is compatible with our plugin.
    /// </summary>
    internal class LatePatcher : MonoBehaviour
    {
        internal Harmony Harmony;
        internal LethalGameVersions compatibleVersions;
        internal ManualLogSource Logger;
        internal Assembly Assembly;
        internal Action VersionCompatibleCallback;
        internal Action VersionIncompatibleCallback;

        private void Update()
        {
            if (GameNetworkManager.Instance != null)
            {
                if (compatibleVersions.CompatibleWith(LC_Info.GameVersion))
                {
                    Logger.LogInfo($"Compatible with Game Version {LC_Info.GameVersion}!");

                    //Might be null if the user didn't want to patch all immediatelly
                    if (Harmony != null)
                    {
                        Logger.LogInfo("About to patch everything!");
                        Harmony.PatchAll(Assembly);
                    }

                    VersionCompatibleCallback?.Invoke();
                }
                else
                {
                    Logger.LogError($"Incompatible due to Game Version {LC_Info.GameVersion}...");
                    VersionIncompatibleCallback?.Invoke();
                }
                DestroyImmediate(gameObject);
            }
        }
    }
}

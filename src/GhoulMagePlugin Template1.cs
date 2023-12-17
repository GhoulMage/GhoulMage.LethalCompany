using BepInEx;
using BepInEx.Logging;
using GhoulMage.LethalCompany;
using System.Reflection;

namespace GhoulMagePlugin_Template1_Namespace {
	[BepInPlugin(GUID, NAME, VERSION)]
	[BepInProcess("Lethal Company.exe")]
	public class GhoulMagePlugin_Template1 : GhoulMagePlugin {
		public const string GUID = "GhoulMagePlugin_Template1.UNIQUE.GUID";
		public const string NAME = "GhoulMagePlugin_Template1";
		public const string VERSION = "0.1.0";

		protected override LethalGameVersions GameCompatibility => new LethalGameVersions("v40", "v45");

		protected override Assembly AssemblyToPatch => Assembly.GetExecutingAssembly();

		internal static ManualLogSource Log;

		protected override void Initialize() {
			Log = Logger;
			base.Startup(GUID, NAME, VERSION, OnSuccesfulLoad, true);
		}
		private static void OnSuccesfulLoad() {
			Log.LogInfo("GhoulMagePlugin_Template1 loaded...");
		}
	}
}

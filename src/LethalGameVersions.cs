namespace GhoulMage.LethalCompany
{
    /// <summary>
    /// Helper class to contain LethalCompany game versions
    /// </summary>
    public class LethalGameVersions
    {
        private string[] _versions;

        public LethalGameVersions(params string[] versions)
        {
            _versions = versions;
        }

        /// <summary>
        /// Is the game compatible with the specified version?<br/>Version string format: '<inheritdoc cref="LC_Info.VersionPrefix"/>'
        /// </summary>
        public bool CompatibleWith(string version)
        {
            foreach (string ver in _versions)
            {
                if (ver.Equals(version))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Is the game compatible with the specified version?
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool CompatibleWith(int version)
        {
            return CompatibleWith($"{LC_Info.VersionPrefix}{version}");
        }
    }
}

namespace GhoulMage.LethalCompany
{
    public class LethalGameVersions
    {
        public const char VersionPrefix = 'v';
        string[] _versions;

        public LethalGameVersions(params string[] versions)
        {
            _versions = versions;
        }

        /// <summary>
        /// Version name format: {VersionPrefix}{versionNumber}
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
        public bool CompatibleWith(int version)
        {
            return CompatibleWith($"{VersionPrefix}{version}");
        }
    }
}

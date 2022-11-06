using System.Reflection;

namespace Acme.Energy.Backend
{
    public class VersionInfo
    {
        private static DateTime _t0;

        public static void Start()
        {
            _t0 = DateTime.Now;
        }
        public static long SecondsRunning
        {
            get
            {
                var t1 = DateTime.Now;
                return (long)(t1 - _t0).TotalSeconds;
            }
        }

        public static DateTime CreationDate
        {
            get
            {
                var asm = Assembly.GetEntryAssembly();
                return asm != null ? GetCreationDate(asm) : DateTime.MinValue;
            }
        }

        private static DateTime GetCreationDate(Assembly assembly)
        {
            var location = assembly.Location;
            return File.GetCreationTime(location);
        }


        public static string Company { get { return GetExecutingAssemblyAttribute<AssemblyCompanyAttribute>(a => a.Company); } }
        public static string Product { get { return GetExecutingAssemblyAttribute<AssemblyProductAttribute>(a => a.Product); } }
        public static string Copyright { get { return GetExecutingAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright); } }
        public static string Trademark { get { return GetExecutingAssemblyAttribute<AssemblyTrademarkAttribute>(a => a.Trademark); } }
        public static string Title { get { return GetExecutingAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title); } }
        public static string Description { get { return GetExecutingAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description); } }
        public static string Configuration { get { return GetExecutingAssemblyAttribute<AssemblyConfigurationAttribute>(a => a.Configuration); } }
        public static string FileVersion { get { return GetExecutingAssemblyAttribute<AssemblyFileVersionAttribute>(a => a.Version); } }

        public static Version Version => Assembly.GetExecutingAssembly()?.GetName().Version ?? new System.Version();

        public static string SemverVersion =>
            // Skip revision
            $"{VersionMajor}.{VersionMinor}.{VersionBuild}";
        public static string VersionFull => Version.ToString();
        public static string VersionMajor => Version.Major.ToString();
        public static string VersionMinor => Version.Minor.ToString();
        public static string VersionBuild => Version.Build.ToString();
        public static string VersionRevision => Version.Revision.ToString();

        private static string GetExecutingAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
        {
            var asm = Assembly.GetExecutingAssembly();
            if (asm == null)
            {
                return string.Empty;
            }
            return Attribute.GetCustomAttribute(asm, typeof(T)) is T attribute ? value.Invoke(attribute) : string.Empty;
        }
    }
}

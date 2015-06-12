using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Cyperus.Designer
{
    delegate void AssemblyLoadedHandler(object sender, EventArgs e);
    
    static class Program
    {
        public static List<Assembly> Assemblies;
        public static Dictionary<Assembly, Type[]> TypesByAssembly;
        public static List<Assembly> VisibleAssemblies;
        public static List<Type> Types;
        public static bool ShowEmptyAssemblies = false;

        public static event AssemblyLoadedHandler AssemblyLoaded;

        private static void AssemblyLoadHandler(object sender, AssemblyLoadEventArgs e)
        {
            var typeQuery =
                from type in e.LoadedAssembly.GetExportedTypes()
                where type.IsSubclassOf(typeof(AbstractNode))
                select type;

            Assemblies.Add(e.LoadedAssembly);
            TypesByAssembly.Add(e.LoadedAssembly, typeQuery.ToArray());
            Types.AddRange(typeQuery.ToArray());

            if ((typeQuery.Count() > 0) || ShowEmptyAssemblies)
            {
                VisibleAssemblies.Add(e.LoadedAssembly);
            }

            if (AssemblyLoaded != null)
            {
                AssemblyLoaded(null, null);
            }
        }

        public static void RebuildVisibleAssembliesList()
        {
            var assebmlyQuery =
                from a in Assemblies
                where ShowEmptyAssemblies || TypesByAssembly[a].Length > 0
                select a;

            VisibleAssemblies = new List<Assembly>(assebmlyQuery);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Listing assebmlies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assemblies = new List<Assembly>(assemblies);

            // Listing types
            TypesByAssembly = new Dictionary<Assembly, Type[]>();
            Types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var typeQuery =
                    from type in assembly.GetExportedTypes()
                    where type.IsSubclassOf(typeof(AbstractNode)) && !type.IsAbstract
                    select type;

                TypesByAssembly.Add(assembly, typeQuery.ToArray());
                Types.AddRange(typeQuery.ToArray());
            }

            RebuildVisibleAssembliesList();

            AppDomain.CurrentDomain.AssemblyLoad += AssemblyLoadHandler;
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

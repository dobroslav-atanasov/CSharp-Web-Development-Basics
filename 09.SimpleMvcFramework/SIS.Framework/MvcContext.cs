﻿namespace SIS.Framework
{
    using System.Reflection;

    public class MvcContext
    {
        private static MvcContext Instance;

        private MvcContext()
        {
        }

        public static MvcContext Get => Instance == null ? (Instance = new MvcContext()) : Instance;

        public string AssemblyName { get; set; } = Assembly.GetEntryAssembly().GetName().Name;

        public string ControllersFolder { get; set; } = "Controllers";

        public string ControllersSuffix { get; set; } = "Controller";

        public string ViewsFolder { get; set; } = "Views";

        public string ModelsFolder { get; set; } = "Models";
    }
}
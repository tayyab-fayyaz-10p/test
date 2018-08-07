using System;
using System.Configuration;
using System.IO;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace SSH.Core
{
    public static class IoC
    {
        private static IUnityContainer container;

        static IoC()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "unity.config");
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");
            container = new UnityContainer();
            container.LoadConfiguration(unitySection);
        }

        public static IUnityContainer Container
        {
            get
            {
                return container;
            }
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public static bool Exists<T>()
        {
            return container.IsRegistered<T>();
        }

        public static IUnityContainer RegisterInstance<T>(T instance)
        {
            container = container.RegisterInstance<T>(instance);
            return container;
        }
    }
}

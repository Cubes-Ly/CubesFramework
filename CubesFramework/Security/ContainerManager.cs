using System;
using System.Security.Cryptography;
using Unity;
using Unity.Resolution;

namespace CubesFramework.Security
{
    /// <summary>
    /// Presents the ioc container for any usable service will be used in the whole framework
    /// The class has implemented in singletone and ioc (dependency injection) design patterns
    /// </summary>
    internal sealed class ContainerManager
    {
        private ContainerManager()
        {
        }

        private readonly static Lazy<IUnityContainer> container =
            new Lazy<IUnityContainer>(() => new UnityContainer());
        /// <summary>
        /// The container object 
        /// </summary>
        public static IUnityContainer ServicesContainer => container.Value
            .RegisterInstance(MD5.Create())
            .RegisterInstance(SHA1.Create())
            .RegisterInstance(SHA256.Create())
            .RegisterInstance(SHA384.Create())
            .RegisterInstance(SHA512.Create())
            .RegisterSingleton<HashAlgorithm, MD5>("hasheralog")
            .RegisterSingleton(typeof(IHashingService), typeof(HashingProvider));
        public static IUnityContainer RegisterInstance(Type t, object instance) => ServicesContainer.RegisterInstance(t, instance);
        public static IUnityContainer RegisterSingleton(Type tfrom, Type tto) => ServicesContainer.RegisterSingleton(tfrom, tto);
        public static IUnityContainer RegisterSingleton(Type tfrom, Type tto, string name) => ServicesContainer.RegisterSingleton(tfrom, tto, name);
        public static IUnityContainer RegisterType(Type tfrom, Type tto) => ServicesContainer.RegisterType(tfrom, tto);
        public static IUnityContainer RegisterType(Type tfrom, Type tto, string name) => ServicesContainer.RegisterType(tfrom, tto, name);
        public static object Resolve(Type t, string name, params ResolverOverride[] overrides) => ServicesContainer.Resolve(t, name, overrides);
        public static object Resolve(Type t) => ServicesContainer.Resolve(t);
        public static object Resolve(Type t, params ResolverOverride[] overrides) => ServicesContainer.Resolve(t, overrides);


    }
}

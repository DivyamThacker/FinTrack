using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Helper
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public static void Register<TService>(TService service)
        {
            services[typeof(TService)] = service;
        }

        public static TService GetService<TService>()
        {
            if (!services.ContainsKey(typeof(TService)))
            {
                throw new ArgumentException($"Service of type {typeof(TService)} is not registered.");
            }
            return (TService)services[typeof(TService)];
        }
    }

}

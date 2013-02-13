using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace NPerfRunner.Wpf
{
    internal class IoC
    {
        private static readonly IoC instance = new IoC();

        private readonly IKernel container;

        public static IoC Instance
        {
            get
            {
                return instance;
            }
        }

        private List<Type> registered = new List<Type>();

        private IoC()
        {
            this.container = new StandardKernel();
        }

        public void RegisterSingleton(Type service, Type type, string name = null)
        {
            registered.Add(service);
            var binding = container.Bind(service).To(type).InSingletonScope();
            if (!string.IsNullOrEmpty(name))
            {
                binding.Named(name);
            }
        }

        public void RegisterType(Type service, Type type, string name = null)
        {
            var binding = container.Bind(service).To(type);
            if (!string.IsNullOrEmpty(name))
            {
                binding.Named(name);
            }
        }

        public object Resolve(Type type, string name = null)
        {
            object result;
            if (string.IsNullOrEmpty(name))
            {
                result = container.Get(type);
            }
            else
            {
                result = container.Get(type, name);
            }

            return result;
        }

        public IEnumerable<object> ResolveAll(Type type, string name = null)
        {
            IEnumerable<object> result;
            if (string.IsNullOrEmpty(name))
            {
                result = container.GetAll(type);
            }
            else
            {
                result = container.GetAll(type, name);
            }

            return result;
        }
    }
}

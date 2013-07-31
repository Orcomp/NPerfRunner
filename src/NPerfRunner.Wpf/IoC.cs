namespace NPerfRunner.Wpf
{
    using System;
    using System.Collections.Generic;
    using Ninject;
    using Ninject.Syntax;

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

        private IoC()
        {
            this.container = new StandardKernel();
        }

        public void RegisterSingleton(Type service, Type type, string name = null)
        {
            var binding = this.container.Bind(service).To(type).InSingletonScope();
            if (!string.IsNullOrEmpty(name))
            {
                binding.Named(name);
            }
        }

        public void RegisterSingleton(Type service, string name = null)
        {
            var binding = this.container.Bind(service).ToSelf().InSingletonScope();
            if (!string.IsNullOrEmpty(name))
            {
                binding.Named(name);
            }
        }

        public void RegisterType(Type service, Type type, string name = null)
        {
            var binding = this.container.Bind(service).To(type);
            if (!string.IsNullOrEmpty(name))
            {
                binding.Named(name);
            }
        }

        public T Resolve<T>(string name = null)
        {
            return (T)this.Resolve(typeof(T), name);
        }

        public object Resolve(Type type, string name = null)
        {
            var result = string.IsNullOrEmpty(name) ? this.container.Get(type) : this.container.Get(type, name);

            return result;
        }

        public IEnumerable<object> ResolveAll(Type type, string name = null)
        {
            var result = string.IsNullOrEmpty(name) ? this.container.GetAll(type) : this.container.GetAll(type, name);

            return result;
        }
    }
}

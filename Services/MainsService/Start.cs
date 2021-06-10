using Autofac;
using System;
using System.Reflection;

namespace MainsService
{
    public class Start
    {
        public static void Builder(ContainerBuilder builder)
        {
            var dataAccess = Assembly.GetExecutingAssembly();
            //builder.RegisterAssemblyTypes(dataAccess)
            //    .Where(t => t.Name.EndsWith("Command"))
            //    .AsImplementedInterfaces().PreserveExistingDefaults();

            builder.RegisterAssemblyTypes(dataAccess)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

        }
    }
}

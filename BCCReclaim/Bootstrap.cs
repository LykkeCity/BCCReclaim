using Autofac;
using BCCReclaim.Settings;
using Common.Helpers.BlockchainExplorerHelper;
using Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCCReclaim
{
    public class Bootstrap
    {
        public static IContainer container = null;
        public static void Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SettingsProvider>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).As<ISettingsProvider>();
            builder.RegisterType<QBitNinjaHelper>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).As<IBlockchainExplorerHelper>();

            container = builder.Build();
        }
    }
}

using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DInjection
{
    public class AutofacModule :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntityDataManager>().As<IDataManger>().SingleInstance();
        }


    }
}

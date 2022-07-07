using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace System.ComponentModel.Design
{
    public static class ServiceContainerExtension
    {
        public static void AddOrReplaceService(this ServiceContainer container, Type serviceType, object serviceInstance)
        {
            if(container.GetService(serviceType) != null)
            {
                container.RemoveService(serviceType);
            }
            container.AddService(serviceType, serviceInstance);
        }
    }
}

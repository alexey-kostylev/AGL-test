using AGL.App.Adapters;
using AGL.App.Logic;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;

namespace AGL.App.Unity
{
    public static class UnityExtension
    {
        public static void WireupAglDependencies(this IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            
            container.RegisterType<IPeopleAdapter, AzurePeopleAdapter>();
            container.RegisterType<IPetsLogic, PetsLogic>();            
            container.RegisterType<IRestClient>(new InjectionFactory(c => 
                new RestClient(ConfigurationManager.AppSettings["peopleUrl"])));

            container.RegisterType<IPetsController, PetsController>();
        }
    }
}

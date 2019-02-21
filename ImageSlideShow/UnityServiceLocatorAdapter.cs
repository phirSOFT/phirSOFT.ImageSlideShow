using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace phirSOFT.ImageSlideShow
{
    class UnityServiceLocatorAdapter : CommonServiceLocator.ServiceLocatorImplBase
    {
        private readonly IUnityContainer _unityContainer;

        /// <summary>
        /// Initializes a new instance of <see cref="UnityServiceLocatorAdapter"/>.
        /// </summary>
        /// <param name="unityContainer">The <see cref="IUnityContainer"/> that will be used
        /// by the <see cref="DoGetInstance"/> and <see cref="DoGetAllInstances"/> methods.</param>
        public UnityServiceLocatorAdapter(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        /// <summary>
        /// Resolves the instance of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>The requested service instance.</returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return _unityContainer.Resolve(serviceType, key);
        }

        /// <summary>
        /// Resolves all the instances of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Sequence of service instance objects.</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _unityContainer.ResolveAll(serviceType);
        }
    }
}

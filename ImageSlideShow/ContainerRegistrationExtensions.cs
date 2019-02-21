using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using phirSOFT.ImageSlideShow.Services;
using Prism.Ioc;

namespace phirSOFT.ImageSlideShow
{
    internal static class IContainerRegistryExtensions
    {
        public static void RegisterContentProvider<T>(this IContainerRegistry containerRegistry, params string[] ignoreSchemas) where T: IContentProvider
        {
            foreach (var attribute in typeof(T).GetCustomAttributes<SupportedSchemeAttribute>())
            {
                
            }
        }
    }
}

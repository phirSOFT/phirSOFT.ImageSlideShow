﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using phirSOFT.ImageSlideShow.Attributes;
using phirSOFT.ImageSlideShow.Controls;
using phirSOFT.ImageSlideShow.Services;
using Prism.Ioc;

namespace phirSOFT.ImageSlideShow
{
    internal static class IContainerRegistryExtensions
    {
        public static void RegisterContentProvider<T>(this IContainerRegistry containerRegistry, params string[] ignoreSchemas) where T: IContentProvider
        {
            var supportedSchemas = typeof(T).GetCustomAttributes<SupportedSchemeAttribute>().Select(s => s.SupportedSchema).Except(ignoreSchemas);

            if(!supportedSchemas.Any())
                return;

          
            foreach (var schema in supportedSchemas)
            {
                containerRegistry.RegisterSingleton<IContentProvider, T>(schema);
            }
            
        }

        public static void RegisterContentSourceEditor<T>(this IContainerRegistry container, params string[] ignoreSchemas)
        {
            
        }

        public static void RegisterContainerSourceEditorAdapter<T, V>(this IContainerRegistry containerRegistry, params string[] ignoreSchemas)
            where T : ISourceEditorAdapter<V>
        {
            var supportedSchemas = typeof(T).GetCustomAttributes<SupportedSchemeAttribute>().Select(s => s.SupportedSchema).Except(ignoreSchemas);

            if (!supportedSchemas.Any())
                return;


            foreach (var schema in supportedSchemas)
            {
                containerRegistry.RegisterSingleton<ISourceEditorAdapter<V>, T>(schema);
            }

        }
    }
}

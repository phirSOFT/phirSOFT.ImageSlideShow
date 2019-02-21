using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Prism.Ioc;
using System.Resources;

namespace phirSOFT.ImageSlideShow.Services
{
    public class SourceEditorService
    {
        private readonly Dictionary<string, (Type, editorTitle)> _mappings;
        private readonly IContainerExtension _container;
        public SourceEditorService(IContainerExtension container)
        {
            _container = container;
        }

        void RegisterEditor(string schema, Type editorType, Func<string> editorTitle)
        {
            if(!(typeof(Control).IsAssignableFrom(editorType)))
                throw new ArgumentException("editorType has to be a control");

            _mappings.Add(schema, editorType);

        }

        public static Func<string> ResourceLookuo(string key, ResourceManager resource)
        {
            return () => resource.GetString(key);
        }

        SourceEditorContext GetSourceEditorControl(string schema)
        {
            return (Control) _container.Resolve(_mappings[schema]);
        }
    }
}

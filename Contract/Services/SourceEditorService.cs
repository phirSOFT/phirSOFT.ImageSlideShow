using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Prism.Ioc;
using System.Resources;
using phirSOFT.ImageSlideShow.Controls;

namespace phirSOFT.ImageSlideShow.Services
{
    public class SourceEditorService
    {
        private readonly Dictionary<string, (Type, Func<string> editorTitle)> _mappings;
        private readonly IContainerExtension _container;
        public SourceEditorService(IContainerExtension container)
        {
            _container = container;
        }

        void RegisterEditor(string schema, Type editorType, Func<string> editorTitle)
        {
            if (!(typeof(Control).IsAssignableFrom(editorType)))
                throw new ArgumentException("editorType has to be a control");

            _mappings.Add(schema, (editorType, editorTitle));

        }

        public static Func<string> ResourceLookuo(string key, ResourceManager resource)
        {
            return () => resource.GetString(key);
        }

        SourceEditorContext GetSourceEditorControl(string schema)
        {
            if (_mappings.TryGetValue(schema, out var mapping))
                return null;
            var context = new SourceEditorContext()
            {
                EditorView = (Control) _container.Resolve(mapping.Item1),
                Title = mapping.editorTitle()
            };

            if(context.EditorView is ISourceEditor editor)
            {
                context.Editor = editor;
            }
            else if(context.EditorView.DataContext is ISourceEditor editorVm)
            {
                context.Editor = editorVm;
            }
            else
            {
                context.Editor = ((ISourceEditorAdapter)_container.Resolve(typeof(ISourceEditorAdapter<>).MakeGenericType(mapping.Item1), schema)).Adapt(context.EditorView);
            }
            return context;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class SourceEditorAttribute : Attribute
    {

        public SourceEditorAttribute(Type editorControl, params string[] supportedSchemas)
        {
            EditorControl = editorControl;
            SupportedSchemas = supportedSchemas;
        }

        public Type EditorControl { get; }
        public string[] SupportedSchemas { get; }
    }

}
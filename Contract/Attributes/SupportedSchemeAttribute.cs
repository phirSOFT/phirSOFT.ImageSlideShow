using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
    sealed class SupportedSchemeAttribute : Attribute
    {
        public SupportedSchemeAttribute(string scheme)
        {
            SupportedSchema = scheme;
        }

        public string SupportedSchema
        {
            get;
        }
    }
}

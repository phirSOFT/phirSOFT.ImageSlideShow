using Analyzer.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class AddSupportedSchemeAttribute : AddAttributeFix
    {
        private protected AddSupportedSchemeAttribute() : base("phirSOFT.ImageSlideShow.Attributes.SupportedSchemeAttribute", "PS0002", "PS0003")
        {
        }
    }
}

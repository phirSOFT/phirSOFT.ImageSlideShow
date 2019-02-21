using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace phirSOFT.ImageSlideShow.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ContentProviderAnalyzer : RequiredAttributeAnalyzer
    {
        public ContentProviderAnalyzer() : base("PS0002", "phirSOFT.ImageSlideShow.Services.IContentProvider", "phirSOFT.ImageSlideShow.Attributes.SupportedSchemeAttribute", true, false)
        {
        }
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UriParserAnalyzer : RequiredAttributeAnalyzer
    {
        public UriParserAnalyzer() : base("PS0003", "phirSOFT.ImageSlideShow.UriParser.UriParserBase`1", "phirSOFT.ImageSlideShow.Attributes.SupportedSchemeAttribute", true, false)
        {
        }
    }
}

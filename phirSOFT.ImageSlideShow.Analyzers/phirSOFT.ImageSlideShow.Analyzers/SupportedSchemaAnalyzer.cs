using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace phirSOFT.ImageSlideShow.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SupportedSchemaAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PS0001";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.InvalidSchemeTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.InvalidSchemeMessage), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.InvalidSchemeDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }
        public override void Initialize(AnalysisContext analysisContext)
        {
            analysisContext.EnableConcurrentExecution();
            analysisContext.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            analysisContext.RegisterCompilationStartAction(compilationContext =>
            {
                var supportedSchemeAttributeType = compilationContext.Compilation.GetTypeByMetadataName("phirSOFT.ImageSlideShow.Attributes.SupportedSchemeAttribute");

                if (supportedSchemeAttributeType == null)
                    return;


                compilationContext.RegisterSymbolAction(sc => AnalyzeSymbol(sc, supportedSchemeAttributeType),
                    SymbolKind.NamedType,
                    SymbolKind.Method,
                    SymbolKind.Field,
                    SymbolKind.Property,
                    SymbolKind.Event);
            });
        }
        private void AnalyzeSymbol(SymbolAnalysisContext context, INamedTypeSymbol supportedSchemeAttributeType)
        {
            ImmutableArray<AttributeData> attributes = context.Symbol.GetAttributes();
            foreach (AttributeData attribute in attributes)
            {
                if (attribute.AttributeClass.Equals(supportedSchemeAttributeType))
                {
                    if (!Uri.CheckSchemeName(attribute.ConstructorArguments.First().Value as string))
                    {
                        AttributeSyntax node = (AttributeSyntax)attribute.ApplicationSyntaxReference.GetSyntax();

                        var schemaSyntax = node.ArgumentList.Arguments[0];
                        var d = Diagnostic.Create(Rule, schemaSyntax.GetLocation(), schemaSyntax.ToString());
                        context.ReportDiagnostic(d);
                    }
                }
            }
        }

    }
}

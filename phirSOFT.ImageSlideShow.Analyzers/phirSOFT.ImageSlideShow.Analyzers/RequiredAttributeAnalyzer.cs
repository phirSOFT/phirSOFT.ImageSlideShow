using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace phirSOFT.ImageSlideShow.Analyzers
{
    public abstract class RequiredAttributeAnalyzer : DiagnosticAnalyzer
    {
        private readonly string typeName;
        private readonly string attributeName;
        private readonly bool checkParents;
        private readonly bool requiredOnAbstracts;

        private protected RequiredAttributeAnalyzer(string diagonsticID, string typeName, string attributeName, bool checkParents, bool requiredOnAbstracts)
        {
            LocalizableString Title = new LocalizableResourceString(nameof(Resources.MissingAttributeTitle), Resources.ResourceManager, typeof(Resources), attributeName);
            LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.MissingAttributeMessage), Resources.ResourceManager, typeof(Resources));
            LocalizableString Description = new LocalizableResourceString(nameof(Resources.MissingAttributeDescription), Resources.ResourceManager, typeof(Resources));

            Rule = new DiagnosticDescriptor(diagonsticID, Title, MessageFormat, "Design", DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
            this.typeName = typeName;
            this.attributeName = attributeName;
            this.checkParents = checkParents;
            this.requiredOnAbstracts = requiredOnAbstracts;
        }

        private DiagnosticDescriptor Rule { get; }
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
        public override void Initialize(AnalysisContext analysisContext)
        {
            analysisContext.EnableConcurrentExecution();
            analysisContext.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            analysisContext.RegisterCompilationStartAction(compilationContext =>
            {
                var attributeType = compilationContext.Compilation.GetTypeByMetadataName(attributeName);
                var baseType = compilationContext.Compilation.GetTypeByMetadataName(typeName);
                if (attributeType == null)
                    return;


                compilationContext.RegisterSymbolAction(sc => AnalyzeSymbol(sc, attributeType, baseType),
                    SymbolKind.NamedType);
            });
        }
        private void AnalyzeSymbol(SymbolAnalysisContext context, INamedTypeSymbol supportedSchemeAttributeType, INamedTypeSymbol iContentProviderType)
        {
            var namedType = (INamedTypeSymbol)context.Symbol;
            if (!(namedType.AllInterfaces.Contains(iContentProviderType) || namedType.InheritsFrom(iContentProviderType)))
                return;

            if (!(namedType.IsAbstract && !requiredOnAbstracts) && !FindSupportedScheme(namedType, supportedSchemeAttributeType, iContentProviderType))
            {
                namedType.CreateDiagnostic(Rule);
            }
        }

        private bool FindSupportedScheme(INamedTypeSymbol namedType, INamedTypeSymbol supportedSchemeAttributeType, INamedTypeSymbol iContentProviderType)
        {
            if (namedType.GetAttributes().Any(a => a.AttributeClass == supportedSchemeAttributeType))
                return true;

            var baseType = namedType.BaseType;

            if (!baseType.AllInterfaces.Contains(iContentProviderType))
                return false;

            return checkParents && FindSupportedScheme(baseType, supportedSchemeAttributeType, iContentProviderType);
        }
    }

}

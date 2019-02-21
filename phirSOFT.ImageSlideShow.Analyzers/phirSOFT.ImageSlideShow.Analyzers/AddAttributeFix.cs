using Analyzer.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace phirSOFT.ImageSlideShow.Analyzers
{
    public class AddAttributeFix : CodeFixProvider
    {
        private protected AddAttributeFix(string attributeName, params string[] appliableDiagnosticIds)
        {
            FixableDiagnosticIds = ImmutableArray.Create(appliableDiagnosticIds);
            AttributeName = attributeName;
        }

        public override ImmutableArray<string> FixableDiagnosticIds {get; }
        public string AttributeName { get; }

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SemanticModel model = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

            INamedTypeSymbol flagsAttributeType = model.Compilation.GetTypeByMetadataName(AttributeName);
            if (flagsAttributeType == null)
            {
                return;
            }

            // We cannot have multiple overlapping diagnostics of this id.
            Diagnostic diagnostic = context.Diagnostics.Single();

            context.RegisterCodeFix(new AddAttributeAction(Resources.MissingAttributeFixTitle,
                                         async ct => await AddOrRemoveFlagsAttribute(context.Document, context.Span, flagsAttributeType, ct).ConfigureAwait(false),
                                         equivalenceKey: Resources.MissingAttributeFixTitle),
                        diagnostic);
        }


        private static async Task<Document> AddOrRemoveFlagsAttribute(Document document, TextSpan span, INamedTypeSymbol flagsAttributeType, CancellationToken cancellationToken)
        {
            DocumentEditor editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            SyntaxNode node = root.FindNode(span);

            SyntaxNode newEnumBlockSyntax = AddFlagsAttribute(editor.Generator, node, flagsAttributeType);

            editor.ReplaceNode(node, newEnumBlockSyntax);
            return editor.GetChangedDocument();
        }

        private static SyntaxNode AddFlagsAttribute(SyntaxGenerator generator, SyntaxNode enumTypeSyntax, INamedTypeSymbol flagsAttributeType)
        {
            return generator.AddAttributes(enumTypeSyntax, generator.Attribute(generator.TypeExpression(flagsAttributeType)));
        }

        public override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        private class AddAttributeAction : DocumentChangeAction
        {
            public AddAttributeAction(string title, Func<CancellationToken, Task<Document>> createChangedDocument, string equivalenceKey)
                : base(title, createChangedDocument, equivalenceKey)
            {
            }

        }
    }
}

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ecoCodeDotnet.Checks;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NoFunctionCallWhenDeclaringForLoop : DiagnosticAnalyzer
{
    private const string Category = "Usage";

    public const string DiagnosticId = "S06001";
    public const string Message = "Do not call a function when declaring a for-type loop";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Message,
        Message,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Calling function when declaring a for-type loop is bad for performance. You should store the result of the function and use it.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get { return ImmutableArray.Create(Rule); }
    }

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ForStatement);
    }

    private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        var loopStatement = (ForStatementSyntax)context.Node;

        // If the Condition is type InvocationExpression, a function is call when declaring a for-type loop
        if (IsConditionInvocationExpression(loopStatement.Condition))
        {
            // Report a Diagnostic
            var diagnostic = Diagnostic.Create(Rule, loopStatement.Condition.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static bool IsConditionInvocationExpression(ExpressionSyntax expressionSyntax)
    {

        if (expressionSyntax is BinaryExpressionSyntax binaryExpressionSyntax)
        {
            if (IsConditionInvocationExpression(binaryExpressionSyntax.Left))
            {
                return true;
            }

            if (IsConditionInvocationExpression(binaryExpressionSyntax.Right))
            {
                return true;
            }

            return false;
        }

        return expressionSyntax.IsKind(SyntaxKind.InvocationExpression);
    }
}


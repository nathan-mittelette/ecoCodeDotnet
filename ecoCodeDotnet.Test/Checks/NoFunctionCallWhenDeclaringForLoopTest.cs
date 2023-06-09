using System;
using Dena.CodeAnalysis.CSharp.Testing;
using System.Threading.Tasks;
using NUnit.Framework;
using ecoCodeDotnet.Checks;
using Assert = NUnit.Framework.Assert;
using Microsoft.CodeAnalysis.Text;
using System.Linq;

namespace ecoCodeDotnet.Test.Checks;

[TestFixture]
public class NoFunctionCallWhenDeclaringForLoopTest
{
    /// <summary>
    /// Test analyze for empty source code
    /// </summary>
    [Test]
    public async Task EmptySourceCode_NoDiagnosticReport()
    {
        const string Source = "";
        var analyzer = new NoFunctionCallWhenDeclaringForLoop();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, Source);

        Assert.That(diagnostics, Is.Empty);
    }


    /// <summary>
    /// Test analyze for containing function call in for-type loop in code NoFunctionCallWhenDeclaringForLoop.cs
    /// </summary>
    [Test]
    public async Task NoFunctionCallWhenDeclaringForLoopTest_ReportOneDiagnostic()
    {
        var source = ChecksTestUtils.ReadCodes("NoFunctionCallWhenDeclaringForLoop.cs");
        var analyzer = new NoFunctionCallWhenDeclaringForLoop();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

        var actual = diagnostics
            .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
            .Where(x => x.Id != "CS8019") // Ignore "Unnecessary using directive"
            .ToArray();

        Assert.That(actual, Has.Length.EqualTo(1));
        Assert.That(actual.First().Id, Is.EqualTo(NoFunctionCallWhenDeclaringForLoop.DiagnosticId));
        Assert.That(actual.First().GetMessage(), Is.EqualTo(NoFunctionCallWhenDeclaringForLoop.Message));

        LocationAssert.HaveTheSpan(
            new LinePosition(22, 24),
            new LinePosition(22, 37),
            actual.First().Location
        );
    }
}


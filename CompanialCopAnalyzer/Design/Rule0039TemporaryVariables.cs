using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0039TemporaryRecordsShouldNotTriggerTableTriggers : DiagnosticAnalyzer
    {
        private static readonly HashSet<string> methodsToCheck = new HashSet<string> { "Insert", "Modify", "Delete", "DeleteAll", "Validate" , "ModifyAll" };
        private static readonly string validateMethod = "Validate";
        private static readonly string modifyAllMethod = "ModifyAll";
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0039TemporaryRecordsShouldNotTriggerTableTriggers);

        public override void Initialize(AnalysisContext context) => context.RegisterOperationAction(new Action<OperationAnalysisContext>(this.AnalyzeTemporaryRecords), OperationKind.InvocationExpression);

        private void AnalyzeTemporaryRecords(OperationAnalysisContext ctx)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol.GetContainingObjectTypeSymbol())) return;

            var invocationExpression = (IInvocationExpression)ctx.Operation;

            if (!methodsToCheck.Contains(invocationExpression.TargetMethod.Name))
                return;

            IOperation? invokingRecord = invocationExpression.Instance;

            if (invokingRecord == null || !(invokingRecord.Type is IRecordTypeSymbol record) || !record.Temporary)
                return;

            if (invocationExpression.TargetMethod.Name != validateMethod && invocationExpression.TargetMethod.Name != modifyAllMethod && !CheckArgumentValue(invocationExpression.Arguments[0]))
                return;

            if (invocationExpression.TargetMethod.Name == modifyAllMethod && !CheckArgumentValue(invocationExpression.Arguments[2]))
                return;

            ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0039TemporaryRecordsShouldNotTriggerTableTriggers, ctx.Operation.Syntax.GetLocation()));
        }

        private bool CheckArgumentValue(IArgument argument)
        {
            if (!argument.Value.ConstantValue.HasValue || argument.Value.ConstantValue.Value is not bool b || !b)
                return false;

            return true;
        }
    }
}
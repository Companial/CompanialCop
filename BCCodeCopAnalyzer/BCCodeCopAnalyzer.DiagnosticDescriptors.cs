using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCCodeCopAnalyzer
{
    internal class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule001VariableNameShouldNotContainWhiteSpace = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0001",
            BCCodeCopAnalyzer.Rule0001VariableNameShouldNotContainWhiteSpaceTitle,
            BCCodeCopAnalyzer.Rule0001VariableNameShouldNotContainWhiteSpaceFormat,
            "Naming",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0001VariableNameShouldNotContainWhiteSpaceDescription,
            null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0002VariableShouldNotContainWildcardSymbols = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0002",
            BCCodeCopAnalyzer.Rule0002VariableShouldNotContainWildcardSymbolsTitle,
            BCCodeCopAnalyzer.Rule0002VariableShouldNotContainWildcardSymbolsFormat,
            "Naming",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0002VariableShouldNotContainWildcardSymbolsDescription,
            null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0003MethodsNameShouldNotContainWhiteSpace = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0003",
            BCCodeCopAnalyzer.Rule0003MethodsNameShouldNotContainWhiteSpaceTitle,
            BCCodeCopAnalyzer.Rule0003MethodsNameShouldNotContainWhiteSpaceFormat,
            "Naming",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0003MethodsNameShouldNotContainWhiteSpaceDescription,
            null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0009First19TableFieldsIDReservedToPrimaryKey = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0009",
            BCCodeCopAnalyzer.Rule0009First19TableFieldsIDReservedToPrimaryKeyTitle,
            BCCodeCopAnalyzer.Rule0009First19TableFieldsIDReservedToPrimaryKeyFormat,
            "Numbering",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0009First19TableFieldsIDReservedToPrimaryKeyDescription,
            (string)null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0011EnumExtensionsValuesNumberedInDedicatedRange = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0011",
            BCCodeCopAnalyzer.Rule0011EnumExtensionsValuesNumberedInDedicatedRangeTitle,
            BCCodeCopAnalyzer.Rule0011EnumExtensionsValuesNumberedInDedicatedRangeFormat,
            "Numbering",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0011EnumExtensionsValuesNumberedInDedicatedRangeDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0014HardcodedIpAddress = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0014",
            BCCodeCopAnalyzer.Rule0014HardcodedIpAddressTitle,
            BCCodeCopAnalyzer.Rule0014HardcodedIpAddressFormat,
            "Security",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0014HardcodedIpAddressDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0015FlowFieldsShouldNotBeEditable = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0015",
            BCCodeCopAnalyzer.Rule0015FlowFieldsShouldNotBeEditableTitle,
            BCCodeCopAnalyzer.Rule0015FlowFieldsShouldNotBeEditableFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0015FlowFieldsShouldNotBeEditableDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0016CommitMustBeExplainedByComment = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0016",
            BCCodeCopAnalyzer.Rule0016CommitMustBeExplainedByCommentTitle,
            BCCodeCopAnalyzer.Rule0016CommitMustBeExplainedByCommentFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0016CommitMustBeExplainedByCommentDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0017DoNotUseObjectIdInSystemFunctions = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0017",
            BCCodeCopAnalyzer.Rule0017DoNotUseObjectIdInSystemFunctionsTitle,
            BCCodeCopAnalyzer.Rule0017DoNotUseObjectIdInSystemFunctionsFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0017DoNotUseObjectIdInSystemFunctionsDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0018CheckForMissingCaptions = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0018",
            BCCodeCopAnalyzer.Rule0018CheckForMissingCaptionsTitle,
            BCCodeCopAnalyzer.Rule0018CheckForMissingCaptionsFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0018CheckForMissingCaptionsDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0019SemicolonAfterProcedureDeclaration = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0019",
            BCCodeCopAnalyzer.Rule0019SemicolonAfterProcedureDeclarationTitle,
            BCCodeCopAnalyzer.Rule0019SemicolonAfterProcedureDeclarationFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0019SemicolonAfterProcedureDeclarationDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0020InternalProcedures = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0020",
            BCCodeCopAnalyzer.Rule0020InternalProceduresTitle,
            BCCodeCopAnalyzer.Rule0020InternalProceduresFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0020InternalProceduresDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0021ToolTipPunctuation = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0021",
            BCCodeCopAnalyzer.Rule0021ToolTipPunctuationTitle,
            BCCodeCopAnalyzer.Rule0021ToolTipPunctuationFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0021ToolTipPunctuationDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0022GlobalVarTriggerAndMethodPosition = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0022",
            BCCodeCopAnalyzer.Rule0022GlobalVarTriggerAndMethodPositionTitle,
            BCCodeCopAnalyzer.Rule0022GlobalVarTriggerAndMethodPositionFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0022GlobalVarTriggerAndMethodPositionDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0023LabelPunctuation = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0023",
            BCCodeCopAnalyzer.Rule0023LabelPunctuationTitle,
            BCCodeCopAnalyzer.Rule0023LabelPunctuationFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0023LabelPunctuationDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0024InternalMethodsMustHaveExplicitParameters = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0024",
            BCCodeCopAnalyzer.Rule0024InternalMethodsMustHaveExplicitParametersTitle,
            BCCodeCopAnalyzer.Rule0024InternalMethodsMustHaveExplicitParametersFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0024InternalMethodsMustHaveExplicitParametersDescription,
            (string)null,
            Array.Empty<string>()
        );
    }
}

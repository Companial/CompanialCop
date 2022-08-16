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

        public static readonly DiagnosticDescriptor Rule0004First19TableFieldsIDReservedToPrimaryKey = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0004",
            BCCodeCopAnalyzer.Rule0004First19TableFieldsIDReservedToPrimaryKeyTitle,
            BCCodeCopAnalyzer.Rule0004First19TableFieldsIDReservedToPrimaryKeyFormat,
            "Numbering",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0004First19TableFieldsIDReservedToPrimaryKeyDescription,
            (string)null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0005EnumExtensionsValuesNumberedInDedicatedRange = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0005",
            BCCodeCopAnalyzer.Rule0005EnumExtensionsValuesNumberedInDedicatedRangeTitle,
            BCCodeCopAnalyzer.Rule0005EnumExtensionsValuesNumberedInDedicatedRangeFormat,
            "Numbering",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0005EnumExtensionsValuesNumberedInDedicatedRangeDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0006HardcodedIpAddress = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0006",
            BCCodeCopAnalyzer.Rule0006HardcodedIpAddressTitle,
            BCCodeCopAnalyzer.Rule0006HardcodedIpAddressFormat,
            "Security",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0006HardcodedIpAddressDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0007FlowFieldsShouldNotBeEditable = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0007",
            BCCodeCopAnalyzer.Rule0007FlowFieldsShouldNotBeEditableTitle,
            BCCodeCopAnalyzer.Rule0007FlowFieldsShouldNotBeEditableFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0007FlowFieldsShouldNotBeEditableDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0008CommitMustBeExplainedByComment = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0008",
            BCCodeCopAnalyzer.Rule0008CommitMustBeExplainedByCommentTitle,
            BCCodeCopAnalyzer.Rule0008CommitMustBeExplainedByCommentFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0008CommitMustBeExplainedByCommentDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0009DoNotUseObjectIdInSystemFunctions = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0009",
            BCCodeCopAnalyzer.Rule0009DoNotUseObjectIdInSystemFunctionsTitle,
            BCCodeCopAnalyzer.Rule0009DoNotUseObjectIdInSystemFunctionsFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0009DoNotUseObjectIdInSystemFunctionsDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0010CheckForMissingCaptions = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0010",
            BCCodeCopAnalyzer.Rule0010CheckForMissingCaptionsTitle,
            BCCodeCopAnalyzer.Rule0010CheckForMissingCaptionsFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0010CheckForMissingCaptionsDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0011SemicolonAfterProcedureDeclaration = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0011",
            BCCodeCopAnalyzer.Rule0011SemicolonAfterProcedureDeclarationTitle,
            BCCodeCopAnalyzer.Rule0011SemicolonAfterProcedureDeclarationFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0011SemicolonAfterProcedureDeclarationDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0012InternalProcedures = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0012",
            BCCodeCopAnalyzer.Rule0012InternalProceduresTitle,
            BCCodeCopAnalyzer.Rule0012InternalProceduresFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0012InternalProceduresDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0013ToolTipPunctuation = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0013",
            BCCodeCopAnalyzer.Rule0013ToolTipPunctuationTitle,
            BCCodeCopAnalyzer.Rule0013ToolTipPunctuationFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0013ToolTipPunctuationDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0014GlobalVarTriggerAndMethodPosition = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0014",
            BCCodeCopAnalyzer.Rule0014GlobalVarTriggerAndMethodPositionTitle,
            BCCodeCopAnalyzer.Rule0014GlobalVarTriggerAndMethodPositionFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0014GlobalVarTriggerAndMethodPositionDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0015LabelPunctuation = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0015",
            BCCodeCopAnalyzer.Rule0015LabelPunctuationTitle,
            BCCodeCopAnalyzer.Rule0015LabelPunctuationFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0015LabelPunctuationDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0016InternalMethodsMustHaveExplicitParameters = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0016",
            BCCodeCopAnalyzer.Rule0016InternalMethodsMustHaveExplicitParametersTitle,
            BCCodeCopAnalyzer.Rule0016InternalMethodsMustHaveExplicitParametersFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0016InternalMethodsMustHaveExplicitParametersDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0017EmptyObjectSections = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0017",
            BCCodeCopAnalyzer.Rule0017EmptyObjectSectionsTitle,
            BCCodeCopAnalyzer.Rule0017EmptyObjectSectionsFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0017EmptyObjectSectionsDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0018ObjectAccessAndExtensibleProperty = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0018",
            BCCodeCopAnalyzer.Rule0018ObjectAccessAndExtensiblePropertyTitle,
            BCCodeCopAnalyzer.Rule0018ObjectAccessAndExtensiblePropertyFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0018ObjectAccessAndExtensiblePropertyDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0019LockedVariableLables = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0019",
            BCCodeCopAnalyzer.Rule0019LockedVariableLablesTitle,
            BCCodeCopAnalyzer.Rule0019LockedVariableLablesFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0019LockedVariableLablesDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0019LockedVariableTokLables = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0019",
            BCCodeCopAnalyzer.Rule0019LockedVariableTokLablesTitle,
            BCCodeCopAnalyzer.Rule0019LockedVariableTokLablesFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0019LockedVariableTokLablesDescription,
            (string)null,
            Array.Empty<string>()
        );
    }
}

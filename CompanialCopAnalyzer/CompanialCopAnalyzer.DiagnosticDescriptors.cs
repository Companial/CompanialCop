using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanialCopAnalyzer
{
    internal class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0001KeyNaming = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0001",
            CompanialCopAnalyzer.Rule0001KeyNamingTitle,
            CompanialCopAnalyzer.Rule0001KeyNamingFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0001KeyNamingDescription,
            (string)null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0002LockedVariableLables = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0002",
            CompanialCopAnalyzer.Rule0002LockedVariableLablesTitle,
            CompanialCopAnalyzer.Rule0002LockedVariableLablesFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0002LockedVariableLablesDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0002LockedVariableTokLables = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0002",
            CompanialCopAnalyzer.Rule0002LockedVariableTokLablesTitle,
            CompanialCopAnalyzer.Rule0002LockedVariableTokLablesFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0002LockedVariableTokLablesDescription,
            (string)null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0003MethodsNameShouldNotContainWhiteSpace = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0003",
            CompanialCopAnalyzer.Rule0003MethodsNameShouldNotContainWhiteSpaceTitle,
            CompanialCopAnalyzer.Rule0003MethodsNameShouldNotContainWhiteSpaceFormat,
            "Naming",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0003MethodsNameShouldNotContainWhiteSpaceDescription,
            null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0004First19TableFieldsIDReservedToPrimaryKey = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0004",
            CompanialCopAnalyzer.Rule0004First19TableFieldsIDReservedToPrimaryKeyTitle,
            CompanialCopAnalyzer.Rule0004First19TableFieldsIDReservedToPrimaryKeyFormat,
            "Numbering",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0004First19TableFieldsIDReservedToPrimaryKeyDescription,
            (string)null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0005EnumExtensionsValuesNumberedInDedicatedRange = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0005",
            CompanialCopAnalyzer.Rule0005EnumExtensionsValuesNumberedInDedicatedRangeTitle,
            CompanialCopAnalyzer.Rule0005EnumExtensionsValuesNumberedInDedicatedRangeFormat,
            "Numbering",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0005EnumExtensionsValuesNumberedInDedicatedRangeDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0006HardcodedIpAddress = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0006",
            CompanialCopAnalyzer.Rule0006HardcodedIpAddressTitle,
            CompanialCopAnalyzer.Rule0006HardcodedIpAddressFormat,
            "Security",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0006HardcodedIpAddressDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0007FlowFieldsShouldNotBeEditable = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0007",
            CompanialCopAnalyzer.Rule0007FlowFieldsShouldNotBeEditableTitle,
            CompanialCopAnalyzer.Rule0007FlowFieldsShouldNotBeEditableFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0007FlowFieldsShouldNotBeEditableDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0008CommitMustBeExplainedByComment = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0008",
            CompanialCopAnalyzer.Rule0008CommitMustBeExplainedByCommentTitle,
            CompanialCopAnalyzer.Rule0008CommitMustBeExplainedByCommentFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0008CommitMustBeExplainedByCommentDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0009DoNotUseObjectIdInSystemFunctions = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0009",
            CompanialCopAnalyzer.Rule0009DoNotUseObjectIdInSystemFunctionsTitle,
            CompanialCopAnalyzer.Rule0009DoNotUseObjectIdInSystemFunctionsFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0009DoNotUseObjectIdInSystemFunctionsDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0010CheckForMissingCaptions = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0010",
            CompanialCopAnalyzer.Rule0010CheckForMissingCaptionsTitle,
            CompanialCopAnalyzer.Rule0010CheckForMissingCaptionsFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0010CheckForMissingCaptionsDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0011SemicolonAfterProcedureDeclaration = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0011",
            CompanialCopAnalyzer.Rule0011SemicolonAfterProcedureDeclarationTitle,
            CompanialCopAnalyzer.Rule0011SemicolonAfterProcedureDeclarationFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0011SemicolonAfterProcedureDeclarationDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0012InternalProcedures = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0012",
            CompanialCopAnalyzer.Rule0012InternalProceduresTitle,
            CompanialCopAnalyzer.Rule0012InternalProceduresFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0012InternalProceduresDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0013ToolTipPunctuation = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0013",
            CompanialCopAnalyzer.Rule0013ToolTipPunctuationTitle,
            CompanialCopAnalyzer.Rule0013ToolTipPunctuationFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0013ToolTipPunctuationDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0014GlobalVarTriggerAndMethodPosition = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0014",
            CompanialCopAnalyzer.Rule0014GlobalVarTriggerAndMethodPositionTitle,
            CompanialCopAnalyzer.Rule0014GlobalVarTriggerAndMethodPositionFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0014GlobalVarTriggerAndMethodPositionDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0015LabelPunctuation = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0015",
            CompanialCopAnalyzer.Rule0015LabelPunctuationTitle,
            CompanialCopAnalyzer.Rule0015LabelPunctuationFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0015LabelPunctuationDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0016InternalMethodsMustHaveExplicitParameters = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0016",
            CompanialCopAnalyzer.Rule0016InternalMethodsMustHaveExplicitParametersTitle,
            CompanialCopAnalyzer.Rule0016InternalMethodsMustHaveExplicitParametersFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0016InternalMethodsMustHaveExplicitParametersDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0017EmptyObjectSections = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0017",
            CompanialCopAnalyzer.Rule0017EmptyObjectSectionsTitle,
            CompanialCopAnalyzer.Rule0017EmptyObjectSectionsFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0017EmptyObjectSectionsDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0018ObjectExtensibleProperty = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0018",
            CompanialCopAnalyzer.Rule0018ObjectExtensiblePropertyTitle,
            CompanialCopAnalyzer.Rule0018ObjectExtensiblePropertyFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0018ObjectExtensiblePropertyDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0018ObjectAccessProperty = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0018",
            CompanialCopAnalyzer.Rule0018ObjectAccessPropertyTitle,
            CompanialCopAnalyzer.Rule0018ObjectAccessPropertyFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0018ObjectAccessPropertyDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0019LocalVariableNameShouldNotContainWhiteSpace = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0019",
            CompanialCopAnalyzer.Rule0019LocalVariableNameShouldNotContainWhiteSpaceTitle,
            CompanialCopAnalyzer.Rule0019LocalVariableNameShouldNotContainWhiteSpaceFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0019LocalVariableNameShouldNotContainWhiteSpaceDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0019LocalVariableNameShouldNotContainWildcardSymbols = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0019",
            CompanialCopAnalyzer.Rule0019LocalVariableNameShouldNotContainWildcardSymbolsTitle,
            CompanialCopAnalyzer.Rule0019LocalVariableNameShouldNotContainWildcardSymbolsFormat,
            "Naming",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0019LocalVariableNameShouldNotContainWildcardSymbolsDescription,
            null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0020GlobalVariableNameShouldNotContainWhiteSpace = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0020",
            CompanialCopAnalyzer.Rule0020GlobalVariableNameShouldNotContainWhiteSpaceTitle,
            CompanialCopAnalyzer.Rule0020GlobalVariableNameShouldNotContainWhiteSpaceFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0020GlobalVariableNameShouldNotContainWhiteSpaceDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0020GlobalVariableNameShouldNotContainWildcardSymbols = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0020",
            CompanialCopAnalyzer.Rule0020GlobalVariableNameShouldNotContainWildcardSymbolsTitle,
            CompanialCopAnalyzer.Rule0020GlobalVariableNameShouldNotContainWildcardSymbolsFormat,
            "Naming",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0020GlobalVariableNameShouldNotContainWildcardSymbolsDescription,
            null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0021ParameterNameShouldNotContainWhiteSpace = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0021",
            CompanialCopAnalyzer.Rule0021ParameterNameShouldNotContainWhiteSpaceTitle,
            CompanialCopAnalyzer.Rule0021ParameterNameShouldNotContainWhiteSpaceFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0021ParameterNameShouldNotContainWhiteSpaceDescription,
            (string)null,
            Array.Empty<string>()
        );
        public static readonly DiagnosticDescriptor Rule0021ParameterNameShouldNotContainWildcardSymbols = new DiagnosticDescriptor(
            CompanialCopAnalyzer.AnalyzerPrefix + "0021",
            CompanialCopAnalyzer.Rule0021ParameterNameShouldNotContainWildcardSymbolsTitle,
            CompanialCopAnalyzer.Rule0021ParameterNameShouldNotContainWildcardSymbolsFormat,
            "Naming",
            DiagnosticSeverity.Warning,
            true,
            CompanialCopAnalyzer.Rule0021ParameterNameShouldNotContainWildcardSymbolsDescription,
            null,
            Array.Empty<string>()
        );
    }
}
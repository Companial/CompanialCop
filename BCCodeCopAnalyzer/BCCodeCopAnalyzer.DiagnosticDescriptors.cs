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

        public static readonly DiagnosticDescriptor Rule0004GlobalVariablesMayBeAboveTriggersProcedures = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0004",
            BCCodeCopAnalyzer.Rule0004GlobalVariablesMayBeAboveTriggersProceduresTitle,
            BCCodeCopAnalyzer.Rule0004GlobalVariablesMayBeAboveTriggersProceduresFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0004GlobalVariablesMayBeAboveTriggersProceduresDescription,
            null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0005ObjectsMustHaveCaptionProperty = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0005",
            BCCodeCopAnalyzer.Rule0005ObjectsMustHaveCaptionPropertyTitle,
            BCCodeCopAnalyzer.Rule0005ObjectsMustHaveCaptionPropertyFormat,
            "Property",
            DiagnosticSeverity.Warning, 
            true,
            BCCodeCopAnalyzer.Rule0005ObjectsMustHaveCaptionPropertyDescription,
            (string)null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0006TableFieldsMustHaveCaptionProperty = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0006",
            BCCodeCopAnalyzer.Rule0006TableFieldsMustHaveCaptionPropertyTitle,
            BCCodeCopAnalyzer.Rule0006TableFieldsMustHaveCaptionPropertyFormat,
            "Property",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0006TableFieldsMustHaveCaptionPropertyDescription,
            (string)null, 
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0007EnumValuesMustHaveCaptionProperty = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0007",
            BCCodeCopAnalyzer.Rule0007EnumValuesMustHaveCaptionPropertyTitle,
            BCCodeCopAnalyzer.Rule0007EnumValuesMustHaveCaptionPropertyFormat,
            "Property",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0007EnumValuesMustHaveCaptionPropertyDescription,
            (string)null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0008PagePartsMustHaveCaptionProperty = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0008",
            BCCodeCopAnalyzer.Rule0008PagePartsMustHaveCaptionPropertyTitle,
            BCCodeCopAnalyzer.Rule0008PagePartsMustHaveCaptionPropertyFormat,
            "Property",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0008PagePartsMustHaveCaptionPropertyDescription,
            (string)null,
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

        public static readonly DiagnosticDescriptor Rule0012EmptyActionsSection = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0012",
            BCCodeCopAnalyzer.Rule0012EmptyActionsSectionTitle,
            BCCodeCopAnalyzer.Rule0012EmptyActionsSectionFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0012EmptyActionsSectionDescription,
            (string)null,
            Array.Empty<string>()
        );

        public static readonly DiagnosticDescriptor Rule0013EmptyOnRunTrigger = new DiagnosticDescriptor(
            BCCodeCopAnalyzer.AnalyzerPrefix + "0013",
            BCCodeCopAnalyzer.Rule0013EmptyOnRunTriggerTitle,
            BCCodeCopAnalyzer.Rule0013EmptyOnRunTriggerFormat,
            "Readability",
            DiagnosticSeverity.Warning,
            true,
            BCCodeCopAnalyzer.Rule0013EmptyOnRunTriggerDescription,
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
    }
}

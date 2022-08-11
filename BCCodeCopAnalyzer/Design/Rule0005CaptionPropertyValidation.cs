using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCCodeCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    internal class Rule0005CaptionPropertyValidation : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(
            DiagnosticDescriptors.Rule0005ObjectsMustHaveCaptionProperty,
            DiagnosticDescriptors.Rule0006TableFieldsMustHaveCaptionProperty,
            DiagnosticDescriptors.Rule0007EnumValuesMustHaveCaptionProperty,
            DiagnosticDescriptors.Rule0008PagePartsMustHaveCaptionProperty);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeObjectCaption), SyntaxKind.PageObject, SyntaxKind.TableObject, SyntaxKind.XmlPortObject, SyntaxKind.ReportObject, SyntaxKind.QueryObject);
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeTableFieldsCaption), SyntaxKind.Field);
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeEnumValuesCaption), SyntaxKind.EnumValue);
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzePagePartsCaption), SyntaxKind.PageAction, SyntaxKind.PagePart, SyntaxKind.PageGroup, SyntaxKind.PageActionGroup);
        }

        public static void AnalyzeObjectCaption(SyntaxNodeAnalysisContext context)
        {
            ObjectSyntax? syntax = context.Node as ObjectSyntax;
            AnalyzeObjectCaptions(context, syntax);
        }

        public static void AnalyzeObjectCaptions(SyntaxNodeAnalysisContext context, ObjectSyntax syntax)
        {
            if (syntax == null)
                return;

            PropertyListSyntax propertyListSyntax = syntax.PropertyList;
            IdentifierNameSyntax identifierNameSyntax = syntax.Name;
            PropertySyntax propertySyntax = GetProperty(propertyListSyntax.Properties, "Caption");

            if (propertySyntax == null)
            {
                ReportObjectsMustHaveCaptionProperty(context, identifierNameSyntax.GetLocation(), syntax.Name.ToString(), syntax.Kind, syntax.Name);
            }
        }

        public static void AnalyzeTableFieldsCaption(SyntaxNodeAnalysisContext context)
        {
            FieldSyntax? syntax = context.Node as FieldSyntax;
            AnalyzeTableFieldsCaptions(context, syntax);
        }

        public static void AnalyzeTableFieldsCaptions(SyntaxNodeAnalysisContext context, FieldSyntax syntax)
        {
            if (syntax == null)
                return;

            PropertyListSyntax propertyListSyntax = syntax.PropertyList;
            IdentifierNameSyntax identifierNameSyntax = syntax.Name;
            PropertySyntax propertySyntax = GetProperty(propertyListSyntax.Properties, "Caption");

            if (propertySyntax == null)
            {
                ReportTableFieldsMustHaveCaptionProperty(context, identifierNameSyntax.GetLocation(), syntax.Name.ToString(), syntax.Kind, syntax.Name);
            }
        }

        public static void AnalyzeEnumValuesCaption(SyntaxNodeAnalysisContext context)
        {
            EnumValueSyntax? syntax = context.Node as EnumValueSyntax;
            AnalyzeEnumValuesCaptions(context, syntax);
        }

        public static void AnalyzeEnumValuesCaptions(SyntaxNodeAnalysisContext context, EnumValueSyntax syntax)
        {
            if (syntax == null)
                return;

            PropertyListSyntax propertyListSyntax = syntax.PropertyList;
            IdentifierNameSyntax identifierNameSyntax = syntax.Name;
            PropertySyntax propertySyntax = GetProperty(propertyListSyntax.Properties, "Caption");

            if (propertySyntax == null)
            {
                ReportEnumValueMustHaveCaptionProperty(context, identifierNameSyntax.GetLocation(), syntax.Name.ToString(), syntax.Kind, syntax.Name);
            }
        }
        public static void AnalyzePagePartsCaption(SyntaxNodeAnalysisContext context)
        {
            dynamic? syntax;
            if (context.Node != null)
            {
                switch (context.Node.Kind)
                {
                    case SyntaxKind.PageAction:
                        syntax = context.Node as PageActionSyntax;
                        break;
                    case SyntaxKind.PageActionGroup:
                        syntax = context.Node as PageActionGroupSyntax;
                        break;
                    case SyntaxKind.PagePart:
                        syntax = context.Node as PagePartSyntax;
                        break;
                    case SyntaxKind.PageGroup:
                        syntax = context.Node as PageGroupSyntax;
                        break;
                    default:
                        syntax = null;
                        break;
                }
                AnalyzePagePartsCaptions(context, syntax);
            }
        }

        public static void AnalyzePagePartsCaptions(SyntaxNodeAnalysisContext context, dynamic syntax)
        {
            if (syntax != null)
            {
                PropertyListSyntax propertyListSyntax = syntax.PropertyList;
                IdentifierNameSyntax identifierNameSyntax = syntax.Name;
                PropertySyntax propertySyntax = GetProperty(propertyListSyntax.Properties, "Caption");
                PropertySyntax showCaptionPropertySyntax = GetProperty(propertyListSyntax.Properties, "ShowCaption");

                if (showCaptionPropertySyntax != null)
                {
                    dynamic showCaptionPropertyValueSyntax = showCaptionPropertySyntax.Value;
                    if (showCaptionPropertyValueSyntax.Value.Value.Value == "false")
                    {
                        return;
                    }
                }


                if (propertySyntax == null)
                {
                    ReportPagePartsMustHaveCaptionProperty(context, propertyListSyntax.GetLocation(), syntax.Name.ToString(), syntax.Kind, syntax.Name);
                }
            }

        }

        private static PropertySyntax GetProperty(
            SyntaxList<PropertySyntaxOrEmpty> properties,
            string propertyName)
        {
            foreach (PropertySyntaxOrEmpty property in properties)
            {
                if (property.Kind != SyntaxKind.EmptyProperty)
                {
                    PropertySyntax propertySyntax = (PropertySyntax)property;
                    if (SemanticFacts.IsSameName(propertySyntax.Name.Identifier.ValueText, propertyName))
                        return propertySyntax;
                }
            }
            return (PropertySyntax)null;
        }

        private static void ReportObjectsMustHaveCaptionProperty(
            SyntaxNodeAnalysisContext syntaxNodeAnalysisContext,
            Location location,
            string valueText,
            SyntaxKind syntaxKind,
            IdentifierNameSyntax syntaxName)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0005ObjectsMustHaveCaptionProperty, location, (object)valueText, (object)syntaxKind, (object)syntaxName));
        }

        private static void ReportTableFieldsMustHaveCaptionProperty(
            SyntaxNodeAnalysisContext syntaxNodeAnalysisContext,
            Location location,
            string valueText,
            SyntaxKind syntaxKind,
            IdentifierNameSyntax syntaxName)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0006TableFieldsMustHaveCaptionProperty, location, (object)valueText, (object)syntaxKind, (object)syntaxName));
        }

        private static void ReportEnumValueMustHaveCaptionProperty(
            SyntaxNodeAnalysisContext syntaxNodeAnalysisContext,
            Location location,
            string valueText,
            SyntaxKind syntaxKind,
            IdentifierNameSyntax syntaxName)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0007EnumValuesMustHaveCaptionProperty, location, (object)valueText, (object)syntaxKind, (object)syntaxName));
        }

        private static void ReportPagePartsMustHaveCaptionProperty(
           SyntaxNodeAnalysisContext syntaxNodeAnalysisContext,
           Location location,
           string valueText,
           SyntaxKind syntaxKind,
           IdentifierNameSyntax syntaxName)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0008PagePartsMustHaveCaptionProperty, location, (object)valueText, (object)syntaxKind, (object)syntaxName));
        }
    }
}

using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace CompanialCopAnalyzer.Library;
public static class RulesTablesHelper
{
    public static string? GetIdentifierName(SyntaxNode identifier)
    {
        if (identifier.GetType() != typeof(IdentifierNameSyntax))
            return null;

        IdentifierNameSyntax identifierNameSyntax = (IdentifierNameSyntax)identifier;

        return identifierNameSyntax.ToString().Replace("\"", "");
    }

    public static Dictionary<string, TableExtensionSyntax> GetTableExtensions(Compilation compilation)
    {
        Dictionary<string, TableExtensionSyntax> tableExtensions = [];

        foreach (SyntaxTree syntaxTree in compilation.SyntaxTrees)
        {
            foreach (SyntaxNode node in syntaxTree.GetRoot().ChildNodes())
            {
                if (node.GetType() != typeof(TableExtensionSyntax))
                    continue;

                TableExtensionSyntax tableExtension = (TableExtensionSyntax)node;

                string? extendedTable = GetIdentifierName(tableExtension.BaseObject.Identifier);

                if (extendedTable == null)
                    continue;

                try
                {
                    tableExtensions.Add(extendedTable, tableExtension);
                }
                catch { }
            }
        }

        return tableExtensions;
    }


    public static List<Tuple<string, string>> LoadTablePairs()
    {
        return [
                new Tuple<string, string>("Contact", "Customer"),
                new Tuple<string, string>("Contact", "Vendor"),
                new Tuple<string, string>("Contact", "Employee"),
                new Tuple<string, string>("Contact", "BankAccount"),

                new Tuple<string, string>("Contact Business Relation", "Office Contact Details"),

                new Tuple<string, string>("Deferral Line", "Deferral Line Archive"),
                new Tuple<string, string>("Deferral Line", "Posted Deferral Line"),
                new Tuple<string, string>("Deferral Header", "Deferral Header Archive"),
                new Tuple<string, string>("Deferral Header", "Posted Deferral Header"),

                new Tuple<string, string>("Gen. Journal Line", "Posted Gen. Journal Line"),
                new Tuple<string, string>("Gen. Journal Line", "Standard General Journal Line"),

                new Tuple<string, string>("Item Journal Line", "Standard Item Journal Line"),
                new Tuple<string, string>("Item Application Entry History", "Item Application Entry"),

                new Tuple<string, string>("Item Reference", "Item Cross Reference"),
                new Tuple<string, string>("Price List Line", "Price Worksheet Line"),
                new Tuple<string, string>("Copy Item Buffer", "Copy Item Parameters"),
                new Tuple<string, string>("Item Templ.", "Item"),

                new Tuple<string, string>("Whse. Item Tracking Line", "Tracking Specification"),
                new Tuple<string, string>("Whse. Item Tracking Line", "Reservation Entry"),
                new Tuple<string, string>("Reservation Entry", "Tracking Specification"),

                new Tuple<string, string>("Detailed CV Ledg. Entry Buffer", "Detailed Cust. Ledg. Entry"),
                new Tuple<string, string>("Detailed CV Ledg. Entry Buffer", "Detailed Vendor Ledg. Entry"),
                new Tuple<string, string>("Detailed CV Ledg. Entry Buffer", "Detailed Employee Ledger Entry"),
                new Tuple<string, string>("CV Ledger Entry Buffer", "Cust. Ledger Entry"),
                new Tuple<string, string>("Vendor Ledger Entry", "Vendor Ledger Entry Buffer"),
                new Tuple<string, string>("Vendor Payment Buffer", "Payment Buffer"),

                new Tuple<string, string>("Tracking Specification", "Reservation Entry"),
                new Tuple<string, string>("Assembly Line", "Posted Assembly Line"),
                new Tuple<string, string>("Assembly Header", "Posted Assembly Header"),

                new Tuple<string, string>("Invt. Document Header", "Invt. Receipt Header"),
                new Tuple<string, string>("Invt. Document Header", "Invt. Shipment Header"),

                new Tuple<string, string>("Bank Acc. Reconciliation", "Bank Account Statement"),
                new Tuple<string, string>("Bank Acc. Reconciliation Line", "Bank Account Statement Line"),

                new Tuple<string, string>("Posted Payment Recon. Line", "Bank Acc. Reconciliation Line"),
                new Tuple<string, string>("Posted Payment Recon. Hdr", "Bank Acc. Reconciliation"),
                new Tuple<string, string>("Cash Flow Worksheet Line", "Suggest Worksheet Lines"),

                new Tuple<string, string>("Direct Debit Collection Entry", "Direct Debit Collection Buffer"),
                new Tuple<string, string>("Applied Payment Entry", "Payment Application Proposal"),

                new Tuple<string, string>("Prod. Order Rtng Comment Line", "Routing Comment Line"),
                new Tuple<string, string>("Prod. Order Routing Personnel", "Routing Personnel"),
                new Tuple<string, string>("Prod. Order Rtng Qlty Meas.", "Prod. Order Rtng Qlty Meas."),
                new Tuple<string, string>("Prod. Order Routing Tool", "Routing Tool"),

                new Tuple<string, string>("Posted Approval Entry", "Posted Approval Entry"),
                new Tuple<string, string>("Posted Approval Comment Line", "Approval Comment Line"),

                new Tuple<string, string>("Sales Header", "Sales Shipment Header"),
                new Tuple<string, string>("Sales Header", "Sales Invoice Header"),
                new Tuple<string, string>("Sales Header", "Sales Cr.Memo Header"),
                new Tuple<string, string>("Sales Header", "Return Receipt Header"),
                new Tuple<string, string>("Sales Header", "Sales Header Archive"),
                new Tuple<string, string>("Sales Comment Line", "Sales Comment Line Archive"),

                new Tuple<string, string>("Purchase Header", "Purch. Rcpt. Header"),
                new Tuple<string, string>("Purchase Header", "Purch. Inv. Header"),
                new Tuple<string, string>("Purchase Header", "Purch. Cr. Memo Hdr."),
                new Tuple<string, string>("Purchase Header", "Return Shipment Header"),
                new Tuple<string, string>("Purchase Header", "Purchase Header Archive"),
                new Tuple<string, string>("Purchase Comment Line", "Purchase Comment Line Archive"),

                new Tuple<string, string>("Sales Line", "Sales Shipment Line"),
                new Tuple<string, string>("Sales Line", "Sales Invoice Line"),
                new Tuple<string, string>("Sales Line", "Sales Cr.Memo Line"),
                new Tuple<string, string>("Sales Line", "Return Receipt Line"),
                new Tuple<string, string>("Sales Line", "Sales Line Archive"),

                new Tuple<string, string>("Purchase Line", "Purch. Rcpt. Line"),
                new Tuple<string, string>("Purchase Line", "Purch. Inv. Line"),
                new Tuple<string, string>("Purchase Line", "Purch. Cr. Memo Line"),
                new Tuple<string, string>("Purchase Line", "Return Shipment Line"),
                new Tuple<string, string>("Purchase Line", "Purchase Line Archive"),

                new Tuple<string, string>("Issued Fin. Charge Memo Header", "Finance Charge Memo Header"),
                new Tuple<string, string>("Issued Fin. Charge Memo Line", "Finance Charge Memo Line"),
                new Tuple<string, string>("Issued Reminder Header", "Reminder Header"),
                new Tuple<string, string>("Issued Reminder Line", "Reminder Line"),
                new Tuple<string, string>("Posted Approval Entry", "Approval Entry"),
                new Tuple<string, string>("Posted Approval Comment Line", "Approval Comment Line"),

                new Tuple<string, string>("VAT Entry Posting Preview", "VAT Entry"),
                new Tuple<string, string>("VAT Posting Setup", "VAT Setup Posting Groups"),
                new Tuple<string, string>("VAT Business Posting Group", "Tax Area Buffer"),
                new Tuple<string, string>("Tax Area", "Tax Area Buffer"),
                new Tuple<string, string>("VAT Product Posting Group", "Tax Group Buffer"),
                new Tuple<string, string>("Tax Group", "Tax Group Buffer"),

                new Tuple<string, string>("Phys. Invt. Order Header", "Pstd. Phys. Invt. Order Hdr"),
                new Tuple<string, string>("Pstd. Phys. Invt. Order Line", "Phys. Invt. Comment Line"),
                new Tuple<string, string>("Pstd. Exp. Phys. Invt. Track", "Exp. Phys. Invt. Tracking"),
                new Tuple<string, string>("Phys. Invt. Comment Line", "Phys. Invt. Comment Line"),
                new Tuple<string, string>("Pstd. Phys. Invt. Record Hdr", "Phys. Invt. Record Header"),
                new Tuple<string, string>("Pstd. Phys. Invt. Record Line", "Phys. Invt. Record Line"),
                new Tuple<string, string>("Invt. Document Header", "Invt. Receipt Header"),
                new Tuple<string, string>("Invt. Document Header", "Invt. Shipment Header"),
                new Tuple<string, string>("Invt. Document Line", "Invt. Receipt Line"),
                new Tuple<string, string>("Invt. Document Line", "Invt. Shipment Line"),

                new Tuple<string, string>("Prod. Order Rtng Comment Line", "Routing Comment Line"),
                new Tuple<string, string>("Prod. Order Routing Personnel", "Routing Personnel"),
                new Tuple<string, string>("Prod. Order Rtng Qlty Meas.", "Routing Quality Measure"),
                new Tuple<string, string>("Prod. Order Routing Tool", "Routing Tool"),
                new Tuple<string, string>("Prod. Order Comp. Cmt Line", "Production BOM Comment Line"),

                new Tuple<string, string>("Warehouse Activity Header", "Registered Whse. Activity Hdr."),
                new Tuple<string, string>("Warehouse Activity Header", "Registered Invt. Movement Hdr."),
                new Tuple<string, string>("Warehouse Activity Header", "Posted Invt. Pick Header"),
                new Tuple<string, string>("Warehouse Activity Header", "Posted Invt. Put-away Header"),

                new Tuple<string, string>("Warehouse Activity Line", "Registered Whse. Activity Line"),
                new Tuple<string, string>("Warehouse Activity Line", "Registered Invt. Movement Line"),
                new Tuple<string, string>("Warehouse Activity Line", "Posted Invt. Pick Line"),
                new Tuple<string, string>("Warehouse Activity Line", "Posted Invt. Put-away Line"),

                new Tuple<string, string>("Posted Whse. Receipt Header", "Warehouse Receipt Header"),
                new Tuple<string, string>("Posted Whse. Shipment Line", "Warehouse Shipment Line"),
                new Tuple<string, string>("Whse. Item Entry Relation", "Item Entry Relation"),

                new Tuple<string, string>("Time Sheet Header", "Time Sheet Header Archive"),
                new Tuple<string, string>("Time Sheet Line", "Time Sheet Line Archive"),
                new Tuple<string, string>("Time Sheet Detail Archive", "Time Sheet Detail"),
                new Tuple<string, string>("Time Sheet Comment Line", "Time Sheet Cmt. Line Archive"),
                new Tuple<string, string>("Employee Time Reg Buffer", "Time Sheet Detail"),

                new Tuple<string, string>("Service Shipment Header", "Service Header"),
                new Tuple<string, string>("Service Shipment Item Line", "Service Item Line"),
                new Tuple<string, string>("Service Shipment Line", "Service Line"),
                new Tuple<string, string>("Service Invoice Header", "Service Header"),
                new Tuple<string, string>("Service Invoice Line", "Service Line"),
                new Tuple<string, string>("Service Cr.Memo Header", "Service Header"),
                new Tuple<string, string>("Service Cr.Memo Line", "Service Line"),
                new Tuple<string, string>("Filed Service Contract Header", "Service Contract Header"),
                new Tuple<string, string>("Filed Contract Line", "Service Contract Line"),

                new Tuple<string, string>("Job", "Job Archive"),
                new Tuple<string, string>("Job Task", "Job Task Archive"),
                new Tuple<string, string>("Job Planning Line", "Job Planning Line Archive"),

                new Tuple<string, string>("Workflow Step Instance Archive", "Workflow Step Instance"),
                new Tuple<string, string>("Workflow Record Change Archive", "Workflow - Record Change"),
                new Tuple<string, string>("Workflow Step Argument Archive", "Workflow Step Argument"),

                new Tuple<string, string>("Purchase Header", "Purch. Inv. Entity Aggregate"),
                new Tuple<string, string>("Purch. Inv. Header", "Purch. Inv. Entity Aggregate"),
                new Tuple<string, string>("Purch. Inv. Line Aggregate", "Purch. Inv. Line"),
                new Tuple<string, string>("Purch. Inv. Line Aggregate", "Purchase Line"),
                new Tuple<string, string>("Purch. Inv. Line Aggregate", "Purch. Cr. Memo Line"),
                new Tuple<string, string>("Sales Header", "Sales Invoice Entity Aggregate"),
                new Tuple<string, string>("Sales Invoice Header", "Sales Invoice Entity Aggregate"),
                new Tuple<string, string>("Sales Invoice Line Aggregate", "Sales Invoice Line"),
                new Tuple<string, string>("Sales Invoice Line Aggregate", "Sales Line"),
                new Tuple<string, string>("Sales Invoice Line Aggregate", "Sales Cr.Memo Line"),
                new Tuple<string, string>("Unlinked Attachment", "Attachment Entity Buffer"),
                new Tuple<string, string>("Attachment Entity Buffer", "Incoming Document Attachment"),
                new Tuple<string, string>("Purchase Header", "Purch. Cr. Memo Entity Buffer"),
                new Tuple<string, string>("Purch. Cr. Memo Hdr.", "Purch. Cr. Memo Entity Buffer"),
                new Tuple<string, string>("Purchase Order Entity Buffer", "Purchase Header"),
                new Tuple<string, string>("Sales Cr. Memo Entity Buffer", "Sales Header"),
                new Tuple<string, string>("Sales Cr. Memo Entity Buffer", "Sales Cr.Memo Header"),
                new Tuple<string, string>("Sales Order Entity Buffer", "Sales Header"),
                new Tuple<string, string>("Sales Quote Entity Buffer", "Sales Header"),

                new Tuple<string, string>("IC Outbox Sales Header", "Sales Header"),
                new Tuple<string, string>("IC Outbox Sales Line", "Sales Line"),
                new Tuple<string, string>("IC Outbox Sales Header", "Sales Invoice Header"),
                new Tuple<string, string>("IC Outbox Sales Line", "Sales Invoice Line"),
                new Tuple<string, string>("IC Outbox Sales Header", "Sales Cr.Memo Header"),
                new Tuple<string, string>("IC Outbox Sales Line", "Sales Cr.Memo Line"),
                new Tuple<string, string>("IC Outbox Purchase Header", "Purchase Header"),
                new Tuple<string, string>("IC Outbox Purchase Line", "Purchase Line"),
                new Tuple<string, string>("Handled IC Inbox Jnl. Line", "IC Inbox Jnl. Line"),
                new Tuple<string, string>("Handled IC Inbox Sales Header", "IC Inbox Sales Header"),
                new Tuple<string, string>("Handled IC Inbox Sales Line", "IC Inbox Sales Line"),
                new Tuple<string, string>("IC Inbox Sales Line", "Sales Line"),
                new Tuple<string, string>("Handled IC Inbox Purch. Header", "IC Inbox Purchase Header"),
                new Tuple<string, string>("Handled IC Inbox Purch. Line", "IC Inbox Purchase Line"),
                new Tuple<string, string>("IC Inbox Purchase Line", "Purchase Line"),
                new Tuple<string, string>("Handled IC Inbox Jnl. Line", "IC Inbox Jnl. Line"),
                new Tuple<string, string>("IC Outbox Jnl. Line", "Handled IC Outbox Jnl. Line"),
                new Tuple<string, string>("IC Outbox Sales Header", "Handled IC Outbox Sales Header"),
                new Tuple<string, string>("IC Outbox Sales Line", "Handled IC Outbox Sales Line"),
                new Tuple<string, string>("IC Outbox Purchase Header", "Handled IC Outbox Purch. Hdr"),
                new Tuple<string, string>("IC Outbox Purchase Line", "Handled IC Outbox Purch. Line"),
                new Tuple<string, string>("IC Outbox Jnl. Line", "IC Inbox Jnl. Line"),
                new Tuple<string, string>("IC Outbox Sales Header", "IC Inbox Sales Header"),
                new Tuple<string, string>("IC Outbox Sales Line", "IC Inbox Sales Line"),
                new Tuple<string, string>("IC Outbox Purchase Header", "IC Inbox Purchase Header"),
                new Tuple<string, string>("IC Outbox Purchase Line", "IC Inbox Purchase Line"),
                new Tuple<string, string>("Handled IC Outbox Trans.", "IC Outbox Transaction"),
                new Tuple<string, string>("IC Inbox Transaction", "Buffer IC Inbox Transaction"),
                new Tuple<string, string>("IC Inbox Jnl. Line", "Buffer IC Inbox Jnl. Line"),
                new Tuple<string, string>("IC Inbox Purchase Header", "Buffer IC Inbox Purch Header"),
                new Tuple<string, string>("IC Inbox Purchase Line", "Buffer IC Inbox Purchase Line"),
                new Tuple<string, string>("IC Inbox Sales Header", "Buffer IC Inbox Purchase Line"),  // Not a mistake, see: ICDataExchangeAPI - PostICSalesHeaderToICPartnerInbox
                new Tuple<string, string>("IC Inbox Sales Line", "Buffer IC Inbox Sales Line"),
                new Tuple<string, string>("IC Inbox/Outbox Jnl. Line Dim.", "Buffer IC InOut Jnl. Line Dim."),
                new Tuple<string, string>("IC Document Dimension", "Buffer IC Document Dimension"),
                new Tuple<string, string>("IC Comment Line", "Buffer IC Comment Line"),

                new Tuple<string, string>("Comment Line", "Comment Line Archive"),
                new Tuple<string, string>("Config. Setup", "Company Information"),
                new Tuple<string, string>("Object Options", "Report Settings"),

                new Tuple<string, string>("Analysis by Dim. Parameters", "Analysis by Dim. User Param."),
                new Tuple<string, string>("G/L Account (Analysis View)", "G/L Account"),
                new Tuple<string, string>("Acc. Schedule Line", "Acc. Sched. KPI Buffer"),
                new Tuple<string, string>("G/L Entry Posting Preview", "G/L Entry"),

                new Tuple<string, string>("Sales Invoice Header", "O365 Sales Document"),
                new Tuple<string, string>("Sales Header", "O365 Sales Document"),

                new Tuple<string, string>("Incoming Document Attachment", "Inc. Doc. Attachment Overview"),
                new Tuple<string, string>("Config. Field Mapping", "Config. Field Map"),

                new Tuple<string, string>("Onboarding Signal", "Onboarding Signal Buffer")
            ];
    }

}

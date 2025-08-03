using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class CustomControls_ucNavigation : System.Web.UI.UserControl
{
    XpressDataContext dc = new XpressDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    public delegate void OnButtonClick(string strValue);
    public event OnButtonClick btnHandler;
    public event OnButtonClick btnHandlerPrev;
    public event OnButtonClick btnHandlerFirst;
    public event OnButtonClick btnHandlerLast;


    public event OnButtonClick btnHandlerSearch;
    public event OnButtonClick btnHandlerAddNew;


    public string SetText
    {
        get
        {
            return txtSerialSearch.Text;
        }

        set
        {

            txtSerialSearch.Text = value;
        }
    }
  

    public bool VisibleText
    {
        get
        {
            return txtSerialSearch.Visible;
        }

        set
        {

            txtSerialSearch.Visible = value;
        }
    }

    public byte EntryType
    {
        get
        {
            if (ViewState["EntryType"] == null) return 0;
            return (byte)ViewState["EntryType"];
        }

        set
        {
            ViewState["EntryType"] = value;
        }
    }



    private int? _SourceDocTypeType_ID;
    public int? SourceDocTypeType_ID
    {
        get
        {
            return _SourceDocTypeType_ID;
        }

        set
        {

            _SourceDocTypeType_ID = value;
        }
    }



    private int? _TypeNavigation;
    public int? TypeNavigation
    {
        get
        {
            return _TypeNavigation;
        }

        set
        {

            _TypeNavigation = value;
        }
    }



    //private int? _ID;
    //public int? ID
    //{
    //    get
    //    {
    //        return _ID;
    //    }

    //    set
    //    {

    //        _ID = value;
    //    }
    //}



    private int? _Link;
    public int? Link
    {
        get
        {
            return _Link;
        }

        set
        {

            _Link = value;
        }
    }


    public int? Res_ID
    {
        get
        {
            if (ViewState["Res_ID"] == null) return 0;
            return (int)ViewState["Res_ID"];
        }

        set
        {
            ViewState["Res_ID"] = value;
        }
    }
   public int? IsPermShow
    {
        get
        {
            if (ViewState["IsPermShow"] == null) return (int?)null;
            return (int?)ViewState["IsPermShow"];
        }

        set
        {
            ViewState["IsPermShow"] = value;
        }
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {

        Res_ID = dc.SearchNavigation(EntryType, SourceDocTypeType_ID, 1, Res_ID, txtSerialSearch.Text,IsPermShow);
        if (Res_ID > 0)
        {
            Response.Redirect(GetLinkCorrect(SourceDocTypeType_ID) + "?ID=" + Res_ID.ToString());

        }

        //  Response.Redirect(PageLinks.InvoiceShortcut + "?ID=" + Res_ID.ToString());
        //if (btnHandlerFirst != null && Res_ID > 0)
        //    btnHandlerFirst(Res_ID.ToString());
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        Res_ID = dc.SearchNavigation(EntryType, SourceDocTypeType_ID, 3, Res_ID, txtSerialSearch.Text,IsPermShow);
        if (Res_ID > 0)
        {
            Response.Redirect(GetLinkCorrect(SourceDocTypeType_ID) + "?ID=" + Res_ID.ToString());

        }
        //if (btnHandler != null && Res_ID > 0)
        //    btnHandler(Res_ID.ToString());
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Res_ID = dc.SearchNavigation(EntryType, SourceDocTypeType_ID, 4, Res_ID, txtSerialSearch.Text,IsPermShow);

        if (Res_ID > 0)
        {
            Response.Redirect(GetLinkCorrect(SourceDocTypeType_ID) + "?ID=" + Res_ID.ToString());

        }
        //if (btnHandlerPrev != null && Res_ID > 0)
        //    btnHandlerPrev(Res_ID.ToString());

    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        Res_ID = dc.SearchNavigation(EntryType, SourceDocTypeType_ID, 2, Res_ID, txtSerialSearch.Text,IsPermShow);
        if (Res_ID > 0)
        {
            Response.Redirect(GetLinkCorrect(SourceDocTypeType_ID) + "?ID=" + Res_ID.ToString());

        }
        //if (btnHandlerLast != null && Res_ID > 0)
        //    btnHandlerLast(Res_ID.ToString());

    }
    protected void lnkSearchInvoice_Click(object sender, EventArgs e)
    {

        Res_ID = dc.SearchNavigation(EntryType, SourceDocTypeType_ID, 5, Res_ID, txtSerialSearch.Text,IsPermShow);

        if (Res_ID > 0)
        {
            Response.Redirect(GetLinkCorrect(SourceDocTypeType_ID) + "?ID=" + Res_ID.ToString());

        }
        //if (btnHandlerSearch != null && Res_ID > 0)
        //    btnHandlerSearch(Res_ID.ToString());
    }
    protected void lnkAddNewItem_Click(object sender, EventArgs e)
    {

        Response.Redirect(GetLinkCorrect(SourceDocTypeType_ID));
        txtSerialSearch.Text = string.Empty;

        //if (btnHandlerAddNew != null)
        //{
        //    btnHandlerAddNew(txtSerialSearch.Text);

        //}
        //txtSerialSearch.Text = string.Empty;
    }

    private string GetLinkCorrect(int? SourceDocTableType = 1)
    {
        //switch (DocumentsTableTypes)
        //{
        //    case 1: return PageLinks.InvoiceShortcut;
        //        break;
        //    case 2: return PageLinks.ReceiptShortcut;
        //        break;
        //    case 4: return PageLinks.ReturnReceipt;
        //        break;
        //    case 5: return PageLinks.ReturnInvoice;
        //        break;
        //    default:
        //        break;
        switch (SourceDocTableType.Value)
        {
            case 0: return PageLinks.Items;
                break;
            case 1: return PageLinks.InvoiceShortcut;
                break;
            case 2: return PageLinks.ReceiptShortcut;
                break;
            case 4: return PageLinks.ReturnReceipt;
                break;
            case 5: return PageLinks.ReturnInvoice;
                break;
            case 6: return PageLinks.JournalEntry;
                break;
            case 7: return PageLinks.InventoryCorrection;
                break;
            case 8: return PageLinks.InventoryTransfer;
                break;
            case 9: return PageLinks.BeginingInventory;
                break;
            case 10: return PageLinks.Payments + "/CashIn";
                break;
            case 11: return PageLinks.Payments + "/CashOut";
                break;
            case 12: return PageLinks.Payments + "/BankDeposit";
                break;
            case 13: return PageLinks.Payments + "/BankWithdraw";
                break;
            case 90:
                switch (EntryType)
                {
                    case 1: return PageLinks.Customers;
                        break;
                    case 2: return PageLinks.Vendors;
                        break;
                    case 3: return PageLinks.Employees;
                        break;
                    default:
                        break;
                }
                break;

            //case DocumentsTableTypes.Checks_Issued:
            //    break;
            //case DocumentsTableTypes.Checks_Received:
            //    break;
            //case DocumentsTableTypes.DocumentryCreditInstallments:
            //    break;
            //case DocumentsTableTypes.Loans:
            //    break;
            //case DocumentsTableTypes.HRPayroll:
            //    break;
            //case DocumentsTableTypes.HR_Loans:
            //    break;
            //case DocumentsTableTypes.ProductionOrder:
            //    break;
            //case DocumentsTableTypes.ProductionOrderExpenses:
            //    break;
            //case DocumentsTableTypes.InvoiceQuota:
            //    break;
            default:
                break;
        }


        return string.Empty;
    }
}
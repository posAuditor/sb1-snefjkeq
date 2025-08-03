using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Reflection;

public partial class CustomControls_OperationsView : System.Web.UI.UserControl
{

    XpressDataContext dc = new XpressDataContext();


    public int SourceDocTypeType_ID
    {
        get
        {
            if (ViewState["SourceDocTypeType_ID"] == null) return 0;
            return (int)ViewState["SourceDocTypeType_ID"];
        }

        set
        {
            ViewState["SourceDocTypeType_ID"] = value;
        }
    }
    public int Source_ID
    {
        get
        {
            if (ViewState["Source_ID"] == null) return 0;
            return (int)ViewState["Source_ID"];
        }

        set
        {
            ViewState["Source_ID"] = value;
        }
    }

    public bool WithOutSecurity
    {
        get
        {
            if (ViewState["WithOutSecurity"] == null) return false;
            return (bool)ViewState["WithOutSecurity"];
        }

        set
        {
            ViewState["WithOutSecurity"] = value;
        }
    }



    //public string SetText
    //{
    //    get
    //    {
    //        return txtSerialSearch.Text;
    //    }

    //    set
    //    {

    //        txtSerialSearch.Text = value;
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WithOutSecurity)
        {
            MyContext con = new MyContext(System.Web.Security.Membership.GetUser(), Request.AppRelativeCurrentExecutionFilePath, string.Empty);
            btnViewOperation.Visible = con.PageData.AllowViewJE;
        }

        this.FillJournalEntriesList();
    }
    protected void btnViewOperation_Click(object sender, EventArgs e)
    {


        mpeFastAddNew.Show();
    }
    public string BetLinkPage(object sourceDoc_ID, object SourceDocTableType_ID)
    {
        var TypeInt = SourceDocTableType_ID.ToIntOrDefault();
        switch (TypeInt)
        {

            case 1: return PageLinks.InvoiceShortcut + "?ID=" + sourceDoc_ID.ToIntOrDefault();
            case 2: return PageLinks.ReceiptShortcut + "?ID=" + sourceDoc_ID.ToIntOrDefault();
            case 4: return PageLinks.ReturnReceipt + "?ID=" + sourceDoc_ID.ToIntOrDefault();
            case 5: return PageLinks.ReturnInvoice + "?ID=" + sourceDoc_ID.ToIntOrDefault();

            case 7: return PageLinks.InventoryCorrection + "?ID=" + sourceDoc_ID.ToIntOrDefault();
            case 8: return PageLinks.InventoryTransfer + "?ID=" + sourceDoc_ID.ToIntOrDefault();
            case 10: return PageLinks.Payments + "/CashIn?ID=" + sourceDoc_ID.ToIntOrDefault();
            case 11: return PageLinks.Payments + "/CashOut?ID=" + sourceDoc_ID.ToIntOrDefault();

            case 12: return PageLinks.Payments + "/BankDeposit?ID=" + sourceDoc_ID.ToIntOrDefault();
            case 13: return PageLinks.Payments + "/BankWithdraw?ID=" + sourceDoc_ID.ToIntOrDefault();

            case 14: return PageLinks.Checks + "/Issued?ID=" + sourceDoc_ID.ToIntOrDefault();
            case 15: return PageLinks.Checks + "/Received?ID=" + sourceDoc_ID.ToIntOrDefault();

            case 18: return PageLinks.Assets + "?ID=" + sourceDoc_ID.ToIntOrDefault();
            case 24: return PageLinks.ProductionOrder + "?ID=" + sourceDoc_ID.ToIntOrDefault();


        }
        return string.Empty;
    }

    public void FillJournalEntriesList()
    {
        gvGeneralJournalList.DataSource = dc.usp_GeneralJournal_SelectById(SourceDocTypeType_ID, Source_ID).CopyToDataTable();
        gvGeneralJournalList.DataBind();
    }


    protected void btnFastAddNew_Click(object sender, EventArgs e)
    {

    }
    protected void BtnCancelAddNew_Click(object sender, EventArgs e)
    {

    }


    protected void gvGeneralJournalList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            GridViewRow row = gvGeneralJournalList.Rows[e.NewSelectedIndex];
            HtmlTableRow tdOperationDetails = (HtmlTableRow)row.FindControl("tdOperationDetails");
            tdOperationDetails.Visible = !tdOperationDetails.Visible;
            ((LinkButton)row.FindControl("lnkbtnimgSelect")).CssClass = tdOperationDetails.Visible ? "grid-collapse" : "grid-expand";
            if (tdOperationDetails.Visible)
            {
                GridView gvDetails = (GridView)row.FindControl("gvOperationDetails");
                gvDetails.DataSource = dc.usp_JournalEntryDetails_Select(gvGeneralJournalList.DataKeys[e.NewSelectedIndex]["ID"].ToInt(), 0);
                gvDetails.DataBind();
            }
            mpeFastAddNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

}
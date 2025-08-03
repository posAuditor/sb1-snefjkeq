using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Accounting_journalEntriesList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtJournalEntriesList
    {
        get
        {
            return (DataTable)Session["dtJournalEntriesList" + this.WinID];
        }

        set
        {
            Session["dtJournalEntriesList" + this.WinID] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                gvJournalEntriesList.Columns[6].Visible = this.MyContext.PageData.IsViewDoc;
                gvJournalEntriesList.Columns[7].Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillJournalEntriesList();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region Control Events

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            int Operation_ID = gvJournalEntriesList.DataKeys[Index]["ID"].ToInt();
            int? Branch_ID = gvJournalEntriesList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\JournalEntry_Print.rpt"));
            doc.SetParameterValue("@Operation_ID", Operation_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(Branch_ID, "JournalEntry"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillJournalEntriesList();
            ddlStatus.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearSrch_Click(object sender, EventArgs e)
    {
        try
        {
            txtDateFromSrch.Clear();
            txtDateToSrch.Clear();
            ddlCurrency.SelectedIndex = 0;
            txtSerialsrch.Clear();
            txtUserRefNo.Clear();
            ddlStatus.SelectedIndex = 0;
            if (acBranch.Enabled) acBranch.Clear();
            this.FillJournalEntriesList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvJournalEntriesList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvJournalEntriesList.PageIndex = e.NewPageIndex;
            gvJournalEntriesList.DataSource = this.dtJournalEntriesList;
            gvJournalEntriesList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvJournalEntriesList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvJournalEntriesList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void lnkExportLine_OnClick(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenWindow", "window.open('ExportJE.aspx?ID=" + (sender as LinkButton).CommandArgument + "');", true);
    }
    protected void lnkAttachments_Click(object sender, EventArgs e)
    {
        var linkButton = sender as LinkButton;
        if (linkButton != null)
        {
            var id = int.Parse(linkButton.CommandArgument);
            // linkButton.Text = Resources.Labels.Attachments + " " + string.Format("({0})", dc.usp_Attachments_Select("~/Sales/InvoiceForm.aspx?ID=" + linkButton.CommandArgument).Count());
            Iframe3.Src = "~/Main/Attachments.aspx?DocumentURI=~%2FAccounting%2FjournalEntry.aspx%3FID%3D" + linkButton.CommandArgument + "&DocumentPath=~%2FAccounting%2FjournalEntry.aspx&DocumentPathInfo=";
            ModalPopupExtender3.Show();
        }
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenWindow", "window.open('ExportJEntry.aspx');", true);
    }


    #endregion

    #region Private Methods

    private void FillJournalEntriesList()
    {
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        this.dtJournalEntriesList = dc.usp_JournalEntryD_Select(null, acBranch.Value.ToNullableInt(), 
                                                                    txtDateFromSrch.Text.ToDate(), 
                                                                    txtDateToSrch.Text.ToDate(),
                                                                    txtSerialsrch.TrimmedText, 
                                                                    DocStatus_ID,
                                                                    Currency_ID,
                                                                    txtUserRefNo.Text,
                                                                    MyContext.CurrentCulture.ToByte(),
                                                                    txtDebitFrom.Text.ToDecimalOrDefault(),
                                                                    txtDebitto.Text.ToDecimalOrDefault(),
                                                                    txtCreditFrom.Text.ToDecimalOrDefault(),
                                                                    txtCreditto.Text.ToDecimalOrDefault(),
                                                                     txtNotes.Text).CopyToDataTable();
        gvJournalEntriesList.DataSource = this.dtJournalEntriesList;
        gvJournalEntriesList.DataBind();
    }

    private void LoadControls()
    {
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        ddlCurrency.Items.Insert(0, new ListItem(Resources.Labels.Select, "-1"));
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvJournalEntriesList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
        
    }

    #endregion
    protected void lnkGenerateOperation_Click(object sender, EventArgs e)
    {
        var id = int.Parse((sender as LinkButton).CommandArgument);
        Response.Redirect("JournalEntry.aspx?Generate_ID=" + id.ToExpressString());
    }
}
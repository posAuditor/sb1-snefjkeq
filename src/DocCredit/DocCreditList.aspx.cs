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

public partial class DocCredit_DocCreditList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtDocCreditList
    {
        get
        {
            return (DataTable)Session["dtDocCreditList" + this.WinID];
        }

        set
        {
            Session["dtDocCreditList" + this.WinID] = value;
        }
    }

    #endregion

    #region ViewState

    private int Receipt_ID
    {
        get
        {
            if (ViewState["Receipt_ID"] == null) return 0;
            return (int)ViewState["Receipt_ID"];
        }

        set
        {
            ViewState["Receipt_ID"] = value;
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
                gvReceiptsList.Columns[11].Visible = this.MyContext.PageData.IsViewDoc;
                gvReceiptsList.Columns[12].Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillReceiptsList();
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

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillReceiptsList();
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
            acVendorName.Clear();
            acBank.Clear();
            txtDocCreditName.Clear();
            this.FilterVendors(null, null);
            this.FillReceiptsList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterVendors(object sender, EventArgs e)
    {
        try
        {
            acBank.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + "," + COA.Banks.ToInt().ToExpressString() + ",false";
            acVendorName.ContextKey = "V," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + ",";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvReceiptsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvReceiptsList.PageIndex = e.NewPageIndex;
            gvReceiptsList.DataSource = this.dtDocCreditList;
            gvReceiptsList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvReceiptsList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvReceiptsList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    
    protected void lnkCLose_Click(object sender, EventArgs e)
    {
        try
        {
            int index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            int result = dc.usp_DocumentryCredit_Close(gvReceiptsList.DataKeys[index]["ID"].ToInt());
            if (result == -20)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.CantCloseBeforePayAndReceive, string.Empty);
                return;
            }
            this.FillReceiptsList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            this.Receipt_ID = gvReceiptsList.DataKeys[Index]["ID"].ToInt();
            int? Branch_ID = gvReceiptsList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\DocCreditWithExpenses_Print.rpt"));
            doc.SetParameterValue("@Receipt_ID", this.Receipt_ID);
            doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.Receipt.ToInt(), "DocumentTaxes.rpt");
            doc.SetParameterValue("@Doc_ID", this.Receipt_ID, "DocumentTaxes.rpt");
            doc.SetParameterValue("@IsReturn", false, "DocumentTaxes.rpt");
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(Branch_ID, "DocumentryCredit"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    #endregion

    #region Private Methods

    private void FillReceiptsList()
    {
        lnkadd.NavigateUrl = PageLinks.DocumentryCredit;

        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        bool ShowDocsWithCurrentExpensesOnly = Request["ShowDocsWithCurrentExpensesOnly"] != null && !Page.IsPostBack;
        this.dtDocCreditList = dc.usp_DocumentryCredit_Select(acBranch.Value.ToNullableInt(), Currency_ID, txtSerialsrch.TrimmedText, acVendorName.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtUserRefNo.Text, DocStatus_ID, MyContext.CurrentCulture.ToByte(), txtDocCreditName.TrimmedText, acBank.Value.ToNullableInt(), ShowDocsWithCurrentExpensesOnly).CopyToDataTable();
        gvReceiptsList.DataSource = this.dtDocCreditList;
        gvReceiptsList.DataBind();
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
        this.FilterVendors(null, null);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
    }

    #endregion

}
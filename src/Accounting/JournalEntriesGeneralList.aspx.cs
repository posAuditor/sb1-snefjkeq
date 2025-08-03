using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using XPRESS.Common;

public partial class Accounting_JournalEntriesGeneralList : UICulturePage
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
                //gvJournalEntriesList.Columns[7].Visible = this.MyContext.PageData.IsPrint;
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            int Operation_ID = gvJournalEntriesList.DataKeys[Index]["ID"].ToInt();
            string Serial_Ext = gvJournalEntriesList.DataKeys[Index]["SerialExt"].ToStringOrEmpty();
            dc.CancelApprovel(Operation_ID);
            //int? Branch_ID = gvJournalEntriesList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            //ReportDocument doc = new ReportDocument();
            //doc.Load(Server.MapPath("~\\Reports\\JournalEntry_Print.rpt"));
            //doc.SetParameterValue("@Operation_ID", Operation_ID);
            //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(Branch_ID, "JournalEntry"), false);
            UserMessages.Message(this.MyContext.PageData.PageTitle, "تم الغاء الاعتماد", string.Empty);
            LogAction(Actions.NotApprove, Serial_Ext, dc);
            this.FillJournalEntriesList();
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

    #endregion

    #region Private Methods

    private void FillJournalEntriesList()
    {
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        var lst = dc.usp_JournalEntryGeneralAccount_ID_Select(null, acBranch.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtSerialsrch.TrimmedText, DocStatus_ID, Currency_ID, txtUserRefNo.Text, MyContext.CurrentCulture.ToByte(),
            acDocNameSrch.Text.ToExpressString(), txtSerialsrchNG.Text.ToExpressString(), acAccount.Value.ToNullableInt());
        //if (lst.First())
        //{

        //}


        this.dtJournalEntriesList = lst.CopyToDataTable();
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
        acDocNameSrch.ContextKey = string.Empty;
        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        ddlCurrency.Items.Insert(0, new ListItem(Resources.Labels.Select, "-1"));
        acAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToString() + "," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + ",,false,false," + this.MyContext.UserProfile.UserId;
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

    public bool IsVisible(string id, int status)
    {                                                                                                                                                  // 
        if (status == 1) return false;
        if (id == "1" || id == "2" || id == "4" || id == "5" || id == "6" || id == "8" || id == "10" || id == "11" || id == "12" || id == "13" || id == "14" || id == "15" || id == "16" || id == "20" || id == "34" || id == "36" || id == "38" || id == "39" || id == "46" || id == "44" || id == "43" || id == "45" || id == "22" || id == "9" || id == "3" || id == "7")
        {
            return true;
        }
        return false;

    }
}
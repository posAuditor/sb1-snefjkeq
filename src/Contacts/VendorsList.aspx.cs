using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;

public partial class Contacts_VendorsList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtVendorsList
    {
        get
        {
            return (DataTable)Session["dtVendorsList" + this.WinID];
        }

        set
        {
            Session["dtVendorsList" + this.WinID] = value;
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
                gvVendorsList.Columns[5].Visible = this.MyContext.PageData.IsViewDoc;
                gvVendorsList.Columns[6].Visible = this.MyContext.PageData.IsDelete;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillVendorsList();
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
            this.FillVendorsList();
            if (acBranch.Enabled) acBranch.AutoCompleteFocus(); else ddlCurrency.Focus();
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
            txtAccountNumber.Clear();
            ddlCurrency.SelectedIndex = 0;
            txtSerialsrch.Clear();
            acArea.Clear();
            acName.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            this.FilterVendors(null, null);
            this.FillVendorsList();
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
            acName.ContextKey = "V," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + "," + acArea.Value;
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvVendorsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvVendorsList.PageIndex = e.NewPageIndex;
            gvVendorsList.DataSource = this.dtVendorsList;
            gvVendorsList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvVendorsList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = dc.usp_Vendors_Delete(gvVendorsList.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvVendorsList.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
            this.FillVendorsList();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillVendorsList()
    {
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        this.dtVendorsList = dc.usp_VendorsList_Select(acBranch.Value.ToNullableInt(), Currency_ID, txtSerialsrch.TrimmedText, acName.Text, txtAccountNumber.TrimmedText).CopyToDataTable();
        gvVendorsList.DataSource = this.dtVendorsList;
        gvVendorsList.DataBind();
    }

    private void LoadControls()
    {
        //acArea.ContextKey = string.Empty;
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
        foreach (DataControlField col in gvVendorsList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
        
    }

    #endregion

}
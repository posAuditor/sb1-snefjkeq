using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;

public partial class FixedAssets_CapitalMaintainList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtCapitalMaintainList
    {
        get
        {
            return (DataTable)Session["dtCapitalMaintainList" + this.WinID];
        }

        set
        {
            Session["dtCapitalMaintainList" + this.WinID] = value;
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
                gvAssetsList.Columns[10].Visible = this.MyContext.PageData.IsViewDoc;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillAssetsList();
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
            this.FillAssetsList();
            txtDateFromSrch.Focus();
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
            txtName.Clear();
            acParentAsset.Clear();
            ddlStatus.SelectedIndex = 0;
            txtUserRefNo.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            acOppositeAccount.Clear();
            this.FilterAccounts(null, null);
            this.FillAssetsList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterAccounts(object sender, EventArgs e)
    {
        try
        {
            acParentAsset.ContextKey = acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue);
            acOppositeAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + ",,true";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAssetsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssetsList.PageIndex = e.NewPageIndex;
            gvAssetsList.DataSource = this.dtCapitalMaintainList;
            gvAssetsList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }



    #endregion

    #region Private Methods

    private void FillAssetsList()
    {
        lnkadd.NavigateUrl = PageLinks.CapitalMaintainence;

        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        this.dtCapitalMaintainList = dc.usp_CapitalMaintenece_Select(txtName.TrimmedText, acParentAsset.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), acOppositeAccount.Value.ToNullableInt(), DocStatus_ID, Currency_ID, acBranch.Value.ToNullableInt(), MyContext.CurrentCulture.ToByte(), txtSerialsrch.TrimmedText, txtUserRefNo.TrimmedText).CopyToDataTable();
        gvAssetsList.DataSource = this.dtCapitalMaintainList;
        gvAssetsList.DataBind();
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
        this.FilterAccounts(null, null);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        
        foreach (DataControlField col in gvAssetsList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

}